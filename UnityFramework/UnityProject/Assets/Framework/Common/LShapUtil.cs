using UnityEngine;
using System.Collections;

public class LShapUtil
{
    /// <summary>
    /// 调用脚本成员函数
    /// </summary>
    public static object CallScriptFunction(object rObj, string rTypeName, string rFuncName, params object[] rArgs)
    {
        var rName = rTypeName.Replace("(Clone)", "");
        return LShapClient.Instance.CallScriptMethod(rObj, rName, rFuncName, rArgs);
    }

    /// <summary>
    /// 调用脚本静态函数
    /// </summary>
    public static object CallScriptFunctionStatic(string rTypeName, string rFuncName, params object[] rArgs)
    {
        var rName = rTypeName.Replace("(Clone)", "");
        return LShapClient.Instance.CallScriptMethodStatic(rName, rFuncName, rArgs);
    }

    /// <summary>
    /// 创建脚本对象
    /// </summary>
    public static object CreateScriptObject(string rTypeName, params object[] rArgs)
    {
        var rName = rTypeName.Replace("(Clone)", "");
        return LShapClient.Instance.CreateScriptObject(rName, rArgs);
    }

}
