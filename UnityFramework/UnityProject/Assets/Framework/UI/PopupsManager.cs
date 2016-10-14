using UnityEngine;
using System.Collections;
using System;

public class PopupsManager : TSingleton<PopupsManager>
{

    PopupsManager()
    {
        Templates = new UITemplates<PopupsBehaviour>(OnCreatedDuplicate);
    }

    public UITemplates<PopupsBehaviour> Templates = new UITemplates<PopupsBehaviour>();


    /// <summary>
    /// 显示弹窗
    /// </summary>
    public T ShowPopups<T>(string popupsName, bool modal = true)
    {
        var popups = (T)(Template(popupsName).GetLShapObject());
        if (popups == null)
        {
            DebugConsole.LogError("popups not find :" + popupsName);
            throw new NullReferenceException("Not find popups");
        }
        else
        {
            LShapUtil.CallScriptFunction(popups, popupsName, "Show", modal);
        }
        return popups;
    }

    /// <summary>
    /// 隐藏弹窗
    /// </summary>
    public void HidePopups<T>(T popups, string popupsName)
    {
        if (popups == null)
        {
            DebugConsole.LogError("popups not find :" + popupsName);
            throw new NullReferenceException("Not find popups");
        }
        else
        {
            LShapUtil.CallScriptFunction(popups, popupsName, "Hide");
        }
    }

    /// <summary>
    /// 取提示窗
    /// </summary>
    public T GetPopups<T>(string popupsName)
    {
        var popups = (T)(Template(popupsName).GetLShapObject());
        return popups;
    }

    /// <summary>
    /// 清理弹窗缓存
    /// </summary>
    public void ClearCache()
    {
        Templates.ClearCache();
    }


    public PopupsBehaviour Template(string template)
    {
        var rPopups = Templates.GetDuplicate(template);
        return rPopups;
    }

    public void OnCreatedDuplicate(PopupsBehaviour duplicate)
    {
        duplicate.transform.SetParent(Global.PopupsWindow, false);
        duplicate.SetLShapObject(LShapUtil.CreateScriptObject(duplicate.TemplateName, duplicate));
    }

}
