# xiaoye97 Avatar Toolkit

## 简介

xiaoye97 Avatar Toolkit是一个`非破坏性`的便利的改模工具.

此工具基于[Modular Avatar][1]和[Animator As Code V1][2]制作的, 开发此插件的主要目的是为了节约我的时间, 远离繁琐的重复工作.

QQ交流群: 973574210, 答案 VRChat

如果对插件有建议, 或者是bug反馈, 都可以加群交流.

一个临时的视频教程(不包括安装步骤), 回头会重置 : https://www.bilibili.com/video/BV1m1421k7tn

## 插件特性

- `非破坏性` 和MA一样, 此插件不会破坏模型, 而是在运行时动态进行修改, 可以安全的删除而不用担心损坏.

- `不需要录制动画和编写FX` 对于插件中提供的功能, 不需要录制开关动画或者切换动画, 也不需要打开动画状态机修改FX层, 由插件统一生成. 这些工作是浪费我时间的主要元凶.

- `自动生成菜单` 对于插件中提供的各种功能, 可以方便的自动生成菜单到指定位置.

- `分组打包` 对于超多衣服要分衣柜的模型或者是要区分免费版或者付费版等需求的模型, 可以使用一个模型轻松的打包成多个模型.

注意: 此插件仅支持2022.3.22f1及以上版本, 老版本的兼容性未知, 由于我是2024年才开始玩VRChat, 所以并未涉足老版本的SDK.

## 插件安装步骤

在VCC中添加社区仓库

#### 官方源

nadena https://vpm.nadena.dev/vpm.json

hai-vr https://hai-vr.github.io/vpm-listing/index.json

xiaoye97 https://xiaoye97.github.io/VRCxiaoye97AvatarToolkit/index.json

#### 中国镜像源

如果你在中国, 可以使用镜像源进行添加

nadena https://vpm.vrczh.org/vpm/nadena

hai-vr https://vpm.vrczh.org/vpm/hai-vr

### 新建项目

添加好仓库后, 新建2022Avatar模板项目, 并添加以下包
- Modular Avatar
- Animator As Code (Alpha)
- Animator As Code - VRChat (Alpha)
- Animator As Code - Modular Avatar (Alpha)
- xiaoye97 Avatar Toolkit


# 使用说明

## 准备工作

将模型素体放到场景中, 右键模型, 点击Modular Avatar->Extract Menu.

在MA生成的Avatar Menu物体上, 添加`XYMenuManager`组件, 这个组件是用于自动化注册菜单的.

在模型的根骨骼上添加一个`XYArmatureName`组件, 如果模型的根骨骼名字为`Armature`且大小写也一样, 则可以不添加此组件, 此组件用于定位根骨骼位置.

## 一些组件通用的属性的说明

`ParameterName` 参数名, 会自动注册到模型的参数列表, 并且在动画状态机中也会自动创建, 不要起重复的参数名.

`RegisterMenuData` 自动注册菜单的选项.

`RegisterMenu` 是否要注册菜单.

`MenuPath` 菜单文件夹的路径, 用斜杠分隔子集. 如 `换装/身体` `换装/衣服` `换装/衣服/连衣裙设置`

`MenuName` 菜单项的名称.

`MenuIcon` 菜单的图标, 可以留空.

## 开关组件

开关组件目前有3种

- XYGameObjectToggle 物体开关
- XYManualGameObjectToggle 手动物体开关
- XYMaterialPropertyToggle 材质属性开关

所有的开关都有一些共同的属性.

`DefaultValue` 默认值, 此开关默认是开还是关, 如果默认为关, 建议将当前的物体也设置为隐藏状态.

`BlendShape` 当开关状态改变时对BlendShape进行改变, 例如做某些无法改变胸部大小的上衣时, 当衣服打开时同时设置固定的胸部大小.

### XYGameObjectToggle

这是最常用的开关, 将其添加到物体上时, 可以对物体及其所有子物体进行开关.

在以往, 如果通过录制动画对MA穿的衣服进行开关, 会出现衣服虽然隐藏了, 但是动骨没有隐藏的情况, 导致性能占用提高.

但是这个物体开关可以记录下所有子物体对其进行开关, 就算是MA的衣服, 子物体的动骨也可以一起隐藏.

### XYManualGameObjectToggle

手动物体开关, 和物体开关不同的是, 此开关不会扫描子物体, 而是需要手动将想要开关的所有物体添加到列表.

### XYMaterialPropertyToggle

材质属性开关, 用于对当前物体上的材质属性进行开关(仅限第一个材质).

材质属性开关有一个列表, 可以记录多个要进行开关的属性.

每个属性要指定他的类型, 支持4种类型的属性: Bool, Int, Float, Color.

