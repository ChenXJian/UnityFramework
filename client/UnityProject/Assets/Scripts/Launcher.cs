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
        InitializeResolution();
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


    void InitializeResolution()
    {
#if UNITY_ANDROID
        if (scaleWidth == 0 && scaleHeight == 0)
        {
            //1080 1920
            //640  1136
            int width = Screen.currentResolution.width;
            DebugConsole.Log(width + "####");
            int height = Screen.currentResolution.height;
            DebugConsole.Log(height + "####");
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
    }
}
