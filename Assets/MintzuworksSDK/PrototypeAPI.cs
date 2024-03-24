using PlayFab.ClientModels;
using PlayFab.Internal;
using PlayFab;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mintzuworks.Domain;
using System.Web;

namespace Mintzuworks.Network
{
    public static class PrototypeAPI
    {
        public const string BaseURL = "http://localhost:8080/";

        #region Title-Wide Management URL
        public const string GetPingURL = "ping";
        public const string GetServerTimeURL = "time";
        public const string GetAllTitleDataURL = "titleDatas";
        public const string GetAllNewsURL = "news";
        #endregion

        #region Authentication URL
        public const string SignUpCustomURL = "signup";
        public const string LoginCustomURL = "login";
        public const string LoginGuestURL = "login/guest";
        public const string AuthGoogleURL = "auth/google";
        public const string AuthAppleURL = "auth/apple";
        public const string RefreshTokenURL = "refresh";
        public const string SocialLoginURL = "auth/social/login";
        public const string SocialLinkingURL = "auth/social/connect";
        public const string SocialUnlinkingURL = "auth/social/disconnect";
        #endregion

        #region User Management URL
        public const string LinkCustomLoginWithCodeURL = "user/link/custom/code/handle";
        public const string SendCustomLoginCodeForLinkingURL = "user/link/custom/code/send";
        public const string LinkCustomLoginWithVerifierURL = "user/link/custom/verifier";
        public const string GetUserInfoURL = "user/info";
        public const string GetBanInfoURL = "user/banInfo";
        public const string GetLinkedAccountsURL = "user/linkedAccounts";
        public const string GetGameDataURL = "user/gameData";
        public const string GetClaimedCouponsURL = "user/claimedCoupons";
        public const string GetInventoryURL = "user/inventory";
        public const string GetPurchaseHistoryURL = "user/purchaseHistory";
        public const string GetLeaderboardDataURL = "user/leaderboardData";
        public const string GetMailInboxURL = "user/mailInbox";
        public const string GetPrizeTableProgressionURL = "user/prizeTableProgression";
        public const string UpdateDisplayNameURL = "user/displayName";
        public const string UpdateGameDataByKeyURL = "user/gameData";
        #endregion

        #region Item Management URL 
        public const string AddItemURL = "item/add/id";
        public const string SubstractItemURL = "item/substract/id";
        public const string BuyItemURL = "item/buy/id";
        public const string SellItemURL = "item/sell/id";
        public const string DrawPrizeURL = "prizeTable/draw";
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

        public static void GetAllTitleData(Action<GetServerTimeResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetAllTitleDataURL, OnSuccess, OnError, useOAuth: false);
        }
        
        public static void GetAllNews(Action<GetServerTimeResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetAllNewsURL, OnSuccess, OnError, useOAuth: false);
        }
        #endregion

        #region Authentication API
        public static void SignUpCustom(LoginCustomRequest request, Action<LoginResponse> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + LoginCustomURL, request, OnSuccess, OnError, useOAuth: false);
        }
        public static void LoginCustom(LoginCustomRequest request, Action<LoginResponse> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + LoginCustomURL, request, OnSuccess, OnError, useOAuth: false);
        }
        public static void LoginGuest(GuestLoginRequest request, Action<LoginResponse> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + LoginGuestURL, request, OnSuccess, OnError, useOAuth: false);
        }
        public static void RefreshAccessToken(LoginCustomRequest request, Action<LoginResponse> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + LoginCustomURL, request, OnSuccess, OnError, useOAuth: false);
        }

        public static void LoginGoogle(Action<LoginResponse> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            Application.OpenURL(BaseURL + AuthGoogleURL);
            Application.deepLinkActivated += (url) => OnSocialLoginCallbackReceived(url, OnSuccess, OnError);
        }

        public static void LoginApple(Action<LoginResponse> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            Application.OpenURL(BaseURL + AuthAppleURL);
            Application.deepLinkActivated += (url) => OnSocialLoginCallbackReceived(url, OnSuccess, OnError);
        }

        private static void OnSocialLoginCallbackReceived(string deeplinkURL,
            Action<LoginResponse> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            Application.deepLinkActivated -= (url) => OnSocialLoginCallbackReceived(url, OnSuccess, OnError);
            string[] parts = deeplinkURL.Split('?');
            if (parts.Length > 1)
            {
                string token = parts[1];
                SocialLogin(new SocialLoginRequest()
                {
                    token = token
                });
            }
        }

        public static void LinkGoogle(Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            Application.OpenURL(BaseURL + AuthAppleURL);
            Application.deepLinkActivated += (url) => OnSocialLinkingCallbackReceived(url, OnSuccess, OnError);
        }
        public static void LinkApple(Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            Application.OpenURL(BaseURL + AuthAppleURL);
            Application.deepLinkActivated += (url) => OnSocialLinkingCallbackReceived(url, OnSuccess, OnError);
        }

        private static void OnSocialLinkingCallbackReceived(string deeplinkURL,
            Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            Application.deepLinkActivated -= (url) => OnSocialLinkingCallbackReceived(url, OnSuccess, OnError);
            string[] parts = deeplinkURL.Split('?');
            if (parts.Length > 1)
            {
                string token = parts[1];
                SocialLinking(new SocialLinkingRequest()
                {
                    token = token
                });
            }
        }

        private static void SocialLogin(SocialLoginRequest request, Action<LoginResponse> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + SocialLoginURL, request, OnSuccess, OnError, useOAuth: false);
        }

        private static void SocialLinking(SocialLinkingRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + SocialLinkingURL, request, OnSuccess, OnError, useOAuth: false);
        }

        public static void SocialUnlinking(SocialUnlinkingRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + SocialUnlinkingURL, request, OnSuccess, OnError, useOAuth: false);
        }
        #endregion

        #region User Management API
        public static void LinkCustomLoginWithCode(LinkCustomLoginRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + LinkCustomLoginWithCodeURL, request, OnSuccess, OnError);
        }

        public static void SendCustomLoginCodeForLinking(LinkCustomLoginRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + SendCustomLoginCodeForLinkingURL, request, OnSuccess, OnError);
        }

        public static void LinkCustomLoginWithVerifier(LinkCustomLoginRequest request, Action<GeneralResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Post(BaseURL + LinkCustomLoginWithVerifierURL, request, OnSuccess, OnError);
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

        public static void GetGameData(Action<GetGameDataResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetGameDataURL, OnSuccess, OnError);
        }

        public static void GetClaimedCoupons(Action<GetClaimedCouponsResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetClaimedCouponsURL, OnSuccess, OnError);
        }

        public static void GetInventory(Action<GetInventoryResult> OnSuccess = null, Action<ErrorResult> OnError = null)
        {
            PrototypeHttp.Get(BaseURL + GetInventoryURL, OnSuccess, OnError);
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
            PrototypeHttp.Put(BaseURL + UpdateGameDataByKeyURL, request, OnSuccess, OnError);
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
    }
}