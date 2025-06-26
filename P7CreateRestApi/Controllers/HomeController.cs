using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet]
        [Route("Admin")]
        public IActionResult Admin()
        {
            return Ok();
        }
    }
}