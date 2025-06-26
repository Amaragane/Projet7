using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Services.Interfaces;
using P7CreateRestApi.DTO.CurveDTO;
using P7CreateRestApi.DTO.Maping;

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
        public async Task<IActionResult> Home()
        {
            var result = await _curvePointService.GetAllCurvePointsAsync();
            return Ok(result.Data);
        }

        [HttpPost]
        [Route("add")]
        public async  Task<IActionResult> AddCurvePoint([FromBody] CreateCurvePointDTO curvePoint)
        {
            var curvePointEntity = CurvePointDTOMappings.ToEntity(curvePoint);
            // Validate the curvePoint object
            if (curvePoint == null)
            {
                return BadRequest("CurvePoint data is required.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid CurvePoint data.");
            }
            // If validation is successful, save the curvePoint to the database
            var result =  await _curvePointService.CreateCurvePointAsync(curvePointEntity);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }
            // Return the created curvePoint
            return Ok(result.Data);
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
        public async Task<IActionResult> UpdateCurvePoint(int id, [FromBody] GetUpdateCurvePointDTO curvePoint)
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
            // Convert GetUpdateCurvePointDTO to CurvePoint entity
            var curvePointEntity = CurvePointDTOMappings.ToEntity(curvePoint);

            var result = await _curvePointService.UpdateCurvePointAsync(id, curvePointEntity);
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