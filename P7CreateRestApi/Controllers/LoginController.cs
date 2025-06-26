using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using P7CreateRestApi.Models;
using P7CreateRestApi.DTO.AuthDTO;
using System.Security.Claims;
using P7CreateRestApi.DTO.CommonDTO;
using P7CreateRestApi.Services.Auth;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IAuthService authService, ILogger<LoginController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseDTO<object>.ErrorResult("Données invalides",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
            }

            var result = await _authService.LoginAsync(loginRequest);

            if (result.IsSuccess)
            {
                return Ok(ApiResponseDTO<AuthResponseDTO>.SuccessResult(result.Data!, "Connexion réussie"));
            }

            return Unauthorized(ApiResponseDTO<object>.ErrorResult(
                result.Errors?.FirstOrDefault() ?? "Connexion échouée"));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseDTO<object>.ErrorResult("Données invalides",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
            }

            var result = await _authService.RegisterAsync(registerRequest);

            if (result.IsSuccess)
            {
                return Ok(ApiResponseDTO<AuthResponseDTO>.SuccessResult(result.Data!, "Inscription réussie"));
            }

            return BadRequest(ApiResponseDTO<object>.ErrorResult(
                result.Errors?.FirstOrDefault() ?? "Inscription échouée"));
        }



        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseDTO<object>.ErrorResult("Données invalides",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponseDTO<object>.ErrorResult("Token invalide"));
            }

            var result = await _authService.ChangePasswordAsync(userId, changePasswordRequest);

            if (result.IsSuccess)
            {
                return Ok(ApiResponseDTO<bool>.SuccessResult(true, "Mot de passe modifié avec succès"));
            }

            return BadRequest(ApiResponseDTO<object>.ErrorResult(
                result.Errors?.FirstOrDefault() ?? "Erreur lors du changement de mot de passe"));
        }


        [HttpPost("validate-token")]
        [Authorize]
        public async Task<IActionResult> ValidateToken()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            var token = authHeader.StartsWith("Bearer ") ? authHeader.Substring(7) : authHeader;

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(ApiResponseDTO<object>.ErrorResult("Token manquant"));
            }

            var result = await _authService.ValidateTokenAsync(token);
            var userInfo = new
            {
                userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                email = User.FindFirst(ClaimTypes.Email)?.Value,
                username = User.FindFirst(ClaimTypes.Name)?.Value,
                roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList()
            };

            return Ok(ApiResponseDTO<object>.SuccessResult(new { valid = result.Data, user = userInfo }));
        }
    }
}