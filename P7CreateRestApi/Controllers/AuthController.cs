﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.DTO.AuthDTO;
using P7CreateRestApi.DTO.CommonDTO;
using P7CreateRestApi.Services.Auth;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, IJwtService jwtService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(loginRequest);

            if (result.IsSuccess)
            {
                _logger.LogInformation("User {Username} logged in successfully", loginRequest.Email);
                return Ok(new
                {
                    success = true,
                    message = "Connexion réussie",
                    data = result.Data
                });
            }
            _logger.LogError("{Email} ou {Password}", loginRequest.Email,loginRequest.Password);
            return Unauthorized(new
            {
                success = false,
                message = result.Errors?.FirstOrDefault() ?? "Échec de la connexion"
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(registerRequest);

            if (result.IsSuccess)
            {
                _logger.LogInformation("The user {UserName} was succesfully registered", registerRequest.Fullname);
                return Ok(new
                {
                    success = true,
                    message = result.Data
                });
            }
            _logger.LogInformation("Registering failed");
            return BadRequest(new
            {
                success = false,
                message = result.Errors?.FirstOrDefault() ?? "Échec de l'inscription"
            });
        }

        [HttpPost("validate-token")]
        [Authorize]
        public async Task<IActionResult> ValidateToken()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { success = false, message = "Token manquant" });
            }

            var result = await _authService.ValidateTokenAsync(token);

            return Ok(new
            {
                success = result.IsSuccess,
                valid = result.Data,
                user = User.Identity?.Name
            });
        }

        [HttpGet("profile")]
        [Authorize]
        public IActionResult GetProfile()
        {
            return Ok(new
            {
                success = true,
                user = new
                {
                    id = User.FindFirst("sub")?.Value ?? User.FindFirst("nameidentifier")?.Value,
                    username = User.Identity?.Name,
                    role = User.FindFirst("role")?.Value,
                    fullname = User.FindFirst("fullname")?.Value
                }
            });
        }
        /// <summary>
        /// 🔄 Endpoint pour renouveler manuellement le token
        /// </summary>
        [HttpPost("renew-token")]
        [Authorize] // L'utilisateur doit être authentifié
        public async Task<ActionResult<ApiResponseDTO<string>>> RenewToken()
        {
            try
            {
                // Récupérer le token actuel
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return BadRequest(ApiResponseDTO<string>.ErrorResult("Token manquant"));
                }

                var currentToken = authHeader.Substring("Bearer ".Length).Trim();

                // Renouveler le token
                var result = await _jwtService.RenewTokenIfNeededAsync(currentToken);

                if (!result.IsSuccess)
                {
                    return BadRequest(ApiResponseDTO<string>.ErrorResult(result.Errors));
                }

                return Ok(ApiResponseDTO<string>.SuccessResult(result.Data!, result.Message!));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResult(ex.Message));
            }
        }

        /// <summary>
        /// ✅ Endpoint pour vérifier le statut du token
        /// </summary>
        [HttpGet("token-status")]
        [Authorize]
        public ActionResult<ApiResponseDTO<TokenStatusDTO>> GetTokenStatus()
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                var currentToken = authHeader?.Substring("Bearer ".Length).Trim();

                if (string.IsNullOrEmpty(currentToken))
                {
                    return BadRequest(ApiResponseDTO<TokenStatusDTO>.ErrorResult("Token manquant"));
                }

                var isValid =  _jwtService.ValidateTokenAsync(currentToken);
                var isExpiringSoon = _jwtService.IsTokenExpiringSoon(currentToken);

                var tokenStatus = new TokenStatusDTO
                {
                    IsValid = isValid.IsSuccess,
                    IsExpiringSoon = isExpiringSoon,
                    NeedsRenewal = isExpiringSoon
                };

                return Ok(ApiResponseDTO<TokenStatusDTO>.SuccessResult(tokenStatus));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<TokenStatusDTO>.ErrorResult(ex.Message));
            }
        }
    }
}