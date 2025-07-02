using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Services.Interfaces;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    public class RuleNameController : ControllerBase
    {
        // TODO: Inject RuleName service
        private readonly IRuleNameService _ruleNameService;
        private readonly ILogger<RuleNameController> _logger;
        public RuleNameController(IRuleNameService ruleNameService, ILogger<RuleNameController> logger)
        {
            _ruleNameService = ruleNameService;
            _logger = logger;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Home()
        {
            // TODO: find all RuleName, add to model
            var ruleNames = await _ruleNameService.GetAllRulesAsync();

            return Ok(ruleNames.Data);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddRuleName([FromBody]RuleName trade)
        {
            if (!ModelState.IsValid)
            { 

                return BadRequest(ModelState);
            }
                var result = await _ruleNameService.CreateRuleAsync(trade);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to create rule: {Errors}", result.Errors);
                return BadRequest(result.Errors);
            }
            _logger.LogInformation("Successfully created rule with ID {RuleId}", result.Data!.Id);
            return Ok(result.Data);
        }


        [HttpGet]
        [Route("update/{id}")]
        public async Task<IActionResult> ShowUpdateForm(int id)
        {
            // TODO: get RuleName by Id and to model then show to the form
            var ruleName = await _ruleNameService.GetRuleByIdAsync(id);
            if (!ruleName.IsSuccess)
            {
                _logger.LogError("Failed to retrieve rule with ID {RuleId}: {Errors}", id, ruleName.Errors);
                return NotFound(ruleName.Errors);
            }
            _logger.LogInformation("Successfully retrieved rule with ID {RuleId}", id);
            return Ok(ruleName.Data);

        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateRuleName(int id, [FromBody] RuleName rating)
        {
            // TODO: check required fields, if valid call service to update RuleName and return RuleName list
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for rule update with ID {RuleId}", id);
                return BadRequest(ModelState);
            }
            if (id <= 0)
            {
                _logger.LogError("Invalid rule ID: {RuleId}", id);
                return BadRequest("Invalid rule ID");
            }

            var result = await _ruleNameService.UpdateRuleAsync(id, rating);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to update rule with ID {RuleId}: {Errors}", id, result.Errors);
                return BadRequest(result.Errors);
            }
            _logger.LogInformation("Successfully updated rule with ID {RuleId}", id);
            return Ok(result.Data);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRuleName(int id)
        {
            // TODO: Find RuleName by Id and delete the RuleName, return to Rule list
            var result = await _ruleNameService.DeleteRuleAsync(id);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to delete rule with ID {RuleId}: {Errors}", id, result.Errors);
                return NotFound(result.Errors);
            }
            _logger.LogInformation("Successfully deleted rule with ID {RuleId}", id);
            return RedirectToAction("Home");
        }
        

    }
}