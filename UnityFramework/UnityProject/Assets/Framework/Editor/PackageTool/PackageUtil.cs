using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class PackageUtil
{
    public static void GeneratorChecklist()
    {
        var rAssetsPath = PackagePlatform.GetPackagePath();
        string newFilePath = rAssetsPath + "/checklist.txt";
        if (File.Exists(newFilePath)) File.Delete(newFilePath);
        
        List<string> files = new List<string>();
        Util.RecursiveDir(rAssetsPath, ref files);

        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        for (int i = 0; i < files.Count; i++)
        {
            string file = files[i];
            //string ext = Path.GetExtension(file);
            if (file.EndsWith(".meta") || file.Contains(".DS_Store")) continue;

            string md5 = Util.MD5File(file);
            string value = file.Replace(rAssetsPath, string.Empty);
            sw.WriteLine(value + "|" + md5);
        }
        sw.Close(); fs.Close();
        AssetDatabase.Refresh();
    }

    public static void GeneratorVersion(string mainVersion, string minorVersion)
    {
        var rAssetsPath = PackagePlatform.GetPackagePath();
        string newFilePath = rAssetsPath + "/version.txt";
        if (File.Exists(newFilePath)) File.Delete(newFilePath);
        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);

        string version = mainVersion + '.' + minorVersion;
        sw.WriteLine(version);
        sw.Close(); fs.Close();
        AssetDatabase.Refresh();
    }

}
