# uEasyKit

## 简述
uEasyKit是基于Unity3D 5.0版本实现的一套使用简单的开源游戏开发工具集

* Unity版本 : 5.2.0f3  
* UI : UGUI
* 热更新系统 : C#代码全平台热更新（L#）
* 资源打包系统 ： Unity5.0新打包系统
* 网络 ： TCP + HTTP


备注: （请第一次运行出问题的同学耐心一点，慢慢跟下问题，多半是引用丢失的问题，我这边平时维护这个的时间也不多，没有过多的时间写文档来说明怎么使用，所以还请出问题的同学耐心跟下问题，首次运行出现的问题都是非常简单的问题）

## 引用

uEasyKit或参考设计思路，或直接代码引用了以下开源项目，在此表示感谢与尊敬

* L#
* SimpleFramework
* JsonFX
* UIWidget
* DOTween
* TsiU

## 支持功能

* C#脚本全平台热更新(L#)
* TCP消息+HTTP请求的封装
* UGUI常用控件实现（ListView, Draggable, DialogBox, Progressbar, Tab）
* 基于MVC理念的简洁UI架构实现
* Assetbundle打包与加载系统
* 丰富的扩展功能（摄像机，行为树...等等）
* 通用模块的统一管理（手势消息分发，场景管理，网络...等等）
* 常用函数的封装（数学计算，文件操作...等等）
* 不同模块间的简洁通信机制（也同样支持脚本代码与原生代码）
* 调式工具集
* 多语言本地化功能（支持图片变更）

## 使用说明

**目录结构** 

1. HotFixCode: 热更新代码工程文件夹
2. UnityProject: Unity工程文件夹
3. HotFixExport: 热更新代码导出的DLL文件夹(生成热更新工程时产生，开发期间会Copy至UnityProject的Resources目录下，方便开发)

**首次运行**  

1.生成Unity的解决方案  
2.添加HotFixCode工程至上一步的解决方案  
3.生成HotFixCode  
4.打开Tool工具里的打包工具，生成场景包与其他资源包  
5.打开Launcher场景，并运行  

**常见问题**  

如果运行出错，请先检查以下问题  
1.脚本引用丢失，资源引用丢失  
2.Assetbundle文件是否存在  
3.AppDefine下的IsPersistentMode是否开启  

## 联系方式
QQ: 648398613  
Email: 648398613@qq.com

## 版本记录
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
 
