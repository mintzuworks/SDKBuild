using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mintzuworks.Example
{
    public class Example_InventoryUI : MonoBehaviour
    {
        public TextMeshProUGUI txtName;
        public TextMeshProUGUI txtAmount;

        public void SetInfo(string itemName, string amount)
        {
            txtName.text = itemName;
            txtAmount.text = amount;
        }
    }
}