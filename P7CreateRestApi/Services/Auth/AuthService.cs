using Microsoft.AspNetCore.Identity;
using P7CreateRestApi.DTO.AuthDTO;
using P7CreateRestApi.Models;
using Dot.Net.WebApi.Services.Models;
using Dot.Net.WebApi.Domain;
using P7CreateRestApi.DTO.UsersDTO;
using P7CreateRestApi.DTO.Maping;

namespace P7CreateRestApi.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtService jwtService,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<ServiceResult<AuthResponseDTO>> LoginAsync(LoginRequestDTO loginRequest)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginRequest.Email);
                if (user == null)
                {
                    return ServiceResult<AuthResponseDTO>.Failure("Email ou mot de passe incorrect");
                }

                // Vérification du mot de passe avec UserManager (sécurisé)
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
                if (!result.Succeeded)
                {
                    return ServiceResult<AuthResponseDTO>.Failure("Email ou mot de passe incorrect");
                }

                // Mettre à jour la dernière connexion
                await _userManager.UpdateAsync(user);

                // Générer le token JWT
                var jwtToken = await _jwtService.GenerateTokenAsync(user);

                var response = new AuthResponseDTO
                {
                    Token = jwtToken,
                    TokenType = "Bearer",
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                    User = new UserDTO
                    {
                        Id = user.Id,
                        Email = user.Email ?? "",
                        Username = user.UserName ?? "",
                        Fullname = user.Fullname,
                        PhoneNumber = user.PhoneNumber,
                        Roles = user.Roles, // Votre champ Role simple

                        EmailConfirmed = user.EmailConfirmed
                    }
                };

                return ServiceResult<AuthResponseDTO>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for {Email}", loginRequest.Email);
                return ServiceResult<AuthResponseDTO>.Failure("Erreur lors de la connexion");
            }
        }

        public async Task<ServiceResult<AuthResponseDTO>> RegisterAsync(RegisterRequestDTO registerRequest)
        {
            try
            {
                // Vérifications d'unicité
                var existingUserByEmail = await _userManager.FindByEmailAsync(registerRequest.Email);
                if (existingUserByEmail != null)
                {
                    return ServiceResult<AuthResponseDTO>.Failure("Un utilisateur avec cet email existe déjà");
                }

                var existingUserByUsername = await _userManager.FindByNameAsync(registerRequest.UserName);
                if (existingUserByUsername != null)
                {
                    return ServiceResult<AuthResponseDTO>.Failure("Ce nom d'utilisateur est déjà pris");
                }

                // Création de l'utilisateur
                var newUser = new User
                {
                    UserName = registerRequest.UserName,
                    Email = registerRequest.Email,
                    Fullname = registerRequest.Fullname,
                    PhoneNumber = registerRequest.PhoneNumber,
                    Roles = registerRequest.Roles,
                    EmailConfirmed = true,
                    Password = ""

                };
                // UserManager gère le hachage du mot de passe automatiquement
                var result = await _userManager.CreateAsync(newUser, registerRequest.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return ServiceResult<AuthResponseDTO>.Failure(errors);
                }
                if (newUser.Roles != null && newUser.Roles.Any())
                {

                    result = await _userManager.AddToRolesAsync(newUser, newUser.Roles);
                }
                _logger?.LogInformation("User created successfully: {Email}", registerRequest.Email);
                var userDTO = UserDTOMappings.ToUserDTO(newUser);

                // Générer le token JWT
                var jwtToken = await _jwtService.GenerateTokenAsync(newUser);

                var response = new AuthResponseDTO
                {
                    Token = jwtToken,
                    TokenType = "Bearer",
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                    User = new UserDTO
                    {
                        Id = newUser.Id,
                        Email = newUser.Email ?? "",
                        Username = newUser.UserName ?? "",
                        Fullname = newUser.Fullname,
                        PhoneNumber = newUser.PhoneNumber,
                        Roles = newUser.Roles!,

                        EmailConfirmed = newUser.EmailConfirmed
                    }
                };

                return ServiceResult<AuthResponseDTO>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for {Email}", registerRequest.Email);
                return ServiceResult<AuthResponseDTO>.Failure("Erreur lors de l'inscription");
            }
        }

        public async Task<ServiceResult<bool>> ChangePasswordAsync(string userId, ChangePasswordDTO changePasswordRequest)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult<bool>.Failure("Utilisateur non trouvé");
                }

                // UserManager gère la validation et le hachage
                var result = await _userManager.ChangePasswordAsync(user,
                    changePasswordRequest.CurrentPassword,
                    changePasswordRequest.NewPassword);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return ServiceResult<bool>.Failure(errors);
                }

                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user {UserId}", userId);
                return ServiceResult<bool>.Failure("Erreur lors du changement de mot de passe");
            }
        }
        public async Task<ServiceResult<bool>> ValidateTokenAsync(string token)
        {
            return  _jwtService.ValidateTokenAsync(token);
        }

        public async Task<ServiceResult<AuthResponseDTO>> RefreshTokenAsync(string refreshToken)
        {
            // Implémentation du refresh token

            throw new NotImplementedException();
        }

        public async Task<ServiceResult<bool>> ResetPasswordAsync(ResetPasswordDTO resetPasswordRequest)
        {
            // Implémentation reset password
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<bool>> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordRequest)
        {
            // Implémentation forgot password
            throw new NotImplementedException();
        }

        // Autres méthodes simples...
    }
}