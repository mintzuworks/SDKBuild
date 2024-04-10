using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using UnityEngine.Networking;
using Utilities.WebRequestRest;

public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
    IStoreController m_StoreController;

    public string consumable = "test_consumable";
    public string nonConsumable = "test_nonconsumable";
    public string subscription = "test_subscription";

    int m_ProcessingPurchasesCount;

    void Start()
    {
        InitializePurchasing();
    }

    void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(consumable, ProductType.Consumable);
        builder.AddProduct(nonConsumable, ProductType.NonConsumable);
        builder.AddProduct(subscription, ProductType.Subscription);

        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyConsumable()
    {
        m_StoreController.InitiatePurchase(consumable);
    }
    public void BuyNonConsumable()
    {
        m_StoreController.InitiatePurchase(nonConsumable);
    }
    public void BuySubscription()
    {
        m_StoreController.InitiatePurchase(subscription);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("In-App Purchasing successfully initialized");
        m_StoreController = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        OnInitializeFailed(error, null);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

        if (message != null)
        {
            errorMessage += $" More details: {message}";
        }

        Debug.Log(errorMessage);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //Retrieve the purchased product
        var product = args.purchasedProduct;

        StartCoroutine(BackEndValidation(product));

        //We return Pending, informing IAP to keep the transaction open while we validate the purchase on our side.
        return PurchaseProcessingResult.Pending;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}'," +
            $" Purchase failure reason: {failureDescription.reason}," +
            $" Purchase failure details: {failureDescription.message}");
    }

    public TMPro.TMP_InputField ipAddress;
    string URL => $"{ipAddress.text}/iap/validate/playstore";

    IEnumerator BackEndValidation(Product product)
    {
        Debug.Log(JsonConvert.SerializeObject(product));
        var receiptObj = JsonConvert.DeserializeObject<JObject>(product.receipt);
        if (!TryGetValue(receiptObj, "Payload", out string payloadJson)) yield break;
        var payloadObj = JsonConvert.DeserializeObject<JObject>(payloadJson);

        if (!TryGetValue(payloadObj, "json", out string jsonString)) yield break;
        var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonString);

        if (!TryGetValue(jsonObject, "packageName", out string packageName) ||
            !TryGetValue(jsonObject, "productId", out string productId) ||
            !TryGetValue(jsonObject, "purchaseToken", out string purchaseToken))
        {
            yield break;
        }
        Debug.Log("Package Name : " + packageName);
        Debug.Log("Product ID : " + productId);
        Debug.Log("Purchase Token : " + purchaseToken);

        var requestData = new PlayStoreVerifyRequest()
        {
            package = packageName,
            productID = productId,
            purchaseToken = purchaseToken
        };

        var jsonData = JsonConvert.SerializeObject(requestData);

        // Start the async operation
        var task = Rest.PostAsync(URL, jsonData);
        yield return new WaitUntil(() => task.IsCompleted);
        Rest.Validate(task.Result, true);

        if(task.Result.Successful)
        {
            Debug.Log("Validation request sent successfully");
            Debug.Log($"Server response: {task.Result}");
        }
        else
        {
            Debug.LogError($"Err Server response: {task.Result}");
        }

        m_ProcessingPurchasesCount++;

        Debug.Log($"Confirming purchase of {product.definition.id}");
        m_StoreController.ConfirmPendingPurchase(product);
    }

    bool TryGetValue(JObject obj, string key, out string value)
    {
        value = string.Empty;
        if (obj.ContainsKey(key))
        {
            value = obj[key].ToString();
            return true;
        }
        else return false;
    }
}

[System.Serializable]
public class PlayStoreVerifyRequest
{
    public string package;
    public string productID;
    public string purchaseToken;
}