using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour
{

    void Awake()
    {
        Launch();
    }

    void LaunchComplete()
    {
        DestroyObject(this);
        DestroyObject(gameObject);
    }

    void Launch()
    {
        LaunchConsloe();
        LaunchGUI();
        LaunchMainUpdate();
    }

    void LaunchConsloe()
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

    void LaunchGUI()
    {
        string rName = "GUIRoot";
        GameObject root = GameObject.Find(rName);

        if (root == null)
        {
            var temp = Resources.Load<GameObject>(rName);
            temp.name = rName;
            temp.tag = rName;

            root = GameObject.Instantiate(temp);
        }
        root.transform.Reset();
        DontDestroyOnLoad(root);
    }

    void LaunchMainUpdate()
    {
        string rName = "MainUpdate";
        GameObject main = GameObject.Find(rName);
        if (main == null)
        {
            main = new GameObject(rName);
            main.name = rName;
            main.tag = rName;
        }
        var game = main.GetComponentSafe<GameController>();
        game.Initialize(GameStart);
    }

    
    void GameStart()
    {
        //程序初始化完毕， 进入游戏
        Localization.Initialize(LocalizationType.zh_CN);
        var scriptMain = Util.CreateScriptObject("MainUpdate");
        gate.GameController.SetScriptMain(scriptMain);
        Util.CallScriptFunction(scriptMain, "MainUpdate", "Init");

        LaunchComplete();
    }

     /*
    void GameStart()
    {
        //程序初始化完毕， 进入游戏
        //gate.PanelManager.PushPanel("SampleLogic");
        
        LaunchComplete();
    }
      * */
}
