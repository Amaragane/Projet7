using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    public class BidListController : ControllerBase
    {
        private readonly IBidListService _bidListService;
        private readonly ILogger<BidListController> _logger; 
        public BidListController(IBidListService service, ILogger<BidListController> logger)
        {
            _bidListService = service;
            _logger = logger;
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("add")]
        public async Task<IActionResult> AddBid([FromBody] CreateBidDTO bidList)
        {

            // Validate the bidList object
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid bidlist data provided by the User {ID}", User.Identity);
                return BadRequest(ModelState);
            }
            // If validation is successful, save the bidList to the database

            // Convert CreateBidDTO to BidList entity
            var bidListEntity = BidDTOMappings.ToEntity(bidList);
            var result = await _bidListService.CreateBidAsync(bidListEntity);
            if (!result.IsSuccess)
            {
                _logger.LogError("Error creating bidList by the User {ID}: {Errors}", User.Identity, result.Errors);
                return BadRequest(result.Errors);
            }
            // Return the created bidList
            _logger.LogInformation("Bid created successfully by the User {ID} with ID: {Id}",User.Identity, result.Data!.BidListId);
            return Ok(result.Data);
        }


        [HttpGet]
        [Authorize(Roles = "User")]
        [Route("get/{id}")]
        public async Task<IActionResult> GetBid(int id)
        {
            // TODO: get bid by Id and to model then show to the form
            if (id <= 0)
            {
                _logger.LogError("Invalid bid ID provided: {Id}", id);
                return BadRequest("Invalid bid ID.");
            }
            var result = await _bidListService.GetBidByIdAsync(id);
            if (!result.IsSuccess)
            {
                _logger.LogError("Error retrieving bid with ID {Id}: {Errors}", id, result.Errors);
                return NotFound(result.Errors);
            }
            _logger.LogInformation("Bid with ID {Id} retrieved successfully by the User {ID}", id, User.Identity);
            return Ok(result.Data);

        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateBid(int id, [FromBody] GetUpdateBidDTO bidList)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            if (id <= 0 || bidList == null)
            {
                _logger.LogError("Invalid bid ID or bid list data provided: ID = {Id}, Data = {Data}", id, bidList);
                return BadRequest("Invalid bid list data.");
            }
            // Convert GetUpdateBidDTO to BidList entity
            var bidListEntity = BidDTOMappings.ToEntity(bidList);
            var result = await _bidListService.UpdateBidAsync(id, bidListEntity);
            if (!result.IsSuccess)
            {
                _logger.LogError("Error updating bid with ID {Id}: {Errors}", id, result.Errors);
                return BadRequest(result.Errors);
            }
            _logger.LogInformation("Bid with ID {Id} updated successfully by the User {ID}", id, User.Identity);
            return Ok(result.Data);
        }

        [HttpDelete]

        [Authorize(Roles = "User")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteBid(int id)
        {
            // TODO: Find bid by Id and delete the bid
            if (id <= 0)
            {
                _logger.LogError("Invalid bid ID provided: {Id}", id);
                return BadRequest("Invalid bid ID.");
            }
            var result = await _bidListService.DeleteBidAsync(id);
            if (!result.IsSuccess)
            {
                _logger.LogError("Error deleting bid with ID {Id}: {Errors}", id, result.Errors);
                return BadRequest(result.Errors);
            }
            _logger.LogInformation("Bid with ID {Id} deleted successfully by the User {ID}", id, User.Identity);
            return Ok(new { Message = "Bid deleted successfully." });

        }
    }
}