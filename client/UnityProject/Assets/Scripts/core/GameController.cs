using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using DG.Tweening;
using UnityEngine.Events;

public class GameController : MonoBehaviour 
{
    object _scriptMainUpdate = null;
    bool inited = false;
    Action onGameStart;

    void Awake()
    {
        DebugConsole.Log("[APP Startup]");
    }

    public void Initialize(Action onComplete)
    {
        if (onComplete != null)
        {
            onGameStart = onComplete;
        }
        else
        {
            DebugConsole.Log("未设置游戏启动函数");
        }

        //取消 Destroy 对象 
        DontDestroyOnLoad(gameObject);

        //平台初始化
        AppPlatform.Initialize();

        //基本设置
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = AppConst.FrameRate;
        UnityEngine.QualitySettings.vSyncCount = AppConst.vSyncCount;
        DOTween.Init().SetCapacity(500, 100);
        DOTween.defaultAutoKill = true;

        //挂载管理器并初始化
        ManagerCollect.Instance.AddManager(ManagerName.Script, ScriptManager.Instance);
        ManagerCollect.Instance.AddManager(ManagerName.Panel, PanelManager.Instance);
        ManagerCollect.Instance.AddManager(ManagerName.Model, ModelManager.Instance);

        ManagerCollect.Instance.AddManager<HttpRequestManager>(ManagerName.HttpRequest);
        ManagerCollect.Instance.AddManager<SocketClientManager>(ManagerName.SocketClient);
        ManagerCollect.Instance.AddManager<ResourcesUpdateManager>(ManagerName.ResourcesUpdate);
        ManagerCollect.Instance.AddManager<CroutineManager>(ManagerName.Croutine);
        ManagerCollect.Instance.AddManager<TimerManager>(ManagerName.Timer);
        ManagerCollect.Instance.AddManager<AssetLoadManager>(ManagerName.Asset);
        ManagerCollect.Instance.AddManager<SceneLoadManager>(ManagerName.Scene);
        ManagerCollect.Instance.AddManager<MusicManager>(ManagerName.Music);
        ManagerCollect.Instance.AddManager<GestureManager>(ManagerName.Gesture);


        gate.TimerManager.Initialize();
        gate.MusicManager.Initialize();
        gate.ScriptManager.Initialize();

        DebugConsole.Log("[APP Initialize complete]");

        gate.ResourcesUpdateManager.ResourceUpdateStart(() =>
        {
            LoadAssetbundleManifest();
        });
    }

    public void SetScriptMain(object rObj)
    {
        if ( rObj != null)
        {
            _scriptMainUpdate = rObj;
        }
    }

    void LoadAssetbundleManifest()
    {
        var tempManager = gate.AssetLoadManager;
        string bundlName = AppPlatform.GetAssetBundleDirName();

        tempManager.DownloadingURL = AppPlatform.GetAssetBundleDirUrl();
        Debug.Log("[AssetBundleDictionaryUrl]:" + tempManager.DownloadingURL);
        tempManager.LoadManifest(bundlName, () =>
        {
            DebugConsole.Log("[APP LoadAssetbundleManifest complete]");
            onGameStart();
        });
    }


    /// <summary>
    /// 退出游戏
    /// </summary>
    void GameEnd()
    {
        Util.CallScriptFunction(_scriptMainUpdate, "MainUpdate", "End");
        _scriptMainUpdate = null;


        gate.AssetLoadManager.UnloadAssetBundles();
        Util.ClearUICache();
        Util.ClearMemory();
        //Caching.CleanCache();
        DebugConsole.Log("[APP UnloadAssetBundles complete]");
    }



    void OnApplicationQuit()
    {
        GameEnd();
        DebugConsole.Log("[APP End]");
    }

    void Update()
    {
        if (_scriptMainUpdate != null && inited)
            Util.CallScriptFunction(_scriptMainUpdate, "MainUpdate", "Update");

    }

    void FixedUpdate()
    {
        if (_scriptMainUpdate != null && inited)
            Util.CallScriptFunction(_scriptMainUpdate, "MainUpdate", "FixedUpdate");
    }

    void LateUpdate()
    {
        if (_scriptMainUpdate != null && inited)
            Util.CallScriptFunction(_scriptMainUpdate, "MainUpdate", "LateUpdate");
    }

}
