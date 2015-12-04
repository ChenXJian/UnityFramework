using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class TextureSetting
{
    public static void ChangeTextureFormatAll(TextureImporterFormat newFormat)
    {
        Object[] textures = GetAllTextures();

        foreach (var texture in textures)
        {
            string path = AssetDatabase.GetAssetPath(texture);

            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

            if (textureImporter.textureFormat != newFormat)
            {
                textureImporter.textureFormat = newFormat;
                AssetDatabase.ImportAsset(path);
                Debug.Log("ChangeTextureFormat: " + path);  
            }
        }
        AssetDatabase.Refresh();
    }

    public static Texture2D[] GetAllTextures()
    {
        if (!Directory.Exists(Application.dataPath + "/" + PackagePlatform.Instance.RawResourcesDirectory))
        {
            Debug.Log("没有找到RawResources");
            return null;
        }
        var rGuids = AssetDatabase.FindAssets("t:texture", new string[] { "Assets/" + PackagePlatform.Instance.RawResourcesDirectory });
        var rPaths = new string[rGuids.Length];

        var rTextures = new Texture2D[rGuids.Length];


        var rCount = 0;
        foreach (var rGuid in rGuids)
        {
            var rPath = rPaths[rCount] = AssetDatabase.GUIDToAssetPath(rGuid);

            var rTexture = AssetDatabase.LoadAssetAtPath(rPath, typeof(Texture2D)) as Texture2D;
            if (rTexture != null)
            {
                rTextures[rCount] = rTexture;
            }
            rCount++;
        }

        return rTextures;
    }
}