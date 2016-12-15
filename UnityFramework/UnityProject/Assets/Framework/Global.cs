using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public static class Global
{
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
    /// 资源更新服务器URL
    /// </summary>
    public static string PackageUpdateURL = "http://000.00.00.000:0000/down/";

    /// <summary>
    /// Package版本文件名
    /// </summary>
    public static string PackageVersionFileName = "version.txt";

    /// <summary>
    /// Package清单文件名
    /// </summary>
    public static string PackageManifestFileName = "checklist.txt";

    /// <summary>
    /// 服务器Package版本文件URL
    /// </summary>
    public static string ServerPackageVersionURL = PackageUpdateURL + "LatestVer.txt";

    public static string BundleExtName = "bundle";

    public static string DataDirName = "UnityFramework";

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

    public static SoundManager SoundManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<SoundManager>(ManagerName.Sound);
        }
    }

    public static AssetLoadManager AssetLoadManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<AssetLoadManager>(ManagerName.AssetLoad);
        }
    }

    public static TaskManager TaskManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<TaskManager>(ManagerName.Task);
        }
    }

    public static GestureManager GestureManager
    {
        get
        {
            return ManagerCollect.Instance.GetManager<GestureManager>(ManagerName.Gesture);
        }
    }

    public static GameController GameController
    {
        get
        {
            GameObject rGo = GameObject.FindWithTag("GameController");
            if (rGo != null)
            {
                return rGo.GetComponent<GameController>();
            }
            else
            {
                throw new NullReferenceException("Not find GameController");
            }
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
}
