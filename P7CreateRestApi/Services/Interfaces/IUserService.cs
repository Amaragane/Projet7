using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services.Models;
using P7CreateRestApi.DTO.UsersDTO;

namespace P7CreateRestApi.Services.Interfaces
{
    public interface IUserService
    {
        ServiceResult<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<ServiceResult<UserDTO>> GetUserByIdAsync(int id);
        Task<ServiceResult<UserDTO>> CreateUserAsync(CreateUserDTO createUser);
        Task<ServiceResult<UserDTO>> UpdateUserAsync(int id, UpdateUserDTO updateUser);
        Task<ServiceResult<bool>> DeleteUserAsync(int id);
    }
}
