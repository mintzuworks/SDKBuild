using Mintzuworks.Domain;
using Mintzuworks.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mintzuworks.Example
{
    public class Example_UserManagementScreen : MonoBehaviour
    {
        public TMP_InputField inputCustomDataKey;
        public TMP_InputField inputCustomDataValue;
        public Button btnGetCustomData;
        public Button btnSetCustomData;
        public Button btnGetBan;
        public Button btnGetClaimedCoupons;
        public Button btnGetPurchaseHistory;
        public Button btnGetGachaProgression;
        public Button btnGetMailInbox;
        public TMP_InputField inputLeaderboardID;
        public Button btnGetLeaderboardData;
        public Button btnLogout;

        private void Start()
        {
            btnGetCustomData.onClick.AddListener(OnClickGetCustomData);
            btnSetCustomData.onClick.AddListener(OnClickSetCustomData);
            btnGetBan.onClick.AddListener(OnClickGetBan);
            btnGetClaimedCoupons.onClick.AddListener(OnClickGetClaimedCoupons);
            btnGetPurchaseHistory.onClick.AddListener(OnClickGetPurchaseHistory);
            btnGetGachaProgression.onClick.AddListener(OnClickGetGachaProgression);
            btnGetMailInbox.onClick.AddListener(OnClickGetMailInbox);
            btnGetLeaderboardData.onClick.AddListener(OnClickGetLeaderboardData);
            btnLogout.onClick.AddListener(OnClickLogout);
        }

        private void OnClickLogout()
        {
            SceneManager.LoadScene("UserScreen");
        }

        private void OnClickGetCustomData()
        {
            PrototypeAPI.GetCustomData(new GetCustomDataByKeyRequest()
            {
                keys = new List<string>()
                {
                    inputCustomDataKey.text
                }
            }, (result) =>
            {
                Debug.Log(result);
            }, OnGeneralError);
        }

        private void OnClickSetCustomData()
        {
            PrototypeAPI.UpdateGameDataByKey(new UpdateGameDataByKeyRequest()
            {
                data = new Dictionary<string, object>()
                {
                    { inputCustomDataKey.text, inputCustomDataValue.text }
                }
            }, 
            
            (result) =>
            {
                Debug.Log(result);
            }, OnGeneralError);
        }

        private void OnClickGetBan()
        {
            PrototypeAPI.GetBanInfo(OnError: OnGeneralError);
        }

        private void OnClickGetClaimedCoupons()
        {
            PrototypeAPI.GetClaimedCoupons(OnError: OnGeneralError);
        }

        private void OnClickGetPurchaseHistory()
        {
            PrototypeAPI.GetPurchaseHistory(OnError: OnGeneralError);
        }

        private void OnClickGetGachaProgression()
        {
            PrototypeAPI.GetPrizeTableProgression(OnError: OnGeneralError);
        }

        private void OnClickGetMailInbox()
        {
            //PrototypeAPI.ge(OnError: OnGeneralError);
        }

        private void OnClickGetLeaderboardData()
        {
            //throw new NotImplementedException();
        }

        public void OnGeneralResult(GeneralResult result)
        {
            Debug.Log($"[{result.httpCode}] -> {result.message}");
        }

        public void OnGeneralError(ErrorResult error)
        {
            Debug.LogError($"[{error.httpCode}] -> {error.message}");
        }
    }
}
