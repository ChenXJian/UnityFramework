using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public delegate TResult UEasyFunc<TResult>();

public static class FunctionExtends
{


    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> handler)
    {
        int i = 0;
        foreach (T item in enumerable)
        {
            handler(item, i);
            i++;
        }
    }

    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> handler)
    {
        foreach (T item in enumerable) handler(item);
    }

    public static void SetPositionX(this Transform tfm, float x)
    {
        tfm.localPosition = new Vector3(x, tfm.localPosition.y, tfm.localPosition.z);
    }

    public static void SetPositionY(this Transform tfm, float y)
    {
        tfm.localPosition = new Vector3(tfm.localPosition.x, y, tfm.localPosition.z);
    }

    public static void SetPositionZ(this Transform tfm, float z)
    {
        tfm.localPosition = new Vector3(tfm.localPosition.x, tfm.localPosition.y, z);
    }

    public static void SetScaleX(this Transform tfm, float x)
    {
        tfm.localScale = new Vector3(x, tfm.localScale.y, tfm.localScale.z);
    }

    public static void SetScaleY(this Transform tfm, float y)
    {
        tfm.localScale = new Vector3(tfm.localScale.x, y, tfm.localScale.z);
    }

    public static void Reset(this Transform tfm)
    {
        tfm.localPosition = Vector3.zero;
        tfm.localScale = Vector3.one;
        tfm.localRotation = Quaternion.identity;

    }

    public static T GetComponentSafe<T>(this GameObject rGo) where T : Component
    {
        var com = rGo.GetComponent<T>();
        if(com != null)
        {
            return com;
        }
        else
        {
            return rGo.AddComponent<T>();
        }
    }
}
