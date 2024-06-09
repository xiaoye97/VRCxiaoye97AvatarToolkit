using UnityEngine;

namespace xiaoye97.AvatarToolkit
{
    /// <summary>
    /// 切换组件基类
    /// </summary>
    public abstract class XYSwitchComponent : XYBaseComponent
    {
        [Header("参数名")] public string ParameterName;
        [Header("默认值")] public int DefaultValue;
        [Header("菜单")] public XYRegisterMenuData RegisterMenuData;
    }
}