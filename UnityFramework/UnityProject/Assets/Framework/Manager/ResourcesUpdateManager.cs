using UnityEngine;
using System.Collections;
using System.IO;

public class ResourcesUpdateManager : MonoBehaviour
{
    System.Action OnResourceUpdateComplete;

    public void ResourceUpdateStart(System.Action func)
    {
        LoadingLayer.Show();
        if (func != null)
        {
            OnResourceUpdateComplete = func;
        }
        CheckExtractResource();
    }

    void ResourceUpdateEnd()
    {
        if (OnResourceUpdateComplete != null)
        {
            OnResourceUpdateComplete.Invoke();
            OnResourceUpdateComplete = null;
        }
        LoadingLayer.SetProgressbarTips("资源解包完成，准备初始化");
    }

    void CheckExtractResource()
    {
        //首次运行时解包资源
        bool needExtracted = true;
        if (File.Exists(AppPlatform.RuntimeAssetsPath + "server.ini") &&
            File.Exists(AppPlatform.RuntimeAssetsPath + "user.ini") &&
            Directory.Exists(AppPlatform.GetRuntimePackagePath()))//解压过了
        {
            needExtracted = false;
        }

        if (needExtracted && Global.IsSandboxMode) // 不为沙盒模式，则不需要解包，直接取Streaming
        {
            //需要解包，那么先解包，再更新
            StartCoroutine(OnExtractResource());
        }
        else
        {
            //不需要解包，直接更新
            StartCoroutine(OnUpdatePackage());
        }
    }

    IEnumerator OnExtractResource()
    {
        LoadingLayer.SetProgressbarTips("开始解包资源");

        //解包config server
        string infile = Application.streamingAssetsPath + "/" + "server.ini";
        string outfile = AppPlatform.RuntimeAssetsPath + "server.ini";
        if (infile != outfile)
        {
            if (File.Exists(outfile)) File.Delete(outfile);
            if (AppPlatform.PlatformCurrent == Platform.Android)
            {
                WWW www = new WWW(infile);
                yield return www;

                if (www.isDone)
                {
                    File.WriteAllBytes(outfile, www.bytes);
                }
                yield return null;
            }
            else
            {
                File.Copy(infile, outfile, true);
            }
            DebugConsole.Log("[extracting config]:>" + infile + "[TO]" + outfile);
            yield return new WaitForEndOfFrame();
        }


        //解包config user
        if (infile != outfile)
        {
            infile = Application.streamingAssetsPath + "/" + "user.ini";
            outfile = AppPlatform.RuntimeAssetsPath + "user.ini";
            if (File.Exists(outfile)) File.Delete(outfile);
            if (AppPlatform.PlatformCurrent == Platform.Android)
            {
                WWW www = new WWW(infile);
                yield return www;

                if (www.isDone)
                {
                    File.WriteAllBytes(outfile, www.bytes);
                }
                yield return null;
            }
            else
            {
                File.Copy(infile, outfile, true);
            }
            DebugConsole.Log("[extracting config]:>" + infile + "[TO]" + outfile);
            yield return new WaitForEndOfFrame();
        }


        //检查是否有预装package需要解包
        bool needExtractPackage = true;

        if (AppPlatform.PlatformCurrent == Platform.Android)
        {
            var ver = AppPlatform.GetPackagePath() + Global.PackageVersionFileName;
            WWW www = new WWW(ver);
            yield return www;

            if (www.error != null)
            {
                DebugConsole.Log("没有预装的Package");
                www.Dispose();
                needExtractPackage = false;
            }

        }
        else
        {
            var ver = AppPlatform.GetPackagePath() + Global.PackageVersionFileName;
            if (!File.Exists(ver))
            {
                DebugConsole.Log("没有预装的Package");
                needExtractPackage = false;
            }
        }

        if (needExtractPackage)
        {
            //清理package文件夹
            if (Directory.Exists(AppPlatform.GetRuntimePackagePath())) Directory.Delete(AppPlatform.GetRuntimePackagePath(), true);
            Directory.CreateDirectory(AppPlatform.GetRuntimePackagePath());

            //解包package的checklist
            infile = AppPlatform.GetPackagePath() + Global.PackageManifestFileName;
            outfile = AppPlatform.GetRuntimePackagePath() + Global.PackageManifestFileName;
            if (File.Exists(outfile)) File.Delete(outfile);
            if (AppPlatform.PlatformCurrent == Platform.Android)
            {
                WWW www = new WWW(infile);
                yield return www;

                if (www.isDone)
                {
                    File.WriteAllBytes(outfile, www.bytes);
                }
                yield return null;
            }
            else
            {
                File.Copy(infile, outfile, true);
            }
            DebugConsole.Log("[extracting checklist]:>" + infile + "[TO]" + outfile);
            yield return new WaitForEndOfFrame();

            //解包package
            string[] files = File.ReadAllLines(outfile);
            float downloadedCount = 0f;
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                string[] rKeyValue = file.Split('|');
                infile = AppPlatform.GetPackagePath() + rKeyValue[0];
                outfile = AppPlatform.GetRuntimePackagePath() + rKeyValue[0];

                DebugConsole.Log("[extracting package]:>" + infile + "[TO]" + outfile);

                string rDirName = Path.GetDirectoryName(outfile);
                if (!Directory.Exists(rDirName)) Directory.CreateDirectory(rDirName);

                if (AppPlatform.PlatformCurrent == Platform.Android)
                {
                    WWW www = new WWW(infile);
                    yield return www;

                    if (www.isDone)
                    {
                        File.WriteAllBytes(outfile, www.bytes);
                    }
                    yield return 0;
                }
                else
                {
                    if (File.Exists(outfile))
                    {
                        File.Delete(outfile);
                    }
                    File.Copy(infile, outfile, true);
                }
                yield return new WaitForEndOfFrame();

                downloadedCount++;
                float p = (downloadedCount / (float)files.Length) * 100f;
                p = Mathf.Clamp(p, 0, 100);
                LoadingLayer.SetProgressbarValue((int)p);
            }
        }

