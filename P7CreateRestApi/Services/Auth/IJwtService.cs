using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services.Models;
using P7CreateRestApi.DTO.AuthDTO;
using System.Security.Claims;

namespace P7CreateRestApi.Services.Auth
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(User user);
        Task<ServiceResult<bool>> ValidateTokenAsync(string token);
        ClaimsPrincipal? GetPrincipalFromToken(string token);
        bool IsTokenExpiringSoon(string token, int minutesBeforeExpiry = 15);
        Task<ServiceResult<string>> RenewTokenIfNeededAsync(string currentToken);
    }
}
