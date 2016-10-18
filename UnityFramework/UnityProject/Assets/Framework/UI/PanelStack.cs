using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


public class PanelStack : TSingleton<PanelStack>
{
    public class Panel
    {
        /// <summary>
        /// 面板对象名
        /// </summary>
        public string PanelName { set; get; }

        /// <summary>
        /// 脚本名
        /// </summary>
        public string LogicName { set; get; }

        /// <summary>
        /// 脚本运行时对象
        /// </summary>
        public object LogicObject { set; get; }

        /// <summary>
        /// 面板是否已被创建
        /// </summary>
        public bool IsCreated { set; get; }

        public Panel()
        {
            PanelName = "Noting";
            LogicName = "Noting";
            LogicObject = null;
            IsCreated = false;
        }
    }


    Stack<Panel> _panelStack = new Stack<Panel>();
    Dictionary<string, GameObject> _panelCache = new Dictionary<string, GameObject>();
    Panel panelCur = new Panel();
    Transform rootNode;

    const string disableName = "Disable";
    const string enableName = "Enable";
    const string startupName = "Startup";
    const string freeName = "Free";

    PanelStack() { }

    Transform RootNode
    {
        get
        {
            if (rootNode == null)
                rootNode = Global.PanelWindow;
            return rootNode;
        }
    }

    public Panel PanelCurrent
    {
        get { return panelCur; }
    }

    public bool TryGetPanel(string rLogicName, out Panel rPanel)
    {
        foreach(Panel rElment in _panelStack)
        {
            if (rElment.LogicName == rLogicName)
            {
                rPanel = rElment;
                return true;
            }
        }
        rPanel = new Panel();
        return false;
    }

    public bool IsExist(string rLogicName)
    {
        bool isExist = false;
        _panelStack.ForEach((item) =>
        {
            if (item.LogicName == rLogicName) isExist = true;
        });
        return isExist;
    }


    public GameObject GetCachedPanel(string rPanelName)
    {
        if (_panelCache.ContainsKey(rPanelName))
        {
            return _panelCache[rPanelName];
        }
        else
        {
            return null;
        }
    }

    public void AddCachePanel(string rPanelName)
    {
        Global.AssetLoadManager.LoadUIPanel(rPanelName, (prefab) =>
        {
            if (!_panelCache.ContainsKey(rPanelName))
            {
                _panelCache.Add(rPanelName, prefab);
            }
        });
    }


    public void RemoveCachePanel(string rPanelName)
    {
        if (_panelCache.ContainsKey(rPanelName))
        {
            _panelCache.Remove(rPanelName);
        }
    }

    public Panel PushPanel(string rLogicName)
    {
        if (panelCur != null && panelCur.LogicName != "Noting" && panelCur.PanelName != "Noting")
        {
            if (panelCur.LogicName == rLogicName)
            {
                Debug.Log(rLogicName + " is repeat");
                return panelCur;
            }

            if (panelCur.LogicObject != null)
            {
                LShapUtil.CallScriptFunction(panelCur.LogicObject, panelCur.LogicName, disableName);
            }
        }


        Panel rPanel = null;
        bool rGot = TryGetPanel(rLogicName, out rPanel);
        if (rGot)
        {
            panelCur = rPanel;

            LShapUtil.CallScriptFunction(panelCur.LogicObject, panelCur.LogicName, enableName);
            StickUpElement(panelCur);
        }
        else
        {
            var rPanelName = rLogicName.Replace("Logic", "Panel");

            Panel rNewPanel = new Panel();
            rNewPanel.IsCreated = false;
            rNewPanel.LogicName = rLogicName;
            rNewPanel.PanelName = rPanelName;
            rNewPanel.LogicObject = LShapUtil.CreateScriptObject(rLogicName);
            _panelStack.Push(rNewPanel);

            panelCur = rNewPanel;
            LShapUtil.CallScriptFunction(panelCur.LogicObject, panelCur.LogicName, startupName, RootNode);

        }
        return panelCur;
    }

    public Panel PopPanel()
    {
        if (_panelStack.Count < 2)
        {
            throw new UnassignedReferenceException("_panelStack don't can Pop Panel");
        }

        var panel = _panelStack.Pop();
        LShapUtil.CallScriptFunction(panel.LogicObject, panel.LogicName, disableName);
        panelCur = _panelStack.Pop();
        LShapUtil.CallScriptFunction(panelCur.LogicObject, panelCur.LogicName, enableName);

        _panelStack.Push(panel);
        _panelStack.Push(panelCur);
        StickDownElement(panel);
        return panel;
    }

    public void ReplacePanel(string rLogicName)
    {
        //check safe
        if (_panelStack.Count < 1)
        {
            DebugConsole.Log("_panelStack is null, don't can replace Panel");
            return;
        }

        if (panelCur.LogicName == rLogicName)
        {
            DebugConsole.Log(rLogicName + " is repeat");
            return;
        }

        Panel panel = _panelStack.Pop();
        LShapUtil.CallScriptFunction(panel.LogicObject, panel.LogicName, freeName);
        panel = null;
        panelCur = new Panel();

        PushPanel(rLogicName);
    }

    public void ClearStack()
    {
        _panelStack.ForEach((item) =>
        {
            LShapUtil.CallScriptFunction(item.LogicObject, item.LogicName, freeName);
            item = null;
        });
        _panelStack.Clear();
        _panelStack.TrimExcess();
    }

    public void ClearStackUnlessFocus()
    {
        List<Panel> temp = new List<Panel>(_panelStack);

        _panelStack.Clear();
        _panelStack.TrimExcess();

        for(int i = 0; i < temp.Count; i++)
        {
            if (temp[i].LogicName == panelCur.LogicName) 
            {
                continue;
            }
            else
            {
                LShapUtil.CallScriptFunction(temp[i].LogicObject, temp[i].LogicName, freeName);
                temp.RemoveAt(i);
            }
        }

        _panelStack = new Stack<Panel>(temp);
    }

    void StickUpElement(Panel element)
    {
        if (element == null) return;

        Stack<Panel> tempStack = new Stack<Panel>();

        while (_panelStack.Count > 0)
        {
            if (_panelStack.Peek() != element)
                tempStack.Push(_panelStack.Pop());
            else _panelStack.Pop();
        }
       
        _panelStack.Clear();

        while (tempStack.Count > 0)
        {
            _panelStack.Push(tempStack.Pop());
        }

        _panelStack.Push(element);
    }

    void StickDownElement(Panel element)
    {
        if (element == null) return;

        Stack<Panel> tempStack = new Stack<Panel>();

        while (_panelStack.Count > 0)
        {
            if (_panelStack.Peek() != element)
                tempStack.Push(_panelStack.Pop());
            else _panelStack.Pop();
        }

        _panelStack.Clear();
        _panelStack.Push(element);
        while (tempStack.Count > 0)
        {
            _panelStack.Push(tempStack.Pop());
        }
    }

    public void Destroy()
    {
        ClearStack();
        panelCur = null;
        _panelStack = null;
    }

}
