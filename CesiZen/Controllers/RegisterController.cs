using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("api/registration")]
public class RegisterController : LoginController
{

    private readonly IRegisterService registerService;

    public RegisterController(
        IRegisterService registerService,
        INotifier notifier,
        IObserver observer) : base(notifier, observer)
    {
        this.registerService = registerService;
    }

    /// <summary>
    /// Register new user
    /// </summary>
    /// <param name="dto">data provided by the client</param>
    /// <response code="200">operation succeeded</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] NewUserDto dto)
    {
        var response = await registerService.Register(dto);

        if (response.IsFailure)
        {
            return BadRequest(new { message = response.Error.Message });
        }

        SubscribeNotifierEvent();
        notifier.NotifyObservers(response.Value);
        UnsubscribeNotifierEvent();

        return Ok(new { message = response.Info.Message });
    }
}
