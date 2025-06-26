using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using P7CreateRestApi.DTO.BidDTO;
using P7CreateRestApi.DTO.Maping;
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
        [Authorize]
        [Route("validate")]
        public async Task<IActionResult> AddBid([FromBody] CreateBidDTO bidList)
        {

            // Validate the bidList object
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // If validation is successful, save the bidList to the database

            // Convert CreateBidDTO to BidList entity
            var bidListEntity = BidDTOMappings.ToEntity(bidList);
            var result = await _bidListService.CreateBidAsync(bidListEntity);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }
            // Return the created bidList
            return Ok(result.Data);
        }


        [HttpPost]
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
        public async Task<IActionResult> UpdateBid(int id, [FromBody] GetUpdateBidDTO bidList)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            if (id <= 0 || bidList == null)
            {
                return BadRequest("Invalid bid list data.");
            }
            // Convert GetUpdateBidDTO to BidList entity
            var bidListEntity = BidDTOMappings.ToEntity(bidList);
            var result = await _bidListService.UpdateBidAsync(id, bidListEntity);
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