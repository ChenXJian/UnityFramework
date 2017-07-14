# UnityFramework

### （此项目，在实际商业项目中，进行了大量重构，热更新部分也将L#完全替换为了ILRuntime, 性能提升巨大，推荐使用ILRuntime， 但这里由于创业缘故，已没有时间继续维护了）

## 简述
UnityFramework是基于Unity3D 5.4版本 + L#实现的一套热更新架构

* Unity版本 : 5.4 
* UI : UGUI
* 热更新系统 : C#代码全平台热更新（L#）
* 资源打包系统 ： Unity5.0新打包系统

## 引用

UnityFramework或参考设计思路，或直接代码引用了以下开源项目，在此表示感谢与尊敬

* L#
* SimpleFramework
* UIWidget
* TsiU

## 支持功能

* C#脚本全平台热更新(L#)
* 简洁的手游UI架构
* 简洁资源管理解决方案，包括Assetbundle
* 丰富的扩展功能（gamepaly，行为树，网络，数据，多语言...等等）
* 常用函数的封装（数学计算，文件操作，调试工具...等等）

## 使用说明

**目录结构** 

1. HotFixCode: 热更新代码工程文件夹
2. UnityProject: Unity工程文件夹
3. HotFixExport: 热更新代码导出的DLL文件夹(生成热更新工程时产生，开发期间会Copy至UnityProject的RawResources目录下，方便开发)

**首次运行**  

1.生成Unity的解决方案  
2.添加HotFixCode工程至上一步的解决方案  
3.编译HotFixCode
4.打开Tool工具里的打包工具，生成资源包  
5.打开Launcher场景，并运行


## 版本记录
V1.0.0_alpha[2016.01.06] 

1. 升级Unity5.4版本
2. 从uEasyKit更名为UnityFramework
3. 大量修复老版本bug
4. 重新组织架构
5. 将数据与网络等模块尽可能的移出架构，便于集成

V0.0.6_alpha[2016.01.06] 

1. 升级Unity5.3版本
2. 优化了高分辨率设备的帧率
3. 更改SceneManager为SceneLoadManager

V0.0.5_alpha[2015.12.20] 

1. 修复打包工具在MAC上打包错误的问题
2. 修改loading方式，脱离Panel的结构，独立了出来
3. 优化了Layer级别的UI
4. 优化了场景加载的部分

V0.0.4_alpha[2015.12.10] 

1. 增加多语言本地化支持

V0.0.3_alpha[2015.12.09] 

1. 从EasyUnityFramework更名为uEasyKit
2. 修复打包时的编辑器BUG
3. 增加了一些便于开发期调试的功能
4. 增加了一些相机相关的功能

V0.0.2_alpha[2015.12.04] 

1. 工程目录结构调整
2. 资源打包工具增加界面，和一些便捷功能
3. 增加了一些曲线求值的函数

V0.0.1_alpha[2015.11.26]  

1. 测试版本
 
