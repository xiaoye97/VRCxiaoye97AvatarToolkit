#if UNITY_EDITOR
using UnityEngine;
using VRC.SDKBase;

namespace xiaoye97.AvatarToolkit
{
    /// <summary>
    /// 打包组物体
    /// 将此组件挂在目标物体上并设置分组,会在打包时根据分组设置自动选择是否打包
    /// </summary>
    public class XYBuildItem : MonoBehaviour, IEditorOnly
    {
        public XYBuildGroup Group;
    }
}
#endif