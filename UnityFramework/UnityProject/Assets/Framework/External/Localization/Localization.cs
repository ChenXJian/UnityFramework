using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using System;

public enum LocalizationType
{
    en_US,
    zh_CN,
}

public static class Localization 
{
	static Dictionary<string, string> dictionary = new Dictionary<string, string>();
    static List<GameObject> localizeGameObjects = new List<GameObject>();
	public static bool localizationLoaded = false;
    static string languageCurrent = "Example";

	public static string Language
	{
        get { return languageCurrent; }
		set
		{
            languageCurrent = value;
			localizationLoaded = false;
            ResetLocalization();
		}
	}

    public static void Initialize(LocalizationType type)
    {
        Language = type.ToString();
    }

    public static void ReloadLocalize()
    {
        foreach (GameObject obj in localizeGameObjects)
        {
            if (obj != null) obj.GetComponent<Localize>().LoadKey();
        }
    }

    public static string Get(string key)
    {
        if (!localizationLoaded)
        {
            Debug.LogWarning("[Localization] Localization hasn't been loaded yet.");
        } 

        if (localizationLoaded)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            Debug.LogWarning("[Localization] Could not find the key " + key);
        }
        return key;
    }

    public static string Get(GameObject localizeObject, string key)
    {
        bool gotLocalizeObject = false;

        for (int i = 0; i < localizeGameObjects.Count; i++ )
        {
            if (localizeGameObjects[i] == localizeObject)
            {
                gotLocalizeObject = true;
            } 
        }

        if (!gotLocalizeObject)
        {
            localizeGameObjects.Add(localizeObject);
        }

        if (localizationLoaded)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            Debug.LogWarning("[Localization] Could not find the key " + key);
        }
        else if (!localizationLoaded)
        {
            Debug.LogWarning("[Localization] Localization hasn't been loaded yet.");
        }
        return key;
    }

    public static void Set(string key, string value)
    {
        if (!localizationLoaded)
        {
            ResetLocalization();
        }

        if (dictionary.ContainsKey(key))
        {
            Dictionary<string, string> temp = new Dictionary<string, string>(dictionary);
            List<string> tempList = new List<string>(dictionary.Keys);
            dictionary.Clear();
            foreach (string str in tempList)
            {
                if (str == key) dictionary.Add(key, value);
                else dictionary.Add(str, temp[str]);
            }
            return;
        }
        dictionary.Add(key, value);
    }

    static void ResetLocalization()
	{
		if (localizationLoaded)
		{
            Debug.Log("LocalizationFile 已经加载过");
            return;
		}

        Global.AssetLoadManager.LoadLocalizationFile(languageCurrent, OnParseLocalizationFile);
	}

    static void OnParseLocalizationFile(TextAsset asset)
    {
        if (asset == null)
        {
            throw new NullReferenceException("Not find LocalizationFile");
        }

        dictionary.Clear();

        string[] separatorLine = new string[] { "\n" };
        List<string> lines = asset.text.Split(separatorLine, System.StringSplitOptions.None).ToList();

        List<string> repeatKeyCheck = new List<string>();
        string[] separatorPair = new string[] { " = " };
        foreach (string line in lines)
        {
            // split each list on the separator
            string[] pair = line.Split(separatorPair, System.StringSplitOptions.None).ToArray();
            if (pair.Length > 2)
            {
                foreach (string elment in pair)
                {
                    if (elment != pair[0] && elment != pair[1])
                    {
                        pair[1] = pair[1] + " = " + elment;
                    }
                }
            }

            // Takes care of empty lines
            if (pair.Length > 1)
            {
                pair[1] = pair[1].Replace("\\n", System.Environment.NewLine);

                if (!repeatKeyCheck.Contains(pair[0]))
                {
                    repeatKeyCheck.Add(pair[0]);
                    dictionary.Add(pair[0], pair[1]);
                }
                else
                {
                    Debug.LogWarning("[Localization] repeat keys (" + pair[0] + ") found while loading localization");
                }
            }
        }

        localizationLoaded = true;

        ReloadLocalize();
    }
}
