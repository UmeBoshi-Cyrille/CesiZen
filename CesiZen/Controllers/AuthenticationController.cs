using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : LoginController
{
    private readonly IAuthenticateService authenticateService;
    private readonly ITokenProvider tokenProvider;
    private readonly IPasswordService passwordService;

    public AuthenticationController(
        IAuthenticateService authenticateService,
        ITokenProvider tokenProvider,
        IPasswordService passwordService,
        IObserver observer,
        INotifier notifier) : base(notifier, observer)
    {
        this.authenticateService = authenticateService;
        this.tokenProvider = tokenProvider;
        this.passwordService = passwordService;

        notifier.MessageEvent += observer.Update!;
    }

    /// <summary>
    /// Verify email Validity from the token provided.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="email"></param>
    /// <response code="200">email is valid</response>
    /// <response code="404">email not found</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpGet("Verify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> VerifyEmail(string token, string email)
    {
        var result = await authenticateService.VerifyEmail(token, email);

        if (result.IsFailure)
        {
            return NotFound(new { message = result.Error.Message });
        }

        return Ok(new { message = result.Info.Message });
    }

    /// <summary>
    /// Remove JwtCookie to clean old accesstoken.
    /// </summary>
    /// <response code="200">cookie deleted</response>
    /// <returns></returns>
    [HttpGet("delete-cookie")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult DeleteCookie()
    {
        Response.Cookies.Delete("JWTCookie");

        var successMessage = string.Format(Message.GetResource("InfoMessages", "CLIENT_DELETE_SUCCESS"), "Cookie");
        return Ok(new { message = successMessage });
    }

    /// <summary>
    /// Allows to login or sign in into the application.
    /// </summary>
    /// <param name="dto">data provided by the client</param>
    /// <response code="200">logged in</response>
    /// <response code="401">not authorized</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPost("authenticate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Authenticate(AuthenticateRequestDto dto)
    {
        var response = await authenticateService.Authenticate(dto);

        if (response.IsFailure)
            return Unauthorized(new { message = response.Error.Message });

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, // Prevents JavaScript access to tokens, mitigating XSS attacks.
            Secure = true, // Cookies marked as secure are only transmitted over HTTPS connections.
            SameSite = SameSiteMode.Strict, // Helps mitigate CSRF attacks when configured properly
            Expires = DateTime.UtcNow.AddMinutes(30)
        };

        Response.Cookies.Append("JWTCookie", response.Value.Token!, cookieOptions);

        return Ok(new { message = response.Info.Message });
        //return Ok(response.Value.Token);
    }

    /// <summary>
    /// Allows the possibility to invalidate session and refresToken stored.
    /// </summary>
    /// <param name="userId">Id provided by the client</param>
    /// <response code="200">operation succeeded</response>
    /// <response code="401">not authorized</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPost("invalidate-tokens")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult InvalidateTokens(string userId)
    {
        var result = tokenProvider.InvalidateTokens(userId).Result;

        if (result.IsFailure)
        {
            return Unauthorized();
        }

        var successMessage = Message.GetResource("InfoMessages", "CLIENT_SESSION_CLOSED");

        return Ok(new { message = result.Info.Message });
    }

    /// <summary>
    /// Allows to log out from the application.
    /// </summary>
    /// <param name="accessToken">accessToken provided by the client</param>
    /// <response code="200">operation succeeded</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Logout(string accessToken)
    {
        var result = await authenticateService.Disconnect(accessToken);

        Response.Cookies.Delete("JWTCookie");

        // Clear any other session-related data if necessary
        //HttpContext.Session.Clear();

        return Ok(new { message = result.Info.Message });
    }

    /// <summary>
    /// Allows to reset your password if lost or forgotten.
    /// </summary>
    /// <param name="dto">data provided by the client</param>
    /// <response code="200">operation succeeded</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ForgotPassword(PasswordResetRequestDto dto)
    {
        var result = await passwordService.ForgotPassword(dto);

        if (result.IsFailure)
            return BadRequest(new { message = result.Error.Message });

        var message = BuildEmailVerificationMessage(dto.Email!);
        notifier.NotifyObservers(message);

        return Ok(new { message = result.Info.Message });
    }

    /// <summary>
    /// Reset password with a new one
    /// </summary>
    /// <param name="dto">data provided by the client</param>
    /// <response code="200">operation succeeded</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ResetPassword(PasswordResetDto dto)
    {
        var result = await passwordService.ResetPassword(dto);

        if (result.IsFailure)
            return BadRequest(new { message = result.Error.Message });

        return Ok(new { message = result.Info.Message });
    }

    private MessageEventDto BuildEmailVerificationMessage(string email)
    {
        return new MessageEventDto
        {
            Email = email,
            Subject = Message.GetResource("Templates", "SUBJECT_RESET_PASSWORD"),
            Body = Message.GetResource("Templates", "TEMPLATE_RESET_PASSWORD"),
        };
    }
}
