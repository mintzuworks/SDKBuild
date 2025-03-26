using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.IO;
using Utilities.WebRequestRest;
using Cysharp.Threading.Tasks;

public class NewsImageUI : MonoBehaviour
{
    public Image targetImage;  // Reference to the UI Image component to apply the texture

    public async UniTaskVoid SetImageUrl(string imageUrl)
    {
        var result = await Rest.DownloadTextureAsync(imageUrl);
        if (result != null)
        {
            if (result != null)
            {
                Rect rect = new Rect(0, 0, result.width, result.height);
                Sprite sprite = Sprite.Create(result, rect, new Vector2(0.5f, 0.5f));
                targetImage.sprite = sprite;
            }
        }
    }
}
