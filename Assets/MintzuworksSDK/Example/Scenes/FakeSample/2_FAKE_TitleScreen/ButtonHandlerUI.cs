using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandlerUI : MonoBehaviour
{
    public TextMeshProUGUI buttonText;  // Reference to the text inside the button
    private string linkUrl;  // The link to open when the button is clicked

    // Set the button text and link URL
    public void SetButton(string text, string url)
    {
        if (buttonText) buttonText.text = text;
        linkUrl = url;

        // Add a listener to the button click

        var myBtn = GetComponent<Button>();
        
        if (myBtn)
            myBtn.onClick.AddListener(OnButtonClick);
    }

    // Open the URL when the button is clicked
    private void OnButtonClick()
    {
        if (!string.IsNullOrEmpty(linkUrl))
        {
            Application.OpenURL(linkUrl);
        }
    }
}
