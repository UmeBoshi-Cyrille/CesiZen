using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegisterController : ControllerBase
{
    private readonly IRegisterService registerService;

    public RegisterController(
        IRegisterService registerService)
    {
        this.registerService = registerService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto user)
    {
        var response = registerService.Register(user).Result;
        if (response.IsFailure)
        {
            return BadRequest(new { message = response.Error.Message });
        };

        return Ok(new { message = response.Info.Message });
    }
}
