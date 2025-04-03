using Cysharp.Threading.Tasks;
using Mintzuworks.Domain;
using Newtonsoft.Json;
using System;
using System.Text;
using UnityEngine;

namespace Mintzuworks.Network
{
    public static class ClientAPI
    {
        public static string BaseURL { get; set; }

        #region Title-Wide Management URL
        private const string GetPingURL = "ping";
        private const string ValidateAccessTokenURL = "validate";
        private const string GetServerTimeURL = "time";
        private const string GetAllExternalTitleDataURL = "titleDatas/external";
        private const string GetAllInternalTitleDataURL = "titleDatas/internal";
        private const string GetExternalTitleDataURL = "titleData/external";
        private const string GetInternalTitleDataURL = "titleData/internal";
        private const string GetAllActiveNewsURL = "news/active";
        #endregion

        #region Authentication URL
        private const string SilentLoginURL = "silentLogin";
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

        #region Web3
        private const string RequestNonceURL = "nonce/request";
        private const string LoginWeb3URL = "login/web3";
        #endregion

        #region Match
        private const string StartMatchURL = "startMatch";
        private const string EndMatchURL = "endMatch";
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
        private const string GetMailInboxURL = "user/mailInbox";
        private const string GetPrizeTableProgressionURL = "user/prizeTableProgression";
        private const string UpdateDisplayNameURL = "user/displayName";
        private const string UpdateCustomDataByKeyURL = "user/customData";
        #endregion

        #region Leaderboard URL
        private const string GetLeaderboardEntryURL = "leaderboard/entries";
        #endregion

        #region Item Management URL 
        private const string AddItemURL = "item/add/id";
        private const string SubstractItemURL = "item/substract/id";
        private const string SubstractItemInstanceURL = "item/substract/instance";
        private const string BuyItemURL = "item/buy/id";
        private const string SellItemURL = "item/sell/id";
        private const string SellItemInstanceURL = "item/sell/instance";
        private const string DrawPrizeURL = "prizeTable/draw";
        private const string GetItemByID = "items/id";
        private const string GetItemByOID = "items/oid";
        private const string UpdateCustomJsonURL = "item/updateCustomJson";
        

        #region Admin Item Management URL
        private const string CatalogueItemURL = "item";
        private const string GetAllCatalogueItemURL = "items";
        #endregion
        #endregion

        #region IAP URL
        private const string ValidatePlayStoreTransactionURL = "iap/validate/playStore";
        private const string ValidateAppStoreTransactionURL = "iap/validate/appStore";
        private const string InitiatePaypalOrderURL = "iap/paypal/initiate";
        private const string VerifyPaypalTransactionURL = "iap/paypal/pool";
        #endregion


        #region Title-Wide Management API
        public static async UniTask<GeneralResult> GetPing()
        {
            return await PrototypeHttp.Get<GeneralResult>(BaseURL + GetPingURL, useOAuth: false);
        }

        public static async UniTask<GeneralResult> ValidateAccessToken()
        {
            return await PrototypeHttp.Get<GeneralResult>(BaseURL + GetPingURL, useOAuth: true);
        }

        public static async UniTask<GetServerTimeResult> GetServerTime()
        {
            return await PrototypeHttp.Get<GetServerTimeResult>(BaseURL + GetServerTimeURL, useOAuth: false);
        }

        public static async UniTask<GetTitleDataResult> GetAllExternalTitleData()
        {
            return await PrototypeHttp.Get<GetTitleDataResult>(BaseURL + GetAllExternalTitleDataURL, useOAuth: false);
        }
        public static async UniTask<GetTitleDataResult> GetExternalTitleData(GetTitleDataRequest request)
        {
            return await PrototypeHttp.Post<GetTitleDataRequest, GetTitleDataResult>(BaseURL + GetExternalTitleDataURL, request, useOAuth: false);
        }

        public static async UniTask<GetTitleDataResult> GetAllInternalTitleData()
        {
            return await PrototypeHttp.Get<GetTitleDataResult>(BaseURL + GetAllInternalTitleDataURL, useOAuth: false);
        }
        public static async UniTask<GetTitleDataResult> GetInternalTitleData(GetTitleDataRequest request)
        {
            return await PrototypeHttp.Post<GetTitleDataRequest, GetTitleDataResult>(BaseURL + GetInternalTitleDataURL, request, useOAuth: false);
        }
        public static async UniTask<GetNewsDataResult> GetAllNews()
        {
            return await PrototypeHttp.Get<GetNewsDataResult>(BaseURL + GetAllActiveNewsURL, useOAuth: false);
        }
        #endregion

