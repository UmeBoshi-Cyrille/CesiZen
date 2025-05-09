using CesiZen.Application.Authorization;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : LoginController
{
    private readonly IAuthenticateService authenticateService;
    private readonly ITokenProvider tokenProvider;
    private readonly IPasswordService passwordService;
    private readonly ILoginQuery loginService;

    public AuthenticationController(
        IAuthenticateService authenticateService,
        ITokenProvider tokenProvider,
        IPasswordService passwordService,
        ILoginQuery loginService,
        INotifier notifier,
        IObserver observer) : base(notifier, observer)
    {
        this.authenticateService = authenticateService;
        this.tokenProvider = tokenProvider;
        this.passwordService = passwordService;
        this.loginService = loginService;
    }

    /// <summary>
    /// Verifies the validity of an email address using the provided token.
    /// </summary>
    /// <param name="token">A unique token to provide for email verification.</param>
    /// <param name="email">The email address to be verified, provided by the client.</param>
    /// <response code="200">The email address is valid and successfully verified.</response>
    /// <response code="404">The specified email or token was not found.</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code if the email is successfully verified.
    /// - A 404 status code if the email or verification token is not found.
    /// - A 500 status code if an unexpected server-side error occurs during the verification process.
    /// </returns>
    [HttpPost("verify-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token, string email)
    {
        var response = await authenticateService.VerifyEmail(token, email);

        if (response.IsFailure)
        {
            return NotFound(new { message = Error.Alert, errors = response.Error.Message });
        }

        return Ok(new { message = response.Info.Message });
    }

    /// <summary>
    /// Resend an email with a new fresh token for email validation.
    /// </summary>
    /// <param name="token">The old unique token to provide for verification.</param>
    /// <param name="email">The email address provided by the client.</param>
    /// <response code="200">The email is successfully sent.</response>
    /// <response code="404">The specified email or token was not found.</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code if the email is successfully sent.
    /// - A 404 status code if the email or verification token is not found.
    /// - A 500 status code if an unexpected server-side error occurs during the verification process.
    /// </returns>
    [HttpPost("resend-verify-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<IActionResult> ResendVerifyEmail([FromBody] string token, string email)
    {
        var response = await authenticateService.ResendEmailVerification(token, email);

        if (response.IsFailure)
        {
            return BadRequest(new { message = Error.Alert, errors = response.Error.Message });
        }

        SubscribeNotifierEvent();
        notifier.NotifyObservers(response.Value);
        UnsubscribeNotifierEvent();

        return Ok(new { message = response.Info.Message });
    }

    /// <summary>
    /// Removes the JWT cookie to invalidate the old access token.
    /// </summary>
    /// <response code="200">The JWT cookie was successfully deleted.</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code if the cookie was successfully deleted.
    /// - A 500 status code if an unexpected server-side error occurs during the cookie removal process.
    /// </returns>
    [HttpPost("delete-cookie")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public IActionResult DeleteCookie()
    {
        Response.Cookies.Delete("JWTCookie");

        var successMessage = LoginInfos.CookieDeleted;
        return Ok(new { message = successMessage });
    }

    /// <summary>
    /// Authenticates a user and provides access to the application by generating an access token.
    /// </summary>
    /// <param name="dto">An object to provide containing the user's login credentials (e.g., username and password).</param>
    /// <response code="200">The user was successfully authenticated, and an access token was issued.</response>
    /// <response code="401">Authentication failed due to invalid credentials.</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code with the generated access token if authentication succeeds.
    /// - A 401 status code if authentication fails due to invalid credentials.
    /// - A 500 status code if an unexpected server-side error occurs during the login process.
    /// </returns>
    [HttpPost("authenticate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<ActionResult<UserResponseDto>> Authenticate([FromBody] AuthenticateRequestDto dto)
    {
        var response = await authenticateService.Authenticate(dto);

        if (response.IsFailure)
            return Unauthorized(new { message = Error.Alert, errors = response.Error.Message });

        var clientType = Request.Headers["X-Client-Type"].ToString();

        if (clientType == "mobile")
        {
            return Ok(new { token = response.Value.Token, message = response.Info.Message });
        }

        SendSecureCookie(token: response.Value.Token!);

        return CreatedAtRoute(nameof(UserQueryController.GetProfile), null, new { response.Value.User, response.Value.IsLoggedIn, response.Value.TokenExpirationTime, response.Value.Token });
    }

    /// <summary>
    /// Invalidates the user's session and associated refresh tokens.
    /// This operation ensures that the user must reauthenticate to access the application.
    /// </summary>
    /// <response code="200">The session and refresh tokens were successfully invalidated.</response>
    /// <response code="401">The user is not authorized to perform this operation.</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code if the session and refresh tokens were successfully invalidated.
    /// - A 401 status code if the user is not authorized to perform this operation.
    /// - A 500 status code if an unexpected server-side error occurs during the invalidation process.
    /// </returns>
    [HttpPost("invalidate-tokens")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization("User, Admin")]
    public async Task<IActionResult> InvalidateTokens()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { message = Error.Alert, errors = UserErrors.NotConnected });
        }

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return BadRequest(new { message = Error.Alert, errors = UserErrors.Unknown });
        }

        var result = await tokenProvider.InvalidateTokens(userId);

        if (result.IsFailure)
        {
            return Unauthorized();
        }

        Response.Cookies.Delete("JWTCookie");

        return Ok(new { message = result.Info.Message });
    }

    /// <summary>
    /// Logs the user out of the application by invalidating the provided access token.
    /// </summary>
    /// <response code="204">The user session was successfully terminated.</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 204 status code if the logout operation succeeds and no content is returned.
    /// - A 500 status code if an unexpected server-side error occurs during the logout process.
    /// </returns>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "User, Admin")]
    public async Task<IActionResult> Logout()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { message = Error.Alert, errors = UserErrors.NotConnected });
        }

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return BadRequest(new { message = Error.Alert, errors = UserErrors.Unknown });
        }

        var result = await authenticateService.Disconnect(userId);

        Response.Cookies.Delete("JWTCookie");

        return Ok(new { message = result.Info.Message });
    }

    /// <summary>
    /// Sends a password reset request for the provided email address.
    /// A reset link containing a secure token will be sent to the user's email if the account exists.
    /// </summary>
    /// <param name="email">The email address to provide to initiate the password reset process.</param>
    /// <response code="200">The password reset request was successfully processed.</response>
    /// <response code="400">The request was invalid or contained errors.</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code if the password reset request is successfully processed.
    /// - A 400 status code if the request is invalid (e.g., malformed email address).
    /// - A 500 status code if an unexpected server-side error occurs during the process.
    /// </returns>
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPasswordRequest([FromBody] ForgetPasswordEmail email)
    {
        var response = await passwordService.ForgotPasswordRequest(email.Email);

        if (response.IsFailure)
        {
            return BadRequest(new { message = Error.Alert, errors = response.Error.Message });
        }

        SubscribeNotifierEvent();
        notifier.NotifyObservers(response.Value);
        UnsubscribeNotifierEvent();

        return Ok(new { message = response.Info.Message });
    }


    /// <summary>
    /// Resets the user's password by verifying the provided token and email.
    /// If the token is valid, the user is redirected to a secure reset password page.
    /// </summary>
    /// <param name="email">The email address to provide associated with the account.</param>
    /// <param name="token">The secure token sent from the user's email for password reset verification.</param>
    /// <response code="200">The token is valid, and the user can proceed to reset their password.</response>
    /// <response code="400">The request was invalid or contained errors (e.g., malformed token).</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code if the token is valid and the operation succeeds.
    /// - A 400 status code if the request is invalid (e.g., malformed token or email).
    /// - A 500 status code if an unexpected server-side error occurs during processing.
    /// </returns>
    [HttpPost("forgot-password-response")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPasswordResponse([FromQuery] string email, string token)
    {
        var response = await passwordService.ForgotPasswordResponse(email, token);

        if (response.IsSuccess)
            return Ok(new { id = response.Value, message = response.Info.Message });

        return BadRequest(new { message = Error.Alert, errors = response.Error.Message });
    }

    /// <summary>
    /// Resets the user's password by verifying the provided user ID and token, and updating it to a new password.
    /// </summary>
    /// <param name="email">Email provided from the response to reset user Id.</param>
    /// <param name="dto">An object containing the new password and its confirmation, provided by the client.</param>
    /// <response code="200">The password was successfully reset.</response>
    /// <response code="400">The request was invalid or contained errors (e.g., mismatched passwords).</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code if the password reset operation succeeds.
    /// - A 400 status code if the request is invalid (e.g., malformed token or mismatched passwords).
    /// - A 500 status code if an unexpected server-side error occurs during processing.
    /// </returns>
    [HttpPost("reset-forgotten-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<IActionResult> ResetForgottenPassword([FromQuery] string email, [FromBody] ResetForgottenPasswordDto dto)
    {
        var userId = await loginService.GetUserIdByEmail(email);

        var response = await passwordService.ResetForgottenPassword(userId.Value, dto);

        if (response.IsFailure)
            return BadRequest(new { message = Error.Alert, errors = response.Error.Message });

        return Ok(new { message = response.Info.Message });
    }

    /// <summary>
    /// Resets the user's password by verifying the provided user ID and token, and updating it to a new password.
    /// </summary>
    /// <param name="dto">An object containing the new password and its confirmation, provided by the client.</param>
    /// <response code="200">The password was successfully reset.</response>
    /// <response code="400">The request was invalid or contained errors (e.g., mismatched passwords).</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code if the password reset operation succeeds.
    /// - A 400 status code if the request is invalid (e.g., malformed token or mismatched passwords).
    /// - A 500 status code if an unexpected server-side error occurs during processing.
    /// </returns>
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { message = Error.Alert, errors = UserErrors.NotConnected });
        }

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return BadRequest(new { message = Error.Alert, errors = UserErrors.Unknown });
        }

        var response = await passwordService.ResetPassword(userId, dto);

        if (response.IsFailure)
            return BadRequest(new { message = Error.Alert, errors = response.Error.Message });

        return Ok(new { message = response.Info.Message });
    }

    /// <summary>
    /// Check access token validity and refresh it if valid.
    /// </summary>
    /// <response code="200">The session was successfully reset.</response>
    /// <response code="400">The request was invalid.</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code if the token reset operation succeeds.
    /// - A 400 status code if the request is invalid (e.g., malformed token).
    /// - A 500 status code if an unexpected server-side error occurs during processing.
    /// </returns>
    [HttpPost("refresh-access-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "User, Admin")]
    public async Task<ActionResult<AuthenticateResponseDto>> RefreshAccessToken()
    {
        var accessToken = HttpContext.GetTokenAsync("access_token");
        var principal = HttpContext.User;

        var response = await tokenProvider.RefreshAccessTokenAsync(accessToken.Result!, principal);

        if (response.IsFailure)
        {
            Response.Cookies.Delete("JWTCookie");
            return BadRequest(new { message = response.Error.Message });
        }

        SendSecureCookie(token: response.Value.Token!);

        return CreatedAtRoute(nameof(UserQueryController.GetProfile), null, new { response.Value.User, response.Value.IsLoggedIn, response.Value.TokenExpirationTime, response.Value.Token });
    }

    private void SendSecureCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, // Prevents JavaScript access to tokens, mitigating XSS attacks.
            Secure = true, // Cookies marked as secure are only transmitted over HTTPS connections.
            SameSite = SameSiteMode.Strict, // Helps mitigate CSRF attacks when configured properly
            Expires = DateTime.UtcNow.AddMinutes(5),
            Path = "/"
        };

        Response.Cookies.Append("JWTCookie", token, cookieOptions);
    }
}
