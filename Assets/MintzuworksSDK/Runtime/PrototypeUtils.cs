using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Mintzuworks.Domain;
using Newtonsoft.Json;
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


        private const string AuthenticationCredentialKey = "AuthenticationCredential";

        public static void SaveAuthenticationCredential(LoginResponse response)
        {
            // Serialize the response to a JSON string using JsonConvert from Newtonsoft.Json
            string responseJson = JsonConvert.SerializeObject(response);

            // Save to PlayerPrefs
            PlayerPrefs.SetString(AuthenticationCredentialKey, responseJson);
            PlayerPrefs.Save();

            Debug.Log("Login response saved to PlayerPrefs.");
        }

        public static LoginResponse LoadAuthenticationCredential()
        {
            // Load the JSON string from PlayerPrefs
            string responseJson = PlayerPrefs.GetString(AuthenticationCredentialKey);

            // Check if the responseJson is null or empty
            if (string.IsNullOrEmpty(responseJson))
            {
                Debug.LogWarning("No login response found in PlayerPrefs.");
                return null;
            }

            // Deserialize the JSON string to a LoginResponse object using JsonConvert from Newtonsoft.Json
            LoginResponse response = JsonConvert.DeserializeObject<LoginResponse>(responseJson);

            Debug.Log("Login response loaded from PlayerPrefs.");

            return response;
        }


        public static long TimeEpoch()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}