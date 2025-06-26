using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.DTO.Maping;
using P7CreateRestApi.DTO.UsersDTO;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Home()
        {
            var result = await _userService.GetAllUsersAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddUser([FromBody]CreateUserDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.CreateUserAsync(user);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Errors);
        }


        [HttpGet]
        [Route("update/{id}")]
        public async Task<IActionResult> ShowUpdateForm(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user.IsSuccess)
            {
                return Ok(user.Data);
            }
            return NotFound(user.Errors);

        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO user)
        {
            // TODO: check required fields, if valid call service to update Trade and return Trade list
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.UpdateUserAsync(id, user);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }
            // Assuming the update was successful, return the updated user
            return Ok(result.Data);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.DeleteUserAsync(id);
            if (user.IsSuccess)
            {
                return RedirectToAction("Home");
            }
            return NotFound(user.Errors);



        }

        [HttpGet]
        [Route("/secure/article-details")]
        public async Task<ActionResult<List<User>>> GetAllUserArticles()
        {
            return Ok();
        }
    }
}