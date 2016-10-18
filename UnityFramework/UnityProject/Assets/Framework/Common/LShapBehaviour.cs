using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class LShapBehaviour : MonoBehaviour
{
    object lShapObject;

    bool isNeedStart = false;
    bool isNeedUpdate = false;
    bool isNeedLateUpdate = false;
    bool isNeedFixedUpdate = false;
    bool isNeedOnDestroy = false;

    const string startName = "Start";
    const string updateName = "Update";
    const string lateUpdateName = "LateUpdate";
    const string fixedUpdateName = "FixedUpdate";
    const string onDestroyName = "OnDestroy";

    protected virtual void Awake()
    {
        CLRSharp.ICLRType rType = null;
        var rName = name.Replace("(Clone)", "");
        bool rGot = LShapClient.Instance.TryGetType(rName, out rType);

        if(rGot)
        {
            if (lShapObject == null)
            {
                SetLShapObject(LShapClient.Instance.CreateScriptObject(rType));
            }
            isNeedStart = LShapClient.Instance.CheckExistMethod(rType, startName);
            isNeedUpdate = LShapClient.Instance.CheckExistMethod(rType, updateName);
            isNeedLateUpdate = LShapClient.Instance.CheckExistMethod(rType, lateUpdateName);
            isNeedFixedUpdate = LShapClient.Instance.CheckExistMethod(rType, fixedUpdateName);
            isNeedOnDestroy = LShapClient.Instance.CheckExistMethod(rType, onDestroyName);
            CallMethod("Awake", gameObject);
        }
        else
        {
            DebugConsole.Log("未找到" + name + "相应的脚本类");
        }
    }

    protected virtual void Start()
    {
        if (isNeedStart) CallMethod("Start");
    }

    protected virtual void Update()
    {
        if (isNeedUpdate) CallMethod("Update");

    }

    protected virtual void LateUpdate()
    {
        if (isNeedLateUpdate) CallMethod("LateUpdate");
    }

    protected virtual void FixedUpdate()
    {
        if (isNeedFixedUpdate) CallMethod("FixedUpdate");
    }


    protected virtual void OnDestroy()
    {
        if (isNeedOnDestroy) CallMethod("OnDestroy");
        lShapObject = null;
        if(name.Contains("Panel")) Global.AssetLoadManager.UnloadUIPanel(name);
    }

    public object GetLShapObject()
    {
        if(lShapObject ==  null)
        {
            DebugConsole.LogError("Get LShapObject is null:" + name);
        }
        return lShapObject;
    }

    public void SetLShapObject(object lObject)
    {
        if (lObject == null)
        {
            DebugConsole.LogError("Set LShapObject is null:" + name);
        }
        lShapObject = lObject;
    }

    public void AddClick(GameObject rGo, Action<GameObject> func)
    {
        if (rGo == null) return;
        rGo.GetComponent<Button>().onClick.AddListener(
            delegate 
            {
                func(rGo);
            }
        );
    }

    public void AddToggleValueChange(GameObject rGo, Action<bool> func)
    {
        if (rGo == null) return;
        Toggle toggle = rGo.GetComponent<Toggle>();
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener((isOn) =>
        {
            func(isOn);
        });
    }

    protected object CallMethod(string rFuncName, params object[] args)
    {
        if (lShapObject == null)
            Debug.LogError("not have object invoke member function");

        return LShapUtil.CallScriptFunction(lShapObject, name, rFuncName, args);
    }
}
