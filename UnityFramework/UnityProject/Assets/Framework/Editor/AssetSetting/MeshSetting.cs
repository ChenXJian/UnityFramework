using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

public class MeshSetting : MonoBehaviour
{
    public static Mesh[] GetAllMesh()
    {
        if (!Directory.Exists(Application.dataPath + "/" + PackagePlatform.RawResourcesDirectory))
        {
            Debug.Log("没有找到RawResources");
            return null;
        }

        var rGuids = AssetDatabase.FindAssets("t:mesh", new string[] { "Assets/" + PackagePlatform.RawResourcesDirectory });
        var rPaths = new string[rGuids.Length];

        var rMeshes = new Mesh[rGuids.Length];


        var rCount = 0;
        foreach (var rGuid in rGuids)
        {
            var rPath = rPaths[rCount] = AssetDatabase.GUIDToAssetPath(rGuid);

            var rTexture = AssetDatabase.LoadAssetAtPath(rPath, typeof(Mesh)) as Mesh;
            if (rTexture != null)
            {
                rMeshes[rCount] = rTexture;
            }
            rCount++;
        }

        return rMeshes;
    }
}
