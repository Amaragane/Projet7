using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories.Interfaces;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurveController : ControllerBase
    {
        private readonly ICurvePointRepository _repository;
        public CurveController(ICurvePointRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult Home()
        {
            return Ok();
        }

        [HttpGet]
        [Route("add")]
        public IActionResult AddCurvePoint([FromBody]CurvePoint curvePoint)
        {
            return Ok();
        }

        [HttpGet]
        [Route("validate")]
        public async Task<IActionResult> Validate([FromBody]CurvePoint curvePoint)
        {
            if (curvePoint == null)
            {
                return BadRequest("CurvePoint data is required.");
            }


            return Ok(curvePoint);
            // TODO: check data valid and save to db, after saving return bid list
        }

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get CurvePoint by Id and to model then show to the form
            return Ok();
        }

        [HttpPost]
        [Route("update/{id}")]
        public IActionResult UpdateCurvePoint(int id, [FromBody] CurvePoint curvePoint)
        {
            // TODO: check required fields, if valid call service to update Curve and return Curve list
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteBid(int id)
        {
            // TODO: Find Curve by Id and delete the Curve, return to Curve list
            return Ok();
        }
    }
}