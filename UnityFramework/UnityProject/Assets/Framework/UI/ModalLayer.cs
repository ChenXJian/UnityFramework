using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class ModleLayer : MonoBehaviour, ITemplatable
{
    bool isTemplate = true;
    public bool IsTemplate
    {
        get
        {
            return isTemplate;
        }
        set
        {
            isTemplate = value;
        }
    }

    public string TemplateName { get; set; }

    static string key = "ModalTemplate";

    public static UITemplates<ModleLayer> Templates = new UITemplates<ModleLayer>();

    static int _ReferencedCount = 0;

    static Dictionary<int, Transform> ReferencedNodes = new Dictionary<int, Transform>();

    static ModleLayer modal;

    /// <summary>
    /// 打开一个模态层
    /// </summary>
    public static int Open(MonoBehaviour parent, Sprite sprite = null, Color? color = null)
    {
        if (!Templates.ExistsTemplate(key))
        {
            CreateTemplate();
        }

        modal = Templates.GetDuplicate(key);
        
        modal.transform.SetParent(parent.transform.parent, false);
        modal.gameObject.SetActive(true);
        modal.transform.SetAsLastSibling();

        var rect = modal.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(Global.ReferenceResolution.x, Global.ReferenceResolution.y);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector3.zero;
        rect.localScale = Vector3.one;

        var img = modal.GetComponent<Image>();

        if (sprite != null)
        {
            img.sprite = sprite;
        }
        if (color != null)
        {
            img.color = (Color)color;
        }


        _ReferencedCount++;


        if (!ReferencedNodes.ContainsKey(parent.gameObject.GetInstanceID()))
        {
            ReferencedNodes.Add(parent.gameObject.GetInstanceID(), parent.transform);
        }
        return parent.gameObject.GetInstanceID();
    }

    /// <summary>
    /// 关闭一个模态层
    /// </summary>
    public static void Close(int index)
    {
        ReferencedNodes.Remove(index);
        if (_ReferencedCount > 1)
        {
            _ReferencedCount--;
            var count = ReferencedNodes.Count;

            var enumerator = ReferencedNodes.Values.GetEnumerator();
            while (count > 0)
            { 

                count--;
                enumerator.MoveNext();
                if (count == 0)
                {
                    modal.transform.SetParent(enumerator.Current.parent);
                    modal.transform.SetSiblingIndex(enumerator.Current.GetSiblingIndex() - 1);
                }
            }
        }
        else
        {
            _ReferencedCount = 0;
            Templates.ReturnCache(modal);
        }
    }

    public static void SetRenderOrder(int index, int order)
    {
        modal.transform.SetSiblingIndex(order);
    }

    public static int GetRenderOrder(int index)
    {
        return modal.transform.GetSiblingIndex();
    }

    static void CreateTemplate()
    {
        var rTemplate = new GameObject(key);

        var rModal = rTemplate.AddComponent<ModleLayer>();
        rTemplate.AddComponent<Image>();
        var temp = Util.Child(Global.MessageCanvas, "Templates");
        rTemplate.transform.SetParent(temp.transform);
        Templates.AddTemplate(key, rModal);
    }
}
