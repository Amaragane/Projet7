using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Services.Interfaces;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RuleNameController : ControllerBase
    {
        // TODO: Inject RuleName service
        private readonly IRuleNameService _ruleNameService;
        public RuleNameController(IRuleNameService ruleNameService)
        {
            _ruleNameService = ruleNameService;
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
                return NotFound(ruleName.Errors);
            }
            return Ok(ruleName.Data);

        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateRuleName(int id, [FromBody] RuleName rating)
        {
            // TODO: check required fields, if valid call service to update RuleName and return RuleName list
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingRuleName = await _ruleNameService.GetRuleByIdAsync(id);
            if (!existingRuleName.IsSuccess)
            {
                return NotFound(existingRuleName.Errors);
            }
            var result = await _ruleNameService.UpdateRuleAsync(id, rating);
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
                return NotFound(result.Errors);
            }
            return RedirectToAction("Home");
        }
    }
}