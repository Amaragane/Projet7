using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services.Models;

namespace P7CreateRestApi.Services.Interfaces
{
    public interface ITradeService
    {
        Task<ServiceResult<IEnumerable<Trade>>> GetAllTradesAsync();
        Task<ServiceResult<Trade>> GetTradeByIdAsync(int id);
        Task<ServiceResult<Trade>> CreateTradeAsync(Trade trade);
        Task<ServiceResult<Trade>> UpdateTradeAsync(int id, Trade trade);
        Task<ServiceResult<bool>> DeleteTradeAsync(int id);
    }
}
