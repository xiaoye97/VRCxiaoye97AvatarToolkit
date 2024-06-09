using System;
using System.Collections.Generic;
using UnityEngine;

namespace xiaoye97.AvatarToolkit
{
    /// <summary>
    /// 普通开关
    /// 会把开关下所有物体都纳入控制
    /// </summary>
    public class XYGameObjectToggle : XYToggleComponent
    {
        public override void Generate(XYAvatarPlugin plugin)
        {
            if (string.IsNullOrWhiteSpace(ParameterName))
            {
                Debug.LogError("参数名不能为空");
                return;
            }

            GameObject[] objs =
                gameObject.GetAllChildGameObjects(true, new List<Type>() { typeof(XYGameObjectToggle) });
            // 排除原角色骨骼物体
            objs = XYTool.RemoveGameObjectItemByName(objs, plugin.ArmatureGameobjectList);
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