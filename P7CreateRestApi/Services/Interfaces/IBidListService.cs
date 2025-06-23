using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services.Models;
namespace P7CreateRestApi.Services.Interfaces
{
    public interface IBidListService
    {
        Task<ServiceResult<IEnumerable<BidList>>> GetAllBidsAsync();
        Task<ServiceResult<BidList>> GetBidByIdAsync(int id);
        Task<ServiceResult<BidList>> CreateBidAsync(BidList bidList);
        Task<ServiceResult<BidList>> UpdateBidAsync(int id, BidList bidList);
        Task<ServiceResult<bool>> DeleteBidAsync(int id);
    }
}
