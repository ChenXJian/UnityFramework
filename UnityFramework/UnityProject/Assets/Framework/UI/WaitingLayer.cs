using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(RectTransform))]
public class WaitingLayer : MonoBehaviour, ITemplatable
{
    bool isTemplate = true;

    public bool IsTemplate
    {
        get  { return isTemplate; }
        set  { isTemplate = value; }
    }

    public static bool IsShow
    {
        get { return isShow; }
    }

    public string TemplateName { get; set; }

    public static UITemplates<WaitingLayer> Templates = new UITemplates<WaitingLayer>();

    static int? modalKey;
    static bool isShow = false;
    //static Tweener rotator = null;
    static WaitingLayer layer = null;
    static string key = "WaitingTemplate";

    public static void Show()
    {
        if (layer) return;
        layer = Templates.GetDuplicate(key);

        layer.transform.SetParent(Global.MessageCanvas, false);
        layer.gameObject.SetActive(true);

        var rect = layer.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(100, 100);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector3.zero;


        var img = layer.GetComponent<Image>();

        /*
        if(rotator != null)
        {
            rotator.Play();
        }
        else
        {
            rotator = img.transform.DORotate(new Vector3(0, 0, 10), 0.2f, RotateMode.Fast).SetLoops(-1, LoopType.Incremental);
        }
        */

        if (layer)
        {
            modalKey = ModleLayer.Open(layer, color: new Color(0.0f, 0.0f, 0.0f, 0.5f));
        }
        else
        {
            modalKey = null;
        }


        if(LoadingLayer.IsShow)
        {
            layer.transform.SetSiblingIndex(LoadingLayer.GetRenderOrder() - 1);
            if (modalKey != null)
            {
                ModleLayer.SetRenderOrder((int)modalKey, layer.transform.GetSiblingIndex());
            }
        }
        else
        {
            Stick();
        }
        isShow = true;
    }

    public static void Hide()
    {
        if(layer)
        {
            //rotator.Pause();
            Templates.ReturnCache(layer);
            layer = null;
            if (modalKey != null)
            {
                ModleLayer.Close((int)modalKey);
            }
        }
        isShow = false;
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
        if (layer != null)
        {
            layer.transform.SetAsLastSibling();
        }
    }
}
