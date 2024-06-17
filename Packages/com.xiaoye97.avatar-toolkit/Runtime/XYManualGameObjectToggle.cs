#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace xiaoye97.AvatarToolkit
{
    /// <summary>
    /// 普通开关
    /// 开关指定的物体列表
    /// </summary>
    public class XYManualGameObjectToggle : XYToggleComponent
    {
        public List<GameObject> GameObjects;

        public override void Generate(XYAvatarPlugin plugin)
        {
            if (string.IsNullOrWhiteSpace(ParameterName))
            {
                Debug.LogError("参数名不能为空");
                return;
            }

            GameObject[] objs = GameObjects.ToArray();
            var layer = plugin.FxController.NewLayer($"开关_{ParameterName}");
            var shown = layer.NewState("显示")
                .WithAnimation(SetOnBlendShapes(plugin.AAC.NewClip().Toggling(objs, true)));
            var hidden = layer.NewState("隐藏")
                .WithAnimation(SetOffBlendShapes(plugin.AAC.NewClip().Toggling(objs, false)));

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
}
#endif