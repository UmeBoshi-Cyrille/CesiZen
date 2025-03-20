using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticateService authenticateService;
    private readonly ITokenProvider tokenProvider;
    private readonly IPasswordService passwordService;

    public AuthenticationController(
        IAuthenticateService authenticateService,
        ITokenProvider tokenProvider,
        IPasswordService passwordService)
    {
        this.authenticateService = authenticateService;
        this.tokenProvider = tokenProvider;
        this.passwordService = passwordService;
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

    [HttpGet("delete-cookie")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult DeleteCookie()
    {
        Response.Cookies.Delete("JWTCookie");

        var successMessage = string.Format(Message.GetResource("InfoMessages", "CLIENT_DELETE_SUCCESS"), "Cookie");
        return Ok(new { message = successMessage });
    }

    [HttpPost("authenticate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Authenticate(AuthenticateRequestDto model)
    {
        var response = await authenticateService.Authenticate(model);

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



    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ForgotPassword(PasswordResetRequestDto request)
    {
        var result = await passwordService.ForgotPassword(request);

        if (result.IsFailure)
            return BadRequest(new { message = result.Error.Message });

        return Ok(new { message = result.Info.Message });
    }

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


    /* Authentication Task
     *  1. Register User
     *      > Check email unicity
     *      > Check Email format
     *      > Check password validity
     *      > Check password confirmation
     *      > Hash Password
     *  2. Authenticate User
     *      > Check Email Format
     *      > Get Login
     *      > Hash Password
     *      > Compare Hashed Password
     *      > Validate or reprove
     *      > Limit Authentication Attempts (5)
     *  3. Authentication Cycle
     *      > Create AccessToken
     *      > Create RefreshToken
     *      > Send AccessToken into cookie scure httpOnly
     *      > Save RefreshToken in Database
     *      > Check Access token presence and signature for each request
     *      > Check Access token expiration time
     *      > Renew Access token every 5 minutes before expiration
     *          > Check for refresh token
     *  4. Log out
     *      > Invalidate Tokens
     *
    */
}
