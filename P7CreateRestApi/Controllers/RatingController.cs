using Dot.Net.WebApi.Controllers.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Services.Interfaces;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    public class RatingController : ControllerBase
    {
        // TODO: Inject Rating service
        private readonly IRatingService _services;
        private readonly ILogger _logger;
        public RatingController(IRatingService services, ILogger<RatingController> logger)
        {
            _services = services;
            _logger = logger;
        }


        [HttpGet]
        [Authorize(Roles = "User")]
        [Route("list")]
        public IActionResult Home()
        {
            // TODO: find all Rating, add to model
            var result = _services.GetAllRatingsAsync().Result;
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to retrieve ratings: {Errors}", result.Errors);
                return NotFound(result.Errors);
            }
            _logger.LogInformation("Successfully retrieved {Count} ratings", result.Data!.Count());

            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("add")]
        public IActionResult AddRatingForm([FromBody]Rating rating)
        {
            if (ModelState.IsValid)
            {
                var result = _services.CreateRatingAsync(rating).Result;
                if (!result.IsSuccess)
                {
                    _logger.LogError("Failed to create rating: {Errors}", result.Errors);
                    return BadRequest(result.Errors);
                }
                _logger.LogInformation("Successfully created rating with ID {RatingId}", result.Data!.Id);


                return Ok(result.Data);
            }
            _logger.LogWarning("Invalid model state for rating creation");
            return BadRequest(ModelState);
        }


        [HttpGet]
        [Authorize(Roles = "User")]
        [Route("update/{id}")]
        public async Task<IActionResult> ShowUpdateForm(int id)
        {
            // TODO: get Rating by Id and to model then show to the form
            var rating = await _services.GetRatingByIdAsync(id);
            if (!rating.IsSuccess)
            {
                _logger.LogInformation("Successfully retrieved rating with ID {RatingId} for update", id);
                return NotFound(rating.Errors);
            }
            if (rating.Data == null)
            {
                _logger.LogWarning("Rating with ID {RatingId} not found", id);
                return NotFound("Rating not found");
            }
            _logger.LogInformation("Rating with ID {RatingId} retrieved successfully for update", id);
            return Ok(rating);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateRating(int id, [FromBody] Rating rating)
        {
            // TODO: check required fields, if valid call service to update Rating and return Rating list
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for rating update with ID {RatingId}", id);
                return BadRequest(ModelState);
            }
            var existingRating =await _services.UpdateRatingAsync(id,rating);
            if (!existingRating.IsSuccess)
            {
                _logger.LogError("Failed to update rating with ID {RatingId}: {Errors}", id, existingRating.Errors);
                return NotFound(existingRating.Errors);
            }

            // Update the existing rating with the new 
            _logger.LogInformation("Successfully updated rating with ID {RatingId}", id);
            return RedirectToAction("Home");
        }

        [HttpDelete]
        [Authorize(Roles = "User")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            // TODO: Find Rating by Id and delete the Rating, return to Rating list

            var deleted = await _services.DeleteRatingAsync(id);
            if (!deleted.IsSuccess)
            {
                _logger.LogError("Failed to delete rating with ID {RatingId}: {Errors}", id, deleted.Errors);
                return NotFound(deleted.Errors);
            }
            _logger.LogInformation("Successfully deleted rating with ID {RatingId}", id);
            return RedirectToAction("Home");
        }
    }
}