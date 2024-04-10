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
    public class Example_AuthenticationScreen : MonoBehaviour
    {
        public bool useDebug = true;
        public TMPro.TMP_InputField inputDeviceID;
        public Button btnLoginGuest;
        public TMPro.TMP_InputField inputCode;
        public TMPro.TMP_InputField inputEmail;
        public TMPro.TMP_InputField inputPassword;
        public Button btnSendVerification;
        public Button btnCheckVerification;
        public Button btnCheckEmailUsed;
        public Button btnCheckSMTP;
        public Button btnSignUpCustom;
        public Button btnLoginCustom;
        public Button btnForgetPassword;

        public Button btnLoginGoogle;
        public Button btnLoginApple;

        public Transform loginWidget;
        public TextMeshProUGUI playerName;
        public Button btnGoToUser;

        private void Awake()
        {
            loginWidget.gameObject.SetActive(false);
        }

        private void Start()
        {
            inputDeviceID.text = SystemInfo.deviceUniqueIdentifier;
            btnLoginGuest.onClick.AddListener(OnClickLoginGuest);
            btnSendVerification.onClick.AddListener(OnClickSendVerification);
            btnCheckVerification.onClick.AddListener(OnClickCheckVerification);
            btnCheckEmailUsed.onClick.AddListener(OnClickCheckEmailUsed);
            btnCheckSMTP.onClick.AddListener(OnClickCheckSMTP);
            btnSignUpCustom.onClick.AddListener(OnClickSignUpCustom);
            btnLoginCustom.onClick.AddListener(OnClickLoginCustom);
            btnForgetPassword.onClick.AddListener(OnClickForgetPassword);
            btnLoginGoogle.onClick.AddListener(OnClickLoginGoogle);
            btnLoginApple.onClick.AddListener(OnClickLoginApple);
            btnGoToUser.onClick.AddListener(OnClickGoToUser);
        }

        private void OnClickGoToUser()
        {
            SceneManager.LoadScene("UserScreen");
        }

        private void OnClickLoginGuest()
        {
            PrototypeAPI.LoginGuest(
                new GuestLoginRequest()
                {
                    deviceID = inputDeviceID.text
                },
                (result) =>
                {
                    ProcessLogin(result);
                },
                OnGeneralError
            );
        }

        private void OnClickSendVerification()
        {
            PrototypeAPI.SendEmailVerificationCode(
                new EmailVerificationRequest()
                {
                    email = inputEmail.text
                },
                OnGeneralResult, 
                OnGeneralError
            );
        }

        private void OnClickCheckVerification()
        {
            PrototypeAPI.HandleEmailVerificationCode(
                new EmailVerificationRequest()
                {
                    email = inputEmail.text,
                    code = inputCode.text
                },
                OnGeneralResult,
                OnGeneralError
            );
        }

        private void OnClickCheckEmailUsed()
        {
            PrototypeAPI.CheckEmailValidity(
                new EmailRequest()
                {
                    email = inputEmail.text,
                },
                OnGeneralResult,
                OnGeneralError
            );
        }

        private void OnClickCheckSMTP()
        {
            PrototypeAPI.CheckEmailAvailability(
                new EmailRequest()
                {
                    email = inputEmail.text,
                },
                OnGeneralResult,
                OnGeneralError
            );
        }

        private void OnClickSignUpCustom()
        {
            PrototypeAPI.SignUpCustom(
                new LoginCustomRequest()
                {
                    email = inputEmail.text,
                    password = inputPassword.text
                },
                (result) =>
                {
                    ProcessLogin(result);
                },
                OnGeneralError
            );
        }

        private void OnClickLoginCustom()
        {
            PrototypeAPI.LoginCustom(
                new LoginCustomRequest()
                {
                    email = inputEmail.text,
                    password = inputPassword.text
                },
                (result) =>
                {
                    ProcessLogin(result);
                },
                OnGeneralError
            );
        }

        private void OnClickForgetPassword()
        {
            PrototypeAPI.SendResetPassword(
                new EmailRequest()
                {
                    email = inputEmail.text,
                },
                OnGeneralResult,
                OnGeneralError
            );
        }

        private void OnClickLoginGoogle()
        {
            PrototypeAPI.LoginGoogle(
                (result) =>
                {
                    ProcessLogin(result);
                },
                OnGeneralError
            );
        }

        private void OnClickLoginApple()
        {
            PrototypeAPI.LoginApple(
                (result) =>
                {
                    ProcessLogin(result);
                },
                OnGeneralError
            );
        }

        private void ProcessLogin(LoginResponse result)
        {
            PrototypeHttp.accessToken = result.accessToken;
            PrototypeHttp.refreshToken = result.refreshToken;

            PrototypeAPI.GetUserInfo((result) =>
            {
                loginWidget.gameObject.SetActive(true);
                playerName.text = result.displayName;
            }, OnGeneralError);
        }

        public void OnGeneralResult(GeneralResult result)
        {
            Debug.Log($"[{result.httpCode}] -> {result.message}");
        }
        public void OnGeneralError(ErrorResult error)
        {
            Debug.LogError($"[{error.httpCode}] -> {error.error}");
        }
    }
}