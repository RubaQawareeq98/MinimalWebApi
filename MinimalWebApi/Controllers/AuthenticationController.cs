using Microsoft.AspNetCore.Mvc;
using MinimalWebApi.Models;
using MinimalWebApi.Services;

namespace MinimalWebApi.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController(IJwtTokenGeneratorService jwtTokenGeneratorService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginRequestBody request)
    {
        var accessToken = await jwtTokenGeneratorService.GenerateToken(request.Username, request.Password);
        if (accessToken is null)
        {
            return Unauthorized();
        }
        return Ok(new
        {
            accessToken
        });
    }
}
