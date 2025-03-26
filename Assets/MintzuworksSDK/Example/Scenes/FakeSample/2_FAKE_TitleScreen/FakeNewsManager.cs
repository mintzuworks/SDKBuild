using Mintzuworks.Domain;
using Mintzuworks.Network;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace Mintzuworks.FakeSample
{
    public class FakeNewsManager : MonoBehaviour
    {
        [Title("News List")]
        public Transform targetNewsListWidget;
        public NewsSelectorUI newsSelectorUI;

        [Title("Content Data")]
        public Transform targetContentWidget;
        public GameObject title;
        public GameObject releaseAt;
        public GameObject contentText;
        public GameObject imagePrefab;
        public GameObject buttonPrefab;

        private const int h1FontSize = 18;
        private const int pFontSize = 13;

        [Title("Testing")]
        public string newsTitle = "My Patch Note";
        public string htmlContent = "<h1>Patch Notes</h1><p>New features in this update:</p><ul><li>Feature A added</li><li>Improved performance</li><li>Bug fixes</li></ul><img src='https://example.com/image.png'><button a='https://example.com'>Learn More</button>";

        private string tempText;
        private void Start()
        {
            GetNews();
        }

        public static System.Action OnHide;
        public static System.Action OnShow;

        List<NewsSelectorUI> list;
        List<NewsData> newsData;
        private async void GetNews()
        {
            list = new List<NewsSelectorUI>();
            DestroyAllChild(targetNewsListWidget);
            OnShow?.Invoke();
            var result = await ClientAPI.GetAllNews();
            OnHide?.Invoke();

            if (result != null && result.httpCode == (int)HttpStatusCode.OK)
            {
                newsData = result.data;
                foreach (var item in result.data)
                {
                    var selectorUI = Instantiate(newsSelectorUI, targetNewsListWidget);
                    if (selectorUI != null)
                    {
                        var id = item.ID;
                        selectorUI.Set(item.title, () =>
                        {
                            OnSelectorClicked(id, selectorUI);
                        });

                        list.Add(selectorUI);
                    }
                }
            }
        }

        private void OnSelectorClicked(string id, NewsSelectorUI ui)
        {
            list.ForEach(item =>
            {
                item.SetHighlight(false);
            });
            ui.SetHighlight(true);
            DestroyAllChild(targetContentWidget);

            var find = newsData.Find(x => x.ID == id);
            if (find != null)
            {
                // Convert the HTML content to TMP format and display it
                var spawnedTitle = Instantiate(title, targetContentWidget);
                var spawnedRelease = Instantiate(releaseAt, targetContentWidget);
                string formattedDate = "Release Date: " + find.createdDate.ToString("MMMM dd, yyyy");

                var textSpawnedTitle = spawnedTitle.GetComponent<TextMeshProUGUI>();
                if (textSpawnedTitle != null)
                {
                    textSpawnedTitle.text = find.title;
                }

                var textSpawnedRelease = spawnedRelease.GetComponent<TextMeshProUGUI>();
                if (textSpawnedRelease != null)
                {
                    textSpawnedRelease.text = formattedDate;
                }

                List<string> htmlElements = SplitHtmlToElements(find.body);
                // Process each element in sequence
                foreach (var element in htmlElements)
                {
                    ConvertHtmlElementToTMP(element);
                }
            }
            else
            {
                Debug.Log("News not found");
            }
        }


        private void TryCreate()
        {
            // Parse HTML and split into a list of tags and content
            List<string> htmlElements = SplitHtmlToElements(htmlContent);

            // Process each element in sequence
            foreach (var element in htmlElements)
            {
                ConvertHtmlElementToTMP(element);
            }
        }

        // Method to split HTML content into individual tags and text
        private List<string> SplitHtmlToElements(string html)
        {
            // Regular expression to capture all tags (e.g., <h1>, <p>, <li>, <img>, <button>)
            string pattern = @"(<h1>.*?</h1>)|(<p>.*?</p>)|(<li>.*?</li>)|(<img src=[""'].*?[""']>)|(<button a=[""'].*?[""']>.*?</button>)";
            MatchCollection matches = Regex.Matches(html, pattern);

            List<string> elements = new List<string>();

            foreach (Match match in matches)
            {
                elements.Add(match.Value);
            }

            return elements;
        }

        // Process individual HTML element and instantiate the corresponding GameObject
        private void ConvertHtmlElementToTMP(string element)
        {
            // Convert <h1> tags
            if (Regex.IsMatch(element, "<h1>.*?</h1>"))
            {
                string content = Regex.Match(element, "<h1>(.*?)</h1>").Groups[1].Value;
                InstantiateText($"<size={h1FontSize}><b>{content}</b></size>");
            }
            // Convert <p> tags
            else if (Regex.IsMatch(element, "<p>.*?</p>"))
            {
                string content = Regex.Match(element, "<p>(.*?)</p>").Groups[1].Value;
                InstantiateText($"<size={pFontSize}>{content}</size>");
            }
            // Convert <li> tags (bullet points)
            else if (Regex.IsMatch(element, "<li>.*?</li>"))
            {
                string content = Regex.Match(element, "<li>(.*?)</li>").Groups[1].Value;
                InstantiateText($"- {content}");
            }
            // Process <img> tags with both single and double quotes
            else if (Regex.IsMatch(element, "<img src=[\"'].*?[\"']>"))
            {
                string imageUrl = Regex.Match(element, "<img src=[\"'](.*?)[\"']>").Groups[1].Value;
                InstantiateImageDownloader(imageUrl);
            }
            // Process <button> tags with both single and double quotes
            else if (Regex.IsMatch(element, "<button a=[\"'].*?[\"']>.*?</button>"))
            {
                string buttonLink = Regex.Match(element, "<button a=[\"'](.*?)[\"']>").Groups[1].Value;
                string buttonText = Regex.Match(element, "<button a=[\"'].*?[\"']>(.*?)</button>").Groups[1].Value;
                InstantiateButton(buttonText, buttonLink);
            }
        }

        // Instantiate a GameObject with TextMeshProUGUI and set its text content
        private void InstantiateText(string content)
        {
            // Instantiate the text prefab at the parent transform
            GameObject textObject = Instantiate(contentText, targetContentWidget);

            // Set the TextMeshProUGUI text content
            TextMeshProUGUI tmpText = textObject.GetComponent<TextMeshProUGUI>();
            if (tmpText != null)
            {
                tmpText.text = content;  // Assign the parsed HTML content
            }
        }
        // Instantiate the prefab and pass the image URL to download
        private void InstantiateImageDownloader(string imageUrl)
        {
            // Instantiate the image prefab at the target transform
            GameObject imageObject = Instantiate(imagePrefab, targetContentWidget);

            // Get the ImageURLDownloader component and pass the URL
            var imageDownloader = imageObject.GetComponent<NewsImageUI>();
            if (imageDownloader != null)
            {
                imageDownloader.SetImageUrl(imageUrl).Forget();  // Set the image URL
            }
        }

        // Instantiate the button prefab with the link and text
        private void InstantiateButton(string buttonText, string buttonLink)
        {
            // Instantiate the button prefab at the target transform
            GameObject buttonObject = Instantiate(buttonPrefab, targetContentWidget);

            // Get the ButtonHandler component and set up the link and text
            var buttonHandler = buttonObject.GetComponent<ButtonHandlerUI>();
            if (buttonHandler != null)
            {
                buttonHandler.SetButton(buttonText, buttonLink);
            }
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
    }
}