using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Repositories.Interfaces
{
    public interface IBidListRepository
    {
        Task<IEnumerable<BidList>> GetAllAsync();
        Task<BidList?> GetByIdAsync(int id);
        Task<BidList> CreateAsync(BidList bidList);
        Task<BidList> UpdateAsync(BidList bidList);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
