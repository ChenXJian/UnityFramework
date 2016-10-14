using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour
{

    void Awake()
    {
        GameController.Instance.Initialize(Startup);

    }

    void Startup()
    {

        LShapClient.Instance.Initialize();

        DebugConsole.Log("[Framework initialize complete]");
        DebugConsole.Log("[Assetbundle prepare complete]");
        DebugConsole.Log("[LShape initialize complete]");
        DebugConsole.Log("[StreamingAssetsPath]：" + Application.streamingAssetsPath);
        DebugConsole.Log("[RuntimeAssetsPath] :" + AppPlatform.RuntimeAssetsPath);
        DebugConsole.Log("[AssetBundleDictionaryUrl]:" + Global.AssetLoadManager.DownloadingURL);

        //程序初始化完毕， 进入游戏
        var scriptGI = LShapUtil.CallScriptFunctionStatic("main", "GetGameInstance");
        Global.GameController.SetScriptGameInstance(scriptGI);
        LShapUtil.CallScriptFunction(scriptGI, "GameInstance", "Init");
        Destroy(gameObject);
    }
}
