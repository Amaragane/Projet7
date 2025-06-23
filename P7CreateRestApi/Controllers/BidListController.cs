using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Services.Interfaces;
using System.Threading.Tasks;
namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BidListController : ControllerBase
    {
        private readonly IBidListService _bidListService;
        public BidListController(IBidListService service)
        {
            _bidListService = service;
        }
        [HttpPost]
        [Route("validate")]
        public async Task<IActionResult> AddBid([FromBody] BidList bidList)
        {

            // Validate the bidList object
            var validationResult = Validate(bidList);
            if (!validationResult)
            {
                return BadRequest(validationResult);
            }
            // If validation is successful, save the bidList to the database
            var result = await _bidListService.CreateBidAsync(bidList);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }
            // Return the created bidList
            return Ok(result.Data);
        }
        private bool Validate(BidList bidList)
        {
            // TODO: check data valid and save to db, after saving return bid list
            if (bidList == null || string.IsNullOrEmpty(bidList.Account) || bidList.BidQuantity <= 0 || string.IsNullOrEmpty(bidList.BidType))
            {
                return false; // Invalid data
            }
            return true; // Valid data

        }

        [HttpGet]
        [Route("update/{id}")]
        public async Task<IActionResult> ShowUpdateForm(int id)
        {
            // TODO: get bid by Id and to model then show to the form
            if (id <= 0)
            {
                return BadRequest("Invalid bid ID.");
            }
            var result = await _bidListService.GetBidByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.Errors);
            }
            return Ok(result.Data);

        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateBid(int id, [FromBody] BidList bidList)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            if (id <= 0 || bidList == null)
            {
                return BadRequest("Invalid bid list data.");
            }
            var result = await _bidListService.UpdateBidAsync(id, bidList);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Data);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteBid(int id)
        {
            // TODO: Find bid by Id and delete the bid
            if (id <= 0)
            {
                return BadRequest("Invalid bid ID.");
            }
            var result = await _bidListService.DeleteBidAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }
            return Ok(new { Message = "Bid deleted successfully." });

        }
    }
}