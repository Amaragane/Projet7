using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.DTO.CurveDTO;
using P7CreateRestApi.DTO.Maping;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Services.Interfaces;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    public class CurveController : ControllerBase
    {
        private readonly ICurvePointService _curvePointService;
        private readonly ILogger<CurveController> _logger;
        public CurveController(ICurvePointService service, ILogger<CurveController> logger)
        {
            _curvePointService = service;
            _logger = logger;
        }
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Home()
        {
            var result = await _curvePointService.GetAllCurvePointsAsync();
            if (!result.IsSuccess)
            {
                _logger.LogError("Error retrieving CurvePoints: {Errors}", result.Errors);
                return BadRequest(result.Errors);
            }
            _logger.LogInformation("CurvePoints retrieved successfully.");
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
                _logger.LogError("CurvePoint data is null.");
                return BadRequest("CurvePoint data is required.");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid CurvePoint data: {Errors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest("Invalid CurvePoint data.");
            }
            // If validation is successful, save the curvePoint to the database
            var result =  await _curvePointService.CreateCurvePointAsync(curvePointEntity);
            if (!result.IsSuccess)
            {
                _logger.LogError("Error creating CurvePoint: {Errors}", result.Errors);
                return BadRequest(result.Errors);
            }
            // Return the created curvePoint
            _logger.LogInformation("CurvePoint created successfully with ID {Id}", result.Data!.Id);
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
                _logger.LogWarning("CurvePoint with ID {Id} not found.", id);
                return NotFound($"CurvePoint with ID {id} not found.");
            }
            _logger.LogInformation("CurvePoint with ID {Id} retrieved successfully.", id);
            return Ok(curvePoint);

        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateCurvePoint(int id, [FromBody] GetUpdateCurvePointDTO curvePoint)
        {
            // TODO: check required fields, if valid call service to update Curve and return Curve list
            if (curvePoint == null)
            {
                _logger.LogError("CurvePoint data is null.");
                return BadRequest("CurvePoint data is required.");
            }
            if (id <= 0)
            {
                _logger.LogError("Invalid CurvePoint ID: {Id}", id);
                return BadRequest("Invalid CurvePoint ID.");
            }
            // Convert GetUpdateCurvePointDTO to CurvePoint entity
            var curvePointEntity = CurvePointDTOMappings.ToEntity(curvePoint);

            var result = await _curvePointService.UpdateCurvePointAsync(id, curvePointEntity);
            if (!result.IsSuccess)
            {
                _logger.LogError("Error updating CurvePoint with ID {Id}: {Errors}", id, result.Errors);
                return BadRequest(result.Errors);
            }
            _logger.LogInformation("CurvePoint with ID {Id} updated successfully.", id);
            return Ok(result.Data);

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCurve(int id)
        {
            // TODO: Find Curve by Id and delete the Curve, return to Curve list
            if (id <= 0)
            {
                _logger.LogError("Invalid CurvePoint ID: {Id}", id);
                return BadRequest("Invalid CurvePoint ID.");
            }
            var result = await _curvePointService.DeleteCurvePointAsync(id);
            if (!result.IsSuccess)
            {
                _logger.LogError("Error deleting CurvePoint with ID {Id}: {Errors}", id, result.Errors);
                return BadRequest(result.Errors);
            }
            _logger.LogInformation("CurvePoint with ID {Id} deleted successfully.", id);
            return Ok(new { Message = "CurvePoint deleted successfully." });

        }
    }
}