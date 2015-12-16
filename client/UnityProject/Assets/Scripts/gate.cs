using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class gate
{
    public static HttpRequestManager HttpRequestManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<HttpRequestManager>(ManagerName.HttpRequest);
        }
    }

    public static ModelManager ModelManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<ModelManager>(ManagerName.Model);
        }
    }

    public static ScriptManager ScriptManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<ScriptManager>(ManagerName.Script);
        }
    }

    public static TimerManager TimerManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<TimerManager>(ManagerName.Timer);
        }
    }

    public static MusicManager MusicManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<MusicManager>(ManagerName.Music);
        }
    }

    public static PanelManager PanelManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<PanelManager>(ManagerName.Panel);
        }
    }

    public static SocketClientManager SocketClientManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<SocketClientManager>(ManagerName.SocketClient);
        }
    }

    public static AssetLoadManager AssetLoadManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<AssetLoadManager>(ManagerName.Asset);
        }
    }

    public static CroutineManager CroutineManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<CroutineManager>(ManagerName.Croutine);
        }
    }

    public static SceneManager SceneManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<SceneManager>(ManagerName.Scene);
        }
    }

    public static GestureManager GestureManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<GestureManager>(ManagerName.Gesture);
        }
    }

    public static ResourcesUpdateManager ResourcesUpdateManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<ResourcesUpdateManager>(ManagerName.ResourcesUpdate);
        }
       
    }

    public static GameController GameController
    {
        get
        {
            return gate.MainUpdate.GetComponent<GameController>();
        }
    }

    public static GameObject GUIRoot
    {
        get
        {
            GameObject rGo = GameObject.FindWithTag("GUIRoot");

            if (rGo == null)
            {
                throw new NullReferenceException("Not find GUIRoot");
            }

            return rGo;
        }
    }

    public static Transform PanelWindow
    {
        get
        {
            GameObject rGo = GameObject.FindWithTag("PanelWindow");

            if (rGo == null)
            {
                throw new NullReferenceException("Not find GUIRoot");
            }

            return rGo.transform;
        }
    }

    public static Transform EffectCamera
    {
        get
        {
            GameObject rGo = GameObject.FindWithTag("EffectCamera");
            if (rGo == null)
            {
                throw new NullReferenceException("Not find EffectCamera");
            }

            return rGo.transform;
        }
    }

    public static Transform MessageCanvas
    {
        get
        {
            GameObject rGo = GameObject.FindWithTag("MessageCanvas");
            if (rGo == null)
            {
                throw new NullReferenceException("Not find MessageCanvas");
            }

            return rGo.transform;
        }
    }

    public static Transform PopupsWindow
    {
        get
        {
            GameObject rGo = GameObject.FindWithTag("PopupsWindow");
            if (rGo == null)
            {
                throw new NullReferenceException("Not find PopupsWindow");
            }

            return rGo.transform;
        }
    }

    public static Transform HttpRequestPool
    {
        get
        {
            var rName = "HttpRequestPool";
            GameObject rGo = GameObject.FindWithTag(rName);
            if (rGo == null)
            {
                rGo = new GameObject(rName);
                rGo.tag = rName;
            }
            return rGo.transform;
        }
    }

    public static Transform Templates
    {
        get
        {
            GameObject rGo = GameObject.FindWithTag("Templates");
            if (rGo == null)
            {
                throw new NullReferenceException("Not find Templates");
            }

            return rGo.transform;
        }
    }

    public static Transform MainUpdate
    {
        get
        {
            GameObject rGo = GameObject.FindWithTag("MainUpdate");
            if (rGo == null)
            {
                throw new NullReferenceException("Not find MainUpdate");
            }

            return rGo.transform;
        }
    }
}
