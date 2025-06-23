using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Services.Models;

namespace P7CreateRestApi.Services.Interfaces
{
    public interface IRuleNameService
    {
        Task<ServiceResult<IEnumerable<RuleName>>> GetAllRulesAsync();
        Task<ServiceResult<RuleName>> GetRuleByIdAsync(int id);
        Task<ServiceResult<RuleName>> CreateRuleAsync(RuleName ruleName);
        Task<ServiceResult<RuleName>> UpdateRuleAsync(int id, RuleName ruleName);
        Task<ServiceResult<bool>> DeleteRuleAsync(int id);
    }
}
