using PlayFab.EconomyModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mintzuworks.Domain
{
    [System.Serializable]
    public class CommonResult
    {
        public long httpCode;
    }

    [System.Serializable]
    public class GeneralResult : CommonResult
    {
        public string message;
    }

    [System.Serializable]
    public class UpdateDisplayNameRequest
    {
        public string displayName;
    }

    [System.Serializable]
    public class GetGameDataByKeyRequest : CommonResult
    {
        public List<string> keys;
    }

    [System.Serializable]
    public class GameProgress
    {
        public string currentQuest;
        public string currentMap;
        public float gameTimer;
    }

    [System.Serializable]
    public class UpdateGameDataByKeyRequest
    {
        public Dictionary<string, object> data;
    }

    [System.Serializable]
    public class LinkCustomLoginRequest
    {
        public string email;
        public string password;
        public string code;
    }

    [System.Serializable]
    public class LinkedAccountInfo
    {
        public string userId;
        public string name;
        public string email;
    }

    [System.Serializable]
    public class LinkedAccountsDictionary : CommonResult
    {
        public Dictionary<string, LinkedAccountInfo> linkedAccounts;
    }

    [System.Serializable]
    public class GetGameDataResult : CommonResult
    {
        public Dictionary<string, string> gameData;
    }

    [System.Serializable]
    public class InventoryData
    {
        public string instanceId;
        public string item; 
        public int amount;
        public Dictionary<string, object> customJson;
        public DateTime lastUpdateAt;
    }

    [System.Serializable]
    public class GetInventoryResult : CommonResult
    {
        public List<InventoryData> inventory;
    }

    [System.Serializable]
    public class PurchaseHistory
    {
        public string historyId;
        public string productId; // Assuming productId is a string representing the ObjectID
        public string orderId;
        public PlatformTarget platform;
        public PurchaseStatus status;
        public System.DateTime createdAt;
    }
    public enum PurchaseStatus
    {
        Cancelled,
        Pending,
        Success
    }

    public enum PlatformTarget
    {
        Android,
        iOS,
        Other
    }


    [System.Serializable]
    public class GetClaimedCouponsResult : CommonResult
    {
        public List<string> Result;
    }

    [System.Serializable]
    public class PrizeTableProgression
    {
        public string id;
        public int openCount;
        public int totalReset;
    }
    [System.Serializable]
    public class GetPrizeTableProgressionResult : CommonResult
    {
        public List<PrizeTableProgression> Result;
    }

    [System.Serializable]
    public class GetPurchaseHistoryResult : CommonResult
    {
        public List<PurchaseHistory> Result;
    }

    [System.Serializable]
    public class LoginCustomRequest
    {
        public string email;
        public string password;
    }

    [System.Serializable]
    public class GuestLoginRequest
    {
        public string deviceId;
    }

    [System.Serializable]
    public class LoginResponse : CommonResult
    {
        public string accessToken;
        public string refreshToken;
    }


    [System.Serializable]
    public class GetBanInfoResult : CommonResult
    {
        public bool status;
        public string reason;
        public DateTime expiredAt;
    }

    [System.Serializable]
    public class GetUserInfoResult : CommonResult
    {
        public string userID;
        public string displayName;
        public string email;
        public string region;
        public DateTime createdAt;
        public DateTime lastUpdateAt;
    }


    [Serializable]
    public class GetServerTimeResult : CommonResult
    {
        public DateTime serverTime;
    }


    [System.Serializable]
    public class ErrorResult
    {
        public long HttpCode;
        public string Error;
    }

    [System.Serializable]
    public class ItemExchangeReadable
    {
        public string itemID;
        public int amount;
    }

    [System.Serializable]
    public class ItemManagementRequest
    {
        public List<ItemExchangeReadable> itemData;
    }

    [System.Serializable]
    public class DrawPrizeRequest
    {
        public string prizeTableID;
        public int drawCount;
    }

    [System.Serializable]
    public class DrawPrizeResult : CommonResult
    {
        public List<ItemExchangeReadable> itemDrawn;
    }

    [System.Serializable]
    public class SocialLoginRequest
    {
        public string token;
    }

    [System.Serializable]
    public class SocialLinkingRequest
    {
        public string token;
    }

    [System.Serializable]
    public class SocialUnlinkingRequest
    {
        public string provider;
    }

}