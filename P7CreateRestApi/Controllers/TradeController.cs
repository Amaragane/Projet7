using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Services.Interfaces;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "User")]
    [Route("[controller]")]
    public class TradeController : ControllerBase
    {
        private readonly ITradeService _tradeService;
        private readonly ILogger<TradeController> _logger;
        public TradeController(ITradeService tradeService, ILogger<TradeController> logger)
        {
            _tradeService = tradeService;
            _logger = logger;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Home()
        {
            // TODO: find all Trade, add to model
            var result = await _tradeService.GetAllTradesAsync();
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to retrieve trades: {Errors}", result.Errors);
                return NotFound(result.Errors);
            }
            _logger.LogInformation("Successfully retrieved {Count} trades", result.Data!.Count());
            return Ok(result.Data);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddTrade([FromBody]Trade trade)
        {
            // validate data and call service to add Trade
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for trade creation");
                return BadRequest(ModelState);
            }
            var result = _tradeService.CreateTradeAsync(trade).Result;
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to create trade: {Errors}", result.Errors);
                return BadRequest(result.Errors);
            }
            _logger.LogInformation("Successfully created trade with ID {TradeId}", result.Data!.TradeId);
            return Ok(result.Data);
        }



        [HttpGet]
        [Route("update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get Trade by Id and to model then show to the form
            var result = _tradeService.GetTradeByIdAsync(id).Result;
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Errors);
        }

        [HttpPost]
        [Route("update/{id}")]
        public IActionResult UpdateTrade(int id, [FromBody] Trade trade)
        {
            // TODO: check required fields, if valid call service to update Trade and return Trade list
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _tradeService.UpdateTradeAsync(id, trade).Result;
            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }
            // Assuming the update was successful, return the updated trade
            return Ok(result.Data);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteTrade(int id)
        {
            // TODO: Find Trade by Id and delete the Trade, return to Trade list
            var result = _tradeService.DeleteTradeAsync(id).Result;
            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }
            // Assuming the delete was successful, return a success response

            return RedirectToAction("Home");
        }
    }
}