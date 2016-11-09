using UnityEngine;
using System.Collections;

public class MeshDeformation : MonoBehaviour
{
    public GameObject target;
    public int num;
    Vector3[] vertexsCur;
    Vector3[] vertexsIni ;
    Mesh mesh;

    Vector3 max;
    Vector3 min;
    bool op = false;

    public float mspeed;
    // Use this for initialization
    void Start () {
        mesh = target.GetComponent<MeshFilter>().mesh;
        //获取目标所有顶点  
        vertexsCur = new Vector3[num];
        vertexsIni = new Vector3[num];
        mesh.vertices.CopyTo(vertexsCur, 0);
        mesh.vertices.CopyTo(vertexsIni, 0);

        /*
        max = min = vertexs[0];
        for (int i = 0; i < vertexs.Length; i++)
        {
            if (vertexs[i].x < min.x)
                min = vertexs[i];
            else if (vertexs[i].x > max.x)
                max = vertexs[i];
        }
        */
        dis();
    }


    void dis()
    {
        for (int i = 0; i < vertexsCur.Length; i++)
        {
            var pos = vertexsCur[i];
            vertexsCur[i] = pos + mesh.normals[i] * Random.Range(1,3);


        }
        //刷新目标网格  
        mesh.vertices = vertexsCur;
        target.GetComponent<MeshFilter>().mesh = mesh;
        op = true;
    }

    void Update()
    {
        if (op == false) return;

        for (int i = 0; i < vertexsCur.Length; i++)
        {
            var distance = vertexsCur[i] - vertexsIni[i];

            vertexsCur[i] = vertexsCur[i] - distance  * (Time.deltaTime * mspeed);

        }
        mesh.vertices = vertexsCur;
        target.GetComponent<MeshFilter>().mesh = mesh;
    }

    /*
    void Update ()
    {
        for (int i = 0; i < vertexs.Length; i++)
        { 
            if (vertexs[i].y > min.y)
            {
                vertexs[i] = new Vector3(vertexs[i].x, vertexs[i].y - Time.deltaTime * speed, vertexs[i].z);
            }
            if (vertexs[i].y < min.y)
            {
                vertexs[i] = new Vector3(vertexs[i].x, min.y, vertexs[i].z);
            }
            if (vertexs[i].y == min.y)
            {
                vertexs[i] += mesh.normals[i] * Time.deltaTime * speed;
            }
        }
        mesh.vertices = vertexs;
        target.GetComponent<MeshFilter>().mesh = mesh;
        max = new Vector3(max.x
            , max.y - Time.deltaTime * speed, max.z);
    }
    */
}
