using UnityEngine;
using UnityEditor;
using System.IO;


public class DefaultPackage : MonoBehaviour
{
    public static void BuildDefaultWindows()
    {
        BuildDefaultSpecify(PackagePlatform.PlatformType.Windows);
    }

    public static void BuildDefaultIOS()
    {
        BuildDefaultSpecify(PackagePlatform.PlatformType.IOS);
    }

    public static void BuildDefaultAndroid()
    {
        BuildDefaultSpecify(PackagePlatform.PlatformType.Android);
    }

    public static void BuildDefaultSpecify(PackagePlatform.PlatformType rBuildPlatform)
    {
        //打包DataTable
        if (Directory.Exists("Assets/RawResources/DataTable"))
        {
            Debug.Log("打包 DataTable");

            string outPath = PackagePlatform.GetPackagePath() + "DataTable";
            if (Directory.Exists(outPath))
            {
                Directory.Delete(outPath, true);
            }
            Directory.CreateDirectory(outPath);

            string inPath = "Assets/RawResources/DataTable";
            string[] files = Directory.GetFiles(inPath);

            float doneCount = 0f;
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                if (file.Contains(".meta")) continue;
                Debug.Log(file);

                string outfile = file.Replace("Assets/RawResources", PackagePlatform.GetPackagePath());

                if (File.Exists(outfile)) File.Delete(outfile);

                File.Copy(file, outfile, true);


                doneCount++;
                float p = (doneCount / (float)files.Length);
                EditorUtility.DisplayProgressBar("Package", "Package Default Resources", p);
            }
            EditorUtility.ClearProgressBar();
        }

        //打包Code
        if (Directory.Exists("Assets/RawResources/Code"))
        {
            Debug.Log("打包 Code");

            string outPath = PackagePlatform.GetPackagePath() + "Code";
            if (Directory.Exists(outPath))
            {
                Directory.Delete(outPath, true);
            }
            Directory.CreateDirectory(outPath);

            string inPath = "Assets/RawResources/Code";
            string[] files = Directory.GetFiles(inPath);

            float doneCount = 0f;
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                if (file.Contains(".meta")) continue;
                Debug.Log(file);

                string outfile = file.Replace("Assets/RawResources", PackagePlatform.GetPackagePath());

                if (File.Exists(outfile)) File.Delete(outfile);

                File.Copy(file, outfile, true);


                doneCount++;
                float p = (doneCount / (float)files.Length);
                EditorUtility.DisplayProgressBar("Package", "Package Default Resources", p);
            }
            EditorUtility.ClearProgressBar();
        }
    }
}
