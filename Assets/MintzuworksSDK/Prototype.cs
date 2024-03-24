using Mintzuworks.Domain;
using Mintzuworks.Network;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mintzuworks
{
    public class Prototype : MonoBehaviour
    {
        public string AccessToken;
        public string RefreshToken;

        [Button]
        public void TestLogin()
        {
            PrototypeAPI.LoginGuest(new GuestLoginRequest()
            {
                deviceId = SystemInfo.deviceUniqueIdentifier,
            }, 
            (result) =>
            {
                Debug.Log(JsonConvert.SerializeObject(result));
                AccessToken = result.accessToken;
                RefreshToken = result.refreshToken;
                if (PrototypeHttp.Headers.ContainsKey("Authorization"))
                {
                    PrototypeHttp.Headers["Authorization"] = $"Bearer {AccessToken}";
                }
                else
                {
                    PrototypeHttp.Headers.TryAdd("Authorization", $"Bearer {AccessToken}");
                }
            },
            (error) =>
            {
                Debug.Log(JsonConvert.SerializeObject(error));
            });
        }

        [Button]
        public void TestCall()
        {
            PrototypeAPI.GetUserInfo((result) =>
            {
                Debug.Log(JsonConvert.SerializeObject(result));
            },
            (error) =>
            {
                Debug.Log(JsonConvert.SerializeObject(error));
            });
        }

        [Button]
        public void TestUpdateGameData()
        {
            UpdateGameDataByKeyRequest request = new UpdateGameDataByKeyRequest();
            request.data = new Dictionary<string, object>()
            {
                { 
                    "Test", new GameProgress()
                    {
                        currentMap = "Test aja",
                        currentQuest = "quest_1",
                        gameTimer = 100f
                    }
                }
            };


            Debug.Log(JsonConvert.SerializeObject(request));
            PrototypeAPI.UpdateGameDataByKey(request, (result) =>
            {
                Debug.Log(JsonConvert.SerializeObject(result));
            },
            (error) =>
            {
                Debug.Log(JsonConvert.SerializeObject(error));
            });
        }

        [Button]
        public void TestUpdateDisplayName()
        {
            UpdateGameDataByKeyRequest request = new UpdateGameDataByKeyRequest();
            request.data = new Dictionary<string, object>()
            {
                {
                    "Test", new GameProgress()
                    {
                        currentMap = "Test aja",
                        currentQuest = "quest_1",
                        gameTimer = 100f
                    }
                }
            };


            Debug.Log(JsonConvert.SerializeObject(request));
            PrototypeAPI.UpdateGameDataByKey(request, (result) =>
            {
                Debug.Log(JsonConvert.SerializeObject(result));
            },
            (error) =>
            {
                Debug.Log(JsonConvert.SerializeObject(error));
            });
        }
    }
}
