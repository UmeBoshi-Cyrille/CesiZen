using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("Test")]
    public IActionResult Test()
    {
        return Ok("Got it");
    }

    [HttpGet("AuthorizedTest")]
    [Authorize]
    public IActionResult AuthorizedTest()
    {
        return Ok("Got it");
    }
}
