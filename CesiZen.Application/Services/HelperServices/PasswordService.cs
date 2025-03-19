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
    private readonly IEmailService emailService;

    public PasswordService(
        IConfiguration configuration,
        ILoginQuery loginQuery,
        ILoginCommand loginCommand,
        ITokenProvider tokenProvider,
        IEmailService emailService)
    {
        this.configuration = configuration;
        this.loginQuery = loginQuery;
        this.loginCommand = loginCommand;
        this.tokenProvider = tokenProvider;
        this.emailService = emailService;
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

    public bool VerifyPassword(Login login, string providedPassword)
    {
        var salt = GetSalt(login.Salt);

        var hashProvidedpassword = PasswordHasher(providedPassword, salt);
        var currentHash = Convert.FromHexString(login.Password);
        var providedHash = Convert.FromHexString(hashProvidedpassword);

        return CryptographicOperations.FixedTimeEquals(providedHash, currentHash);
    }

    public async Task<IResult> ResetPassword(PasswordResetDto dto)
    {
        if (dto.NewPassword != dto.ConfirmPassword)
        {
            return Result.Failure(
                Error.NotMatch(
                    Message.GetResource("ErrorMessages", "CLIENT_RESETPASSWORD_NOTMATCH")));
        }

        var login = await loginQuery.GetByResetPasswordToken(dto.Token);
        if (login.Value == null)
        {
            return Result.Failure(
                Error.NullValue(string.Format(
                    Message.GetResource("ErrorMessages", "CLIENT_NOTFOUND"), "Login")));
        }
        else if (login.Value.PasswordResetTokenExpiry < DateTime.UtcNow)
        {
            return Result.Failure(
                Error.TimeOut(
                    Message.GetResource("ErrorMessages", "CLIENT_EXPIRED_LINK")));
        }

        var authentifier = HashPassword(dto.NewPassword);
        await loginCommand.ResetPassword(dto.Token, authentifier.Password);

        return Result.Success(
            Info.Success(
                Message.GetResource("InfoMessages", "CLIENT_RESETPASSWORD_SUCCESS")));
    }

    public async Task<IResult> ForgotPassword(PasswordResetRequestDto request)
    {
        var login = await loginQuery.GetByEmail(request.Email);

        if (login == null)
        {
            return Result.Failure(
                Error.NullValue(string.Format(
                    Message.GetResource("ErrorMessages", "CLIENT_NOTFOUND"), "Login")));
        }

        var token = tokenProvider.GenerateVerificationToken();

        await SavePasswordResetToken(login.Value, token);
        await SendForgotPasswordEmail(login.Value.Email, token);

        return Result.Success(
            Info.Success(
                Message.GetResource("InfoMessages", "CLIENT_EMAIL_VERIFICATION")));
    }
    #endregion

    #region Private Methods
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

        var hash = EncryptionHelper.HashString(secret, password, salt);

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
        await loginCommand.UpdateResetPasswordToken(login);
    }

    private async Task SendForgotPasswordEmail(string email, string token)
    {
        var resetLink = $"https://yourapp.com/reset-password?token={token}";
        var resetPasswordTemplate = Message.GetResource("Templates", "RESET_PASSWORD");
        var templateContent = EmailService.ReplaceLinkContent(resetPasswordTemplate, resetLink, "resetLink");
        var subject = configuration["Email:ResetPwdSubject"];

        await emailService.SendEmailAsync(email, templateContent, subject);
    }
    #endregion
}
