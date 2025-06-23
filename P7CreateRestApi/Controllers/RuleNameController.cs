using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories.Interfaces;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RuleNameController : ControllerBase
    {
        // TODO: Inject RuleName service
        private readonly IRuleNameRepository _ruleNameRepository;
        public RuleNameController(IRuleNameRepository ruleNameRepository)
        {
            _ruleNameRepository = ruleNameRepository;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Home()
        {
            // TODO: find all RuleName, add to model
            var ruleNames = await _ruleNameRepository.GetAllAsync();
            return Ok(ruleNames);
        }

        [HttpGet]
        [Route("add")]
        public IActionResult AddRuleName([FromBody]RuleName trade)
        {
            return Ok();
        }

        [HttpGet]
        [Route("validate")]
        public async Task<IActionResult> Validate([FromBody]RuleName trade)
        {
            // TODO: check data valid and save to db, after saving return RuleName list
            if (trade == null)
            {
                return BadRequest("RuleName cannot be null.");
            }

            return Ok();
        }

        [HttpGet]
        [Route("update/{id}")]
        public async Task<IActionResult> ShowUpdateForm(int id)
        {
            // TODO: get RuleName by Id and to model then show to the form
            var ruleName = await _ruleNameRepository.GetByIdAsync(id);
            if (ruleName == null)
            {
                return NotFound($"RuleName with ID {id} not found.");
            }
            return Ok(ruleName);

        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateRuleName(int id, [FromBody] RuleName rating)
        {
            // TODO: check required fields, if valid call service to update RuleName and return RuleName list
            if (rating == null || string.IsNullOrEmpty(rating.Name) || string.IsNullOrEmpty(rating.Description))
            {
                return BadRequest("Invalid RuleName data.");
            }
            var existingRuleName = await _ruleNameRepository.GetByIdAsync(id);
            if (existingRuleName == null)
            {
                return NotFound($"RuleName with ID {id} not found.");
            }

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteRuleName(int id)
        {
            // TODO: Find RuleName by Id and delete the RuleName, return to Rule list
            return Ok();
        }
    }
}