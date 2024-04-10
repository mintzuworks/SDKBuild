using Mintzuworks.Domain;
using Mintzuworks.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Utilities.WebRequestRest;

namespace Mintzuworks.Network
{
    public class PrototypeHttp
    {
        public static bool useDebug = true;
        private const string content_type = "Content-Type";
        private const string authorization = "Authorization";
        private const string content_length = "Content-Length";
        private const string application_json = "application/json";
        private const string application_octet_stream = "application/octet-stream";
        private const string x_auth = "x-auth";
        private const string x_permission = "x-permission";

        public static string x_auth_key;
        public static string x_permission_key;
        public static string accessToken;
        public static string refreshToken;
        public static string Bearertoken => $"Bearer {accessToken}";
        //Global Event
        public static event System.Action OnRequestFailed;

        public static Dictionary<string, string> Headers = new Dictionary<string, string>()
        {
            { content_type, application_json },
        };
        private static RestParameters CreateHeader(bool isOAuth, bool isCrucial)
        {
            if (!isOAuth && !isCrucial) return null;

            if (isCrucial)
            {
                PermissionData reqHeader = new PermissionData();
                reqHeader.timestamp = DateTime.Now;
                reqHeader.nonce = PrototypeUtils.GenerateRandomCode(16);
                var json = JsonConvert.SerializeObject(reqHeader);
                var enc = PrototypeUtils.EncryptToBase64(json, x_permission_key);

                if (Headers.ContainsKey(x_auth)) Headers[authorization] = x_auth_key;
                else Headers.Add(x_auth, x_auth_key);

                if (Headers.ContainsKey(x_permission)) Headers[authorization] = enc;
                else Headers.Add(x_permission, enc);
            }
            else
            {
                Headers.Remove(x_auth);
                Headers.Remove(x_permission);
            }

            if (isOAuth)
            {
                if(Headers.ContainsKey(authorization)) Headers[authorization] = Bearertoken;
                else Headers.Add(authorization, Bearertoken);
            }
            else
            {
                Headers.Remove(authorization);
            }

            return new RestParameters(Headers);
        }

        public static async void Post<TRequest, TResult>(string url, TRequest body,
            System.Action<TResult> OnSuccess = null,
            System.Action<ErrorResult> OnFailed = null, 
            bool useOAuth = true, bool isCrucial = false)
            where TResult : CommonResult
        {
            RestParameters restParameters = CreateHeader(useOAuth, isCrucial);
            var response = await Rest.PostAsync(url, JsonConvert.SerializeObject(body), parameters: restParameters);
            if (response.Successful) ProcessSuccessfulResponse(OnSuccess, OnFailed, response);
            else ProcessUnsuccessfulResponse(OnFailed, response);
        }

        public static async void Put<TRequest, TResult>(string url, TRequest body,
            System.Action<TResult> OnSuccess = null,
            System.Action<ErrorResult> OnFailed = null,
            bool useOAuth = true, bool isCrucial = false)
            where TResult : CommonResult
        {
            RestParameters restParameters = CreateHeader(useOAuth, isCrucial);
            var response = await Rest.PutAsync(url, JsonConvert.SerializeObject(body), parameters: restParameters);
            if (response.Successful) ProcessSuccessfulResponse(OnSuccess, OnFailed, response);
            else ProcessUnsuccessfulResponse(OnFailed, response);
        }

        public static async void Get<TResult>(string url, 
            System.Action<TResult> OnSuccess = null,
            System.Action<ErrorResult> OnFailed = null,
            bool useOAuth = true, bool isCrucial = false) 
            where TResult : CommonResult
        {
            RestParameters restParameters = CreateHeader(useOAuth, isCrucial);
            var response = await Rest.GetAsync(url, parameters: restParameters);
            if (response.Successful) ProcessSuccessfulResponse(OnSuccess, OnFailed, response);
            else ProcessUnsuccessfulResponse(OnFailed, response);
        }

        private static void ProcessSuccessfulResponse<TResult>(Action<TResult> OnSuccess, Action<ErrorResult> OnFailed, Response response) where TResult : CommonResult
        {
            response.Validate(true);
            var deserializedResult = JsonConvert.DeserializeObject<TResult>(response.Body);
            if (deserializedResult != null)
            {
                deserializedResult.httpCode = response.Code;
                OnSuccess?.Invoke(deserializedResult);
            }
            else
            {
                OnFailed?.Invoke(new ErrorResult()
                {
                    httpCode = 500,
                    error = "deserialize error",
                });
            }
        }
        private static void ProcessUnsuccessfulResponse(Action<ErrorResult> OnFailed, Response response)
        {
            OnFailed?.Invoke(new ErrorResult()
            {
                httpCode = response.Code,
                error = response.Error,
                message = response.Body
            });
        }
    }

    public class PermissionData
    {
        public DateTime timestamp;
        public string nonce;
    }
}