        #region Authentication API
        public static async UniTask<GeneralResult> SilentLogin()
        {
            return await PrototypeHttp.Get<GeneralResult>(BaseURL + SilentLoginURL, useOAuth: true);
        }
        public static async UniTask<LoginResponse> SignUpCustom(LoginCustomRequest request)
        {
            return await PrototypeHttp.Post<LoginCustomRequest, LoginResponse>(BaseURL + SignUpCustomURL, request, useOAuth: false);
        }

        public static async UniTask<LoginResponse> LoginCustom(LoginCustomRequest request)
        {
            return await PrototypeHttp.Post<LoginCustomRequest, LoginResponse>(BaseURL + LoginCustomURL, request, useOAuth: false);
        }

        public static async UniTask<LoginResponse> LoginGuest(GuestLoginRequest request)
        {
            return await PrototypeHttp.Post<GuestLoginRequest, LoginResponse>(BaseURL + LoginGuestURL, request, useOAuth: false);
        }
        public static async UniTask<LoginResponse> RefreshAccessToken(RefreshTokenRequest request, Action<LoginResponse> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            return await PrototypeHttp.Post<RefreshTokenRequest, LoginResponse>(BaseURL + RefreshTokenURL, request, useOAuth: false);
        }

        public static void LoginGoogle()
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

        public static void LoginApple()
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

        public static void LinkGoogle(StartAuthenticationRequest request)
        {
            string json = JsonConvert.SerializeObject(request);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            string base64Encoded = Convert.ToBase64String(bytes);
            Application.OpenURL(BaseURL + AuthGoogleURL + "?data=" + base64Encoded);
        }
        public static void LinkApple(StartAuthenticationRequest request)
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

        public static async UniTask<LoginResponse> SocialLogin(SocialLoginRequest request)
        {
            return await PrototypeHttp.Post<SocialLoginRequest, LoginResponse>(BaseURL + VerifySocialLoginURL, request, useOAuth: false);
        }

        public static async UniTask<CheckSocialRedisLinkResponse> SocialLink(SocialLoginRequest request)
        {
            return await PrototypeHttp.Post<SocialLoginRequest, CheckSocialRedisLinkResponse>(BaseURL + VerifySocialLinkURL, request, useOAuth: false);
        }

        public static async UniTask<GeneralResult> CheckEmailValidity(EmailRequest request)
        {
            return await PrototypeHttp.Post<EmailRequest, GeneralResult>(BaseURL + CheckEmailValidityURL, request, useOAuth: false);
        }
        public static async UniTask<GeneralResult> SendResetPassword(EmailRequest request)
        {
            return await PrototypeHttp.Post<EmailRequest, GeneralResult>(BaseURL + SendResetPasswordURL, request, useOAuth: false);
        }
        #endregion

        #region Web3
        public static async UniTask<NonceResult> RequestNonce(NonceRequest request)
        {
            return await PrototypeHttp.Post<NonceRequest, NonceResult>(BaseURL + RequestNonceURL, request, useOAuth: false);
        }

        public static async UniTask<LoginResponse> LoginWeb3(LoginWeb3Request request)
        {
            return await PrototypeHttp.Post<LoginWeb3Request, LoginResponse>(BaseURL + LoginWeb3URL, request, useOAuth: false);
        }
        #endregion

        #region Match API
        public static async UniTask<MatchResult> StartMatch(StartMatchRequest request)
        {
            return await PrototypeHttp.Post<StartMatchRequest, MatchResult>(BaseURL + StartMatchURL, request);
        }

        public static async UniTask<MatchResult> EndMatch(EndMatchRequest request)
        {
            return await PrototypeHttp.Post<EndMatchRequest, MatchResult>(BaseURL + EndMatchURL, request, isCrucial: true);
        }
        #endregion

