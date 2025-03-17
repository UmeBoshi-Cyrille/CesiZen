using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticateService authenticateService;
    private readonly ITokenProvider tokenProvider;
    private readonly ILoginQuery loginQuery;
    private readonly IPasswordService passwordService;

    public AuthenticationController(
        IAuthenticateService authenticateService,
        ITokenProvider tokenProvider,
        ILoginQuery loginQuery,
        IPasswordService passwordService)
    {
        this.authenticateService = authenticateService;
        this.tokenProvider = tokenProvider;
        this.loginQuery = loginQuery;
        this.passwordService = passwordService;
    }

    [HttpGet("Verify")]
    public async Task<IActionResult> VerifyEmail(string token, string email)
    {
        var result = await authenticateService.VerifyEmail(token, email);

        if (result.IsFailure)
        {
            return BadRequest(new { result.Error.Message });
        }

        return Ok(new { result.Info.Message });
    }

    [HttpGet("delete-cookie")]
    public IActionResult DeleteCookie()
    {
        Response.Cookies.Delete("JWTCookie");

        var successMessage = string.Format(Message.GetResource("InfoMessages", "CLIENT_DELETE_SUCCESS"), "Cookie");
        return Ok(new { successMessage });
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

        return Ok(new { response.Info.Message });
        //return Ok(response.Value.Token);
    }

    [HttpPost("invalidate-tokens")]
    public IActionResult InvalidateTokens(string userId)
    {
        var result = tokenProvider.InvalidateTokens(userId).Result;

        if (result.IsFailure)
        {
            return Unauthorized();
        }

        var successMessage = Message.GetResource("InfoMessages", "CLIENT_SESSION_CLOSED");

        return Ok(new { result.Info.Message });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(string accessToken)
    {
        var result = await authenticateService.Disconnect(accessToken);

        Response.Cookies.Delete("JWTCookie");

        // Clear any other session-related data if necessary
        //HttpContext.Session.Clear();
        var message = "Logged out successfully";
        return Ok(new { result.Info.Message });
    }



    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(PasswordResetRequestDto request)
    {
        var result = await passwordService.ForgotPassword(request);

        var successMessage = "If the email exists, a password reset link has been sent.";
        var failedMessage = "Couldn't send email to the provided adress";

        if (result.IsSuccess)
            return Ok(new { result.Info.Message });

        return BadRequest(new { result.Error.Message });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(PasswordResetDto dto)
    {
        var result = passwordService.ResetPassword(dto).Result;

        var successMessage = "Password has been reset successfully.";
        var failedMessage = "Invalid or expired token.";

        if (result.IsFailure)
            return BadRequest(new { result.Error.Message });

        return Ok(new { result.Info.Message });
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
