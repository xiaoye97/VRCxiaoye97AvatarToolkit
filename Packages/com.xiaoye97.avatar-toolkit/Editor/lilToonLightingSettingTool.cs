using System;
using UnityEditor;
using UnityEngine;

namespace xiaoye97.AvatarToolkit
{
    public class lilToonLightingSettingTool : EditorWindow
    {
        [MenuItem("Tools/xiaoye97AvatarToolkit/lilToonLightingSettingTool")]
        public static void OpenWindow()
        {
            GetWindow<lilToonLightingSettingTool>("lilToon Lighting Setting Tool");
        }

        public float LowerBrightnessLimit = 0.05f;
        public float UpperBrightnessLimit = 1f;
        public float MonochromeLighting = 0.4f;
        public float EnviromentStrengthOnShadowColor = 0;

        private void OnGUI()
        {
            GUILayout.Label("此处可以一键对项目中所有的lilToon材质球进行光照设置");
            LowerBrightnessLimit = EditorGUILayout.Slider("亮度下限", LowerBrightnessLimit, 0, 1);
            UpperBrightnessLimit = EditorGUILayout.Slider("亮度上限", UpperBrightnessLimit, 0, 10);
            MonochromeLighting = EditorGUILayout.Slider("单色照明", MonochromeLighting, 0, 1);
            EnviromentStrengthOnShadowColor = EditorGUILayout.Slider("阴影颜色的环境强度", EnviromentStrengthOnShadowColor, 0, 1);
            if (GUILayout.Button("应用到所有lilToon材质球"))
            {
                ApplyToAllMaterial();
            }
        }
        
        private void ApplyToAllMaterial()
        {
            Material[] materials = Resources.FindObjectsOfTypeAll<Material>();
            foreach (Material material in materials)
            {
                if (material.shader.name == "lilToon" || material.shader.name.StartsWith("_lil"))
                {
                    ApplyToMaterial(material);
                }
            }
        }
        
        private void ApplyToMaterial(Material material)
        {
            string path = AssetDatabase.GetAssetPath(material);
            material.SetFloat("_LightMinLimit", LowerBrightnessLimit);
            material.SetFloat("_LightMaxLimit", UpperBrightnessLimit);
            material.SetFloat("_MonochromeLighting", MonochromeLighting);
            material.SetFloat("_ShadowEnvStrength", EnviromentStrengthOnShadowColor);
            Debug.Log($"修改了[{material.name}]({material.shader.name})的lilToon参数, 路径:{path}", material);
        }
    }
}