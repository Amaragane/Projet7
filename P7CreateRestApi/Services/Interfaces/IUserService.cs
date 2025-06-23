using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services.Models;

namespace P7CreateRestApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<IEnumerable<User>>> GetAllUsersAsync();
        Task<ServiceResult<User>> GetUserByIdAsync(int id);
        Task<ServiceResult<User>> GetUserByUsernameAsync(string username);
        Task<ServiceResult<User>> CreateUserAsync(User user);
        Task<ServiceResult<User>> UpdateUserAsync(int id, User user);
        Task<ServiceResult<bool>> DeleteUserAsync(int id);
    }
}
