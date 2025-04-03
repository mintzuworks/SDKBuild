using Cysharp.Threading.Tasks;  // Add UniTask namespace
using Mintzuworks.Domain;
using Mintzuworks.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.WebRequestRest;

namespace Mintzuworks.Network
{
    public class PrototypeHttp
    {
        public static bool useDebug = true;
        private const string content_type = "Content-Type";
        private const string authorization = "Authorization";
        private const string device = "Device";
        private const string content_length = "Content-Length";
        private const string application_json = "application/json";
        private const string application_octet_stream = "application/octet-stream";
        private const string x_sign = "X-Signature";
        private const string x_permission = "x-permission";

        public static string x_auth_key;
        public static string x_permission_key;
        public static string accessToken;
        public static string refreshToken;
        public static string Bearertoken => $"Bearer {accessToken}";

        public static System.Action OnRequestFailed { get; set; }

        public static Dictionary<string, string> Headers = new Dictionary<string, string>()
        {
            { content_type, application_json },
        };

        private static RestParameters CreateHeader<T>(bool isOAuth, bool isCrucial, T request)
        {
            if (!isOAuth && !isCrucial) return null;

            if (isCrucial)
            {
                var reqHeader = new SignedData<T>
                {
                    time = PrototypeUtils.TimeEpoch(),
                    data = request
                };

                var json = JsonConvert.SerializeObject(reqHeader);
                var encrypted = AesCFBUtil.EncryptToBase64(json, accessToken);
                Headers[x_sign] = encrypted;
            }
            else
            {
                Headers.Remove(x_sign);
            }

            if (isOAuth)
            {
                Headers[authorization] = Bearertoken;
                Headers[device] = SystemInfo.deviceUniqueIdentifier;
            }
            else
            {
                Headers.Remove(authorization);
                Headers.Remove(device);
            }
            return new RestParameters(Headers);
        }

        // PUT method refactored to use UniTask and return TResult directly
        public static async UniTask<TResult> Put<TRequest, TResult>(string url, TRequest body,
            bool useOAuth = true, bool isCrucial = false)
            where TResult : CommonResult, new()
        {
            // Create the headers for the request
            RestParameters restParameters = CreateHeader(useOAuth, isCrucial, body);

            // Await the PostAsync call, which returns a Task<Response>
            var response = await Rest.PutAsync(url, JsonConvert.SerializeObject(body), parameters: restParameters);

            // Process the response and return the result or throw an exception in case of failure
            return ProcessResponse<TResult>(response);
        }

        // POST method refactored to use UniTask and return TResult directly
        public static async UniTask<TResult> Post<TRequest, TResult>(string url, TRequest body,
            bool useOAuth = true, bool isCrucial = false)
            where TResult : CommonResult, new()
        {
            // Create the headers for the request
            RestParameters restParameters = CreateHeader(useOAuth, isCrucial, body);

            // Await the PostAsync call, which returns a Task<Response>
            var response = await Rest.PostAsync(url, JsonConvert.SerializeObject(body), parameters: restParameters);

            // Process the response and return the result or throw an exception in case of failure
            return ProcessResponse<TResult>(response);
        }

        // GET method refactored to use UniTask and return TResult directly
        public static async UniTask<TResult> Get<TResult>(string url, bool useOAuth = true, bool isCrucial = false)
            where TResult : CommonResult, new()
        {
            RestParameters restParameters = CreateHeader(useOAuth, isCrucial, default(TResult));

            // Await the GetAsync call
            var response = await Rest.GetAsync(url, parameters: restParameters);

            // Process the response
            return ProcessResponse<TResult>(response);
        }

        // Process the response and return the result directly or throw an exception
        private static TResult ProcessResponse<TResult>(Response response) where TResult : CommonResult, new()
        {
            // Deserialize and return the successful result
            response.Validate(true);

            if (response.Successful)
            {
                var deserializedResult = JsonConvert.DeserializeObject<TResult>(response.Body);

                if (deserializedResult != null)
                {
                    deserializedResult.httpCode = response.Code;
                    return deserializedResult;
                }
            }

            // Return a TResult with just the httpCode set to 500
            return new TResult
            {
                httpCode = response.Code,
            };
        }
    }
}
public class ResponseException : Exception
{
    public int HttpCode { get; }
    public string Error { get; }
    public string ResponseBody { get; }

    public ResponseException(int httpCode, string error, string responseBody)
        : base($"Request failed with status code {httpCode}: {error}")
    {
        HttpCode = httpCode;
        Error = error;
        ResponseBody = responseBody;
    }
}