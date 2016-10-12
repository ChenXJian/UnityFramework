using UnityEngine;
using UnityEditor;
using System.IO;

public class SceneBundlePackage
{
    public static void BuildScenebundleWindows()
    {
        BuildSceneSpecify(PackagePlatform.PlatformType.Windows);
    }

    public static void BuildScenebundleIOS()
    {
        BuildSceneSpecify(PackagePlatform.PlatformType.IOS);
    }

    public static void BuildScenebundleAndroid()
    {
        BuildSceneSpecify(PackagePlatform.PlatformType.Android);
    }

    public static void BuildSceneSpecify(PackagePlatform.PlatformType rBuildPlatform)
    {
        if (!Directory.Exists(PackagePlatform.sceneDirectoryPath))
        {
            Debug.Log("没有需要打包的场景文件");
            return;
        }

        PackagePlatform.platformCurrent = rBuildPlatform;

        string rOutPath = PackagePlatform.GetSceneBundlePath();
        if (Directory.Exists(rOutPath) == false)
            Directory.CreateDirectory(rOutPath);
        //修改PackagePlatform.Instance.sceneDirectoryPath
        var guids = AssetDatabase.FindAssets("t:scene", new string[] { PackagePlatform.sceneDirectoryPath });
        var rInPaths = new string[guids.Length];
        var rCount = 0;

        foreach(var guid in guids)
        {
            var rPath = rInPaths[rCount] = AssetDatabase.GUIDToAssetPath(guid);

            var rName = rPath.Substring(rPath.LastIndexOf('/') + 1, rPath.LastIndexOf('.') - rPath.LastIndexOf('/') - 1);

            var rfullPath = rOutPath + "/" + rName.ToLower() + "." + Global.BundleExtName;

            BuildPipeline.BuildPlayer(new string[] { rPath }, rfullPath, PackagePlatform.GetBuildTarget(), BuildOptions.BuildAdditionalStreamedScenes);

            Debug.Log("build scene:>" + rPath);
            rCount++;
        }

    }
}
