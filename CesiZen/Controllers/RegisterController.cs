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
    /// Registers a new user and creates their account.
    /// </summary>
    /// <param name="dto">The data provided by the client, including user details such as username, password, and other required fields.</param>
    /// <response code="200">The user was successfully registered.</response>
    /// <response code="400">The request was invalid or contained errors (e.g., validation failure).</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code with the newly created user data if the registration succeeds.
    /// - A 400 status code if the request is invalid, such as missing required fields or failing validation checks.
    /// - A 500 status code if an unexpected server-side error occurs during the registration process.
    /// </returns>
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
