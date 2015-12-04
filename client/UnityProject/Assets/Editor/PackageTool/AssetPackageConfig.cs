using System;

[System.Serializable] 
public class BundleInfo
{
    /// <summary>
    /// 资源包名
    /// </summary>
    public string name;

    /// <summary>
    /// 资源包的原始路径
    /// </summary>
    public string assetPath;

    /// <summary>
    /// 打包类型
    /// </summary>
    public string packageType;
}

[System.Serializable] 
public class AssetPackageConfig
{
    public string sceneDirectoryPath;
    public BundleInfo[] bundles;
}
