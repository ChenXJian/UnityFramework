using UnityEngine;
using System.Collections;
using UnityEditor;
using JsonFx.Json;
using System.IO;
using System.Collections.Generic;

public class PackageWindow : EditorWindow
{
    static Rect windowRect = new Rect(0, 0, 350, 300);

    string sceneDirectoryPath = null;

    bool needPackageScene = false;
    bool needGeneratorFileList = true;

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

        PackagePlatform.Instance.platformCurrent = (PackagePlatform.PlatformType)EditorGUIExtension.EnumToolbar(PackagePlatform.Instance.platformCurrent);

        OnOptionsView();

        OnPathSettingView();

        if (GUILayout.Button("Package", GUILayout.Width(100)))
        {
            EditorApplication.delayCall += Package;
            Close();
        }

        GUILayout.Space(10f);
    }

    void OnPathSettingView()
    {
        GUILayout.Space(10f);
        if (needPackageScene)
        {
            var tempStr = EditorGUIExtension.FolderSelector("Scene Directory :", sceneDirectoryPath);
            if (string.IsNullOrEmpty(tempStr))
            {
                sceneDirectoryPath = "";
            }
            else
            {
                var index = tempStr.IndexOf("Assets");
                var length = tempStr.Length;
                sceneDirectoryPath = tempStr.Substring(index, length - index);
            }
        }

        GUILayout.Space(10f);
    }

    void OnOptionsView()
    {
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        needPackageScene = GUILayout.Toggle(needPackageScene, "With Package Scene");
        needGeneratorFileList = GUILayout.Toggle(needGeneratorFileList, "Generator File List");
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);

        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        switch (PackagePlatform.Instance.platformCurrent)
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
        EditorUserBuildSettings.SwitchActiveBuildTarget(PackagePlatform.Instance.GetBuildTarget());

        SwitchTextureFormat();

        if (needPackageScene)
        {
            PackagePlatform.Instance.sceneDirectoryPath = sceneDirectoryPath;
            SceneBundlePackage.BuildSceneSpecify(PackagePlatform.Instance.platformCurrent);
        }

        AssetbundlePackage.BuildAssetbundlesSpecify(PackagePlatform.Instance.platformCurrent);
        
        if (needGeneratorFileList)
            GeneratorResourceTable.GeneratorResourceFileList();


        EditorUserBuildSettings.SwitchActiveBuildTarget(temp);
        SavePackageConfig();
       // Close();
    }

    void SwitchTextureFormat()
    {
        switch (PackagePlatform.Instance.platformCurrent)
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
        string path = Application.dataPath + "/" + PackagePlatform.Instance.assetbundleConfigPath;
        if (string.IsNullOrEmpty(path))
        {
            Debug.Log("打包配置文件未找到");
            return;
        }

        string str = File.ReadAllText(path);
        AssetPackageConfig apc = JsonReader.Deserialize<AssetPackageConfig>(str);

        apc.sceneDirectoryPath = sceneDirectoryPath;
        str = JsonWriter.Serialize(apc);

        FileAssist.CoverFile(path, str);
    }

    void LoadPackageConfig()
    {
        string path = Application.dataPath + "/" + PackagePlatform.Instance.assetbundleConfigPath;
        if (string.IsNullOrEmpty(path))
        {
            Debug.Log("打包配置文件未找到");
            return;
        }

        string str = File.ReadAllText(path);
        AssetPackageConfig apc = JsonReader.Deserialize<AssetPackageConfig>(str);

        sceneDirectoryPath = apc.sceneDirectoryPath;
    }


    void OnInspectorUpdate()
    {
        Repaint();
    }

}