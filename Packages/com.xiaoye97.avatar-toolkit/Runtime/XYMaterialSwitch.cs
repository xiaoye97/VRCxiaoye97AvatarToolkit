using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace xiaoye97.AvatarToolkit
{
    /// <summary>
    /// 材质切换器
    /// </summary>
    public class XYMaterialSwitch : XYSwitchComponent
    {
        public int MaterialSlot;
        public List<XYMaterialSwitchData> Datas;

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
            var layer = plugin.FxController.NewLayer($"切换_{ParameterName}");
            var param = layer.IntParameter(ParameterName);
            layer.OverrideValue(param, DefaultValue);
            for (int i = 0; i < Datas.Count; i++)
            {
                var data = Datas[i];
                var state = layer.NewState($"切换{i}")
                    .WithAnimation(plugin.AAC.NewClip().SwappingMaterial(r, MaterialSlot, data.Material));
                layer.AnyTransitionsTo(state).When(param.IsEqualTo(i));
            }

            plugin.RegisterIntParam(ParameterName, DefaultValue, param);

            if (RegisterMenuData.RegisterMenu)
            {
                string path = RegisterMenuData.MenuPath + "/" + RegisterMenuData.MenuName;
                for (int i = 0; i < Datas.Count; i++)
                {
                    var data = Datas[i];
                    plugin.MA.EditMenuItem(plugin.XYMenuManager.SubMenuByPath(path)
                            .MenuItem(data.MenuName).gameObject)
                        .ToggleSets(param, i)
                        .WithIcon(data.MenuIcon);
                }
            }
        }
    }

    [Serializable]
    public class XYMaterialSwitchData
    {
        public Material Material;
        public string MenuName;
        public Texture2D MenuIcon;
    }
}