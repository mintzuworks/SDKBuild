using Cysharp.Threading.Tasks;
using Mintzuworks.Domain;
using Mintzuworks.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Mintzuworks.FakeSample
{
    public class FakeLoadingScreenManager : MonoBehaviour
    {
        public Slider loadingProgress;
        public TextMeshProUGUI loadingInfo;

        private int totalTasks = 5; // Total number of tasks
        private int completedTasks = 0; // Track completed tasks
        public ServerConfig serverConfig;
        private async UniTaskVoid Start()
        {
            try
            {
                ClientAPI.BaseURL = serverConfig.serverURL;
                await PingCheck();
                TrackProgress();
                await UniTask.NextFrame();

                await VersionCheck();
                TrackProgress();
                await UniTask.NextFrame();

                await ServerCheck();
                TrackProgress();
                await UniTask.NextFrame();

                await PreloadShaders();
                TrackProgress();
                await UniTask.NextFrame();

                await CheckForAddressableUpdates();
                TrackProgress();
                await UniTask.NextFrame();

                GeneralWrite("All success.. Getting into the game");
            }

            catch (ResponseException ex)
            {
                // Handle custom error message and response details
                GeneralWrite($"App error: {ex.Message}, Status Code: {ex.HttpCode}, Error: {ex.Error}");
            }

            catch (Exception ex)
            {
                // Handle any other generic exceptions
                GeneralWrite($"App error: {ex.Message}");
            }
        }

        #region Ping Check
        private async UniTask PingCheck()
        {
            GeneralWrite("Checking ping...");

            var startTime = Time.realtimeSinceStartup;
            var result = await ClientAPI.GetPing();
            var endTime = Time.realtimeSinceStartup;
            var pingInMs = (endTime - startTime) * 1000;

            if (result != null)
            {
                GeneralWrite($"Ping: {pingInMs:F2} ms");
            }
            else
            {
                GeneralWrite("Failed to check ping.");
            }
        }
        #endregion

        #region Version Check
        private async UniTask VersionCheck()
        {
            GeneralWrite("Checking apps version...");
            var result = await ClientAPI.GetExternalTitleData(
                new GetTitleDataRequest()
                {
                    keys = new List<string>()
                    {
                        GetVersionKey()
                    }
                }
            );

            if (result != null)
            {
                if (result.titleDataValues.TryGetValue(GetVersionKey(), out var gameVersionValue))
                {
                    var gameStatusValue = JsonConvert.DeserializeObject<GameVersionResultData>(gameVersionValue.ToString());
                    Debug.Log($"Current [{gameStatusValue.currentVersion}] || Min [{gameStatusValue.minRequiredVersion}]");
                    GeneralWrite("Version checked");
                }
            }
        }
        #endregion

        #region Server Check
        private async UniTask ServerCheck()
        {
            GeneralWrite("Checking server status...");
            var result = await ClientAPI.GetExternalTitleData(
                new GetTitleDataRequest()
                {
                    keys = new List<string>()
                    {
                        GameStatusKey()
                    }
                }
            );

            if (result != null)
            {
                if (result.titleDataValues.TryGetValue(GameStatusKey(), out var gameVersionValue))
                {
                    var gameStatusValue = JsonConvert.DeserializeObject<GameStatusResultData>(gameVersionValue.ToString());
                    if (gameStatusValue.status == 500)
                    {
                        GeneralWrite("Server not healthy : " + gameStatusValue.reason);
                        throw new ResponseException(500, gameStatusValue.reason, "");
                    }
                    else
                    {
                        GeneralWrite("Server healthy");
                    }
                }
            }
        }
        #endregion

        #region Shader Compiling
        public ShaderVariantCollection shaderVariants;
        private async UniTask PreloadShaders()
        {
            if (shaderVariants != null)
            {
                // Since WarmUp must be on the main thread, you don't need to switch to a thread pool here
                GeneralWrite("Compiling shader...");
                shaderVariants.WarmUp();  // This should be on the main thread
                GeneralWrite("Shader preloading complete");
            }
            else
            {
                Debug.LogWarning("No shader variants to preload.");
            }
        }
        #endregion

        #region Check Catalog
        private async UniTask CheckForAddressableUpdates()
        {
            GeneralWrite("Checking catalog...");

            AsyncOperationHandle<List<string>> checkHandle = Addressables.CheckForCatalogUpdates();
            await checkHandle.Task;

            if (checkHandle.Status == AsyncOperationStatus.Succeeded && checkHandle.Result.Count > 0)
            {
                GeneralWrite("New content available, updating...");
                AsyncOperationHandle downloadHandle = Addressables.UpdateCatalogs(checkHandle.Result);
                await downloadHandle.Task;

                GeneralWrite("Content update complete");
            }
            else
            {
                GeneralWrite("No new updates found");
            }
        }

        #endregion

        #region Result Class

        public string GetVersionKey()
        {
            string temp = "APP_VERSION";

#if UNITY_ANDROID
            temp = "APP_VERSION_ANDROID";

#elif UNITY_IOS
            temp = "APP_VERSION_IOS";
#else
            temp = "APP_VERSION_ANDROID";
#endif
            return temp;
        }


        public string GameStatusKey()
        {
            string temp = "GAME_STATUS";

#if UNITY_ANDROID
            temp = "GAME_STATUS_ANDROID";

#elif UNITY_IOS
            temp = "GAME_STATUS_IOS";
#else
            temp = "GAME_STATUS_ANDROID";
#endif
            return temp;
        }

        [System.Serializable]
        public class GameVersionResultData
        {
            public string currentVersion;
            public string minRequiredVersion;
        }


        [System.Serializable]
        public class GameStatusResultData
        {
            public string reason;
            public int status;
        }

        #endregion

        #region Generic
        // Helper method to wrap a task and track its progress
        private void TrackProgress()
        {
            completedTasks++;
            float progress = ((float)completedTasks / totalTasks) * 100;
            loadingProgress.value = progress;
        }

        public void OnGeneralError(ErrorResult error)
        {
            Debug.LogError($"[{error.httpCode}] -> {error.error}");
        }

        private void GeneralWrite(string text)
        {
            Debug.Log(text);
            if (loadingInfo) loadingInfo.text = text;
        }
        #endregion
    }

}