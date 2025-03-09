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

    public AuthenticationController(
        IAuthenticateService authenticateService,
        ITokenProvider tokenProvider)
    {
        this.authenticateService = authenticateService;
        this.tokenProvider = tokenProvider;
    }

    [HttpGet("Verify")]
    public async Task<IActionResult> VerifyEmail(string token, string email)
    {
        var result = authenticateService.VerifyEmail(token, email);

        if (result.Result.IsFailure)
        {
            return BadRequest(result.Result.Error.Message);
        }

        return Ok("Email confirmed.");
    }

    [HttpGet("delete-cookie")]
    public IActionResult DeleteCookie()
    {
        Response.Cookies.Delete("JWTCookie");
        return Ok(string.Format(Message.GetResource("InfoMessages", "CLIENT_DELETE_SUCCESS"), "Cookie"));
    }

    [HttpPost("authenticate")]
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

        //return Ok(response.Info.Message);
        return Ok(response.Value.Token);
    }

    [HttpGet("delete-cookie")]
    public IActionResult DeleteCookie()
    {
        Response.Cookies.Delete("JWTCookie");
        return Ok(string.Format(Message.GetResource("InfoMessages", "CLIENT_DELETE_SUCCESS"), "Cookie"));
    }

    [HttpPost("invalidate-tokens")]
    public IActionResult InvalidateTokens(string userId)
    {
        var result = tokenProvider.InvalidateTokens(userId).Result;

        if (result.IsFailure)
        {
            return Unauthorized();
        }

        return Ok(Message.GetResource("InfoMessages", "CLIENT_SESSION_CLOSED"));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(string accessToken)
    {
        var result = authenticateService.Disconnect(accessToken);

        Response.Cookies.Delete("JWTCookie");

        // Clear any other session-related data if necessary
        //HttpContext.Session.Clear();

        return Ok("Logged out successfully");
    }

    [HttpGet("Verify")]
    public async Task<IActionResult> VerifyEmail(string token, string email)
    {
        var result = authenticateService.VerifyEmail(token, email);

        if (result.Result.IsFailure)
        {
            return BadRequest(result.Result.Error.Message);
        }

        return Ok("Email confirmed.");
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
