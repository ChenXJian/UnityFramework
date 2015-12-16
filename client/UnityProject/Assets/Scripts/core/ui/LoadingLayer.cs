using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class LoadingLayer : MonoBehaviour, ITemplatable
{
    bool isTemplate = true;

    public bool IsTemplate
    {
        get { return isTemplate; }
        set { isTemplate = value; }
    }

    public static bool IsShow 
    {
        get { return isShow; }
    }

    public string TemplateName { get; set; }
    public static UITemplates<LoadingLayer> Templates = new UITemplates<LoadingLayer>();
    public static Progressbar progressbar = null;
    public static Text textTips = null;

    static bool isShow = false;
    static LoadingLayer layer = null;
    static string key = "LoadingTemplate";


    public static void Show()
    {
        if (layer) return;
        layer = Templates.Instance(key);

        layer.transform.SetParent(gate.MessageCanvas, false);
        layer.gameObject.SetActive(true);

        var rect = layer.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(0, 0);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector3.zero;

        if (progressbar == null)
        {
            progressbar = Util.Get<Progressbar>(layer.gameObject, "Progressbar");
        }
        if (textTips == null)
        {
            textTips = Util.Get<Text>(layer.gameObject, "TextTips");
        }

        Stick();

        isShow = true;
    }

    public static void Hide()
    {
        if (layer)
        {
            Templates.ReturnCache(layer);
            layer = null;
        }

        isShow = false;
    }

    public static void SetProgressbarValue(int rProgress)
    {
        if (progressbar != null)
        {
            progressbar.Value = rProgress;
        }
    }

    public static void SetProgressbarTips(string rText)
    {
        if (textTips != null)
        {
            textTips.text = rText;
        }
    }

    public static int GetRenderOrder()
    {
        if (layer != null)
        {
            return layer.transform.GetSiblingIndex();
        }
        else
        {
            throw new NullReferenceException("WaitingLayer is null");
        }
    }

    public static void SetRenderOrder(int order)
    {
        if (layer != null)
        {
            layer.transform.SetSiblingIndex(order);
        }
        else
        {
            throw new NullReferenceException("WaitingLayer is null");
        }
    }

    public static void Stick()
    {
        if(layer != null)
        {
            layer.transform.SetAsLastSibling();
        }
    }
    
}
