using Mintzuworks.Domain;
using Mintzuworks.Network;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mintzuworks.FakeSample
{
    public class FakeItemManagement : MonoBehaviour
    {
        [Title("Item List")]
        public Transform targetItemListWidget;
        public ItemSelectorUI itemSelectorUI;
        public TMP_InputField inputAmount;
        public Button btnAdd;
        public Button btnBuy;
        public Button btnSubstract;
        public Button btnSubstractInstance;
        public Button btnSell;
        public Button btnSellInstance;

        [Title("Menu")]
        public Button btnShowAllItem;
        public Button btnShowDefaultInventory;
        private Image imageShowAllItem;
        private Image imageShowDefaultInventory;

        public ServerConfig serverConfig;
        public static System.Action OnHide;
        public static System.Action OnShow;

        ItemManagementState state;
        List<CatalogueItem> allItemData;
        List<ItemSelectorUI> selectors;
        List<ItemInstance> itemInstances;
        CatalogueItem selectedItem;
        ItemInstance selectedItemInstance;
        private void Start()
        {
            #region Initialization
            ClientAPI.BaseURL = serverConfig.serverURL;
            imageShowAllItem = btnShowAllItem.GetComponent<Image>();
            imageShowDefaultInventory = btnShowDefaultInventory.GetComponent<Image>();
            DehighlightAllButton();
            selectors = new List<ItemSelectorUI>();
            allItemData = new List<CatalogueItem>();
            #endregion

            #region UI Callback
            btnShowAllItem.onClick.AddListener(OnShowAllItem);
            btnShowDefaultInventory.onClick.AddListener(OnShowDefaultInventory);

            btnAdd.onClick.AddListener(OnAddSelectedItem);
            btnBuy.onClick.AddListener(OnBuySelectedItem);
            btnSubstract.onClick.AddListener(OnSubstractSelectedItem);
            btnSubstractInstance.onClick.AddListener(OnSubstractSelectedItemInstance);
            btnSell.onClick.AddListener(OnSellSelectedItem);
            btnSellInstance.onClick.AddListener(OnSellSelectedItemInstance);
            #endregion

            #region Starting Value
            OnShowAllItem();
            HighlightButton(imageShowAllItem);
            #endregion
        }


        #region Item Management Usecase

        private void SetState(ItemManagementState state)
        {
            this.state = state;

            switch (state)
            {
                case ItemManagementState.Gallery:
                    btnAdd.interactable = true;
                    btnBuy.interactable = true;
                    btnSubstract.interactable = true;
                    btnSell.interactable = true;
                    btnSubstractInstance.interactable = false;
                    btnSellInstance.interactable = false;
                    break;
                case ItemManagementState.Inventory:
                    btnAdd.interactable = false;
                    btnBuy.interactable = false;
                    btnSubstract.interactable = false;
                    btnSell.interactable = false;
                    btnSubstractInstance.interactable = true;
                    btnSellInstance.interactable = true;
                    break;
                default:
                    break;
            }
        }
        private async void OnShowAllItem()
        {
            SetState(ItemManagementState.Gallery);
            selectors.Clear();
            DestroyAllChild(targetItemListWidget);
            OnShow?.Invoke();
            var result = await ClientAPI.GetAllItem();
            OnHide?.Invoke();

            if (result != null && result.httpCode == (int)HttpStatusCode.OK)
            {
                allItemData = result.data;
                foreach (var item in result.data)
                {
                    var selectorUI = Instantiate(itemSelectorUI, targetItemListWidget);
                    if (selectorUI != null)
                    {
                        var id = item.ID;
                        selectorUI.SetInfo(GetItemTranslation(item.ItemID), GetIconPath(item.ItemID), () =>
                        {
                            OnItemSelected(id, selectorUI);
                        });

                        selectorUI.SetHighlight(false);
                        selectors.Add(selectorUI);
                    }
                }
            }

            DehighlightAllButton();
            HighlightButton(imageShowAllItem);
        }
        private async void OnShowDefaultInventory()
        {
            SetState(ItemManagementState.Inventory);
            itemInstances = new List<ItemInstance>();
            selectors.Clear();
            DestroyAllChild(targetItemListWidget);
            OnShow?.Invoke();
            var result = await ClientAPI.GetInventory();
            OnHide?.Invoke();

            if (result != null && result.httpCode == (int)HttpStatusCode.OK)
            {
                itemInstances = result.data;
                foreach (var item in result.data)
                {
                    var selectorUI = Instantiate(itemSelectorUI, targetItemListWidget);
                    if (selectorUI != null)
                    {
                        var id = item.itemID;
                        selectorUI.SetInfo(GetItemTranslation(id), GetIconPath(id), () =>
                        {
                            OnItemInstanceSelected(item.OID, selectorUI);
                        }, item.amount);

                        selectorUI.SetHighlight(false);
                        selectors.Add(selectorUI);
                    }
                }
            }
            DehighlightAllButton();
            HighlightButton(imageShowDefaultInventory);
        }
        private async void OnAddSelectedItem()
        {
            if (selectedItem == null) return;

            var result = await ClientAPI.AddItem(new ItemManagementRequest()
            {
                itemData = new List<ItemExchangeReadable>()
                {
                    new ItemExchangeReadable()
                    {
                        itemID = selectedItem.ItemID,
                        amount = int.Parse(inputAmount.text),
                    }
                }
            });

            if (result != null && result.httpCode == (int)HttpStatusCode.OK)
            {
                Debug.Log("Inventory added");
            }
            else OnError(result.httpCode);
        }
        private async void OnBuySelectedItem()
        {
            if (selectedItem == null) return;

            var result = await ClientAPI.BuyItem(new ItemManagementRequest()
            {
                itemData = new List<ItemExchangeReadable>()
                {
                    new ItemExchangeReadable()
                    {
                        itemID = selectedItem.ItemID,
                        amount = int.Parse(inputAmount.text),
                    }
                }
            });

            if (result != null && result.httpCode == (int)HttpStatusCode.OK)
            {
                Debug.Log("Bought!");
            }
            else OnError(result.httpCode);
        }

        private async void OnSubstractSelectedItem()
        {
            if (selectedItem == null) return;

            var result = await ClientAPI.SubstractItem(new ItemManagementRequest()
            {
                itemData = new List<ItemExchangeReadable>()
                {
                    new ItemExchangeReadable()
                    {
                        itemID = selectedItem.ItemID,
                        amount = int.Parse(inputAmount.text)
                    }
                }
            });

            if (result != null && result.httpCode == (int)HttpStatusCode.OK)
            {
                Debug.Log("Substracted");
            }
            else OnError(result.httpCode);
        }

        private async void OnSubstractSelectedItemInstance()
        {
            if (selectedItemInstance == null) return;

            var result = await ClientAPI.SubstractItemInstance(new ItemManagementRequest()
            {
                itemData = new List<ItemExchangeReadable>()
                {
                    new ItemExchangeReadable()
                    {
                        itemID = selectedItemInstance.instanceID,
                        amount = int.Parse(inputAmount.text)
                    }
                }
            });

            if (result != null && result.httpCode == (int)HttpStatusCode.OK)
            {
                Debug.Log("Substracted");
            }
            else OnError(result.httpCode);
        }

        private async void OnSellSelectedItem()
        {
            if (selectedItem == null) return;

            var result = await ClientAPI.SellItem(new ItemManagementRequest()
            {
                itemData = new List<ItemExchangeReadable>()
                {
                    new ItemExchangeReadable()
                    {
                        itemID = selectedItem.ItemID,
                        amount = int.Parse(inputAmount.text)
                    }
                }
            });

            if (result != null && result.httpCode == (int)HttpStatusCode.OK)
            {
                Debug.Log("Sold");
            }
            else OnError(result.httpCode);
        }

        private async void OnSellSelectedItemInstance()
        {
            if (selectedItemInstance == null) return;

            var result = await ClientAPI.SellItemInstance(new ItemManagementRequest()
            {
                itemData = new List<ItemExchangeReadable>()
                {
                    new ItemExchangeReadable()
                    {
                        itemID = selectedItemInstance.instanceID,
                        amount = int.Parse(inputAmount.text)
                    }
                }
            });

            if (result != null && result.httpCode == (int)HttpStatusCode.OK)
            {
                Debug.Log("Sold");
            }
            else OnError(result.httpCode);
        }


        private static void OnError(long httpCode)
        {
            switch (httpCode)
            {
                case (int)HttpStatusCode.BadRequest:
                    Debug.Log("Please check your request");
                    break;
                case (int)HttpStatusCode.NotFound:
                    Debug.Log("Item is not found in the inventory");
                    break;
                case (int)HttpStatusCode.Conflict:
                    Debug.Log("Insufficient item amount");
                    break;
                default:
                    Debug.Log("Server error");
                    break;
            }
        }

        private void OnItemSelected(string id, ItemSelectorUI selectorUI)
        {
            selectors.ForEach((x) =>
            {
                x.SetHighlight(false);
            });
            selectorUI.SetHighlight(true);
            
            var find = allItemData.Find(x => x.ID == id);
            if (find != null)
            {
                selectedItem = find;
            }
        }

        private void OnItemInstanceSelected(string id, ItemSelectorUI selectorUI)
        {
            selectors.ForEach((x) =>
            {
                x.SetHighlight(false);
            });
            selectorUI.SetHighlight(true);

            var find = itemInstances.Find(x => x.OID == id);
            if (find != null)
            {
                selectedItemInstance = find;
            }
        }
        #endregion

        #region UI Utilities
        private void DehighlightAllButton()
        {
            imageShowAllItem.color = Color.white;
            imageShowDefaultInventory.color = Color.white;
        }
        private void HighlightButton(Image image)
        {
            Color unhighlight = Color.white;
            if (ColorUtility.TryParseHtmlString("#EE973B", out var parsedUnhighlight))
            {
                unhighlight = parsedUnhighlight;
            }
            image.color = unhighlight;
        }
        public string GetItemTranslation(string itemID)
        {
            // var temp = LocalizationManager.GetTranslation($"item/{itemID}/title");
            // if (string.IsNullOrEmpty(temp))
            //     temp = itemID;
            return itemID;
        }
        public string GetIconPath(string itemID)
        {
            return $"InventoryIcon/{itemID}.png";
        }

        private void DestroyAllChild(Transform target)
        {
            if (!Application.isPlaying) return;
            for (int i = target.childCount - 1; i >= 0; i--)
            {
                var child = target.GetChild(i);
                Destroy(child.gameObject);
            }
        }
        #endregion

        public enum ItemManagementState
        {
            Gallery,
            Inventory
        }
    }
}
