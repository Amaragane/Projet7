using Dot.Net.WebApi.Controllers.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Services.Interfaces;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatingController : ControllerBase
    {
        // TODO: Inject Rating service
        private readonly IRatingService _services;
        public RatingController(IRatingService services)
        {
            _services = services;
        }


        [HttpGet]
        [Route("list")]
        public IActionResult Home()
        {
            // TODO: find all Rating, add to model
            var result = _services.GetAllRatingsAsync().Result;

            return Ok(result.Data);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddRatingForm([FromBody]Rating rating)
        {
            if (ModelState.IsValid)
            {
                var result = _services.CreateRatingAsync(rating).Result;
                return Ok(result.Data);
            }
            return BadRequest(ModelState);
        }


        [HttpGet]
        [Route("update/{id}")]
        public async Task<IActionResult> ShowUpdateForm(int id)
        {
            // TODO: get Rating by Id and to model then show to the form
            var rating = await _services.GetRatingByIdAsync(id);
            if (rating.IsSuccess)
            {
                return NotFound(rating.Errors);
            }
            return Ok(rating);
        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateRating(int id, [FromBody] Rating rating)
        {
            // TODO: check required fields, if valid call service to update Rating and return Rating list
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingRating =await _services.UpdateRatingAsync(id,rating);
            if (!existingRating.IsSuccess)
            {
                return NotFound(existingRating.Errors);
            }

            // Update the existing rating with the new values



            return RedirectToAction("Home");
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            // TODO: Find Rating by Id and delete the Rating, return to Rating list

            var deleted = await _services.DeleteRatingAsync(id);
            if (!deleted.IsSuccess)
            {
                return NotFound(deleted.Errors);
            }
            return RedirectToAction("Home");
        }
    }
}