当开关打开时, 对应的属性会被设置为TrueValue中记录的值, 反之, 当开关关闭时, 对应的属性会被设置为FalseValue中记录的值.

例如我们要做一个纹身的贴花, 并且这个纹身带有流光效果, 那么我们可以在TrueValue中将贴花的透明度和流光的发光强度设置为正值, 在FalseValue中将他们设置为0.

## 切换组件

切换组件目前有1种

- XYMaterialSwitch 材质切换

所有的切换组件都有一些共同的属性.

`DefaultValue` 默认值, 表示默认使用列表中的哪一个项.

### XYMaterialSwitch

材质切换主要用于对当前物体上的材质属性进行切换(仅限第一个材质).

材质切换有一个列表, 可以记录多个要进行切换的材质.

## 轮盘组件

轮盘组件是使用Float参数进行数值变化的组件.

轮盘组件目前有1种

- XYMaterialPropertyRadial 材质属性轮盘

所有的轮盘组件都有一些共同的属性.

`DefaultValue` 默认值, 表示默认状态下使用的数值.
`MinValue` 最小值, 表示数值的最小值.
`MaxValue` 最大值, 表示数值的最大值.

### XYMaterialPropertyRadial

材质属性轮盘用于对当前物体上的材质的Float类型属性进行控制(仅限第一个材质).

## 预设

在使用本插件进行模型制作时, 大量使用到了开关, 如果换衣服时, 手动去开关每套衣服, 会显得有些繁琐.

所以, 可以使用预设功能, 方便的切换不同的衣服, 以及进行不同衣服之间的混搭.

要使用预设功能, 我们可以在模型下新建一个子物体, 这个子物体当作我们的预设列表的父物体.

在这个物体下建多个子物体, 每个子物体代表一个预设, 并为他们添加`XYPreset`组件.

预设组件可以记录Bool, Int, Float值, 可以在玩家使用这个预设时设置对应的值.

建议第一个预设命名为`默认`, 并且不添加任何参数, 这样, 在使用此预设时, 所有的参数都会回到默认的状态.

要注意的是, 预设是对默认状态的变更, 例如, 我们默认状态下穿了服装A, 当我们用预设换装到B时, 需要先将 服装A 参数设置为False, 然后将 服装B 参数设置为True.

## 打包分组

有些时候, 如果我们的衣服非常多, 想要将一个模型拆分成2个, 或者是想要区分公开模和有独特功能的私人模. 那么使用打包分组功能, 我们对一个模型进行差分, 方便的从一个模型打包出多个模型.

在模型下新建一个子物体, 这个子物体当作我们的打包分组列表的父物体.

在这个物体下建多个子物体, 每个子物体代表一个打包分组, 并为他们添加`XYBuildGroup`组件.

例如, 我们可以建立一个特殊衣服组和一个DPS组.

打包分组组件上只有`EnableBuild`一个属性, 用于标记此分组是否进行打包.

在希望进行分组控制的物体和预设上, 添加 `XYBuildItem`组件, 并将之前创建好的分组拖入到`Group`属性中.

这样, 通过修改分组上的EnableBuild属性, 已经可以构建出不同版本的模型.

 不过, 为了方便多模型的上传以及组的切换, 我们可以在模型根物体上加一个`XYBuildManager`组件.

在XYBuildManager组件上可以存多个配置, 每个配置都可以写上BlueprintID和对应的分组设置.

在`UsedConfigIndex`处填入想要使用的配置的索引(从0开始), 然后点击ChangeConfig, 即可快速切换到对应的打包设置.

## 一些额外的组件

### XYGameObjectActive

此组件用于在打包时改变物体的显示隐藏状态.

例如, 有一个物体我们希望它默认是开的, 但是在编辑时不想一直显示着它妨碍编辑.

此时, 可以将此物体隐藏, 然后添加`XYGameObjectActive`组件, 并将`Active`设置为True, 那么, 在打包时, 此物体会自动显示.

反之, 如果将`Active`设置为False, 那么, 在打包时, 此物体会自动隐藏.

### XYParameterRegister

对于普通的参数创建需求, 可以之间使用MAParameter组件进行设置.

但是, MAParameter不支持使用VRC Expression Parameters资源进行批量的参数添加.

很偶尔的情况, 从其他作者那里获取的资源是使用的VRC Expression Parameters资源设置的参数, 他们一般是破坏性的工作流程, 直接将这些参数合并到模型中.

此时, 使用XYParameterRegister, 并将VRC Expression Parameters资源拖入`ParameterAsset`属性中, 即可生成一个对应这些参数的MAParameter, 来达到非破坏性添加的效果.

[1]:https://modular-avatar.nadena.dev/docs/intro
[2]:https://docs.hai-vr.dev/docs/products/animator-as-code