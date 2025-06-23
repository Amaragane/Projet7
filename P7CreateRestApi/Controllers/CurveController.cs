using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Services.Interfaces;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurveController : ControllerBase
    {
        private readonly ICurvePointService _curvePointService;
        public CurveController(ICurvePointService service)
        {
            _curvePointService = service;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult Home()
        {
            return Ok();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddCurvePoint([FromBody]CurvePoint curvePoint)
        {
            return Ok();
        }

        private bool Validate(CurvePoint curvePoint)
        {
            if (curvePoint == null)
            {
                return false;
            }


            return true;
            // TODO: check data valid and save to db, after saving return curvepoint 
        }

        [HttpGet]
        [Route("update/{id}")]
        public async Task<IActionResult>ShowUpdateForm(int id)
        {
            // TODO: get CurvePoint by Id and to model then show to the form
            var curvePoint = await _curvePointService.GetCurvePointByIdAsync(id);
            if (curvePoint == null)
            {
                return NotFound($"CurvePoint with ID {id} not found.");
            }

            return Ok(curvePoint);

        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateCurvePoint(int id, [FromBody] CurvePoint curvePoint)
        {
            // TODO: check required fields, if valid call service to update Curve and return Curve list
            if (curvePoint == null)
            {
                return BadRequest("CurvePoint data is required.");
            }
            if (id <= 0)
            {
                return BadRequest("Invalid CurvePoint ID.");
            }
            var result = await _curvePointService.UpdateCurvePointAsync(id, curvePoint);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Data);

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCurve(int id)
        {
            // TODO: Find Curve by Id and delete the Curve, return to Curve list
            if (id <= 0)
            {
                return BadRequest("Invalid CurvePoint ID.");
            }
            var result = await _curvePointService.DeleteCurvePointAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }
            return Ok(new { Message = "CurvePoint deleted successfully." });

        }
    }
}