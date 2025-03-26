using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using UnityEngine.UI;

public class FakeAddressableConfigurator : MonoBehaviour
{
    public Image uiBackground;
    public string key = "background";
    public SpriteAtlas atlas;
    public string remoteCatalogUrl = "https://ecx-test-addressable.s3.amazonaws.com/StandaloneWindows64/catalog.json";
    public string atlasKey = "itemku";
    private void Start()
    {
        // Initialize Addressables
        Addressables.InitializeAsync().Completed += FakeAddressableConfigurator_Completed;
    }

    private void FakeAddressableConfigurator_Completed(AsyncOperationHandle<UnityEngine.AddressableAssets.ResourceLocators.IResourceLocator> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            //Load Remote Catalog
            Addressables.LoadContentCatalogAsync(remoteCatalogUrl).Completed += OnRemoteCatalogLoaded;
        }
        else
        {
            Debug.LogError("Failed to initialize Addressables.");
        }
    }

    private void OnRemoteCatalogLoaded(AsyncOperationHandle<IResourceLocator> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Remote catalog loaded successfully.");
            
        }
        else
        {
            Debug.LogError("Failed to load remote catalog.");
        }
    }

    [Button]
    private void LoadAtlasImage()
    {
        if (atlas)
        {
            var sprite = atlas.GetSprite(atlasKey);
            if (sprite != null)
            {
                uiBackground.sprite = sprite;
            }
        }
    }

    [Button]
    private void LoadAsset()
    {
        var handle = Addressables.LoadAssetAsync<Sprite>(key);
        handle.Completed += Handle_Completed;
    }

    private void Handle_Completed(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<Sprite> handler)
    {
        if (handler.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            if (handler.Result)
            {
                if (uiBackground)
                {
                    uiBackground.sprite = handler.Result;
                }
            }
        }
        else
        {
            Debug.Log("Fail to load");
        }
    }
}
