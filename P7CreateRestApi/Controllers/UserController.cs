using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.DTO.Maping;
using P7CreateRestApi.DTO.UsersDTO;
using Microsoft.AspNetCore.Authorization;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private ILogger _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult Home()
        {
            var result =  _userService.GetAllUsersAsync();
            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully retrieved {Count} users", result.Data!.Count());
                return Ok(result.Data);
            }
            _logger.LogError("Failed to retrieve users: {Errors}", result.Errors);
            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddUser([FromBody]CreateUserDTO user)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for user creation");
                return BadRequest(ModelState);
            }
            var result = await _userService.CreateUserAsync(user);
            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully created user with ID {UserId}", result.Data!.Id);
                return Ok(result.Data);
            }
            _logger.LogError("Failed to create user: {Errors}", result.Errors);
            return BadRequest(result.Errors);
        }


        [HttpGet]
        [Route("update/{id}")]
        public async Task<IActionResult> ShowUpdateForm(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user.IsSuccess)
            {
                _logger.LogInformation("Successfully retrieved user with ID {UserId}", id);
                return Ok(user.Data);
            }
            _logger.LogError("Failed to retrieve user with ID {UserId}: {Errors}", id, user.Errors);
            return NotFound(user.Errors);

        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO user)
        {
            // TODO: check required fields, if valid call service to update Trade and return Trade list
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for user update");
                return BadRequest(ModelState);
            }
            var result = await _userService.UpdateUserAsync(id, user);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to update user with ID {UserId}: {Errors}", id, result.Errors);
                return BadRequest(result.Errors);
            }
            // Assuming the update was successful, return the updated user
            _logger.LogInformation("Successfully updated user with ID {UserId}", id);
            return Ok(result.Data);
        }

        [HttpDelete]
        [Authorize(Roles ="Admin")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.DeleteUserAsync(id);
            if (user.IsSuccess)
            {
                _logger.LogInformation("Successfully deleted user with ID {UserId}", id);
                return RedirectToAction("Home");
            }
            _logger.LogError("Failed to delete user with ID {UserId}: {Errors}", id, user.Errors);
            return NotFound(user.Errors);



        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("/secure/article-details")]
        public ActionResult<List<User>> GetAllUserArticles()
        {
            return Ok();
        }
    }
}