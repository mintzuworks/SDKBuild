using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackendTest : MonoBehaviour
{
    public TMPro.TMP_InputField input;
    public UniWebView webView;
    // Start is called before the first frame update
    public void Click()
    {
        Debug.Log(input.text);
        webView.Load(input.text);
        webView.Show();

        webView.OnMessageReceived += WebView_OnMessageReceived;
        webView.OnPageFinished += WebView_OnPageFinished;
    }

    private void WebView_OnPageFinished(UniWebView webView, int statusCode, string url)
    {
        Debug.Log(url);
        Debug.Log(statusCode);
    }

    private void WebView_OnMessageReceived(UniWebView webView, UniWebViewMessage message)
    {
        Debug.Log(message);
    }
}
