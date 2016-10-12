using System;

[System.Serializable] 
public class AssetBundlePackageInfo
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
public class PackageConfig
{
    public string mainVersion;
    public string minorVersion;
    public string rawScenePath;
    public string rawCodePath;
    public string rawConfigPath;
    public string rawDataTablePath;
    public AssetBundlePackageInfo[] bundles;
}
