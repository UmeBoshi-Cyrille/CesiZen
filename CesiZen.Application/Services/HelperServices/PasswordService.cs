using CesiZen.Application.Helper;
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

    public PasswordService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public Authentifier HashPassword(string password, User? user = null)
    {
        byte[]? salt = null;

        if (user != null)
            salt = GetSalt(user.Login.Salt);

        var authentifier = HashPassword(password, salt);

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

    private Authentifier HashPassword(string password, byte[]? salt = null)
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
}
