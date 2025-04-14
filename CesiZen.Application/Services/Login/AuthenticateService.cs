using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using Serilog;

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
        ) : base(logger, userCommand, passwordService, loginQuery, tokenProvider)
    {
        this.loginCommand = loginCommand;
        this.userQuery = userQuery;
    }

    public async Task<IResult<AuthenticateResponseDto>> Authenticate(AuthenticateRequestDto dto)
    {
        var response = new AuthenticateResponseDto();
        var user = await GetLogin(dto.Identifier);

        if (user.Value == null)
        {
            logger.Error(user.Error.Message);
            return Result<AuthenticateResponseDto>.Failure(UserErrors.ClientAuthenticationFailed);
        }

        var result = LoginAttemps(user.Value.Login!, dto.Password);

        response.IsLoggedIn = result.IsSuccess;

        if (!response.IsLoggedIn)
        {
            return Result<AuthenticateResponseDto>.Failure(UserErrors.ClientAuthenticationFailed);
        }

        var tokenDto = tokenProvider.GenerateRefreshToken(user.Value.Id);
        tokenDto.Value.UserId = user.Value.Id;
        tokenDto.Value.Role = user.Value.Role;

        var token = tokenProvider.GenerateAccessToken(tokenDto.Value);
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

    public async Task<IResult> Disconnect(int userId)
    {
        var result = await tokenProvider.InvalidateTokens(userId);

        if (result.IsFailure)
        {
            return Result.Failure(UserErrors.ClientDisconnectFailed);
        }

        return Result.Success();
    }

    #region Private Methods
    private async Task<IResult<AuthenticationUserDto>> GetLogin(string identifier)
    {
        if (string.IsNullOrEmpty(identifier))
            return Result<AuthenticationUserDto>.Failure(UserErrors.LogNotFound(identifier));

        var validity = identifier.IsValidEmail();
        var user = await userQuery.GetByIdentifier(identifier, validity);

        return Result<AuthenticationUserDto>.Success(user.Value);
    }

    private IResult LoginAttemps(AuthenticationLoginDto login, string providedPassword)
    {
        if (!IsLoginUnlocked(login).Result)
        {
            var time = CalculateLockTime(login).ToString();
            return Result.Failure(
                    Error.AuthenticationFailed(string.Format(
                        Message.GetResource("ErrorMessages", "CLIENT_LOGINATTEMPS_LOCKTIME"), time)));
        }

        var result = passwordService.IsCorrectPassword(login.Salt, login.Password, providedPassword);

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

    private async Task<bool> IsLoginUnlocked(AuthenticationLoginDto dto)
    {
        if (dto.AccountIsLocked)
        {
            if (dto.LockoutEndTime > DateTime.UtcNow)
            {
                return false;
            }

            dto.AccountIsLocked = false;
            dto.AccessFailedCount = 0;
            dto.LockoutEndTime = null;
            var login = dto.MapLoginAccess();
            await loginCommand.UpdateLoginAttemps(login);
        }

        return true;
    }

    private async Task<bool> IsLimitAttempsReached(AuthenticationLoginDto dto)
    {
        dto.AccessFailedCount++;
        var login = dto.MapLoginAccess();
        await loginCommand.UpdateLoginAttempsCount(login);

        if (login.AccessFailedCount >= 5)
        {
            login.AccountIsLocked = true;
            login.LockoutEndTime = DateTime.UtcNow.AddMinutes(5);
            await loginCommand.UpdateLoginAttemps(login);

            return false;
        }

        return true;
    }

    private static int CalculateLockTime(AuthenticationLoginDto login)
    {
        TimeSpan remainingTime = (TimeSpan)(login.LockoutEndTime - DateTime.UtcNow)!;

        return (int)remainingTime.TotalMinutes;
    }
    #endregion
}
