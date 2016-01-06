using UnityEngine;
using System;
using System.Collections;

public class SceneLoadManager : MonoBehaviour 
{
    int progress = 0;
    string loadSceneName = null;
    int displayProgress = 0;

    const string callSetTips = "SetProgressbarTips";
    const string callSetValue = "SetProgressbarValue";

    AsyncOperation async = null;
    AssetBundle sceneBundle = null;

    UnityEngine.Events.UnityAction OnInitializeScene = null;
    UnityEngine.Events.UnityAction OnInitializeSceneComplete = null;

    public bool IsLoading { get; set; }

    public void EnterScene(string rSceneName, UnityEngine.Events.UnityAction rOnInitSceneBegin = null, UnityEngine.Events.UnityAction rOnInitSceneEnd = null)
    {
        if (String.IsNullOrEmpty(rSceneName))
        {
            throw new NullReferenceException("loadSceneName is null");
        }
        else
        {
            loadSceneName = rSceneName;
        }

        OnInitializeScene = rOnInitSceneBegin;
        OnInitializeSceneComplete = rOnInitSceneEnd;

        LoadingLayer.Show();

        LoadScene();
    }

    void LoadScene()
    {
        StartCoroutine(LoadSceneBundle());
    }

    IEnumerator LoadSceneBundle()
    {

        LoadingLayer.SetProgressbarTips("Loading Scene...");
        LoadingLayer.SetProgressbarValue(1);

        var rUrl = AppPlatform.GetSceneBundleDirUrl() + loadSceneName.ToLower() + ".unity3d";
        Debug.Log("[DownloadSceneBundle]:>" + rUrl);
        var download = new WWW(rUrl);
        yield return download;

        if (download.error != null)
        {
            Debug.LogError(download.error);
            yield break;
        }

        sceneBundle = download.assetBundle;

        StartCoroutine(LoadSceneInternal());
    }

    IEnumerator LoadSceneInternal()
    {
        displayProgress = 5;
        LoadingLayer.SetProgressbarTips("Scene Initialize ...");
        LoadingLayer.SetProgressbarValue(displayProgress);

        async = Application.LoadLevelAsync(loadSceneName);
        IsLoading = true;
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            progress = (int)async.progress * 100;
            while (displayProgress < progress)
            {
                ++displayProgress;
                LoadingLayer.SetProgressbarValue(displayProgress);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }

        progress = 90;
        while (displayProgress < progress)
        {
            ++displayProgress;
            LoadingLayer.SetProgressbarValue(displayProgress);
            yield return new WaitForEndOfFrame();
        }
         
        async.allowSceneActivation = true;
        InitializeScene();

        IsLoading = false;
    }

    void InitializeScene()
    {
        Util.ClearUICache();
        Util.ClearMemory();

        if (OnInitializeScene != null)
        {
            OnInitializeScene.Invoke();
            OnInitializeScene = null;
        }

        StartCoroutine(OnInitializeComplete());
    }

    IEnumerator OnInitializeComplete()
    {
        progress = 100;

        while (displayProgress < progress)
        {

            ++displayProgress;
            LoadingLayer.SetProgressbarValue(displayProgress);
            yield return new WaitForEndOfFrame();
        }

        loadSceneName = null;
        async = null;
        progress = 0;
        displayProgress = 0;
        sceneBundle.Unload(false);

        LoadingLayer.Hide();

        if (OnInitializeSceneComplete != null)
        {
            OnInitializeSceneComplete.Invoke();
            OnInitializeSceneComplete = null;
        }

    }
}