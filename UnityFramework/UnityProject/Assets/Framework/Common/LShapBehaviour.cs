using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class LShapBehaviour : MonoBehaviour
{
    object lShapObject;

    bool isNeedAwake = false;
    bool isNeedStart = false;
    bool isNeedOnDestroy = false;



    const string awakeName = "Awake";
    const string startName = "Start";
    const string updateName = "Update";
    const string lateUpdateName = "LateUpdate";
    const string fixedUpdateName = "FixedUpdate";
    const string onDestroyName = "OnDestroy";

    //3D碰撞
    const string onTriggerEnterName = "OnTriggerEnter";
    const string onTriggerExitName = "OnTriggerExit";
    const string onTriggerStayName = "OnTriggerStay";

    const string onCollisionEnterName = "OnCollisionEnter";
    const string onCollisionExitName = "OnCollisionExit";
    const string onCollisionStayName = "OnCollisionStay";


    CLRSharp.IMethod onTriggerEnter = null;
    CLRSharp.IMethod onTriggerExit = null;
    CLRSharp.IMethod onTriggerStay = null;

    CLRSharp.IMethod onCollisionEnter = null;
    CLRSharp.IMethod onCollisionExit = null;
    CLRSharp.IMethod onCollisionStay = null;

    //2D碰撞
    const string onTriggerEnter2DName = "OnTriggerEnter2D";
    const string onTriggerExit2DName = "OnTriggerExit2D";
    const string onTriggerStay2DName = "OnTriggerStay2D";

    const string onCollisionEnter2DName = "OnCollisionEnter2D";
    const string onCollisionExit2DName = "OnCollisionExit2D";
    const string onCollisionStay2DName = "OnCollisionStay2D";


    CLRSharp.IMethod onTriggerEnter2D = null;
    CLRSharp.IMethod onTriggerExit2D = null;
    CLRSharp.IMethod onTriggerStay2D = null;

    CLRSharp.IMethod onCollisionEnter2D = null;
    CLRSharp.IMethod onCollisionExit2D = null;
    CLRSharp.IMethod onCollisionStay2D = null;

    CLRSharp.IMethod update = null;
    CLRSharp.IMethod lateUpdate = null;
    CLRSharp.IMethod fixedUpdate = null;


    protected virtual void Awake()
    {
        if (!LShapClient.Instance.IsScriptInited)
        {
            LShapClient.Instance.Initialize();
        }

        CLRSharp.ICLRType rType = null;

        string typeName = name;
        var i = name.IndexOf(" ", 0);
        if (i >= 0) typeName = name.Remove(i);

        i = typeName.IndexOf("(", 0);
        if (i >= 0) typeName = name.Remove(i);

        bool rGot = LShapClient.Instance.TryGetType(typeName, out rType);

        if(rGot)
        {
            if (lShapObject == null)
            {
                SetLShapObject(LShapClient.Instance.CreateScriptObject(rType));
            }

            isNeedAwake = LShapClient.Instance.CheckExistMethod(rType, awakeName, gameObject);
            isNeedStart = LShapClient.Instance.CheckExistMethod(rType, startName);
            isNeedOnDestroy = LShapClient.Instance.CheckExistMethod(rType, onDestroyName);


            LShapClient.Instance.TryGetMethod(rType, updateName, out update);
            LShapClient.Instance.TryGetMethod(rType, lateUpdateName, out lateUpdate);
            LShapClient.Instance.TryGetMethod(rType, fixedUpdateName, out fixedUpdate);

            LShapClient.Instance.TryGetMethod(rType, onTriggerEnterName, out onTriggerEnter, new Collider());
            LShapClient.Instance.TryGetMethod(rType, onTriggerExitName, out onTriggerExit, new Collider());
            LShapClient.Instance.TryGetMethod(rType, onTriggerStayName, out onTriggerStay, new Collider());
            LShapClient.Instance.TryGetMethod(rType, onCollisionEnterName, out onCollisionEnter, new Collision());
            LShapClient.Instance.TryGetMethod(rType, onCollisionExitName, out onCollisionExit, new Collision());
            LShapClient.Instance.TryGetMethod(rType, onCollisionStayName, out onCollisionStay, new Collision());


            LShapClient.Instance.TryGetMethod(rType, onTriggerEnter2DName, out onTriggerEnter2D, new Collider2D());
            LShapClient.Instance.TryGetMethod(rType, onTriggerExit2DName, out onTriggerExit2D, new Collider2D());
            LShapClient.Instance.TryGetMethod(rType, onTriggerStay2DName, out onTriggerStay2D, new Collider2D());
            LShapClient.Instance.TryGetMethod(rType, onCollisionEnter2DName, out onCollisionEnter2D, new Collision2D());
            LShapClient.Instance.TryGetMethod(rType, onCollisionExit2DName, out onCollisionExit2D, new Collision2D());
            LShapClient.Instance.TryGetMethod(rType, onCollisionStay2DName, out onCollisionStay2D, new Collision2D());


            if (isNeedAwake)
            {
                CallMethod("Awake", gameObject);
            }
        }
        else
        {
            DebugConsole.Log("未找到" + name + "相应的脚本类");
        }
    }

    protected virtual void Start()
    {
        if (isNeedStart) CallMethod(startName);
    }

    protected virtual void OnDestroy()
    {
        if (isNeedOnDestroy) CallMethod("OnDestroy");
        lShapObject = null;
        if(name.Contains("Panel")) Global.AssetLoadManager.UnloadUIPanel(name);
    }

    protected virtual void Update()
    {
        if (update != null) update.Invoke(LShapClient.context, lShapObject, null);
    }

    protected virtual void LateUpdate()
    {
        if (lateUpdate != null) lateUpdate.Invoke(LShapClient.context, lShapObject, null);
    }

    protected virtual void FixedUpdate()
    {
        if (fixedUpdate != null) fixedUpdate.Invoke(LShapClient.context, lShapObject, null);
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if (onTriggerEnter!= null) onTriggerEnter.Invoke(LShapClient.context, lShapObject, new object[] { collider });
    }

    protected virtual void OnTriggerExit(Collider collider)
    {
        if (onTriggerExit != null) onTriggerExit.Invoke(LShapClient.context, lShapObject, new object[] { collider });
    }

    protected virtual void OnTriggerStay(Collider collider)
    {
        if (onTriggerStay != null) onTriggerStay.Invoke(LShapClient.context, lShapObject, new object[] { collider });
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (onCollisionEnter != null) onCollisionEnter.Invoke(LShapClient.context, lShapObject, new object[] { collision });
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if (onCollisionExit != null) onCollisionExit.Invoke(LShapClient.context, lShapObject, new object[] { collision });
    }

    protected virtual void OnCollisionStay(Collision collision)
    {
        if (onCollisionStay != null) onCollisionStay.Invoke(LShapClient.context, lShapObject, new object[] { collision });
    }


    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (onTriggerEnter2D != null) onTriggerEnter2D.Invoke(LShapClient.context, lShapObject, new object[] { collider });
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        if (onTriggerExit2D != null) onTriggerExit2D.Invoke(LShapClient.context, lShapObject, new object[] { collider });
    }

    protected virtual void OnTriggerStay2D(Collider2D collider)
    {
        if (onTriggerStay2D != null) onTriggerStay2D.Invoke(LShapClient.context, lShapObject, new object[] { collider });
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (onCollisionEnter2D != null) onCollisionEnter2D.Invoke(LShapClient.context, lShapObject, new object[] { collision });
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (onCollisionExit2D != null) onCollisionExit2D.Invoke(LShapClient.context, lShapObject, new object[] { collision });
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (onCollisionStay2D != null) onCollisionStay2D.Invoke(LShapClient.context, lShapObject, new object[] { collision });
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
