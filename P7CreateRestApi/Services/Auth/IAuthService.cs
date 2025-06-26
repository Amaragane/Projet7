using Dot.Net.WebApi.Services.Models;
using P7CreateRestApi.DTO.AuthDTO;
using P7CreateRestApi.DTO.UsersDTO;

namespace P7CreateRestApi.Services.Auth
{
    public interface IAuthService
    {
        Task<ServiceResult<AuthResponseDTO>> LoginAsync(LoginRequestDTO loginRequest);
        Task<ServiceResult<AuthResponseDTO>> RegisterAsync(RegisterRequestDTO registerRequest);
        Task<ServiceResult<bool>> ChangePasswordAsync(string userId, ChangePasswordDTO changePasswordRequest);
        Task<ServiceResult<bool>> ValidateTokenAsync(string token);
    }
}