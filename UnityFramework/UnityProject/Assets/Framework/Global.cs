using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public static class Global
{

    public static string BundleExtName = "bundle";

    public static string DataDirName = "UnityFramework";

    /// <summary>
    /// 限帧（-1： 不限制）
    /// </summary>
    public static int FrameRate = -1;

    /// <summary>
    /// 垂直同步
    /// </summary>
    public static int VSyncCount = 0;

    /// <summary>
    /// 是否走沙盒资源路径
    /// </summary>
    public static bool IsSandboxMode = false;

    /// <summary>
    /// 是否开启资源更新    
    /// </summary>
    public static bool IsUpdateMode = false;

    /// <summary>
    /// 约定分辨率
    /// </summary>
    public static Vector2 ReferenceResolution = new Vector2(720, 1280);




    /// <summary>
    /// 服务器配置表
    /// </summary>
    static ConfigFile _ServerConfig;
    public static ConfigFile ServerConfig
    {
        get
        {
            if (_ServerConfig == null)
            {
                _ServerConfig = new ConfigFile();
                _ServerConfig.Load(AppPlatform.RuntimeAssetsPath + "server.ini");
            }

            return _ServerConfig;
        }
    }

    /// <summary>
    /// 用户配置表
    /// </summary>
    static ConfigFile _UserConfig;
    public static ConfigFile UserConfig
    {
        get
        {
            if (_UserConfig == null)
            {
                _UserConfig = new ConfigFile();
                _UserConfig.Load(AppPlatform.RuntimeAssetsPath + "user.ini");
            }

            return _UserConfig;
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

    public static PopupsManager PopupsManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<PopupsManager>(ManagerName.Popups);
        }
    }

    public static AssetLoadManager AssetLoadManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<AssetLoadManager>(ManagerName.Asset);
        }
    }

    public static CoroutineManager CoroutineManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<CoroutineManager>(ManagerName.Coroutine);
        }
    }

    public static SceneLoadManager SceneLoadManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<SceneLoadManager>(ManagerName.Scene);
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
            return Global.MainUpdate.GetComponent<GameController>();
        }
    }

    public static GameObject UIRoot
    {
        get
        {
            GameObject rGo = GameObject.Find("UIRoot");

            if (rGo == null)
            {
                throw new NullReferenceException("Not find UIRoot");
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
                throw new NullReferenceException("Not find PanelWindow");
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
