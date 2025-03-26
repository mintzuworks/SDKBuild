using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Mintzuworks.FakeSample
{
    public class NewsSelectorUI : MonoBehaviour
    {
        public Image image;
        public TextMeshProUGUI title;
        public Button selfButton;

        public void Set(string title, UnityAction OnClick)
        {
            this.title.text = title;
            selfButton.onClick.AddListener(OnClick);
        }

        public void SetHighlight(bool val)
        {
            Color highlight = Color.white;
            if (ColorUtility.TryParseHtmlString("#EE973B", out var parsedUnhighlight))
            {
                highlight = parsedUnhighlight;
            }
            image.color = !val ? Color.white : highlight;
        }
    }
}