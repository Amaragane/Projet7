using Dot.Net.WebApi.Controllers;

namespace P7CreateRestApi.Repositories.Interfaces
{
    public interface IRuleNameRepository
    {
        Task<IEnumerable<RuleName>> GetAllAsync();
        Task<RuleName?> GetByIdAsync(int id);
        Task<RuleName> CreateAsync(RuleName ruleName);
        Task<RuleName> UpdateAsync(RuleName ruleName);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
