using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Services.Models;

namespace P7CreateRestApi.Services.Interfaces
{
    public interface IRatingService
    {
        Task<ServiceResult<IEnumerable<Rating>>> GetAllRatingsAsync();
        Task<ServiceResult<Rating>> GetRatingByIdAsync(int id);
        Task<ServiceResult<Rating>> CreateRatingAsync(Rating rating);
        Task<ServiceResult<Rating>> UpdateRatingAsync(int id, Rating rating);
        Task<ServiceResult<bool>> DeleteRatingAsync(int id);
    }
}
