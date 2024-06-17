#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using AnimatorAsCode.V1;
using UnityEngine;
using xiaoye97.AvatarToolkit.AACEx;

namespace xiaoye97.AvatarToolkit
{
    /// <summary>
    /// 材质属性开关
    /// 将指定材质属性的两种状态进行开关控制
    /// (注意!不适宜同一个Renderer上有2个材质的情况,会因为动画记录上无法区分,会将两个材质都赋值)
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class XYMaterialPropertyToggle : XYToggleComponent
    {
        public List<MaterialPropertyToggleData> Datas;

        public override void Generate(XYAvatarPlugin plugin)
        {
            if (string.IsNullOrWhiteSpace(ParameterName))
            {
                Debug.LogError("参数名不能为空");
                return;
            }

            if (Datas == null || Datas.Count == 0) return;
            Renderer r = GetComponent<Renderer>();
            if (r == null) return;
            var mat = r.material;
            var layer = plugin.FxController.NewLayer($"开关_{ParameterName}");
            AacFlClip showClip = plugin.AAC.NewClip();
            AacFlClip hideClip = plugin.AAC.NewClip();
            for (int i = 0; i < Datas.Count; i++)
            {
                var data = Datas[i];
                showClip = showClip.MaterialProperty(r, data.PropertyName, data.ValueType, data.TrueValue);
                hideClip = hideClip.MaterialProperty(r, data.PropertyName, data.ValueType, data.FalseValue);
            }

            var shown = layer.NewState("显示")
                .WithAnimation(showClip);
            var hidden = layer.NewState("隐藏")
                .WithAnimation(hideClip);

            var itemParam = layer.BoolParameter(ParameterName);
            layer.OverrideValue(itemParam, DefaultValue);

            hidden.TransitionsTo(shown).When(itemParam.IsTrue());
            shown.TransitionsTo(hidden).When(itemParam.IsFalse());

            plugin.RegisterBoolParam(ParameterName, DefaultValue, itemParam);

            if (RegisterMenuData.RegisterMenu)
            {
                plugin.MA.EditMenuItem(plugin.XYMenuManager.SubMenuByPath(RegisterMenuData.MenuPath)
                        .MenuItem(RegisterMenuData.MenuName).gameObject)
                    .Toggle(itemParam)
                    .WithIcon(RegisterMenuData.MenuIcon);
            }
        }
    }

    [Serializable]
    public class MaterialPropertyToggleData
    {
        public string PropertyName;
        public MaterialPropertyValueType ValueType;
        public MaterialPropertyValue TrueValue;
        public MaterialPropertyValue FalseValue;
    }
}
#endif