        LoadingLayer.SetProgressbarTips("解包资源文件完成");
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(OnUpdatePackage());
    }


    IEnumerator OnUpdatePackage()
    {
        if (!Global.IsUpdateMode)
        {
            ResourceUpdateEnd();
            yield break;
        }

        LoadingLayer.SetProgressbarTips("开始更新资源");

        //比对服务器版本
        WWW www = new WWW(Global.ServerPackageVersionURL);
        yield return www;

        if (www.error != null)
        {
            DebugConsole.Log("未在服务器上找到最新版本文件");
            www.Dispose();
            yield break;
        }
        var lastestVer = www.text;
        var curVer = "";
        if (File.Exists(AppPlatform.GetRuntimePackagePath() + Global.PackageVersionFileName))
        {
            curVer = FileUtil.ReadFile(AppPlatform.GetRuntimePackagePath() + Global.PackageVersionFileName);
        }

        if (curVer == lastestVer)
        {
            DebugConsole.Log("当前package已是最新版本，无需更新");
            www.Dispose();
            yield break;
        }


        // 获取更新文件列表
        string listUrl = Global.PackageUpdateURL + lastestVer + "/" + AppPlatform.GetPackageName() + "/" + Global.PackageManifestFileName;

        www = new WWW(listUrl);
        yield return www;

        if (www.error != null)
        {
            DebugConsole.Log("未在服务器上找到checklist");
            www.Dispose();
            yield break;
        }

        if (!Directory.Exists(AppPlatform.GetRuntimePackagePath()))
        {
            Directory.CreateDirectory(AppPlatform.GetRuntimePackagePath());
        }

        File.WriteAllBytes(AppPlatform.GetRuntimePackagePath() + Global.PackageManifestFileName, www.bytes);

        //按照更新列表增量更新文件
        string fileslist = www.text;
        string[] files = fileslist.Split('\n');
        float downloadedCount = 0f;
        for (int i = 0; i < files.Length; i++)
        {


            if (string.IsNullOrEmpty(files[i])) continue;
            string[] keyValue = files[i].Split('|');
            string fileName = keyValue[0];
            string localfilePath = (AppPlatform.GetRuntimePackagePath() + fileName).Trim();
            string path = Path.GetDirectoryName(localfilePath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //检查是否可以更新这个文件
            bool canUpdate = !File.Exists(localfilePath);
            if (!canUpdate)
            {
                string remoteMd5 = keyValue[1].Trim();
                string localMd5 = Util.MD5File(localfilePath);
                canUpdate = !remoteMd5.Equals(localMd5);
                if (canUpdate) File.Delete(localfilePath);
            }

            //可以更新这个文件
            if (canUpdate)
            {
                string fileUrl = Global.PackageUpdateURL + lastestVer + "/" + AppPlatform.GetPackageName() + "/" + fileName;
                Debug.Log(fileUrl);
                www = new WWW(fileUrl);
                yield return www;

                if (www.error != null)
                {
                    Debug.Log(www.error + path);
                    yield break;
                }
                File.WriteAllBytes(localfilePath, www.bytes);

                downloadedCount++;
                float p = (downloadedCount / (float)files.Length) * 100f;
                p = Mathf.Clamp(p, 0, 100);
                LoadingLayer.SetProgressbarValue((int)p);
            }
        }

        www.Dispose();
        LoadingLayer.SetProgressbarTips("更新资源完成");
        yield return new WaitForEndOfFrame();

        ResourceUpdateEnd();
    }
}