        #region Email Verification API
        public static async UniTask<GeneralResult> CheckEmailAvailability(EmailRequest request)
        {
            return await PrototypeHttp.Post<EmailRequest, GeneralResult>(BaseURL + CheckEmailAvailibilityURL, request, useOAuth: false);
        }

        public static async UniTask<GeneralResult> SendEmailVerificationCode(EmailVerificationRequest request)
        {
            return await PrototypeHttp.Post<EmailVerificationRequest, GeneralResult>(BaseURL + SendEmailVerificationCodeURL, request, useOAuth: false);
        }

        public static async UniTask<GeneralResult> HandleEmailVerificationCode(EmailVerificationRequest request)
        {
            return await PrototypeHttp.Post<EmailVerificationRequest, GeneralResult>(BaseURL + HandleEmailVerificationCodeURL, request, useOAuth: false);
        }
        #endregion

        #region User Management API
        public static async UniTask<GeneralResult> LinkCustomLogin(LinkCustomLoginRequest request)
        {
            return await PrototypeHttp.Post<LinkCustomLoginRequest, GeneralResult>(BaseURL + LinkCustomLoginURL, request);
        }

        public static async UniTask<GeneralResult> SocialUnlinking(SocialUnlinkingRequest request)
        {
            return await PrototypeHttp.Post<SocialUnlinkingRequest, GeneralResult>(BaseURL + SocialUnlinkingURL, request);
        }

        public static async UniTask<GetUserInfoResult> GetUserInfo()
        {
            return await PrototypeHttp.Get<GetUserInfoResult>(BaseURL + GetUserInfoURL);
        }
        public static async UniTask<GetBanInfoResult> GetBanInfo()
        {
            return await PrototypeHttp.Get<GetBanInfoResult>(BaseURL + GetBanInfoURL);
        }

        public static async UniTask<LinkedAccountsDictionary> GetLinkedAccounts()
        {
            return await PrototypeHttp.Get<LinkedAccountsDictionary>(BaseURL + GetLinkedAccountsURL);
        }

        public static async UniTask<GetCustomDataResult> GetCustomDataByKey(GetCustomDataByKeyRequest request)
        {
            return await PrototypeHttp.Post<GetCustomDataByKeyRequest, GetCustomDataResult>(BaseURL + GetCustomDataURL, request);
        }

        public static async UniTask<GeneralResult> UpdateCustomDataByKey(UpdateGameDataByKeyRequest request)
        {
            return await PrototypeHttp.Put<UpdateGameDataByKeyRequest, GeneralResult>(BaseURL + UpdateCustomDataByKeyURL, request);
        }

        public static async UniTask<GeneralResult> UpdateCustomJson(UpdateCustomJsonRequest request)
        {
            return await PrototypeHttp.Put<UpdateCustomJsonRequest, GeneralResult>(BaseURL + UpdateCustomJsonURL, request);
        }

        public static async UniTask<GetClaimedCouponsResult> GetClaimedCoupons()
        {
            return await PrototypeHttp.Get<GetClaimedCouponsResult>(BaseURL + GetClaimedCouponsURL);
        }

        public static async UniTask<GetInventoryResult> GetInventoryAggregate()
        {
            return await PrototypeHttp.Get<GetInventoryResult>(BaseURL + GetInventoryAggregateURL);
        }

        public static async UniTask<GetInventoryResult> GetInventory()
        {
            return await PrototypeHttp.Get<GetInventoryResult>(BaseURL + GetInventoryURL);
        }

        public static async UniTask<GetPurchaseHistoryResult> GetPurchaseHistory()
        {
            return await PrototypeHttp.Get<GetPurchaseHistoryResult>(BaseURL + GetPurchaseHistoryURL);
        }

        //public static async UniTask GetLeaderboardData(Action<LeaderboardDataResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        //{
        //    await PrototypeHttp.Get(BaseURL + GetLeaderboardDataURL, OnSuccess, OnError);
        //}

        // public static async UniTask<MailInboxResult> GetMailInbox()
        // {
        //     return await PrototypeHttp.Get<MailInboxResult>(BaseURL + GetMailInboxURL);
        // }

        public static async UniTask<GetPrizeTableProgressionResult> GetPrizeTableProgression()
        {
            return await PrototypeHttp.Get<GetPrizeTableProgressionResult>(BaseURL + GetPrizeTableProgressionURL);
        }

