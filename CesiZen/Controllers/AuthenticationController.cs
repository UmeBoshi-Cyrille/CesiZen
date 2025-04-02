using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : LoginController
{
    private readonly IConfiguration configuration;
    private readonly IAuthenticateService authenticateService;
    private readonly ITokenProvider tokenProvider;
    private readonly IPasswordService passwordService;

    public AuthenticationController(
        IConfiguration configuration,
        IAuthenticateService authenticateService,
        ITokenProvider tokenProvider,
        IPasswordService passwordService,
        INotifier notifier,
        IObserver observer) : base(notifier, observer)
    {
        this.authenticateService = authenticateService;
        this.tokenProvider = tokenProvider;
        this.passwordService = passwordService;
        this.configuration = configuration;
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
    [HttpGet("verify")]
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
    /// <returns>Success message</returns>
    [HttpGet("delete-cookie")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    public async Task<IActionResult> InvalidateTokens(int userId)
    {
        var result = await tokenProvider.InvalidateTokens(userId);

        if (result.IsFailure)
        {
            return Unauthorized();
        }

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

        return Ok(new { message = result.Info.Message });
    }

    /// <summary>
    /// Send a request to reset password if lost or forgotten.
    /// </summary>
    /// <param name="email">email provided by the client</param>
    /// <response code="200">operation succeeded</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ForgotPasswordRequest(string email)
    {
        var result = await passwordService.ForgotPasswordRequest(email);

        if (result.IsFailure)
        {
            return BadRequest(new { message = result.Error.Message });
        }

        SubscribeNotifierEvent();
        notifier.NotifyObservers(result.Value);
        UnsubscribeNotifierEvent();

        return Ok(new { message = result.Info.Message });
    }


    /// <summary>
    /// Allows to reset your password if lost or forgotten by checking received token and redirecting to a Reset password page.
    /// </summary>
    /// <param name="email">email provided by the client</param>
    /// <param name="token">token provided by the client</param>
    /// <response code="200">operation succeeded</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPost("forgot-password-response")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ForgotPasswordResponse(string email, string token)
    {
        var result = await passwordService.ForgotPasswordResponse(email, token);

        var successMessage = "If the email has been verified.";
        var failMessage = "Couldn't send email to the provided adress";

        if (result.IsSuccess)
            return Ok(new { successMessage });

        return BadRequest(new { failMessage });
    }

    /// <summary>
    /// Reset password with a new one
    /// </summary>
    /// <param name="userId">id provided by the client</param>
    /// <param name="dto">data provided by the client</param>
    /// <response code="200">operation succeeded</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ResetPassword(int userId, PasswordResetDto dto)
    {
        var result = await passwordService.ResetPassword(userId, dto);

        if (result.IsFailure)
            return BadRequest(new { message = result.Error.Message });

        return Ok(new { message = result.Info.Message });
    }
}
