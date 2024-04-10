using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImaginationOverflow.UniversalDeepLinking;
using System;
using Newtonsoft.Json;
public class DeeplinkExample : MonoBehaviour
{
    void Start()
    {
        DeepLinkManager.Instance.LinkActivated += Instance_LinkActivated;
    }

    private void Instance_LinkActivated(LinkActivation s)
    {
        Debug.Log(JsonConvert.SerializeObject(s));
    }
}
