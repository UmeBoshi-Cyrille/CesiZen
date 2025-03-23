using CesiZen.Application.Helper;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace CesiZen.Application.Services;

public class PasswordService : IPasswordService
{
    private readonly IConfiguration configuration;
    private readonly ILoginQuery loginQuery;
    private readonly ILoginCommand loginCommand;
    private readonly ITokenProvider tokenProvider;

    public PasswordService(
        IConfiguration configuration,
        ILoginQuery loginQuery,
        ILoginCommand loginCommand,
        ITokenProvider tokenProvider)
    {
        this.configuration = configuration;
        this.loginQuery = loginQuery;
        this.loginCommand = loginCommand;
        this.tokenProvider = tokenProvider;
    }

    #region Public Methods
    public Authentifier HashPassword(string password, Login? login = null)
    {
        byte[]? salt = null;

        if (login != null)
            salt = GetSalt(login.Salt);

        var authentifier = HashSaltedPassword(password, salt);

        return authentifier;
    }

    public bool IsCorrectPassword(Login login, string providedPassword)
    {
        var salt = GetSalt(login.Salt);

        var hashProvidedpassword = PasswordHasher(providedPassword, salt);
        var currentHash = Convert.FromHexString(login.Password);
        var providedHash = Convert.FromHexString(hashProvidedpassword);

        return CryptographicOperations.FixedTimeEquals(providedHash, currentHash);
    }

    public async Task<IResult> ResetPassword(int userId, PasswordResetDto dto)
    {
        var login = await loginQuery.GetByUserId(userId);

        var result = await ResetPassword(login.Value, dto);

        return Result.Success(UserInfos.ClientPasswordModified);
    }

    public async Task<IResult<MessageEventArgs>> ForgotPasswordRequest(string email)
    {
        var login = await loginQuery.GetByEmail(email);

        if (login == null)
        {
            return Result<MessageEventArgs>.Failure(LoginErrors.LoginNotFound);
        }

        var result = await ResetPasswordAttemps(login.Value);

        if (result.IsFailure)
        {
            return Result<MessageEventArgs>.Failure(LoginErrors.LoginNotFound);
        }

        var token = tokenProvider.GenerateVerificationToken();

        await SaveResetPasswordToken(login.Value, token);

        var message = BuildEmailVerificationMessage(login.Value.Email, token);

        return Result<MessageEventArgs>.Success(message, UserInfos.ClientVerificationEmailSent);
    }

    public async Task<IResult> ForgotPasswordResponse(string email, string token)
    {
        var resetPassword = await loginQuery.GetResetPassword(email, token);

        if (resetPassword.IsFailure)
        {
            return Result<string>.Failure(LoginErrors.ResetPasswordNotFound);
        }

        if (resetPassword.Value.ExpirationTime < DateTime.UtcNow)
        {
            return Result.Failure(LoginErrors.ExpiredLink);
        }

        return Result.Success();
    }
    #endregion

    #region Private Methods
    private async Task<IResult> ResetPassword(Login login, PasswordResetDto dto)
    {
        if (login == null)
        {
            return Result.Failure(LoginErrors.LoginNotFound);
        }

        if (!IsCorrectPassword(login, dto.CurrentPassword!))
        {
            return Result.Failure(LoginErrors.PasswordNotMatch);
        }

        var authentifier = HashPassword(dto.NewPassword!, login);
        await loginCommand.UpdatePassword(login.UserId, authentifier.Password);

        return Result.Success(LoginInfos.ResetPasswordSucceed);
    }

    private async Task SaveResetPasswordToken(Login login, string token)
    {
        login.ResetPasswords = [];

        ResetPassword resetPassword = new()
        {
            ResetToken = token,
            ExpirationTime = DateTime.UtcNow.AddHours(24),
            CreateAt = DateTime.UtcNow,
        };

        login.ResetPasswords.Add(resetPassword);
        await loginCommand.UpdateLogin(login);
    }

    private Authentifier HashSaltedPassword(string password, byte[]? salt = null)
    {
        var keySize = EncryptionHelper.GetPasswordKeySize();

        if (salt == null)
            salt = RandomNumberGenerator.GetBytes(keySize);

        var passwordHash = PasswordHasher(password, salt);
        var hashSalt = HashSalt(salt);

        Authentifier authentifier = new(passwordHash, hashSalt);

        return authentifier;
    }

    private string PasswordHasher(string password, byte[] salt)
    {
        var secret = configuration.GetValue<string>("Salt:Secret");

        var hash = EncryptionHelper.HashString(secret!, password, salt);

        return hash;
    }

    private string HashSalt(byte[] salt)
    {
        var secret = configuration.GetValue<string>("Salt:Secret");
        var saltString = Convert.ToBase64String(salt);

        return EncryptionHelper.Encrypt(saltString + secret);
    }

    private byte[] GetSalt(string hashSalt)
    {
        byte[]? salt = null;

        try
        {
            var pattern = configuration.GetValue<string>("Salt:Secret");
            var regexOptions = RegexOptions.Compiled;

            var result = EncryptionHelper.Decrypt(hashSalt);
            var decryptedSalt = Regex.Replace(result, Regex.Escape(pattern!), "", regexOptions);

            salt = Convert.FromBase64String(decryptedSalt);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return salt!;
    }

    private async Task SavePasswordResetToken(Login login, string token)
    {
        login.PasswordResetToken = token;
        login.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(24);
        await loginCommand.AddResetPasswordToken(login);
    }

    private MessageEventArgs BuildEmailVerificationMessage(string email, string token)
    {
        var template = Message.GetResource("Templates", "TEMPLATE_RESET_PASSWORD");
        var verificationLink = $"{configuration["App:Url"]}/reset-password?token={token}";
        var htmlTemplate = template.Replace("{{url}}", verificationLink);

        return new MessageEventArgs
        {
            Email = email,
            Subject = Message.GetResource("Templates", "SUBJECT_RESET_PASSWORD"),
            Body = htmlTemplate,
        };
    }

    private async Task<IResult> ResetPasswordAttemps(Login login)
    {
        var unlocked = await IsResetPasswordUnlocked(login);

        if (!unlocked)
        {
            var time = TimeService.CalculateLockTime(login.LockoutEndTime).ToString();
            return Result.Failure(
                    Error.AuthenticationFailed(string.Format(
                        Message.GetResource("ErrorMessages", "CLIENT_LOGINATTEMPS_LOCKTIME"), time)));
        }

        if (!IsLimitAttempsReached(login).Result)
        {
            return Result.Failure(LoginErrors.ForgotPasswordAttempsReached);
        }

        return Result.Success();
    }

    private async Task<bool> IsResetPasswordUnlocked(Login login)
    {
        if (login.ResetIsLocked)
        {
            if (login.ResetLockoutEndTime > DateTime.UtcNow)
            {
                return false;
            }

            login.ResetIsLocked = false;
            login.ResetFailedCount = 0;
            login.ResetLockoutEndTime = null;
            await loginCommand.UpdateResetPasswordAttemps(login);
        }

        return true;
    }

    private async Task<bool> IsLimitAttempsReached(Login login)
    {
        login.ResetFailedCount++;
        await loginCommand.UpdateResetPasswordAttempsCount(login.Id, login.ResetFailedCount);

        if (login.ResetFailedCount >= 5)
        {
            login.AccountIsLocked = true;
            login.LockoutEndTime = DateTime.UtcNow.AddMinutes(5);
            await loginCommand.UpdateResetPasswordAttemps(login);

            return false;
        }

        return true;
    }
    #endregion
}
