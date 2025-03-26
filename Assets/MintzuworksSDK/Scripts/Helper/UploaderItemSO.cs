using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mintzuworks.Network;
using GacoGames.InventorySystem;
using Utilities.WebRequestRest;
using Newtonsoft.Json;
using System;
using Sirenix.OdinInspector;
using Mintzuworks.Domain;
public class UploaderItemSO : MonoBehaviour
{
    public ServerConfig ServerConfig;
    public List<ItemSO> Items;

    public void Start()
    {
        ClientAPI.BaseURL = ServerConfig.serverURL;
    }

    [Button]
    public async void UploadItem()
    {
        foreach (var item in Items)
        {
            var catalogItem = new CatalogueItem()
            {
                ItemID = item.itemId,
                Description = item.Description,
                Type = item.Type.ToString(),
                IsStackable = false,
                IsRechargeable = false,
                StartingAmount = 0,
                MaxAmount = 0,
                RechargeRate = 0,
                MaxRecharge = 0,
                BuyPrice = new List<ItemExchange>(),
                CustomSellPrice = new List<ItemExchange>(),
                CustomJSON = new Dictionary<string, object>()
            };

            var data = JsonConvert.SerializeObject(catalogItem);
            var result = await Rest.PostAsync(ServerConfig.serverURL + "item", data);

            result.Validate(debug: true);
            if (!result.Successful)
            {
                Debug.LogError($"{item.itemId} failed to upload.");
            }
        }

        Debug.Log("Upload done!");
    }

    [Button]
    public async void TestGetAllItem()
    {
        ClientAPI.BaseURL = ServerConfig.serverURL;
        var result = await ClientAPI.GetAllItem();

        if (result != null)
        {
            foreach (var item in result.data)
            {
                Debug.Log($"{item.ItemID} gotcha!");
            }
        }
    }
}
