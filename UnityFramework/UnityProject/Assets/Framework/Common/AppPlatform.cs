using UnityEngine;
using System.Collections;

public enum Platform
{
    OSXEditor = 0,
    OSXPlayer,
    WindowsPlayer,
    WindowsEditor,
    IPhonePlayer,
    Android,
    Unkown,
}

public static class AppPlatform 
{
    public static string[] PlatformPathPrefixs = 
    {
        "file:///",         //OSXEditor
        "file:///",         //OSXPlayer
        "file:///",         //WindowsPlayer
        "file:///",         //WindowsEditor
        "file://",          //IphonePlayer
        "file:///",         //Android
    };

    public static string[] PlatformNames = 
    {
        "OSX",
        "OSX",
        "Windows",
        "Windows",
        "IOS",
        "Android"
    };

    public static bool[] PlatformIsEditor = 
    {
        true,
        false,
        false,
        true,
        false,
        false
    };

    public static Platform PlatformCurrent { set; get; }

    //运行时资源路径，解包后
    public static string RuntimeAssetsPath
    {
        get
        {
            //移动平台走沙盒
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/" + Global.DataDirName + "/";
            }

            if (Global.IsSandboxMode)
            {
                return "c:/" + Global.DataDirName + "/";

            }
            return Application.streamingAssetsPath + "/";
        }
    }

    public static string GetRawResourcesPath()
    {
        return Application.dataPath + "/RawResources/";
    }

    public static string GetRuntimePackagePath()
    {
        return RuntimeAssetsPath + GetPackageName() + "/";
    }

    public static string GetPackagePath()
    {
        return Application.streamingAssetsPath + "/" + GetPackageName() + "/";
    }

    public static string GetPackageName()
    {
        int index = (int)AppPlatform.PlatformCurrent;
        return "Package_" + AppPlatform.PlatformNames[index];
    }


    public static string GetAssetBundleDirName()
    {
        return "AssetBundles";
    }

    public static string GetRuntimeAssetBundleUrl()
    {
        int index = (int)AppPlatform.PlatformCurrent;
        return AppPlatform.PlatformPathPrefixs[index] + RuntimeAssetsPath + GetPackageName() + "/" + GetAssetBundleDirName() + "/";
    }

    //弃用
    public static string GetSceneBundleDirName()
    {
        return "SceneBundles";
    }

    //弃用
    public static string GetRuntimeSceneBundleUrl()
    {
        int index = (int)AppPlatform.PlatformCurrent;
        return AppPlatform.PlatformPathPrefixs[index] + RuntimeAssetsPath + GetPackageName() + "/" + GetSceneBundleDirName() + "/";
    }

    public static void Initialize()
    {
        AppPlatform.PlatformCurrent = RuntimePlatform_To_AppPlaform(Application.platform);
    }

    private static Platform RuntimePlatform_To_AppPlaform(RuntimePlatform runtimePlatform)
    {
        switch (runtimePlatform)
        {
            case UnityEngine.RuntimePlatform.Android: return Platform.Android;
            case UnityEngine.RuntimePlatform.IPhonePlayer: return Platform.IPhonePlayer;
            case UnityEngine.RuntimePlatform.OSXEditor: return Platform.OSXEditor;
            case UnityEngine.RuntimePlatform.OSXPlayer: return Platform.OSXPlayer;
            case UnityEngine.RuntimePlatform.WindowsEditor: return Platform.WindowsEditor;
            case UnityEngine.RuntimePlatform.WindowsPlayer: return Platform.WindowsPlayer;
            default: return Platform.Unkown;
        }
    }

}