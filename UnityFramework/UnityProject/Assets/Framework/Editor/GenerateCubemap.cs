using UnityEngine;
using System.Collections;
using UnityEditor;

public class GenerateCubemap : ScriptableWizard
{


    public Transform renderPos;
    public Cubemap cubemap;



    void OnWizardUpdate()
    {
        helpString = "指定渲染位置与Cubemap资源";

        if (renderPos != null && cubemap != null)
        {
            isValid = true;
        }
        else
        {
            isValid = false;
        }
    }


    void OnWizardCreate()
    {
        GameObject go = new GameObject("camera", typeof(Camera));

        go.transform.position = renderPos.position;
        go.transform.rotation = renderPos.rotation;

        go.GetComponent<Camera>().RenderToCubemap(cubemap);
        DestroyImmediate(go);
    }

    [MenuItem("Tools/GenerateCubemap")]
    static void RenderCubemap()
    {
        ScriptableWizard.DisplayWizard("RenderCubemap", typeof(GenerateCubemap), "Render");
    }
}
