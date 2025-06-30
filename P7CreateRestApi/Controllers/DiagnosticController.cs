using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Services.Auth;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/diag")]
public class DiagnosticController : ControllerBase
{
    [HttpGet("auth")]
    [Authorize]
    public IActionResult CheckAuth()
    {
        return Ok(new
        {
            IsAuthenticated = User.Identity?.IsAuthenticated,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
    [Authorize(Roles="Admin")]
    [HttpGet("claims")]
    public IActionResult GetClaims()
    {

        var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

        return Ok(new
        {
            IsAuthenticated = isAuthenticated,
            Claims = claims
        });
    }
    [HttpGet("manual-validation")]
    public IActionResult ManualValidation([FromServices] IJwtService jwtService)
    {
        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader)) return Unauthorized();
        var isAuthenticated = User.Identity?.IsAuthenticated ?? false;

        var token = authHeader.Replace("Bearer ", "");
        var result = jwtService.ValidateTokenAsync(token).Data;

        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

        return Ok(new
        {
            Resultat = result,
            Token = token,
            IsAuthenticated = isAuthenticated,
            Claims = claims
        });
    }
}
