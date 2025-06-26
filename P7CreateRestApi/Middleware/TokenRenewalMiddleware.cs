using P7CreateRestApi.Services.Auth;

namespace P7CreateRestApi.Middleware
{
    public class TokenRenewalMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenRenewalMiddleware> _logger;

        // Supprimer IJwtService du constructeur
        public TokenRenewalMiddleware(
            RequestDelegate next,
            ILogger<TokenRenewalMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // Injecter IJwtService dans InvokeAsync
        public async Task InvokeAsync(HttpContext context, IJwtService jwtService)
        {
            // vérifier si on doit renouveler le token
            await TryRenewTokenAsync(context, jwtService);
            // Traiter la requête normalement
            await _next(context);

        }

        private async Task TryRenewTokenAsync(HttpContext context, IJwtService jwtService)
        {
            try
            {
                // Vérifier seulement pour les réponses réussies et authentifiées
                if (context.Response.StatusCode != 200 )  return;
                

                // Récupérer le token depuis l'en-tête Authorization
                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return;
                }

                var currentToken = authHeader.Substring("Bearer ".Length).Trim();
                var principal =  jwtService.ValidateTokenAsync(currentToken);
                if (!principal.IsSuccess) return; // Token invalide
                // Vérifier si le token va expirer bientôt
                if (!jwtService.IsTokenExpiringSoon(currentToken))
                {
                    return; // Token encore bon
                }

                // Renouveler le token
                var renewalResult =  await jwtService.RenewTokenIfNeededAsync(currentToken);
                if (renewalResult.IsSuccess && renewalResult.Data != currentToken)
                {
                    // Ajouter le nouveau token dans les headers de réponse
                    context.Response.Headers["X-New-Token"] = renewalResult.Data;
                    context.Response.Headers["X-Token-Renewed"] = "true";

                    _logger.LogInformation("Token automatically renewed for user");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in token renewal middleware");
                // Ne pas faire échouer la requête si le renouvellement échoue
            }
        }
    }
}
