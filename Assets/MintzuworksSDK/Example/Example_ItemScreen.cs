using Mintzuworks.Domain;
using Mintzuworks.Network;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mintzuworks.Example
{
    public class Example_ItemScreen : MonoBehaviour
    {
        public TMP_InputField inputItemID;
        public TMP_InputField inputItemAmount;
        public Button btnAddItem;
        public Button btnSubstractItem;
        public Button btnBuyItem;
        public Button btnSellItem;
        public TMP_InputField inputPrizeTableID;
        public TMP_InputField inputDrawAmount;
        public Button btnDrawPrize;

        public Button btnRefreshInventory;
        public Button btnRefreshInventoryAggregate;
        public Button btnSubstractItemInstance;
        public Button btnLogout;

        public ScrollRect svInventory;
        public Example_InventoryUI inventoryPrefab;
        public LayoutGroup layoutInventory;

        private void Start()
        {
            btnAddItem.onClick.AddListener(OnClickAddItem);
            btnSubstractItem.onClick.AddListener(OnClickSubstractItem);
            btnBuyItem.onClick.AddListener(OnClickBuyItem);
            btnSellItem.onClick.AddListener(OnClickSellItem);
            btnDrawPrize.onClick.AddListener(OnClickDrawPrize);
            btnRefreshInventory.onClick.AddListener(OnClickRefreshInventory);
            btnRefreshInventoryAggregate.onClick.AddListener(OnClickRefreshInventoryAggregate);
            btnSubstractItemInstance.onClick.AddListener(OnClickSubstractItemInstance);
            btnLogout.onClick.AddListener(OnClickLogout);

            RefreshInventoryListing();
        }

        List<InventoryData> currentInventory;
        private void RefreshInventoryListing()
        {
            PrototypeAPI.GetInventory((result) =>
            {
                currentInventory = result.data;
                DestroyAllChildren(layoutInventory.transform);
                foreach (var item in result.data)
                {
                    var inventory = Instantiate(inventoryPrefab, layoutInventory.transform);
                    inventory.SetInfo(item.itemID, item.amount.ToString());
                }

                StartCoroutine(LateDirty());
            }, OnGeneralError);
        }

        IEnumerator LateDirty()
        {
            layoutInventory.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.15f);
            layoutInventory.gameObject.SetActive(true);
        }

        private void OnClickAddItem()
        {
            PrototypeAPI.AddItem(new ItemManagementRequest()
            {
                itemData = new List<ItemExchangeReadable>()
                {
                    new ItemExchangeReadable()
                    {
                        itemID = inputItemID.text,
                        amount = int.Parse(inputItemAmount.text),
                    }
                }
            }, 
            (result) =>
            {
                OnGeneralResult(result);
                RefreshInventoryListing();
            }, OnGeneralError);
        }

        private void OnClickSubstractItem()
        {
            PrototypeAPI.SubstractItem(new ItemManagementRequest()
            {
                itemData = new List<ItemExchangeReadable>()
                {
                    new ItemExchangeReadable()
                    {
                        itemID = inputItemID.text,
                        amount = int.Parse(inputItemAmount.text),
                    }
                }
            },
            (result) =>
            {
                OnGeneralResult(result);
                RefreshInventoryListing();
            }, OnGeneralError);
        }

        private void OnClickSubstractItemInstance()
        {
            PrototypeAPI.SubstractItemInstance(new ItemManagementRequest()
            {
                itemData = new List<ItemExchangeReadable>()
                {
                    new ItemExchangeReadable()
                    {
                        itemID = currentInventory[0].instanceID,
                        amount = 1,
                    }
                }
            },
            (result) =>
            {
                OnGeneralResult(result);
                RefreshInventoryListing();
            }, OnGeneralError);
        }

        private void OnClickBuyItem()
        {
            var req = new ItemManagementRequest()
            {
                itemData = new List<ItemExchangeReadable>()
                {
                    new ItemExchangeReadable()
                    {
                        itemID = inputItemID.text,
                        amount = int.Parse(inputItemAmount.text),
                    }
                }
            };

            Debug.Log("[Test] => " + JsonConvert.SerializeObject(req));
            PrototypeAPI.BuyItem(req,
            (result) =>
            {
                OnGeneralResult(result);
                RefreshInventoryListing();
            }, OnGeneralError);
        }

        private void OnClickSellItem()
        {
            PrototypeAPI.SellItem(new ItemManagementRequest()
            {
                itemData = new List<ItemExchangeReadable>()
                {
                    new ItemExchangeReadable()
                    {
                        itemID = inputItemID.text,
                        amount = int.Parse(inputItemAmount.text),
                    }
                }
            },
            (result) =>
            {
                OnGeneralResult(result);
                RefreshInventoryListing();
            }, OnGeneralError);
        }

        private void OnClickDrawPrize()
        {
            PrototypeAPI.DrawPrize(new DrawPrizeRequest()
            {
                drawCount = int.Parse(inputDrawAmount.text),
                prizeTableID = inputPrizeTableID.text,
            },
            (result) =>
            {
                foreach (var item in result.itemDrawn)
                {
                    Debug.Log($"[Draw Prize] => Item Drawn : {item.itemID}");
                }
                RefreshInventoryListing();
            }, OnGeneralError);
        }

        private void OnClickRefreshInventory()
        {
            RefreshInventoryListing();
        }

        private void OnClickRefreshInventoryAggregate()
        {
            PrototypeAPI.GetInventoryAggregate((result) =>
            {
                currentInventory = result.data;
                DestroyAllChildren(layoutInventory.transform);
                foreach (var item in result.data)
                {
                    var inventory = Instantiate(inventoryPrefab, layoutInventory.transform);
                    inventory.SetInfo(item.itemID, item.amount.ToString());
                }

                StartCoroutine(LateDirty());
            }, OnGeneralError);
        }

        private void OnClickLogout()
        {
            SceneManager.LoadScene("UserScreen");
        }

        public void OnGeneralResult(GeneralResult result)
        {
            Debug.Log($"[{result.httpCode}] -> {result.message}");
        }

        public void OnGeneralError(ErrorResult error)
        {
            Debug.LogError($"[{error.httpCode}] -> {error.message}");
        }
        public void DestroyAllChildren(Transform parentTransform)
        {
            for (int i = parentTransform.childCount - 1; i >= 0; i--)
            {
                Destroy(parentTransform.GetChild(i).gameObject);
            }
        }
    }
}