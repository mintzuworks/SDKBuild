using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemSelectorUI : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public Image itemIcon;
    public GameObject highlight;
    public Button myButton;
    public TextMeshProUGUI amount;
    public async void SetInfo(string itemName, string iconPath, UnityAction OnClick = null, int amount = 0)
    {
        if (this.amount)
        {
            this.amount.gameObject.SetActive(amount > 0);
            this.amount.text = amount.ToString();
        }
        this.itemName.text = itemName;

        if (myButton)
            myButton.onClick.AddListener(OnClick);
        
        try
        {
            var isExist = await Addressables.LoadResourceLocationsAsync(iconPath).ToUniTask();
            if (isExist != null)
            {
                if (isExist.Count > 0)
                {
                    var result = await Addressables.LoadAssetAsync<Sprite>(iconPath).ToUniTask(autoReleaseWhenCanceled: true);
                    if (result != null)
                    {
                        itemIcon.sprite = result;
                    }
                }
            }
        }
        catch (InvalidKeyException)
        {
            // Suppress the error if the key does not exist
            Debug.LogWarning($"No location found for key: {iconPath}. Icon loading skipped.");
        }
    }

    public void SetHighlight(bool value)
    {
        if (highlight) highlight.SetActive(value);
    }
}
