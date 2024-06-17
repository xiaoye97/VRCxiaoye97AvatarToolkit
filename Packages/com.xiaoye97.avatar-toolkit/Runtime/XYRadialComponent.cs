#if UNITY_EDITOR
using UnityEngine;

namespace xiaoye97.AvatarToolkit
{
    /// <summary>
    /// 轮盘组件基类
    /// </summary>
    public abstract class XYRadialComponent : XYBaseComponent
    {
        [Header("参数名")] public string ParameterName;
        [Header("默认值")] public float DefaultValue;
        public float MinValue;
        public float MaxValue = 1;
        [Header("属性名")] public string PropertyName;
        [Header("菜单")] public XYRegisterMenuData RegisterMenuData;
    }
}
#endif