        public static async UniTask<GeneralResult> UpdateDisplayName(UpdateDisplayNameRequest request)
        {
            return await PrototypeHttp.Put<UpdateDisplayNameRequest, GeneralResult>(BaseURL + UpdateDisplayNameURL, request);
        }
        #endregion

        #region Item Management API
        public static async UniTask<GeneralResult> AddItem(ItemManagementRequest request)
        {
            return await PrototypeHttp.Post<ItemManagementRequest, GeneralResult>(BaseURL + AddItemURL, request);
        }
        public static async UniTask<GeneralResult> SubstractItem(ItemManagementRequest request)
        {
            return await PrototypeHttp.Post<ItemManagementRequest, GeneralResult>(BaseURL + SubstractItemURL, request);
        }
        public static async UniTask<GeneralResult> SubstractItemInstance(ItemManagementRequest request)
        {
            return await PrototypeHttp.Post<ItemManagementRequest, GeneralResult>(BaseURL + SubstractItemInstanceURL, request);
        }
        public static async UniTask<GeneralResult> BuyItem(ItemManagementRequest request)
        {
            return await PrototypeHttp.Post<ItemManagementRequest, GeneralResult>(BaseURL + BuyItemURL, request);
        }
        public static async UniTask<GeneralResult> SellItem(ItemManagementRequest request)
        {
            return await PrototypeHttp.Post<ItemManagementRequest, GeneralResult>(BaseURL + SellItemURL, request);
        }
        public static async UniTask<GeneralResult> SellItemInstance(ItemManagementRequest request)
        {
            return await PrototypeHttp.Post<ItemManagementRequest, GeneralResult>(BaseURL + SellItemInstanceURL, request);
        }
        public static async UniTask<DrawPrizeResult> DrawPrize(DrawPrizeRequest request)
        {
            return await PrototypeHttp.Post<DrawPrizeRequest, DrawPrizeResult>(BaseURL + DrawPrizeURL, request);
        }
        public static async UniTask<GetAllItemResult> GetAllCatalogueItem()
        {
            return await PrototypeHttp.Get<GetAllItemResult>(BaseURL + GetAllCatalogueItemURL);
        }
        #endregion

        #region Leaderboard
        public static async UniTask<GetLeaderboardResult> GetLeaderboardEntry(GetLeaderboardRequest request)
        {
            string url = $"{BaseURL}{GetLeaderboardEntryURL}?name={request.statisticName}&version={request.version}";
            return await PrototypeHttp.Get<GetLeaderboardResult>(url, useOAuth: false);
        }

        #endregion

        #region IAP API
        public static async UniTask<ValidatePlayStoreResult> ValidatePlayStoreTransaction(ValidatePlayStoreRequest request)
        {
            return await PrototypeHttp.Post<ValidatePlayStoreRequest, ValidatePlayStoreResult>(BaseURL + ValidatePlayStoreTransactionURL, request);
        }
        public static async UniTask<ValidateAppStoreResult> ValidateAppStoreTransaction(ValidateAppStoreRequest request)
        {
            return await PrototypeHttp.Post<ValidateAppStoreRequest, ValidateAppStoreResult>(BaseURL + ValidateAppStoreTransactionURL, request);
        }
        public static async UniTask<InitiatePaypalOrderResult> InitiatePaypalOrder(InitiatePaypalOrderRequest request)
        {
            var result = await PrototypeHttp.Post<InitiatePaypalOrderRequest, InitiatePaypalOrderResult>(BaseURL + InitiatePaypalOrderURL, request);

            if (result != null && result.httpCode == 200)
            {
                if (!string.IsNullOrEmpty(result.orderID))
                {
                    Application.OpenURL($"https://www.sandbox.paypal.com/checkoutnow?token={result.orderID}");
                }
            }

            return result;
        }

        public static async UniTask<VerifyPaypalTransactionResult> VerifyPaypalTransaction(VerifyPaypalTransactionRequest request)
        {
            return await PrototypeHttp.Post<VerifyPaypalTransactionRequest, VerifyPaypalTransactionResult>(BaseURL + VerifyPaypalTransactionURL, request);
        }
        #endregion
    }
}