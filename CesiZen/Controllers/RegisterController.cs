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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] UserDto user)
    {
        var response = await registerService.Register(user);
        if (response.IsFailure)
        {
            return BadRequest(new { message = response.Error.Message });
        };

        return Ok(new { message = response.Info.Message });
    }
}
