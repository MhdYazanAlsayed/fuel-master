using FuelMaster.Website.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace FuelMaster.Website.Infrastructure.Services;

public class EncryptionService : IEncryptionService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public EncryptionService(IConfiguration configuration)
    {
        var encryptionKey = configuration["Encryption:Key"];
        if (string.IsNullOrEmpty(encryptionKey))
        {
            // Generate a default key for development (NOT for production!)
            encryptionKey = "DefaultEncryptionKey32Chars!!"; // Must be 32 characters
        }

        // Ensure key is exactly 32 bytes (256 bits) for AES-256
        var keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
        _key = new byte[32];
        Array.Copy(keyBytes, _key, Math.Min(keyBytes.Length, 32));

        // Generate a fixed IV (in production, use a unique IV per encryption)
        // For simplicity, we'll use a derived IV from the key
        using var sha256 = SHA256.Create();
        _iv = sha256.ComputeHash(_key).Take(16).ToArray();
    }

    public string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
            return string.Empty;

        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        return Convert.ToBase64String(cipherBytes);
    }

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText))
            return string.Empty;

        try
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            var cipherBytes = Convert.FromBase64String(cipherText);
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
        catch
        {
            // Return empty string if decryption fails
            return string.Empty;
        }
    }
}

