using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
    public class ListOfResult<TResult> : GeneralResult
    {

    }

    [System.Serializable]
    public class GeneralResultWithData<T> : CommonResult
    {
        public T data;
    }

    [System.Serializable]
    public class UpdateDisplayNameRequest
    {
        public string displayName;
    }

    [System.Serializable]
    public class GetCustomDataByKeyRequest
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
    public class GetTitleDataRequest
    {
        public List<string> keys;
    }
    [System.Serializable]
    public class GetTitleDataResult : CommonResult
    {
        public Dictionary<string, object> titleDataValues;
    }

    [System.Serializable]
    public class NewsData
    {
        [JsonProperty("_id")]
        public string ID;
        public string title;
        public string body;
        public DateTime createdDate;
        public bool status;
    }
    [System.Serializable]
    public class GetNewsDataResult : CommonResult
    {
        public List<NewsData> data;
    }

    [System.Serializable]
    public class LinkCustomLoginRequest
    {
        public string email;
        public string password;
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
        public Dictionary<string, LinkedAccountInfo> data = new Dictionary<string, LinkedAccountInfo>();
    }

    [System.Serializable]
    public class GetCustomDataResult : CommonResult
    {
        public Dictionary<string, object> data;
    }

    [System.Serializable]
    public class ItemInstance
    {
        public string instanceID;
        [JsonProperty("item")]
        public string OID;
        public string itemID;
        public int amount;
        public Dictionary<string, object> customJson;
        public DateTime createdAt;
        public DateTime lastUpdateAt;
    }

    [System.Serializable]
    public class GetInventoryResult : CommonResult
    {
        public List<ItemInstance> data;
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

    public enum SocialProvider
    {
        Google,
        Apple,
        DeviceID
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
        public List<PrizeTableProgression> data;
    }

    [System.Serializable]
    public class GetPurchaseHistoryResult : CommonResult
    {
        public List<PurchaseHistory> data;
    }

    [System.Serializable]
    public class LoginCustomRequest
    {
        public string email;
        public string password;
    }

    [System.Serializable]
    public class RefreshTokenRequest
    {
        public string refreshToken;
    }

    [System.Serializable]
    public class EmailVerificationRequest
    {
        public string email;
        public string code;
    }
    [System.Serializable]
    public class EmailRequest
    {
        public string email;
    }

    [System.Serializable]
    public class NonceRequest
    {
        public string walletAddress;
    }

    [System.Serializable]
    public class NonceResult : CommonResult
    {
        public string nonce;
    }

    //Login Web3 Request
    [System.Serializable]
    public class LoginWeb3Request
    {
        public string walletAddress;
        public string nonce;
        public string signature;
        public string deviceID;
    }

    [System.Serializable]
    public class GuestLoginRequest
    {
        public string deviceID;
        public bool autoSignup = true;
    }

    [System.Serializable]
    public class LoginResponse : CommonResult
    {
        public string accessToken;
        public string refreshToken;
        public long accessValidUntil;
        public long refreshValidUntil;

        public DateTime AccessValidUntilDate => UnixTimeConverter.FromUnixTimeSeconds(accessValidUntil);
        public DateTime RefreshValidUntilDate => UnixTimeConverter.FromUnixTimeSeconds(refreshValidUntil);
    }

    [System.Serializable]
    public class CheckSocialRedisLinkResponse : CommonResult
    {
        public bool result;
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
        public string customID;
        public string displayName;
        public string email;
        public string region;
        public string lastDeviceID;
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
        [JsonProperty("HttpCode")]
        public long httpCode;
        [JsonProperty("Error")]
        public string error;
        public string message;
    }

    [System.Serializable]
    public class GetAllItemResult : CommonResult
    {
        public List<CatalogueItem> data;
    }

    [Serializable]
    public class CatalogueItem
    {
        [JsonProperty("_id")]
        public string ID { get; set; }

        [JsonProperty("itemID")]
        public string ItemID { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("isStackable")]
        public bool IsStackable { get; set; }

        [JsonProperty("isRechargeable")]
        public bool IsRechargeable { get; set; }

        [JsonProperty("startingAmount")]
        public int StartingAmount { get; set; }

        [JsonProperty("maxAmount")]
        public int MaxAmount { get; set; }

        [JsonProperty("rechargeRate")]
        public int RechargeRate { get; set; }

        [JsonProperty("rechargeIntervalType")]
        public int RechargeIntervalType { get; set; } // Assuming enum is int

        [JsonProperty("rechargeInterval")]
        public int RechargeInterval { get; set; }

        [JsonProperty("maxRecharge")]
        public int MaxRecharge { get; set; }

        [JsonProperty("buyPrice")]
        public List<ItemExchange> BuyPrice { get; set; }

        [JsonProperty("customSellPrice")]
        public List<ItemExchange> CustomSellPrice { get; set; }

        [JsonProperty("customJSON")]
        public Dictionary<string, object> CustomJSON { get; set; }

        [JsonProperty("isOnAccountCreation")]
        public bool IsOnAccountCreation { get; set; }

        [JsonProperty("isOnAccountCreationAmt")]
        public int IsOnAccountCreationAmt { get; set; }

        [JsonProperty("enableStatistic")]
        public bool EnableStatistic { get; set; }

        [JsonProperty("statisticType")]
        public int StatisticType { get; set; } // Assuming enum is int

        // Uncomment if needed
        // [JsonProperty("durableDuration")]
        // public int DurableDuration { get; set; }
    }



    [Serializable]
    public class ItemExchange
    {
        public string ItemID { get; set; }
        public int Amount { get; set; }
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
    public class ValidatePlayStoreRequest
    {
        public string package;
        public string productID;
        public string purchaseToken;
    }
    [System.Serializable]
    public class ValidateAppStoreRequest
    {
        public string receiptData;
        public string password;
        public bool excludeOldTransactions;
    }
    [System.Serializable]
    public class InitiatePaypalOrderRequest
    {
        public string productID;
    }

    [System.Serializable]
    public class VerifyPaypalTransactionRequest
    {
        public string orderID;
    }
    [System.Serializable]
    public class VerifyPaypalTransactionResult : CommonResult
    {
        public string status;
    }
    [System.Serializable]
    public class InitiatePaypalOrderResult : CommonResult
    {
        public string orderID;
    }

    [System.Serializable]
    public class ValidateAppStoreResult : CommonResult
    {
        public string transactionID;
        public string originalTransactionId;
        public string webOrderLineItemId;
        public string bundleId;
        public string productId;
        public string subscriptionGroupIdentifier;
        public long purchaseDate;
        public long originalPurchaseDate;
        public long expiresDate;
        public int quantity;
        public string type;
        public string appAccountToken;
        public string inAppOwnershipType;
        public long signedDate;
        public int offerType;
        public string offerIdentifier;
        public long revocationDate;
        public int? revocationReason;
        public bool isUpgraded;
        public string storefront;
        public string storefrontId;
        public string transactionReason;
        public string environment;
        public int price;
        public string currency;
        public string offerDiscountType;
    }

    [System.Serializable]
    public class ValidatePlayStoreResult : CommonResult
    {
        public long acknowledgementState;
        public long consumptionState;
        public string developerPayload;
        public string kind;
        public string obfuscatedExternalAccountId;
        public string obfuscatedExternalProfileId;
        public string orderId;
        public string productId;
        public long purchaseState;
        public string purchaseTimeMillis;
        public string purchaseToken;
        public long? purchaseType;
        public long quantity;
        public string regionCode;
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
        public string id;
    }

    [Serializable]
    public class StartAuthenticationRequest
    {
        public string deviceID;
        public string eventName;
        public string userID;
    }

    [System.Serializable]
    public class SocialUnlinkingRequest
    {
        public string provider;
    }

    

    [System.Serializable]
    public class UpdateCustomJsonRequest
    {
        public string instanceID;
        public string itemID;
        public string mode;
        public Dictionary<string, object> data;
    }


}