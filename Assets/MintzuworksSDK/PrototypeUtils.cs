using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Mintzuworks.Utils
{
    public class PrototypeUtils 
    {
        public static string EncryptToBase64(string text, string salt)
        {
            AesCryptoServiceProvider AEScryptoProvider = new AesCryptoServiceProvider();
            AEScryptoProvider.BlockSize = 128;
            AEScryptoProvider.KeySize = 128;
            AEScryptoProvider.Key = Encoding.ASCII.GetBytes(salt);
            AEScryptoProvider.Mode = CipherMode.ECB;
            AEScryptoProvider.Padding = PaddingMode.PKCS7;

            byte[] txtByteData = Encoding.ASCII.GetBytes(text);
            ICryptoTransform trnsfrm = AEScryptoProvider.CreateEncryptor(AEScryptoProvider.Key, AEScryptoProvider.IV);

            byte[] result = trnsfrm.TransformFinalBlock(txtByteData, 0, txtByteData.Length);
            return Convert.ToBase64String(result);
        }

        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+-=[]{}|<>?";
        public static string GenerateRandomCode(int length)
        {
            var random = new System.Random();
            string code = "";
            for (int i = 0; i < length; i++)
            {
                code += Chars[random.Next(Chars.Length)];
            }
            return code;
        }
    }
}