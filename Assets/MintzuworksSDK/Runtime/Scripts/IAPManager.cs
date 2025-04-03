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
using UnityEngine.UI;
using UnityEngine.Purchasing.Security;
using Mintzuworks.Network;
using Mintzuworks.Domain;
using System;
using UnityEngine.SceneManagement;
using System.Net;
using Cysharp.Threading.Tasks;

namespace Mintzuworks.Manager
{
    public class IAPManager : MonoBehaviour, IDetailedStoreListener
    {
        public TMPro.TMP_InputField productInput;
        public Button btnBuyPlayStore;
        public Button btnBuyAppStore;
        public Button btnInitiatePaypalOrder;
        public Button btnCheckPaypalState;
        public Button btnBack;
        IStoreController m_StoreController;
        IAppleExtensions m_AppleExtensions;

        int m_ProcessingPurchasesCount;

        private void Start()
        {
            btnBuyPlayStore.onClick.AddListener(BuyProductPlaystore);
            btnBuyAppStore.onClick.AddListener(BuyProductApple);
            btnInitiatePaypalOrder.onClick.AddListener(OnClickInitiatePaypalOrder);
            btnCheckPaypalState.onClick.AddListener(OnClickCheckPaypalState);
            btnBack.onClick.AddListener(OnClickBack);
        }

        private void OnClickBack()
        {
            SceneManager.LoadScene("UserScreen");
        }

        string currentOrder;
        bool paypalOrder;
        private async void OnClickInitiatePaypalOrder()
        {
            paypalOrder = true;
            var result = await ClientAPI.InitiatePaypalOrder(new InitiatePaypalOrderRequest
            {
                productID = productInput.text
            });

            if (result != null && result.httpCode == (int)HttpStatusCode.OK)
            {
                currentOrder = result.orderID;
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            OnClickCheckPaypalState();
        }
        public void OnGeneralError(ErrorResult error)
        {
            Debug.LogError($"[{error.httpCode}] -> {error.message}");
        }
        private async void OnClickCheckPaypalState()
        {
            if (!paypalOrder) return;
            var result = await ClientAPI.VerifyPaypalTransaction(new VerifyPaypalTransactionRequest()
            {
                orderID = currentOrder,
            });

            if (result != null && result.httpCode == (int)HttpStatusCode.OK)
            {
                Debug.Log($"[Paypal] Order State : {result.status}");
                switch (result.status)
                {
                    case "completed":
                        paypalOrder = false;
                        break;

                    case "cancelled":
                        paypalOrder = false;
                        break;
                }
            }
        }

        void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            builder.AddProduct(productInput.text, ProductType.NonConsumable);
            UnityPurchasing.Initialize(this, builder);
        }

        void BuyProductPlaystore()
        {
            InitializePurchasing();
            m_StoreController.InitiatePurchase(productInput.text);
        }

        void BuyProductApple()
        {
            InitializePurchasing();
            m_StoreController.InitiatePurchase(productInput.text);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            m_StoreController = controller;
            m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
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

        IEnumerator BackEndValidation(Product product)
        {
            Debug.Log(JsonConvert.SerializeObject(product));

            //If IOS >= 7
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            // Get a reference to IAppleConfiguration during IAP initialization.
            var appleConfig = builder.Configure<IAppleConfiguration>();

            byte[] receiptData = null;
            try
            {
                receiptData = System.Convert.FromBase64String(appleConfig.appReceipt);
                Debug.Log("Receipt is base64 encoded.");
            }
            catch (System.FormatException)
            {
                Debug.Log("Receipt is not base64 encoded.");
            }

            string receiptDataIos7 = string.Empty;
            if (receiptData != null)
                receiptDataIos7 = receiptData.ToString();

            if (receiptDataIos7 == "")
            {
                //If IOS < 7
                var receiptTest = m_AppleExtensions.GetTransactionReceiptForProduct(product);
                Debug.Log(receiptTest);

            }
            AppleReceipt receipt = new AppleValidator(AppleTangle.Data()).Validate(receiptData);
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


            var requestData = new ValidatePlayStoreRequest
            {
                package = packageName,
                productID = productId,
                purchaseToken = purchaseToken
            };

            var jsonData = JsonConvert.SerializeObject(requestData);

            ValidatePlayStore(requestData).Forget();
            ValidateAppStore(new ValidateAppStoreRequest()
            {
                receiptData = receiptDataIos7
            }).Forget();
        }

        public async UniTaskVoid ValidatePlayStore(ValidatePlayStoreRequest req)
        {
            var result = await ClientAPI.ValidatePlayStoreTransaction(req);
            Debug.Log(result);
        }


        public async UniTaskVoid ValidateAppStore(ValidateAppStoreRequest req)
        {
            var result = await ClientAPI.ValidateAppStoreTransaction(req);
            Debug.Log(result);
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
}