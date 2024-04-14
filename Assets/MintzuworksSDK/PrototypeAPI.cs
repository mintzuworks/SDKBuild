using System;
using UnityEngine;
using Mintzuworks.Domain;
using Newtonsoft.Json;
using System.Text;
using UnityEditor.PackageManager.Requests;

namespace Mintzuworks.Network
{
    public static class PrototypeAPI
    {
        private const string BaseURL = "http://localhost:8080/";

        #region Title-Wide Management URL
        private const string GetPingURL = "ping";
        private const string GetServerTimeURL = "time";
        private const string GetAllExternalTitleDataURL = "titleDatas/external";
        private const string GetAllInternalTitleDataURL = "titleDatas/internal";
        private const string GetExternalTitleDataURL = "titleData/external";
        private const string GetInternalTitleDataURL = "titleData/internal";
        private const string GetAllActiveNewsURL = "news/active";
        #endregion

        #region Authentication URL
        private const string SignUpCustomURL = "signup";
        private const string LoginCustomURL = "login";
        private const string LoginGuestURL = "login/guest";
        private const string AuthGoogleURL = "auth/google";
        private const string AuthAppleURL = "auth/apple";
        private const string RefreshTokenURL = "refresh";
        private const string VerifySocialLoginURL = "auth/pool/login";
        private const string VerifySocialLinkURL = "auth/pool/link";
        private const string CheckEmailValidityURL = "validator/userEmail";
        private const string SendResetPasswordURL = "resetpass/send";
        #endregion

        #region Email Verification URL
        private const string CheckEmailAvailibilityURL = "emailverificaton/check";
        private const string SendEmailVerificationCodeURL = "emailverificaton/send";
        private const string HandleEmailVerificationCodeURL = "emailverificaton/handle";
        #endregion

        #region User Management URL
        private const string LinkCustomLoginURL = "user/link/custom";
        private const string SocialUnlinkingURL = "user/unlink";
        private const string GetUserInfoURL = "user/info";
        private const string GetBanInfoURL = "user/banInfo";
        private const string GetLinkedAccountsURL = "user/linkedAccounts";
        private const string GetCustomDataURL = "user/customData";
        private const string GetClaimedCouponsURL = "user/claimedCoupons";
        private const string GetInventoryURL = "user/inventory";
        private const string GetInventoryAggregateURL = "user/inventory/aggregate";
        private const string GetPurchaseHistoryURL = "user/purchaseHistory";
        private const string GetLeaderboardDataURL = "user/leaderboardData";
        private const string GetMailInboxURL = "user/mailInbox";
        private const string GetPrizeTableProgressionURL = "user/prizeTableProgression";
        private const string UpdateDisplayNameURL = "user/displayName";
        private const string UpdateCustomDataByKeyURL = "user/customData";
        #endregion

        #region Item Management URL 
        private const string AddItemURL = "item/add/id";
        private const string SubstractItemURL = "item/substract/id";
        private const string SubstractItemInstanceURL = "item/substract/instance";
        private const string BuyItemURL = "item/buy/id";
        private const string SellItemURL = "item/sell/id";
        private const string DrawPrizeURL = "prizeTable/draw";
        #endregion

        #region IAP URL
        private const string ValidatePlayStoreTransactionURL = "iap/validate/playStore";
        private const string ValidateAppStoreTransactionURL = "iap/validate/appStore";
        private const string InitiatePaypalOrderURL = "iap/paypal/initiate";
        private const string VerifyPaypalTransactionURL = "iap/paypal/pool";
        #endregion


        #region Title-Wide Management API
        public static void GetPing(Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetPingURL, OnSuccess, OnError, useOAuth: false);
        }

