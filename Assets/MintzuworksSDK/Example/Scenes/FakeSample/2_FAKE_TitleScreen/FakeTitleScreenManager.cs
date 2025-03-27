using Cysharp.Threading.Tasks;
using Mintzuworks.Domain;
using Mintzuworks.Network;
using Mintzuworks.Utils;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mintzuworks.FakeSample
{
    public class FakeTitleScreenManager : MonoBehaviour
    {
        #region Panel Selection
        [Title("Panel Selection")]
        public Transform panelSelection;
        public Button btnChooseEmail;
        public Button btnChooseGuest;
        public Button btnChooseGoogle;
        public Button btnChooseApple;
        #endregion

        #region Panel Logged In
        [Title("Panel Logged In")]
        public Transform panelLoggedIn;
        public Button btnOpenNews;
        public Button btnLogout;
        public Button btnPlay;
        public TextMeshProUGUI welcomeText;
        public Button btnOpenServerList;

        [Title("Panel Server List")]
        public Transform panelServerSelection;
        public List<ServerConfig> serverConfigs;
        #endregion

        #region Panel Loading
        [Title("Panel Loading")]
        public Transform panelLoading;

        public void ShowLoading()
        {
            panelLoading.gameObject.SetActive(true);
        }

        public void HideLoading()
        {
            panelLoading.gameObject.SetActive(false);
        }
        #endregion

        #region Panel Login Email
        [Title("Panel Login Email")]
        public Transform panelLoginEmail;
        public TMP_InputField inputLoginEmail;
        public TMP_InputField inputLoginPassword;
        public Button btnOpenRegister;
        public Button btnOpenForgotPassword;
        public Button btnLogin;
        public Button btnCancelLogin;
        #endregion

        #region Panel Register Email
        [Title("Panel Register Email")]
        public Transform panelVerifyIdentity;
        public Transform panelRegisterEmail;
        public TMP_InputField inputRegisterEmail;
        public TMP_InputField inputRegisterVerifCode;
        public TMP_InputField inputRegisterPassword;
        public TMP_InputField inputRegisterConfirmPassword;
        public Button btnSendVerification;
        public Button btnVerify;
        public Button btnCancelVerify;
        public Button btnRegister;
        public Button btnCancelRegister;
        #endregion

        #region Panel User Agreement 
        [Title("Panel User Agreement")]
        public Transform panelUserAgreement;
        public Toggle toggleTnC;
        public Toggle togglePolicy;
        public Button btnAcceptAgreement;
        public Button btnCancelAgreement;

        public void CheckAgreement()
        {
            btnAcceptAgreement.interactable = toggleTnC.isOn && togglePolicy.isOn;
        }
        #endregion

        #region Panel News
        [Title("Panel News")]
        public Transform panelNews;
        public Button btnCancelNews;
        #endregion
        public ServerConfig serverConfig;
        private LoginMethod loginMethod;
        private async UniTaskVoid Start()
        {
            try
            {
                FakeNewsManager.OnHide = HideLoading;
                FakeNewsManager.OnShow = ShowLoading;

                ClientAPI.BaseURL = serverConfig.serverURL;

                #region User Agreement
                toggleTnC.isOn = false;
                togglePolicy.isOn = false;
                btnAcceptAgreement.interactable = false;

                toggleTnC.onValueChanged.AddListener((val) =>
                {
                    CheckAgreement();
                });

                togglePolicy.onValueChanged.AddListener((val) =>
                {
                    CheckAgreement();
                });
                #endregion

                #region UI Function Callback

                btnPlay.onClick.AddListener(() =>
                {
                    SceneManager.LoadScene("FakeLobby");
                });

                btnChooseEmail.onClick.AddListener(() =>
                {
                    OnChooseEmail();
                });

                btnOpenRegister.onClick.AddListener(() =>
                {
                    panelLoginEmail.gameObject.SetActive(false);
                    panelVerifyIdentity.gameObject.SetActive(true);
                });

                btnOpenForgotPassword.onClick.AddListener(() =>
                {
                    OnClickForgetPassword();
                });

                btnSendVerification.onClick.AddListener(() =>
                {
                    OnClickSendVerification();
                });

                btnCancelLogin.onClick.AddListener(() =>
                {
                    panelLoginEmail.gameObject.SetActive(false);
                    OnBackToSelection();
                });

                btnCancelRegister.onClick.AddListener(() =>
                {
                    panelRegisterEmail.gameObject.SetActive(false);
                    OnBackToSelection();
                });

                btnVerify.onClick.AddListener(() =>
                {
                    OnClickVerify();
                });

                btnCancelVerify.onClick.AddListener(() =>
                {
                    panelVerifyIdentity.gameObject.SetActive(false);
                    OnBackToSelection();
                });

                btnRegister.onClick.AddListener(() =>
                {
                    if (inputRegisterPassword.text != inputRegisterConfirmPassword.text) return;

                    loginMethod = LoginMethod.Email;
                    panelRegisterEmail.gameObject.SetActive(false);
                    panelUserAgreement.gameObject.SetActive(true);
                });

                btnAcceptAgreement.onClick.AddListener(() =>
                {
                    switch (loginMethod)
                    {
                        case LoginMethod.Guest:
                            OnClickLoginGuest(true);
                            break;
                        case LoginMethod.Email:
                            OnClickRegisterEmail();
                            break;
                        case LoginMethod.Google:
                            break;
                        case LoginMethod.Apple:
                            break;
                        default:
                            break;
                    }
                });

                btnOpenNews.onClick.AddListener(() =>
                {
                    panelNews.gameObject.SetActive(true);
                });

                btnCancelNews.onClick.AddListener(() =>
                {
                    panelNews.gameObject.SetActive(false);
                });
                #endregion

                #region Login Methods
                btnLogin.onClick.AddListener(() =>
                {
                    OnClickLoginCustom();
                });

                btnChooseGuest.onClick.AddListener(() =>
                {
                    loginMethod = LoginMethod.Guest;
                    OnClickLoginGuest(false);
                });

                btnChooseGoogle.onClick.AddListener(() =>
                {
                    loginMethod = LoginMethod.Google;
                    OnClickLoginGoogle();
                });

                btnChooseApple.onClick.AddListener(() =>
                {
                    loginMethod = LoginMethod.Apple;
                    OnClickLoginApple();
                });

                btnLogout.onClick.AddListener(() =>
                {
                    PrototypeHttp.accessToken = string.Empty;
                    PrototypeHttp.refreshToken = string.Empty;

                    panelLoggedIn.gameObject.SetActive(false);
                    panelSelection.gameObject.SetActive(true);
                });
                #endregion

                #region Silent Login
                OnSilentLogin();
                #endregion
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

        #region UI Functions
        private void OnChooseEmail()
        {
            panelSelection.gameObject.SetActive(false);
            panelLoginEmail.gameObject.SetActive(true);
        }

        private void OnBackToSelection()
        {
            panelSelection.gameObject.SetActive(true);
        }
        #endregion

        #region Login Methods
        private async void OnClickLoginCustom()
        {
            ShowLoading();
            var result = await ClientAPI.LoginCustom(
                new LoginCustomRequest()
                {
                    email = inputLoginEmail.text,
                    password = inputLoginPassword.text
                }
            );

            HideLoading();
            if (result != null && result.httpCode == (int)HttpStatusCode.OK)
            {
                ProcessLogin(result);
            }
        }

        private async void OnSilentLogin()
        {
            var auth = PrototypeUtils.LoadAuthenticationCredential();
            if (auth != null)
            {
                ShowLoading();

                // Setup the Old Auth
                Debug.Log("Silent login with existing credentials");
                Debug.Log(JsonConvert.SerializeObject(auth));
                PrototypeHttp.accessToken = auth.accessToken;
                PrototypeHttp.refreshToken = auth.refreshToken;

                var result = await ClientAPI.SilentLogin();
                if (result.IsResultOK())
                {
                    ProcessLogin(auth);
                }

                else if (result.IsResultCode(HttpStatusCode.Unauthorized))
                {
                    Debug.Log("Silent login failed, refreshing token");
                    var refreshResult = await ClientAPI.RefreshAccessToken(new RefreshTokenRequest()
                    {
                        refreshToken = auth.refreshToken
                    });

                    if (refreshResult.IsResultOK())
                    {
                        ProcessLogin(refreshResult);
                    }
                }

                else
                {
                    Debug.Log("Silent login failed, clearing credentials");
                    PrototypeUtils.SaveAuthenticationCredential(null);
                }

                HideLoading();
            }
        }

        private async void OnClickLoginGuest(bool force)
        {
            ShowLoading();
            var result = await ClientAPI.LoginGuest(new GuestLoginRequest()
            {
                autoSignup = force,
                deviceID = SystemInfo.deviceUniqueIdentifier
            });

            if (result != null)
            {
                if (result.httpCode == 302)
                {
                    HideLoading();
                    panelSelection.gameObject.SetActive(false);
                    panelUserAgreement.gameObject.SetActive(true);
                }
                else
                {
                    HideLoading();
                    ProcessLogin(result);
                }
            }
        }

        private void OnClickLoginGoogle()
        {
            ShowLoading();
            ClientAPI.LoginGoogle();
            CheckingForCallback().Forget();
        }

        private void OnClickLoginApple()
        {
            ShowLoading();
            ClientAPI.LoginApple();
            CheckingForCallback().Forget();
        }

        private async UniTaskVoid CheckingForCallback()
        {
            while (true)
            {
                await UniTask.WaitForSeconds(1f);
                if (Application.isFocused)
                {
                    var callbackCheck = await CheckVerifySocialAuth();
                    if (callbackCheck)
                    {
                        break;
                    }
                }
            }
        }

        private async void OnClickSendVerification()
        {
            ShowLoading();
            Debug.Log("Checking email availability");
            var emailResult = await ClientAPI.CheckEmailValidity(new EmailRequest()
            {
                email = inputRegisterEmail.text,
            });

            HideLoading();
            if (emailResult != null)
            {
                if (emailResult.httpCode == 302)
                {
                    Debug.Log("Email already used!");
                }
                else
                {
                    var result = await ClientAPI.SendEmailVerificationCode(
                        new EmailVerificationRequest()
                        {
                            email = inputRegisterEmail.text
                        }
                    );

                    HideLoading();
                    if (result != null)
                    {
                        Debug.Log("Email send!");
                        btnSendVerification.interactable = false;

                        await UniTask.RunOnThreadPool(async () =>
                        {
                            await UniTask.WaitForSeconds(3);
                            btnSendVerification.interactable = true;
                        });
                    }
                }
            }
        }

        private async void OnClickVerify()
        {
            ShowLoading();

            var result = await ClientAPI.HandleEmailVerificationCode(
                new EmailVerificationRequest()
                {
                    email = inputRegisterEmail.text,
                    code = inputRegisterVerifCode.text
                }
            );

            HideLoading();
            if (result != null)
            {
                if (result.httpCode == 400 || result.httpCode == 404)
                {
                    Debug.Log("Code doesn't match");
                    return;
                }

                panelVerifyIdentity.gameObject.SetActive(false);
                panelRegisterEmail.gameObject.SetActive(true);
            }
        }

        private async void OnClickForgetPassword()
        {
            ShowLoading();
            var result = await ClientAPI.SendResetPassword(
                new EmailRequest()
                {
                    email = inputLoginEmail.text,
                }
            );
            HideLoading();
            if (result.IsResultOK())
            {
                Debug.Log("Forget Password link sent");
            }
        }

        private async UniTask<bool> CheckVerifySocialAuth()
        {
            var result = await ClientAPI.SocialLogin(new SocialLoginRequest()
            {
                id = SystemInfo.deviceUniqueIdentifier,
            });

            if (result != null && result.httpCode == (int)HttpStatusCode.OK)
            {
                HideLoading();
                ProcessLogin(result);
                return true;
            }

            return false;
        }

        private async void OnClickRegisterEmail()
        {
            ShowLoading();
            var result = await ClientAPI.SignUpCustom(
                new LoginCustomRequest()
                {
                    email = inputRegisterEmail.text,
                    password = inputRegisterPassword.text
                }
            );

            HideLoading();
            if (result != null && result.httpCode == (int)HttpStatusCode.OK)
            {
                ProcessLogin(result);
            }
        }

        private async void ProcessLogin(LoginResponse result)
        {
            PrototypeHttp.accessToken = result.accessToken;
            PrototypeHttp.refreshToken = result.refreshToken;

            // Debug.Log("Access Token valid until " + result.AccessValidUntilDate);
            // Debug.Log("Refresh Token valid until " + result.RefreshValidUntilDate);

            PrototypeUtils.SaveAuthenticationCredential(result);
            ShowLoading();
            GetUserInfoResult getUserResult = await ClientAPI.GetUserInfo();
            HideLoading();
            if (getUserResult != null && getUserResult.httpCode == (int)HttpStatusCode.OK)
            {
                panelSelection.gameObject.SetActive(false);
                panelUserAgreement.gameObject.SetActive(false);
                panelRegisterEmail.gameObject.SetActive(false);
                panelLoginEmail.gameObject.SetActive(false);
                panelLoggedIn.gameObject.SetActive(true);
                welcomeText.text = $"Welcome {getUserResult.displayName}";
            }
        }
        #endregion

        #region Generic

        public enum LoginMethod
        {
            Guest,
            Email,
            Google,
            Apple
        }

        public void OnGeneralError(ErrorResult error)
        {
            Debug.LogError($"[{error.httpCode}] -> {error.error}");
        }

        private void GeneralWrite(string text)
        {
            Debug.Log(text);
        }
        #endregion

        #region Testing
        [Button]
        public async void TestGetData()
        {
            var test = await ClientAPI.GetCustomDataByKey(new GetCustomDataByKeyRequest()
            {
                keys = new List<string>()
                {
                    "Test"
                }
            });

            if (test.IsResultOK())
            {
                if (test.data.ContainsKey("test"))
                    Debug.Log(test.data["Test"]);
            }
        }

        [Button]
        public async void TestUpdateData()
        {
            var test = await ClientAPI.UpdateCustomDataByKey(new UpdateGameDataByKeyRequest()
            {
                data = new Dictionary<string, object>() {
                    {"Test", "Test"}
                }
            });

            if (test.IsResultOK())
            {
                Debug.Log("Updated");
            }
        }
        #endregion
    }

}