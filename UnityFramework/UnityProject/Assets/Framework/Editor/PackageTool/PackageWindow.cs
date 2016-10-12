using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;

public class PackageWindow : EditorWindow
{
    static Rect windowRect = new Rect(0, 0, 380, 360);

    string mainVersion = null;
    string minorVersion = null;
    string rawScenePath = null;
    string rawCodePath = null;
    string rawConfigPath = null;
    string rawDataTablePath = null;

    bool needPackageScene = false;

    TextureFormatWindows textureFormatWin = TextureFormatWindows.AutomaticCompressed;
    TextureFormatIOS textureFormatIOS = TextureFormatIOS.AutomaticCompressed;
    TextureFormatAndroid textureFormatAnd = TextureFormatAndroid.AutomaticCompressed;
    TextureFormatOSX textureFormatOSX = TextureFormatOSX.AutomaticCompressed;


    [MenuItem("Tools/Package", priority = 0)]
    static void StartupWindow()
    {
        PackageWindow window = (PackageWindow)EditorWindow.GetWindowWithRect(typeof(PackageWindow), windowRect, true, "Package");
        window.Show();
        window.LoadPackageConfig();
        
    }

    void OnGUI()
    {
        GUILayout.Space(10f);

        PackagePlatform.platformCurrent = (PackagePlatform.PlatformType)EditorGUIExtension.EnumToolbar(PackagePlatform.platformCurrent);

        OnOptionsView();

        OnPathSettingView();

        GUILayout.Space(20f);
        if (GUILayout.Button("Package", GUILayout.Width(370)))
        {
            EditorApplication.delayCall += Package;
            Close();
        }
        GUILayout.Space(10f);
    }

    void OnPathSettingView()
    {
        GUILayout.Space(10f);
        //场景打包路径
        if (needPackageScene)
        {
            var scenePath = EditorGUIExtension.FolderSelector("Scene Directory :", rawScenePath);
            if (string.IsNullOrEmpty(scenePath))
            {
                rawScenePath = "";
            }
            else
            {
                var index = scenePath.IndexOf("Assets");
                var length = scenePath.Length;
                rawScenePath = scenePath.Substring(index, length - index);
            }
        }

        GUILayout.Space(10f);

        //代码打包路径
        var codePath = EditorGUIExtension.FolderSelector("Code Directory :", rawCodePath);
        if (string.IsNullOrEmpty(codePath))
        {
            rawCodePath = "";
        }
        else
        {
            var index = codePath.IndexOf("Assets");
            var length = codePath.Length;
            rawCodePath = codePath.Substring(index, length - index);
        }

        GUILayout.Space(10f);

        //DataTable打包路径
        var tablePath = EditorGUIExtension.FolderSelector("DataTable Directory :", rawDataTablePath);
        if (string.IsNullOrEmpty(codePath))
        {
            rawDataTablePath = "";
        }
        else
        {
            var index = tablePath.IndexOf("Assets");
            var length = tablePath.Length;
            rawDataTablePath = tablePath.Substring(index, length - index);
        }

        GUILayout.Space(10f);
    }

    void OnOptionsView()
    {
        
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        needPackageScene = GUILayout.Toggle(needPackageScene, "With Package Scene");
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);

        //版本号
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Main Version");
        mainVersion = GUILayout.TextField(mainVersion, 5);
        GUILayout.Label("Minor Version");
        minorVersion = GUILayout.TextField(minorVersion, 5);
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);

        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        switch (PackagePlatform.platformCurrent)
        {
            case PackagePlatform.PlatformType.Windows:
                textureFormatWin = (TextureFormatWindows)EditorGUIExtension.EnumComboBox("TextureFormat: ", textureFormatWin);
                break;
            case PackagePlatform.PlatformType.IOS:
                textureFormatIOS = (TextureFormatIOS)EditorGUIExtension.EnumComboBox("TextureFormat: ", textureFormatIOS);
                break;
            case PackagePlatform.PlatformType.Android:
                textureFormatAnd = (TextureFormatAndroid)EditorGUIExtension.EnumComboBox("TextureFormat: ", textureFormatAnd);
                break;
            case PackagePlatform.PlatformType.OSX:
                textureFormatOSX = (TextureFormatOSX)EditorGUIExtension.EnumComboBox("TextureFormat: ", textureFormatOSX);
                break;
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
    }

    void Package()
    {

        var temp = EditorUserBuildSettings.activeBuildTarget;

        EditorUserBuildSettings.SwitchActiveBuildTarget(PackagePlatform.GetBuildTarget());

        //切换贴图格式
        SwitchTextureFormat();

        //打包其他文件
        DefaultPackage.BuildDefaultSpecify(PackagePlatform.platformCurrent);


        //打包Assetbundle
        AssetBundlePackage.BuildAssetbundlesSpecify(PackagePlatform.platformCurrent);

        //打包场景
        if (needPackageScene)
        {
            PackagePlatform.sceneDirectoryPath = rawScenePath;
            SceneBundlePackage.BuildSceneSpecify(PackagePlatform.platformCurrent);
        }

        //生成版本号
        var minor = int.Parse(minorVersion);
        minor++;
        minorVersion = minor.ToString();
        PackageUtil.GeneratorVersion(mainVersion, minorVersion);

        //生成文件表
        PackageUtil.GeneratorChecklist();

        EditorUserBuildSettings.SwitchActiveBuildTarget(temp);
        SavePackageConfig();
    }

    void SwitchTextureFormat()
    {
        switch (PackagePlatform.platformCurrent)
        {
            case PackagePlatform.PlatformType.Windows:
                TextureSetting.ChangeTextureFormatAll((TextureImporterFormat)textureFormatWin);
                break;
            case PackagePlatform.PlatformType.IOS:
                TextureSetting.ChangeTextureFormatAll((TextureImporterFormat)textureFormatIOS);
                break;
            case PackagePlatform.PlatformType.Android:
                TextureSetting.ChangeTextureFormatAll((TextureImporterFormat)textureFormatAnd);
                break;
            case PackagePlatform.PlatformType.OSX:
                TextureSetting.ChangeTextureFormatAll((TextureImporterFormat)textureFormatOSX);
                break;
        }
    }

    void SavePackageConfig()
    {
        string path = Application.dataPath + "/" + PackagePlatform.packageConfigPath;
        if (string.IsNullOrEmpty(path))
        {
            Debug.Log("打包配置文件未找到");
            return;
        }

        string str = File.ReadAllText(path);

        PackageConfig pc = JsonUtility.FromJson<PackageConfig>(str);
        pc.mainVersion = mainVersion;
        pc.minorVersion = minorVersion;
        pc.rawScenePath = rawScenePath;
        pc.rawCodePath = rawCodePath;
        pc.rawConfigPath = rawConfigPath;
        pc.rawDataTablePath = rawDataTablePath;

        str = JsonUtility.ToJson(pc, true);

        FileUtil.CoverFile(path, str);
    }

    void LoadPackageConfig()
    {
        string path = Application.dataPath + "/" + PackagePlatform.packageConfigPath;
        if (string.IsNullOrEmpty(path))
        {
            Debug.Log("打包配置文件未找到");
            return;
        }

        string str = File.ReadAllText(path);
        PackageConfig pc = JsonUtility.FromJson<PackageConfig>(str);

        mainVersion = pc.mainVersion;
        minorVersion = pc.minorVersion;
        rawScenePath = pc.rawScenePath;
        rawCodePath = pc.rawCodePath;
        rawConfigPath = pc.rawConfigPath;
        rawDataTablePath = pc.rawDataTablePath;
    }


    void OnInspectorUpdate()
    {
        Repaint();
    }

}