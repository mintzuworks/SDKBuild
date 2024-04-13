using IngameDebugConsole;
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
    public class Example_UserScreen : MonoBehaviour
    {
        public TextMeshProUGUI txtUserID;
        public TextMeshProUGUI txtDisplayName;
        public TMP_InputField inputNewDisplayName;
        public Button btnUpdateDisplayName;
        public Button btnForgetPassword;
        public Button btnRefreshToken;
        public Button btnCheckSSO;
        public Button btnGoToItemManagement;

        public Button btnLinkWithGoogle;
        public Button btnLinkWithApple;
        public Button btnUnlinkGoogle;
        public Button btnUnlinkApple;
        public Button btnVerifyLink;

        public Button btnIAP;
        public Button btnLogout;

        private GetUserInfoResult currentUser;
        private void Start()
        {
            btnUpdateDisplayName.onClick.AddListener(OnClickUpdateDisplayName);
            btnForgetPassword.onClick.AddListener(OnClickForgetPassword);
            btnRefreshToken.onClick.AddListener(OnClickRefreshToken);
            btnCheckSSO.onClick.AddListener(OnClickCheckSSO);
            btnLogout.onClick.AddListener(OnClickLogout);
            btnGoToItemManagement.onClick.AddListener(OnClickGoToItemManagement);

            btnLinkWithGoogle.onClick.AddListener(OnClickLinkWithGoogle);
            btnLinkWithApple.onClick.AddListener(OnClickLinkWithApple);
            btnVerifyLink.onClick.AddListener(OnClickVerifyLink);
            btnUnlinkGoogle.onClick.AddListener(OnClickUnlinkGoogle);
            btnUnlinkApple.onClick.AddListener(OnClickUnlinkApple);
            btnIAP.onClick.AddListener(GoToIAP);

            GetUserInfo();
            GetLinkedAccounts();
        }

        private void GoToIAP()
        {
            SceneManager.LoadScene("IapScreen");
        }

        private void OnClickUnlinkGoogle()
        {
            PrototypeAPI.SocialUnlinking(new SocialUnlinkingRequest()
            {
                provider = SocialProvider.Google.ToString().ToLower(),
            },

            result =>
            {
                OnGeneralResult(result);
                GetLinkedAccounts();
            }, OnGeneralError);
        }

        private void OnClickUnlinkApple()
        {
            PrototypeAPI.SocialUnlinking(new SocialUnlinkingRequest()
            {
                provider = SocialProvider.Apple.ToString().ToLower(),
            },
            result =>
            {
                OnGeneralResult(result);
                GetLinkedAccounts();
            }, OnGeneralError);
        }

        private void OnClickLinkWithGoogle()
        {
            isLinkToThirdParty = true;

            var request = new StartAuthenticationRequest()
            {
                deviceID = SystemInfo.deviceUniqueIdentifier,
                eventName = "link",
                userID = currentUser.userID,
            };
            PrototypeAPI.LinkGoogle(request);
        }

        private void OnClickLinkWithApple()
        {
            isLinkToThirdParty = true;
            var request = new StartAuthenticationRequest()
            {
                deviceID = SystemInfo.deviceUniqueIdentifier,
                eventName = "link",
                userID = currentUser.userID,
            };
            PrototypeAPI.LinkApple(request);
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                //Debug.Log("After login you can make pool here to automatically logged in!");
                if (isLinkToThirdParty)
                {
                    isLinkToThirdParty = false;
                    OnClickVerifyLink();
                }
                //You can m
            }
        }
        bool isLinkToThirdParty;
        private void OnClickVerifyLink()
        {
            PrototypeAPI.SocialLink(new SocialLoginRequest()
            {
                id = currentUser.userID,
            }, 
            result =>
            {
                if(result.result)
                {
                    Debug.Log("[Link] => Success");
                } 
                else
                {
                    Debug.Log("[Link] => Failed");
                }

                GetLinkedAccounts();
            }, OnGeneralError);
        }

        private void OnClickGoToItemManagement()
        {
            SceneManager.LoadScene("ItemScreen");
        }

        private void OnClickCheckSSO()
        {
            PrototypeAPI.GetUserInfo((result) =>
            {
                if (result.lastDeviceID != SystemInfo.deviceUniqueIdentifier)
                {
                    Debug.Log($"[SSO] => Result ID : {result.lastDeviceID}");
                    Debug.Log($"[SSO] => Your ID : {SystemInfo.deviceUniqueIdentifier}");
                    Debug.Log("[SSO] => ID doesn't match. Logging out");
                }
                else
                {
                    Debug.Log("[SSO] => ID match. Go on");
                }
            }, OnGeneralError);
        }

        private void GetUserInfo()
        {
            PrototypeAPI.GetUserInfo((result) =>
            {
                txtDisplayName.text = "Display Name : " + result.displayName;
                txtUserID.text = "User ID : " + result.userID;
                currentUser = result;
            }, OnGeneralError);
        }

        private void GetLinkedAccounts()
        {
            PrototypeAPI.GetLinkedAccounts((result) =>
            {
                if (result.data == null) return;

                if (result.data.ContainsKey(SocialProvider.Google.ToString().ToLower()))
                {
                    btnLinkWithGoogle.interactable = false;
                }

                if (result.data.ContainsKey(SocialProvider.Apple.ToString().ToLower()))
                {
                    btnLinkWithApple.interactable = false;
                }
            }, OnGeneralError);
        }
        private void OnClickUpdateDisplayName()
        {
            PrototypeAPI.UpdateDisplayName(new UpdateDisplayNameRequest()
            {
                displayName = inputNewDisplayName.text,
            }, (result) =>
            {
                OnGeneralResult(result);
                GetUserInfo();
            }, OnGeneralError);
        }

        private void OnClickForgetPassword()
        {
            if (string.IsNullOrEmpty(currentUser.email))
            {
                Debug.Log("Current user doesn't have email");
                return;
            }

            PrototypeAPI.SendResetPassword(new EmailRequest()
            {
                email = currentUser.email,
            }, OnGeneralResult, OnGeneralError);
        }

        private void OnClickRefreshToken()
        {
            PrototypeAPI.RefreshAccessToken(new RefreshTokenRequest()
            {
                refreshToken = PrototypeHttp.refreshToken,
            }, (result) =>
            {
                PrototypeHttp.accessToken = result.accessToken;
            }, OnGeneralError);
        }

        private void OnClickLogout()
        {
            PrototypeHttp.accessToken = string.Empty;
            PrototypeHttp.refreshToken = string.Empty;
            SceneManager.LoadScene("AuthScreen");
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