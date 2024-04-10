using Mintzuworks.Domain;
using Mintzuworks.Network;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mintzuworks.Example
{
    public class Example_LoadingScreen : MonoBehaviour
    {
        public bool useDebug = true;
        public Button btnPing;
        public Button btnCheckMaintenance;
        public Button btnGetGameVersion;
        public Button btnGetServerTime;
        public Button btnGetNews;
        public Button btnGoToAuthenticationExample;

        private void Start()
        {
            btnPing.onClick.AddListener(OnClickPing);
            btnCheckMaintenance.onClick.AddListener(OnClickCheckMaintenance);
            btnGetGameVersion.onClick.AddListener(OnClickGetGameVersion);
            btnGetServerTime.onClick.AddListener(OnClickGetServerTime);
            btnGetNews.onClick.AddListener(OnClickGetNews);
            btnGoToAuthenticationExample.onClick.AddListener(OnClickAuthExample);
            PrototypeHttp.useDebug = useDebug;
        }

        private void OnClickPing()
        {
            PrototypeAPI.GetPing(
                (result) =>
                {
                    Debug.Log("OnResult.Ping!");
                },
                OnGeneralError
            );
        }

        [System.Serializable]
        public class GameStatusResultData
        {
            public string reason;
            public int status;
        }

        private void OnClickCheckMaintenance()
        {
            PrototypeAPI.GetExternalTitleData(
                new GetTitleDataRequest()
                {
                    keys = new List<string>()
                    {
                        "game_status"
                    }
                },
                (result) =>
                {
                    if (result.titleDataValues.TryGetValue("game_status", out var gameStatus))
                    {
                        var gameStatusValue = JsonConvert.DeserializeObject<GameStatusResultData>(gameStatus.ToString());
                        Debug.Log("Status => " + gameStatusValue.status);
                        Debug.Log("Reason => " + gameStatusValue.reason);
                    }
                },
                OnGeneralError
            );
        }

        [System.Serializable]
        public class GameVersionResultData
        {
            public string value;
        }

        private void OnClickGetGameVersion()
        {
            PrototypeAPI.GetExternalTitleData(
                new GetTitleDataRequest()
                {
                    keys = new List<string>()
                    {
                        "game_version"
                    }
                },
                (result) =>
                {
                    if(result.titleDataValues.TryGetValue("game_version", out var gameVersionValue))
                    {
                        var gameStatusValue = JsonConvert.DeserializeObject<GameVersionResultData>(gameVersionValue.ToString());
                        Debug.Log("Current Version => " + gameStatusValue.value);
                    }
                },
                OnGeneralError
            );
        }

        private void OnClickGetServerTime()
        {
            PrototypeAPI.GetServerTime(
                (result) =>
                {
                    Debug.Log("Server time (UTC) => " + result.serverTime);
                },
                OnGeneralError
            );
        }

        private void OnClickGetNews()
        {
            PrototypeAPI.GetAllNews((result) =>
            {
                foreach (var item in result.data)
                {
                    Debug.Log("[News] => " + item.title);
                }
            }, OnGeneralError);
        }

        private void OnClickAuthExample()
        {
            SceneManager.LoadScene("AuthScreen");
        }

        public void OnGeneralError(ErrorResult error)
        {
            Debug.LogError($"[{error.httpCode}] -> {error.error}");
        }

    }
}