using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Services.Models;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Services.Interfaces;

namespace P7CreateRestApi.Services
{
    public class RuleNameService : IRuleNameService
    {
        private readonly IRuleNameRepository _ruleNameRepository;

        public RuleNameService(IRuleNameRepository ruleNameRepository)
        {
            _ruleNameRepository = ruleNameRepository;
        }

        public async Task<ServiceResult<IEnumerable<RuleName>>> GetAllRulesAsync()
        {
            try
            {
                var rules = await _ruleNameRepository.GetAllAsync();
                return ServiceResult<IEnumerable<RuleName>>.Success(rules);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<RuleName>>.Failure($"Erreur lors de la récupération des règles: {ex.Message}");
            }
        }

        public async Task<ServiceResult<RuleName>> GetRuleByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return ServiceResult<RuleName>.Failure("L'ID doit être supérieur à 0");

                var rule = await _ruleNameRepository.GetByIdAsync(id);
                if (rule == null)
                    return ServiceResult<RuleName>.Failure("Règle non trouvée");

                return ServiceResult<RuleName>.Success(rule);
            }
            catch (Exception ex)
            {
                return ServiceResult<RuleName>.Failure($"Erreur lors de la récupération de la règle: {ex.Message}");
            }
        }

        public async Task<ServiceResult<RuleName>> CreateRuleAsync(RuleName ruleName)
        {
            try
            {

                var createdRule = await _ruleNameRepository.CreateAsync(ruleName);
                return ServiceResult<RuleName>.Success(createdRule, "Règle créée avec succès");
            }
            catch (Exception ex)
            {
                return ServiceResult<RuleName>.Failure($"Erreur lors de la création de la règle: {ex.Message}");
            }
        }

        public async Task<ServiceResult<RuleName>> UpdateRuleAsync(int id, RuleName ruleName)
        {
            try
            {
                if (id <= 0)
                    return ServiceResult<RuleName>.Failure("L'ID doit être supérieur à 0");

                if (!await _ruleNameRepository.ExistsAsync(id))
                    return ServiceResult<RuleName>.Failure("Règle non trouvée");

                ruleName.Id = id;

                var updatedRule = await _ruleNameRepository.UpdateAsync(ruleName);
                return ServiceResult<RuleName>.Success(updatedRule, "Règle mise à jour avec succès");
            }
            catch (Exception ex)
            {
                return ServiceResult<RuleName>.Failure($"Erreur lors de la mise à jour de la règle: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> DeleteRuleAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return ServiceResult<bool>.Failure("L'ID doit être supérieur à 0");

                if (!await _ruleNameRepository.ExistsAsync(id))
                    return ServiceResult<bool>.Failure("Règle non trouvée");

                var deleted = await _ruleNameRepository.DeleteAsync(id);
                return ServiceResult<bool>.Success(deleted, "Règle supprimée avec succès");
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure($"Erreur lors de la suppression de la règle: {ex.Message}");
            }
        }

        public async Task<ServiceResult<RuleName>> GetRuleByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return ServiceResult<RuleName>.Failure("Le nom de la règle est requis");

                var allRules = await _ruleNameRepository.GetAllAsync();
                var rule = allRules.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                if (rule == null)
                    return ServiceResult<RuleName>.Failure("Règle non trouvée");

                return ServiceResult<RuleName>.Success(rule);
            }
            catch (Exception ex)
            {
                return ServiceResult<RuleName>.Failure($"Erreur lors de la recherche de la règle: {ex.Message}");
            }
        }



        
    }
}
