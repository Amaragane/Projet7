using Dot.Net.WebApi.Controllers.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories.Interfaces;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatingController : ControllerBase
    {
        // TODO: Inject Rating service
        private readonly IRatingRepository _repository;
        public RatingController(IRatingRepository repository)
        {
            _repository = repository;
        }


        [HttpGet]
        [Route("list")]
        public IActionResult Home()
        {
            // TODO: find all Rating, add to model
            return Ok();
        }

        [HttpGet]
        [Route("add")]
        public IActionResult AddRatingForm([FromBody]Rating rating)
        {
            return Ok();
        }

        private bool Validate(Rating rating)
        {
            // TODO: check data valid and save to db, after saving return Rating list
            return false;
        }

        [HttpGet]
        [Route("update/{id}")]
        public async Task<IActionResult> ShowUpdateForm(int id)
        {
            // TODO: get Rating by Id and to model then show to the form
            var rating = await _repository.GetByIdAsync(id);
            if (rating == null)
            {
                return NotFound();
            }
            return Ok(rating);
        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateRating(int id, [FromBody] Rating rating)
        {
            // TODO: check required fields, if valid call service to update Rating and return Rating list
            if (rating == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (rating.Id != id)
            {
                return BadRequest("Rating ID mismatch.");
            }
            // à finir de vérifier
            var existingRating = await _repository.GetByIdAsync(id);
            if (existingRating == null)
            {
                return NotFound();
            }
            // Update the existing rating with the new values

            await _repository.UpdateAsync(rating);
            

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            // TODO: Find Rating by Id and delete the Rating, return to Rating list
            var exists = await _repository.ExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }
            var deleted = await _repository.DeleteAsync(id);
            return Ok();
        }
    }
}