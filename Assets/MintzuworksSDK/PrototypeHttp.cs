using Mintzuworks.Domain;
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
    public class PrototypeHttp : Singleton<PrototypeHttp>
    {
        public static bool useDebug = true;

        private const string content_type = "Content-Type";
        private const string content_length = "Content-Length";
        private const string application_json = "application/json";
        private const string application_octet_stream = "application/octet-stream";

        //Global Event
        public static event System.Action OnRequestFailed;

        public static Dictionary<string, string> Headers = new Dictionary<string, string>()
        {
            { content_type, application_json },
        };

        public static async void Post<TRequest, TResult>(string url, TRequest body,
            System.Action<TResult> OnSuccess = null,
            System.Action<ErrorResult> OnFailed = null, 
            bool useOAuth = true)
            where TResult : CommonResult
        {
            RestParameters restParameters = !useOAuth ? null : new RestParameters(Headers);
            var response = await Rest.PostAsync(url, JsonConvert.SerializeObject(body), parameters: restParameters);
            if (response.Successful) ProcessSuccessfulResponse(OnSuccess, OnFailed, response);
            else ProcessUnsuccessfulResponse(OnFailed, response);
        }

        public static async void Put<TRequest, TResult>(string url, TRequest body,
            System.Action<TResult> OnSuccess = null,
            System.Action<ErrorResult> OnFailed = null,
            bool useOAuth = true)
            where TResult : CommonResult
        {
            RestParameters restParameters = !useOAuth ? null : new RestParameters(Headers);
            var response = await Rest.PutAsync(url, JsonConvert.SerializeObject(body), parameters: restParameters);
            if (response.Successful) ProcessSuccessfulResponse(OnSuccess, OnFailed, response);
            else ProcessUnsuccessfulResponse(OnFailed, response);
        }

        public static async void Get<TResult>(string url, 
            System.Action<TResult> OnSuccess = null,
            System.Action<ErrorResult> OnFailed = null,
            bool useOAuth = true) 
            where TResult : CommonResult
        {
            RestParameters restParameters = !useOAuth ? null : new RestParameters(Headers);
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
                OnSuccess(deserializedResult);
            }
            else
            {
                OnFailed?.Invoke(new ErrorResult()
                {
                    HttpCode = 500,
                    Error = "deserialize error",
                });
            }
        }
        private static void ProcessUnsuccessfulResponse(Action<ErrorResult> OnFailed, Response response)
        {
            OnFailed?.Invoke(new ErrorResult()
            {
                HttpCode = response.Code,
                Error = response.Error,
            });
        }

    }
}