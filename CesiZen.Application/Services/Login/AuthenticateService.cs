using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Interfaces;
using Serilog;
using System.Text.RegularExpressions;

namespace CesiZen.Application.Services;

public sealed class AuthenticationService : ALoginService, IAuthenticateService
{
    private readonly ILoginCommand loginCommand;
    private readonly IUserQuery userQuery;

    public AuthenticationService(
        ILogger logger,
        IUserCommand userCommand,
        IUserQuery userQuery,
        IPasswordService passwordService,
        ILoginQuery loginQuery,
        ILoginCommand loginCommand,
        ITokenProvider tokenProvider,
        IEmailService emailService
        ) : base(logger, userCommand, passwordService, loginQuery, emailService, tokenProvider)
    {
        this.loginCommand = loginCommand;
        this.userQuery = userQuery;
    }

    public async Task<IResult<AuthenticateResponseDto>> Authenticate(AuthenticateRequestDto request)
    {
        var response = new AuthenticateResponseDto();
        var login = await GetLogin(request.Identifier);

        if (login.Value == null)
        {
            return Result<AuthenticateResponseDto>.Failure(
                Error.NotFound(
                    Message.GetResource("ErrorMessages", "CLIENT_AUTHENTICATION_FAILED")));
        }

        var result = LoginAttemps(login.Value, request.Password).Result;

        response.IsLoggedIn = result.IsSuccess;

        if (!response.IsLoggedIn)
        {
            return Result<AuthenticateResponseDto>.Failure(
                Error.AuthenticationFailed(
                    Message.GetResource("ErrorMessages", "CLIENT_AUTHENTICATION_MISMATCH")));
        }

        var token = tokenProvider.GenerateAccessToken(login.Value.UserId);
        response.Token = token;

        return Result<AuthenticateResponseDto>.Success(response,
                    Info.Success(
                        Message.GetResource("InfoMessages", "CLIENT_AUTHENTICATION_SUCCESS")));
    }

    public async Task<IResult> VerifyEmail(string token, string email)
    {
        var login = await loginQuery.GetByEmail(email);
        if (login == null)
        {
            return Result.Failure(Error.NotFound(login.Error.Message));
        }
        else if (login.Value.EmailVerificationToken != token)
        {
            return Result.Failure(Error.NotMatch(login.Error.Message));
        }

        var dto = new EmailVerificationDto
        {
            Email = email,
            EmailVerified = true,
            EmailVerificationToken = null
        };

        var result = await loginCommand.UpdateEmailVerification(dto);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result.Failure(
                Error.OperationFailed(Message.GetResource("ErrorMessages", "CLIENT_EMAIL_VERIFICATION_FAILED")));
        }

        return Result.Success(
                Info.Success(Message.GetResource("InfoMessages", "CLIENT_EMAIL_VERIFIED")));
    }

    public async Task<IResult> Disconnect(string accessToken)
    {
        var sessionId = tokenProvider.GetTokenSessionId(accessToken);

        var userId = userQuery.GetUserId(sessionId).Result;

        await tokenProvider.InvalidateTokens(userId.Value);

        return Result.Success();
    }

    #region Private Methods
    private bool IsValidEmail(string email)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]{2,3}$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }

    private async Task<IResult<Login>> GetLogin(string identifier)
    {
        Login login = null;
        var validity = IsValidEmail(identifier);

        if (validity)
            login = loginQuery.GetByEmail(identifier).Result.Value;

        return Result<Login>.Success(login);
    }

    private async Task<IResult> LoginAttemps(Login login, string password)
    {
        if (!IsLoginUnlocked(login).Result)
        {
            var time = CalculateLockTime(login).ToString();
            return Result.Failure(
                    Error.AuthenticationFailed(string.Format(
                        Message.GetResource("ErrorMessages", "CLIENT_LOGINATTEMPS_LOCKTIME"), time)));
        }

        var result = passwordService.VerifyPassword(login, password);

        if (result)
        {
            return Result.Success();
        }

        if (!IsLimitAttempsReached(login).Result)
        {
            return Result.Failure(
                    Error.AuthenticationFailed(
                        Message.GetResource("ErrorMessages", "CLIENT_LOGINATTEMPS")));
        }

        return Result.Failure(
                Error.AuthenticationFailed(
                    Message.GetResource("ErrorMessages", "CLIENT_AUTHENTICATION_FAILED")));
    }

    private async Task<bool> IsLoginUnlocked(Login login)
    {
        if (login.IsLocked)
        {
            if (login.LockoutEndTime > DateTime.UtcNow)
            {
                return false;
            }

            login.IsLocked = false;
            login.AccessFailedCount = 0;
            login.LockoutEndTime = null;
            await loginCommand.UpdateLoginAttemps(login);
        }

        return true;
    }

    private async Task<bool> IsLimitAttempsReached(Login login)
    {
        login.AccessFailedCount++;
        await loginCommand.UpdateLoginAttempsCount(login);

        if (login.AccessFailedCount >= 5)
        {
            login.IsLocked = true;
            login.LockoutEndTime = DateTime.UtcNow.AddMinutes(5);
            await loginCommand.UpdateLoginAttemps(login);

            return false;
        }

        return true;
    }

    private int CalculateLockTime(Login login)
    {
        TimeSpan remainingTime = (TimeSpan)(login.LockoutEndTime - DateTime.UtcNow);

        return (int)remainingTime.TotalMinutes;
    }
    #endregion
}
