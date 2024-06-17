#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace xiaoye97.AvatarToolkit.Editor
{
    [CustomEditor(typeof(XYBuildManager))]
    public class XYBuildManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("ChangeConfig"))
            {
                XYBuildManager manager = target as XYBuildManager;
                manager.ChangeConfig();
            }
        }
    }
}
#endif