using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using P7CreateRestApi.Services.Auth;
using System.Text;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<JwtService> _logger;

    public JwtService(
        IConfiguration configuration,
        UserManager<User> userManager,
        ILogger<JwtService> logger)
    {
        _configuration = configuration;
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    /// ✅ Génère un token JWT avec durée de vie raisonnable
    /// </summary>
    public async Task<string> GenerateTokenAsync(User user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim("fullname", user.Fullname),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

        // Fix for CS1503: Convert IList<string> to individual claims for roles
        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var jwtSettings = _configuration.GetSection("JwtSettings").Get<JwtSettings>();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1), // ⏰ 1 heures de durée de vie
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// ✅ Valide un token JWT
    /// </summary>
    public  ServiceResult<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);
            var audience = _configuration["JwtSettings:Audience"]; // Récupération explicite

            // Validation manuelle de l'audience
            if (string.IsNullOrEmpty(audience))
                throw new InvalidOperationException("JWT Audience not configured");

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = audience, // 🔑 Utilisation de ValidAudience (singulier)
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return ServiceResult<bool>.Success(true);
        }
        catch (SecurityTokenExpiredException)
        {
            return ServiceResult<bool>.Failure("Token expiré");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token validation failed");
            return ServiceResult<bool>.Failure("Token invalide");
        }
    }

    /// <summary>
    /// 🔍 Extrait les claims d'un token (même si expiré)
    /// </summary>
    public ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "");

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = false, // ⚠️ Ne pas valider l'expiration
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return principal;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// ⏰ Vérifie si le token va expirer bientôt
    /// </summary>
    public bool IsTokenExpiringSoon(string token, int minutesBeforeExpiry = 15)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            if (jwtToken.ValidTo == DateTime.MinValue)
                return true;

            var expirationTime = jwtToken.ValidTo;
            var warningTime = DateTime.UtcNow.AddMinutes(minutesBeforeExpiry);

            return expirationTime <= warningTime;
        }
        catch
        {
            return true; // Si erreur, considérer comme expirant
        }
    }

    /// <summary>
    /// 🔄 FONCTION PRINCIPALE: Renouvelle le token si nécessaire
    /// </summary>
    public async Task<ServiceResult<string>> RenewTokenIfNeededAsync(string currentToken)
    {
        try
        {
            // 1. Vérifier si le token est encore valide
            var validationResult =  ValidateTokenAsync(currentToken);
            if (validationResult.IsSuccess)
            {
                // 2. Vérifier s'il va expirer bientôt
                if (!IsTokenExpiringSoon(currentToken))
                {
                    // Token encore bon, pas besoin de renouveler
                    return ServiceResult<string>.Success(currentToken, "Token encore valide");
                }
            }

            // 3. Token expiré ou va expirer, le renouveler
            var principal = GetPrincipalFromToken(currentToken);
            if (principal == null)
            {
                return ServiceResult<string>.Failure("Impossible de lire le token");
            }

            // 4. Récupérer l'utilisateur depuis les claims
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return ServiceResult<string>.Failure("Token invalide: utilisateur introuvable");
            }

            var user = await _userManager.FindByIdAsync(userIdClaim.Value);
            if (user == null || !user.IsActive)
            {
                return ServiceResult<string>.Failure("Utilisateur introuvable ou inactif");
            }

            // 5. Générer un nouveau token
            var newToken = await GenerateTokenAsync(user);

            _logger.LogInformation("Token renewed for user {UserId}", user.Id);
            return ServiceResult<string>.Success(newToken, "Token renouvelé avec succès");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error renewing token");
            return ServiceResult<string>.Failure("Erreur lors du renouvellement du token");
        }
    }
}