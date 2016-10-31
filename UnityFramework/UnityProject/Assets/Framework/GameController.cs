using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using UnityEngine.Events;

public class GameController : MonoBehaviour 
{
    private GameController() { }
    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            if(null == _instance)
            {
                string rName = "MainTick";
                GameObject main = GameObject.Find(rName);
                if (main == null)
                {
                    main = new GameObject(rName);
                    main.name = rName;
                    main.tag = rName;
                }
                _instance = main.GetComponentSafe<GameController>();
            }

            return _instance;
        }
    }

    object scriptGameInstance = null;
    Action onStartupFunc;

    public void Initialize(Action onComplete)
    {
        if (onComplete != null)
        {
            onStartupFunc = onComplete;
        }

        //取消 Destroy 对象 
        DontDestroyOnLoad(gameObject);

        InitConsole();
        InitUIRoot();
        InitResolution();

        //平台初始化
        AppPlatform.Initialize();

        //基本设置
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = Global.FrameRate;
        UnityEngine.QualitySettings.vSyncCount = Global.VSyncCount;

        //挂载管理器并初始化
        ManagerCollect.Instance.AddManager<TaskManager>(ManagerName.Task);
        ManagerCollect.Instance.AddManager<AssetLoadManager>(ManagerName.AssetLoad);
        ManagerCollect.Instance.AddManager<SoundManager>(ManagerName.Sound);
        ManagerCollect.Instance.AddManager<GestureManager>(ManagerName.Gesture);

        //创建运行时资源目录
        FileUtil.CreateFolder(AppPlatform.RuntimeAssetsPath);

        AssetsUpdater.Run(() =>
        {
            LoadAssetbundleManifest();
        });
    }


    void InitConsole()
    {
        string rName = "DebugConsloe";
        GameObject consloe = GameObject.Find(rName);
        if (consloe == null)
        {
            consloe = new GameObject(rName);
            consloe.name = rName;
        }
        var com = consloe.GetComponentSafe<DebugConsole>();
        com.IsDebugMode = true;
        com.isOpenBriefView = true;
    }

    void InitUIRoot()
    {
        string rName = "UIRoot";
        GameObject root = GameObject.Find(rName);

        if (root == null)
        {
            var temp = Resources.Load<GameObject>(rName);
            temp.name = rName;
            root = GameObject.Instantiate(temp);
        }
        root.transform.Reset();
        DontDestroyOnLoad(root);
    }

    void InitResolution()
    {
        /*
#if UNITY_ANDROID
        if (scaleWidth == 0 && scaleHeight == 0)
        {
            //1080 1920
            //640  1136
            int width = Screen.currentResolution.width;
            int height = Screen.currentResolution.height;
            int designWidth = (int)AppConst.ReferenceResolution.x;
            int designHeight = (int)AppConst.ReferenceResolution.y;
            float s1 = (float)designWidth / (float)designHeight;
            float s2 = (float)width / (float)height;
            if (s1 < s2)
            {
                designWidth = (int)Mathf.FloorToInt(designHeight * s2);
            }
            else if (s1 > s2)
            {
                designHeight = (int)Mathf.FloorToInt(designWidth / s2);
            }
            float contentScale = (float)designWidth / (float)width;
            if (contentScale < 1.0f)
            {
                scaleWidth = designWidth;
                scaleHeight = designHeight;
            }
        }

        if (scaleWidth > 0 && scaleHeight > 0)
        {
            if (scaleWidth % 2 == 0)
            {
                scaleWidth += 1;
            }
            else
            {
                scaleWidth -= 1;
            }
            Screen.SetResolution(scaleWidth, scaleHeight, false);
        }
#endif
        */
    }

    public void SetScriptGameInstance(object rObj)
    {
        if ( rObj != null)
        {
            scriptGameInstance = rObj;
        }
    }

    void LoadAssetbundleManifest()
    {
        LoadingLayer.SetProgressbarTips("游戏初始化中");
        LoadingLayer.SetProgressbarValue(5);
        string bundlName = AppPlatform.GetAssetBundleDirName();
        Global.AssetLoadManager.DownloadingURL = AppPlatform.GetRuntimeAssetBundleUrl();
        Global.AssetLoadManager.LoadManifest(bundlName, () =>
        {
            onStartupFunc();
        });
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    void GameEnd()
    {
        LShapUtil.CallScriptFunction(scriptGameInstance, "GameInstance", "End");
        scriptGameInstance = null;
        Global.AssetLoadManager.UnloadAssetBundles();
        UIUtil.ClearUICache();
        Util.ClearMemory();
        //Caching.CleanCache();
        
    }

    void OnApplicationQuit()
    {
        GameEnd();
    }

    void Update()
    {
        if (scriptGameInstance != null)
            LShapUtil.CallScriptFunction(scriptGameInstance, "GameInstance", "Update");
    }

    void FixedUpdate()
    {
        if (scriptGameInstance != null)
            LShapUtil.CallScriptFunction(scriptGameInstance, "GameInstance", "FixedUpdate");
    }

    void LateUpdate()
    {
        if (scriptGameInstance != null)
            LShapUtil.CallScriptFunction(scriptGameInstance, "GameInstance", "LateUpdate");
    }

}
