using Bogus;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using System.Security.Cryptography;
using System.Text;

namespace CesiZen.Infrastructure.Seeders;

internal static class UserSeeder
{
    internal static Faker<User> FakeGenerator()
    {

        return new Faker<User>()
            .UseSeed(42)
            .RuleFor(i => i.Firstname, f => f.Name.FirstName())
            .RuleFor(i => i.Lastname, f => f.Name.LastName())
            .RuleFor(i => i.Username, (f, i) =>
            {
                var name = i.Firstname + i.Lastname;
                return name + f.IndexFaker + 1;
            })
            .RuleFor(i => i.CreatedAt, f => f.Date.Past(3).ToUniversalTime())
            .RuleFor(i => i.UpdatedAt, f => f.Date.Past(2).ToUniversalTime())
            .RuleFor(i => i.IsActive, f => f.Random.Bool())
            .RuleFor(i => i.Role, f => f.PickRandom("User", "Admin"))
            .RuleFor(i => i.BreathExercises, (f, u) =>
            {
                return BreathExerciseSeeder.FakeGenerator().Generate(f.Random.Int(1, 7))
                                .Select(exercise =>
                                {
                                    exercise.UserId = u.Id;
                                    return exercise;
                                }).ToList();
            });
    }

    public static Faker<Login> FakeLoginGenerator()
    {
        var passwordRegex = @"^(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?])(?=.*[a-zA-Z])(?=.*\d)(?=.{12,})[^\s]+$";

        return new Faker<Login>()
            .UseSeed(42)
            .RuleFor(i => i.Email, f => f.Internet.Email())
            .RuleFor(i => i.EmailVerified, f => f.Random.Bool())
            .RuleFor(i => i.Password, (f, l) =>
            {
                var authentifier = f.Internet.Password(f.Random.Int(12, 16), true, passwordRegex).HashSaltedPassword();
                l.Password = authentifier.Password;
                l.Salt = authentifier.HashSalt;

                return l.Password;
            })
            .RuleFor(i => i.Salt, (f, l) => l.Salt);

    }

    #region Password Hasher
    private static Authentifier HashSaltedPassword(this string password)
    {
        var keySize = GetPasswordKeySize();
        var salt = RandomNumberGenerator.GetBytes(keySize);

        var passwordHash = PasswordHasher(password, salt);
        var hashSalt = HashSalt(salt);

        Authentifier authentifier = new(passwordHash, hashSalt);

        return authentifier;
    }

    private static string PasswordHasher(string password, byte[] salt)
    {
        var secret = "AP4JE5ND3_Z8MFJPE65_DOH30D41S_NEHC47S65";

        var hash = HashString(secret!, password, salt);
        return hash;
    }

    private static string HashSalt(byte[] salt)
    {
        var secret = "AP4JE5ND3_Z8MFJPE65_DOH30D41S_NEHC47S65";
        var saltString = Convert.ToBase64String(salt);

        return Encrypt(saltString + secret);
    }
    #endregion

    #region Encryptor
    private const int passwordKeySize = 64;
    private const int encryptionKeySize = 256;
    private const int BlockSize = 128;
    private const int SaltSize = 16;

    public static string Encrypt(string plainText)
    {
        string EncryptionKey = "abc123";
        byte[] salt = GenerateRandomSalt();
        using (var aes = Aes.Create())
        {
            aes.KeySize = encryptionKeySize;
            aes.BlockSize = BlockSize;

            var key = new Rfc2898DeriveBytes(EncryptionKey, salt, 50000, HashAlgorithmName.SHA256);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(salt, 0, SaltSize);
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                }
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        string EncryptionKey = "abc123";
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        byte[] salt = new byte[SaltSize];
        Array.Copy(cipherBytes, 0, salt, 0, SaltSize);

        using (var aes = Aes.Create())
        {
            aes.KeySize = encryptionKeySize;
            aes.BlockSize = BlockSize;

            var key = new Rfc2898DeriveBytes(EncryptionKey, salt, 50000, HashAlgorithmName.SHA256);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            using (var memoryStream = new MemoryStream(cipherBytes, SaltSize, cipherBytes.Length - SaltSize))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(cryptoStream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }

    private static byte[] GenerateRandomSalt()
    {
        byte[] salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    public static string HashString(string secret, string password, byte[] salt)
    {
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithmName = HashAlgorithmName.SHA512;

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(secret + password),
            salt,
            iterations,
            hashAlgorithmName,
            passwordKeySize
        );

        return Convert.ToHexString(hash);
    }

    public static int GetPasswordKeySize()
    {
        return passwordKeySize;
    }
    #endregion
}
