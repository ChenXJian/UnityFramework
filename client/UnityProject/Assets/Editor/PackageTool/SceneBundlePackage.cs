using UnityEngine;
using UnityEditor;
using JsonFx.Json;
using System.IO;

public class SceneBundlePackage
{
    public static void BuildAssetbundleWindows()
    {
        BuildSceneSpecify(PackagePlatform.PlatformType.Windows);
    }

    public static void BuildAssetbundleIOS()
    {
        BuildSceneSpecify(PackagePlatform.PlatformType.IOS);
    }

    public static void BuildAssetbundleAndroid()
    {
        BuildSceneSpecify(PackagePlatform.PlatformType.Android);
    }

    public static void BuildSceneSpecify(PackagePlatform.PlatformType rBuildPlatform)
    {
        if (!Directory.Exists(PackagePlatform.Instance.sceneDirectoryPath))
        {
            Debug.Log("没有需要打包的场景文件");
            return;
        }

        PackagePlatform.Instance.platformCurrent = rBuildPlatform;

        string rOutPath = PackagePlatform.Instance.GetSceneBundlePath();
        if (Directory.Exists(rOutPath) == false)
            Directory.CreateDirectory(rOutPath);
        //修改PackagePlatform.Instance.sceneDirectoryPath
        var guids = AssetDatabase.FindAssets("t:scene", new string[] { PackagePlatform.Instance.sceneDirectoryPath });
        var rInPaths = new string[guids.Length];
        var rCount = 0;

        foreach(var guid in guids)
        {
            var rPath = rInPaths[rCount] = AssetDatabase.GUIDToAssetPath(guid);

            var rName = rPath.Substring(rPath.LastIndexOf('/') + 1, rPath.LastIndexOf('.') - rPath.LastIndexOf('/') - 1);

            var rfullPath = rOutPath + "/" + rName.ToLower() + ".unity3d";

            BuildPipeline.BuildPlayer(new string[] { rPath }, rfullPath, PackagePlatform.Instance.GetBuildTarget(), BuildOptions.BuildAdditionalStreamedScenes);

            Debug.Log("build scene:>" + rPath);
            rCount++;
        }

    }
}
