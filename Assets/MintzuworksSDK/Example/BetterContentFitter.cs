using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BetterContentFitter : ContentSizeFitter, ILayoutChild
{
    public Action OnChange { get; set; }

    [System.NonSerialized] private RectTransform m_Rect;
    private RectTransform rectTransform
    {
        get
        {
            if (m_Rect == null)
                m_Rect = GetComponent<RectTransform>();
            return m_Rect;
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        SetDirty();
        if (!Application.isPlaying) return;

        var listLayout = GetComponentsInChildren<ILayoutChild>().ToList();
        listLayout.Remove(this);

        foreach (var item in listLayout)
        {
            item.OnChange += UpdateSelf; 
        }
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        if (!Application.isPlaying) return;
        var listLayout = GetComponentsInChildren<ILayoutChild>().ToList();
        listLayout.Remove(this);

        foreach (var item in listLayout)
        {
            item.OnChange -= UpdateSelf;
        }
    }

    public void UpdateSelf()
    {
        StartCoroutine(LateDirty());
    }

    public override void SetLayoutHorizontal()
    {
        base.SetLayoutHorizontal();
        OnChange?.Invoke();
    }

    public override void SetLayoutVertical()
    {
        base.SetLayoutVertical();
        OnChange?.Invoke();
    }

    IEnumerator LateDirty()
    {
        if(!Application.isPlaying) yield break;

        yield return new WaitForEndOfFrame();
        SetDirty();
    }
}

public interface ILayoutChild
{
    public System.Action OnChange { get; set; }
}
