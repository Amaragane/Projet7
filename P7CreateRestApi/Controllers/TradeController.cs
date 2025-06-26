using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using P7CreateRestApi.Services.Interfaces;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradeController : ControllerBase
    {
        private readonly ITradeService _tradeService;
        public TradeController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Home()
        {
            // TODO: find all Trade, add to model
            var result = await _tradeService.GetAllTradesAsync();
            return Ok(result.Data);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddTrade([FromBody]Trade trade)
        {
            // validate data and call service to add Trade
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _tradeService.CreateTradeAsync(trade).Result;
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