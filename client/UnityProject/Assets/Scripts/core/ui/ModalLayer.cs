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

    public static UITemplates<ModleLayer> Templates = new UITemplates<ModleLayer>();

    static string key = "ModalTemplate";
    static Dictionary<int, ModleLayer> usedPool = new Dictionary<int, ModleLayer>();

    /// <summary>
    /// 打开一个模态层
    /// </summary>
    public static int Open(MonoBehaviour parent, Sprite sprite = null, Color? color = null)
    {
        if (!Templates.Exists(key))
        {
            CreateTemplate();
        }

        var modal = Templates.Instance(key);
        
        modal.transform.SetParent(parent.transform.parent, false);
        modal.gameObject.SetActive(true);
        modal.transform.SetAsLastSibling();

        var rect = modal.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(AppConst.ReferenceResolution.x, AppConst.ReferenceResolution.y);
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

        usedPool.Add(modal.GetInstanceID(), modal);
        return modal.GetInstanceID();
    }

    /// <summary>
    /// 关闭一个模态层
    /// </summary>
    public static void Close(int index)
    {
        Templates.ReturnCache(usedPool[index]);
        usedPool.Remove(index);
    }

    public static void SetRenderOrder(int index, int order)
    {
        usedPool[index].transform.SetSiblingIndex(order);
    }

    public static int GetRenderOrder(int index)
    {
        return usedPool[index].transform.GetSiblingIndex();
    }

    static void CreateTemplate()
    {
        var rTemplate = new GameObject(key);

        var rModal = rTemplate.AddComponent<ModleLayer>();
        rTemplate.AddComponent<Image>();
        rTemplate.transform.SetParent(gate.Templates);

        Templates.Add(key, rModal);
    }
}
