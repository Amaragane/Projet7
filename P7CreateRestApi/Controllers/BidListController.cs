using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories.Interfaces;
using System.Threading.Tasks;
namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BidListController : ControllerBase
    {
        private readonly IBidListRepository _repository;
        public BidListController(IBidListRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        [Route("validate")]
        public async Task<IActionResult> Validate([FromBody] BidList bidList)
        {
            // TODO: check data valid and save to db, after saving return bid list
            if (bidList == null)
            {
                return BadRequest("BidList data is required.");
            }
            // Here you would typically validate the bidList object
            if(string.IsNullOrEmpty(bidList.Account) || string.IsNullOrEmpty(bidList.BidType))
            {
                return BadRequest("Invalid bid list data.");
            }
            await _repository.CreateAsync(bidList); // Save to database

            return Ok(bidList);
        }

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            return Ok();
        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateBid(int id, [FromBody] BidList bidList)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            if (bidList == null || !await _repository.ExistsAsync(id))
            {
                return NotFound();
            }
            bidList.BidListId = id; // Ensure the ID is set correctly
            var updatedBid = await _repository.UpdateAsync(bidList);
            return Ok(updatedBid);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteBid(int id)
        {
            if (!await _repository.ExistsAsync(id))
            {
                return NotFound();
            }
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted)
            {
                return BadRequest("Failed to delete the bid.");
            }
            return Ok();
        }
    }
}