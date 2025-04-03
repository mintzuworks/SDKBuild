using System;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

public static class AesCFBUtil
{
    public static string EncryptToBase64(string plainText, string password)
    {
        var keyBytes = Encoding.UTF8.GetBytes(password);
        Array.Resize(ref keyBytes, 32); // Truncate or zero-pad to 32 bytes (just like Go)

        var iv = new byte[16];
        new SecureRandom().NextBytes(iv);

        var cipher = new BufferedBlockCipher(new CfbBlockCipher(new AesEngine(), 128)); // CFB-128
        cipher.Init(true, new ParametersWithIV(new KeyParameter(keyBytes), iv));

        var inputBytes = Encoding.UTF8.GetBytes(plainText);
        var outputBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
        var length = cipher.ProcessBytes(inputBytes, 0, inputBytes.Length, outputBytes, 0);
        cipher.DoFinal(outputBytes, length);

        // Match Go: IV + CipherBytes → Base64
        var full = new byte[iv.Length + outputBytes.Length];
        Buffer.BlockCopy(iv, 0, full, 0, iv.Length);
        Buffer.BlockCopy(outputBytes, 0, full, iv.Length, outputBytes.Length);

        return Convert.ToBase64String(full);
    }

    public static string DecryptFromBase64(string base64Combined, string password)
    {
        var fullBytes = Convert.FromBase64String(base64Combined);
        var iv = new byte[16];
        var cipherBytes = new byte[fullBytes.Length - iv.Length];
        Buffer.BlockCopy(fullBytes, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullBytes, iv.Length, cipherBytes, 0, cipherBytes.Length);

        var keyBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
        var cipher = new BufferedBlockCipher(new CfbBlockCipher(new AesEngine(), 128));
        cipher.Init(false, new ParametersWithIV(new KeyParameter(keyBytes), iv));

        var outputBytes = new byte[cipher.GetOutputSize(cipherBytes.Length)];
        var length = cipher.ProcessBytes(cipherBytes, 0, cipherBytes.Length, outputBytes, 0);
        cipher.DoFinal(outputBytes, length);

        return Encoding.UTF8.GetString(outputBytes).TrimEnd('\0');
    }
}
