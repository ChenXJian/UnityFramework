# uEasyKit

## 简述
uEasyKit是基于Unity3D 5.0版本实现的一套使用简单的开源游戏开发工具集

* Unity版本 : 5.2.0f3  
* UI : UGUI
* 热更新系统 : C#代码全平台热更新（L#）
* 资源打包系统 ： Unity5.0新打包系统
* 网络 ： TCP + HTTP
* 静态数据生成部分暂不开源

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
* 常用函数的封装（数学计算，文件操作）
* 不同模块间的简洁通信机制（也同样支持脚本代码与原生代码）
* 调式工具集

## 使用说明

**目录结构** 

1. HotFixCode: 热更新代码工程文件夹
2. UnityProject: Unity工程文件夹
3. HotFixExport: 热更新代码导出的DLL文件夹(生成热更新工程时产生，开发期间会Copy至UnityProject的Resources目录下，方便开发)

## 联系方式
QQ: 648398613  
Email: 648398613@qq.com

## 版本记录
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
 