        public static void GetServerTime(Action<GetServerTimeResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetServerTimeURL, OnSuccess, OnError, useOAuth: false);
        }

        public static void GetAllExternalTitleData(Action<GetServerTimeResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetAllExternalTitleDataURL, OnSuccess, OnError, useOAuth: false);
        }
        public static void GetAllInternalTitleData(Action<GetTitleDataResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetAllInternalTitleDataURL, OnSuccess, OnError, useOAuth: false);
        }
        public static void GetExternalTitleData(GetTitleDataRequest request, Action<GetTitleDataResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + GetExternalTitleDataURL, request, OnSuccess, OnError, useOAuth: false);
        }
        public static void GetInternalTitleData(GetTitleDataRequest request, Action<GetTitleDataResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + GetInternalTitleDataURL, request, OnSuccess, OnError, useOAuth: false);
        }
        public static void GetAllNews(Action<GetNewsDataResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetAllActiveNewsURL, OnSuccess, OnError, useOAuth: false);
        }
        #endregion

        #region Authentication API
        public static void SignUpCustom(LoginCustomRequest request, Action<LoginResponse> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + SignUpCustomURL, request, OnSuccess, OnError, useOAuth: false);
        }
        public static void LoginCustom(LoginCustomRequest request, Action<LoginResponse> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + LoginCustomURL, request, OnSuccess, OnError, useOAuth: false);
        }
        public static void LoginGuest(GuestLoginRequest request, Action<LoginResponse> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + LoginGuestURL, request, OnSuccess, OnError, useOAuth: false);
        }
        public static void RefreshAccessToken(RefreshTokenRequest request, Action<LoginResponse> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + RefreshTokenURL, request, OnSuccess, OnError, useOAuth: false);
        }

        public static void LoginGoogle(Action<LoginResponse> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            //Encode
            var request = new StartAuthenticationRequest()
            {
                deviceID = SystemInfo.deviceUniqueIdentifier,
                eventName = "login"
            };
            string json = JsonConvert.SerializeObject(request);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            string base64Encoded = Convert.ToBase64String(bytes);
            Application.OpenURL(BaseURL + AuthGoogleURL + "?data=" + base64Encoded);
        }

        public static void LoginApple(Action<LoginResponse> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            //Encode
            var request = new StartAuthenticationRequest()
            {
                deviceID = SystemInfo.deviceUniqueIdentifier,
                eventName = "login"
            };
            string json = JsonConvert.SerializeObject(request);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            string base64Encoded = Convert.ToBase64String(bytes);
            Application.OpenURL(BaseURL + AuthAppleURL + "?data=" + base64Encoded);
        }

        public static void LinkGoogle(StartAuthenticationRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            string json = JsonConvert.SerializeObject(request);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            string base64Encoded = Convert.ToBase64String(bytes);
            Application.OpenURL(BaseURL + AuthGoogleURL + "?data=" + base64Encoded);
        }
        public static void LinkApple(StartAuthenticationRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            string json = JsonConvert.SerializeObject(request);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            string base64Encoded = Convert.ToBase64String(bytes);
            Application.OpenURL(BaseURL + AuthAppleURL + "?data=" + base64Encoded);
        }

        private static void OnSocialLinkingCallbackReceived(string deeplinkURL,
            Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            Application.deepLinkActivated -= (url) => OnSocialLinkingCallbackReceived(url, OnSuccess, OnError);
            string[] parts = deeplinkURL.Split('?');
            if (parts.Length > 1)
            {
                string token = parts[1];
            }
        }

        public static void SocialLogin(SocialLoginRequest request, Action<LoginResponse> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + VerifySocialLoginURL, request, OnSuccess, OnError, useOAuth: false);
        }

        public static void SocialLink(SocialLoginRequest request, Action<CheckSocialRedisLinkResponse> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + VerifySocialLinkURL, request, OnSuccess, OnError, useOAuth: false);
        }

        public static void CheckEmailValidity(EmailRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + CheckEmailValidityURL, request, OnSuccess, OnError, useOAuth: false);
        }
        public static void SendResetPassword(EmailRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + SendResetPasswordURL, request, OnSuccess, OnError, useOAuth: false);
        }
        #endregion

        #region Email Verification API
        public static void CheckEmailAvailability(EmailRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + CheckEmailAvailibilityURL, request, OnSuccess, OnError, useOAuth: false);
        }

        public static void SendEmailVerificationCode(EmailVerificationRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + SendEmailVerificationCodeURL, request, OnSuccess, OnError, useOAuth: false);
        }

        public static void HandleEmailVerificationCode(EmailVerificationRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + HandleEmailVerificationCodeURL, request, OnSuccess, OnError, useOAuth: false);
        }
        #endregion

        #region User Management API
        public static void LinkCustomLogin(LinkCustomLoginRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + LinkCustomLoginURL, request, OnSuccess, OnError);
        }

        public static void SocialUnlinking(SocialUnlinkingRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + SocialUnlinkingURL, request, OnSuccess, OnError);
        }

        public static void GetUserInfo(Action<GetUserInfoResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetUserInfoURL, OnSuccess, OnError);
        }

        public static void GetBanInfo(Action<GetBanInfoResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetBanInfoURL, OnSuccess, OnError);
        }

        public static void GetLinkedAccounts(Action<LinkedAccountsDictionary> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetLinkedAccountsURL, OnSuccess, OnError);
        }

        public static void GetCustomData(Action<GetCustomDataResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetCustomDataURL, OnSuccess, OnError);
        }

        public static void GetClaimedCoupons(Action<GetClaimedCouponsResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetClaimedCouponsURL, OnSuccess, OnError);
        }

        public static void GetInventory(Action<GetInventoryResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetInventoryURL, OnSuccess, OnError);
        }

        public static void GetInventoryAggregate(Action<GetInventoryResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetInventoryAggregateURL, OnSuccess, OnError);
        }

        public static void GetPurchaseHistory(Action<GetPurchaseHistoryResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetPurchaseHistoryURL, OnSuccess, OnError);
        }

        //public static void GetLeaderboardData(Action<LeaderboardDataResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        //{
        //    PrototypeHttp.Get(BaseURL + GetLeaderboardDataURL, OnSuccess, OnError);
        //}

        //public static void GetMailInbox(Action<MailInboxResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        //{
        //    PrototypeHttp.Get(BaseURL + GetMailInboxURL, OnSuccess, OnError);
        //}

        public static void GetPrizeTableProgression(Action<GetPrizeTableProgressionResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetPrizeTableProgressionURL, OnSuccess, OnError);
        }

        public static void UpdateDisplayName(UpdateDisplayNameRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Put(BaseURL + UpdateDisplayNameURL, request, OnSuccess, OnError);
        }

        public static void UpdateGameDataByKey(UpdateGameDataByKeyRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Put(BaseURL + UpdateCustomDataByKeyURL, request, OnSuccess, OnError);
        }
        #endregion

        #region Item Management API
        public static void AddItem(ItemManagementRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + AddItemURL, request, OnSuccess, OnError);
        }

        public static void SubstractItem(ItemManagementRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + SubstractItemURL, request, OnSuccess, OnError);
        }

        public static void SubstractItemInstance(ItemManagementRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + SubstractItemInstanceURL, request, OnSuccess, OnError);
        }

        public static void BuyItem(ItemManagementRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + BuyItemURL, request, OnSuccess, OnError);
        }

        public static void SellItem(ItemManagementRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + SellItemURL, request, OnSuccess, OnError);
        }

        public static void DrawPrize(DrawPrizeRequest request, Action<DrawPrizeResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + DrawPrizeURL, request, OnSuccess, OnError);
        }
        #endregion

        #region IAP API
        public static void ValidatePlayStoreTransaction(ValidatePlayStoreRequest request, Action<ValidatePlayStoreResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + ValidatePlayStoreTransactionURL, request, OnSuccess, OnError);
        }
        public static void ValidateAppStoreTransaction(ValidateAppStoreRequest request, Action<ValidateAppStoreResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + ValidateAppStoreTransactionURL, request, OnSuccess, OnError);
        }
        public static void InitiatePaypalOrder(InitiatePaypalOrderRequest request, Action<InitiatePaypalOrderResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post<InitiatePaypalOrderRequest, InitiatePaypalOrderResult>(BaseURL + InitiatePaypalOrderURL, request,
                (result) =>
                {
                    OnSuccess?.Invoke(result);

                    if(!string.IsNullOrEmpty(result.orderID))
                    {
                        Application.OpenURL($"https://www.sandbox.paypal.com/checkoutnow?token={result.orderID}");
                    }
                }, OnError);
        }

        public static void VerifyPaypalTransaction(VerifyPaypalTransactionRequest request, Action<VerifyPaypalTransactionResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + VerifyPaypalTransactionURL, request, OnSuccess, OnError);
        }
        #endregion
    }
}