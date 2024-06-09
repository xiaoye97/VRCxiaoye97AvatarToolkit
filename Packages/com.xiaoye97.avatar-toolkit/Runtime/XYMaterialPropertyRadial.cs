using UnityEngine;
using xiaoye97.AvatarToolkit.AACEx;

namespace xiaoye97.AvatarToolkit
{
    /// <summary>
    /// 材质属性轮盘
    /// </summary>
    public class XYMaterialPropertyRadial : XYRadialComponent
    {
        public override void Generate(XYAvatarPlugin plugin)
        {
            if (string.IsNullOrWhiteSpace(ParameterName))
            {
                Debug.LogError("参数名不能为空");
                return;
            }

            Renderer r = GetComponent<Renderer>();
            if (r == null) return;
            var mat = r.material;
            var layer = plugin.FxController.NewLayer($"轮盘_{ParameterName}");
            var clip = plugin.AAC.NewClip().MaterialPropertyRadial(r, PropertyName, MinValue, MaxValue);
            var param = layer.FloatParameter(ParameterName);
            layer.OverrideValue(param, DefaultValue);
            layer.NewState("轮盘").WithAnimation(clip).MotionTime(param);

            plugin.RegisterFloatParam(ParameterName, DefaultValue, param);
            if (RegisterMenuData.RegisterMenu)
            {
                plugin.MA.EditMenuItem(plugin.XYMenuManager.SubMenuByPath(RegisterMenuData.MenuPath)
                        .MenuItem(RegisterMenuData.MenuName).gameObject)
                    .Radial(param)
                    .WithIcon(RegisterMenuData.MenuIcon);
            }
        }
    }
}