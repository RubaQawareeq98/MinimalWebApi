using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MinimalWebApi.Controllers;

[Route("api/home")]
[ApiController]
public class TestController : ControllerBase
{
    [Authorize]
    [HttpGet]
    public IActionResult GetWeather()
    {
        return Ok("Welcome! You are authorized to view this home data.");
    }
}

