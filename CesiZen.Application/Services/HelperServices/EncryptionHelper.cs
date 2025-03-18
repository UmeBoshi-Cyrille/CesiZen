using System.Security.Cryptography;
using System.Text;

namespace CesiZen.Application.Helper;

public static class EncryptionHelper
{
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
}



