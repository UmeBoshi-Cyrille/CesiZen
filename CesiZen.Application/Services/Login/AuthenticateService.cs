using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
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

    public async Task<IResult<AuthenticateResponseDto>> Authenticate(AuthenticateRequestDto dto)
    {
        var response = new AuthenticateResponseDto();
        var login = await GetLogin(dto.Identifier);

        if (login.Value == null)
        {
            logger.Error(login.Error.Message);
            return Result<AuthenticateResponseDto>.Failure(UserErrors.ClientAuthenticationFailed);
        }

        var result = LoginAttemps(login.Value, dto.Password);

        response.IsLoggedIn = result.IsSuccess;

        if (!response.IsLoggedIn)
        {
            return Result<AuthenticateResponseDto>.Failure(UserErrors.ClientAuthenticationFailed);
        }

        var token = tokenProvider.GenerateAccessToken(login.Value.UserId);
        response.Token = token;

        return Result<AuthenticateResponseDto>.Success(response, UserInfos.ClientAuthentified);
    }

    public async Task<IResult> VerifyEmail(string token, string email)
    {
        var login = await loginQuery.GetByEmail(email);
        if (login.IsFailure)
        {
            logger.Error(login!.Error.Message);
            return Result.Failure(UserErrors.ClientNotFound);
        }
        else if (login.Value.EmailVerificationToken != token)
        {
            return Result.Failure(UserErrors.ClientEmailVerificationFailed);
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
            return Result.Failure(UserErrors.ClientEmailVerificationFailed);
        }

        return Result.Success(UserInfos.ClientEmailVerified);
    }

    public async Task<IResult> Disconnect(string accessToken)
    {
        var sessionId = tokenProvider.GetTokenSessionId(accessToken);

        var userId = userQuery.GetUserId(sessionId).Result;

        if (string.IsNullOrEmpty(sessionId) || userId.IsFailure)
        {
            return Result.Failure(UserErrors.ClientNotFound);
        }

        var result = await tokenProvider.InvalidateTokens(userId.Value);

        if (result.IsFailure)
        {
            return Result.Failure(UserErrors.ClientDisconnectFailed);
        }

        return Result.Success();
    }

    #region Private Methods
    private static bool IsValidEmail(string email)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]{2,3}$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }

    private async Task<IResult<Login>> GetLogin(string identifier)
    {
        var validity = IsValidEmail(identifier);

        if (!validity)
            return Result<Login>.Failure(UserErrors.LogNotFound(identifier));

        var login = await loginQuery.GetByEmail(identifier);

        return Result<Login>.Success(login.Value);
    }

    private IResult LoginAttemps(Login login, string password)
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

    private static int CalculateLockTime(Login login)
    {
        TimeSpan remainingTime = (TimeSpan)(login.LockoutEndTime - DateTime.UtcNow)!;

        return (int)remainingTime.TotalMinutes;
    }
    #endregion
}
