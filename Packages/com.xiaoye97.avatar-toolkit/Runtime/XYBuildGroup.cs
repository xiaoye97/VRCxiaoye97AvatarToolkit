using UnityEngine;
using VRC.SDKBase;

namespace xiaoye97.AvatarToolkit
{
    /// <summary>
    /// 打包组
    /// 未启用的组在打包时会忽略
    /// </summary>
    public class XYBuildGroup : MonoBehaviour, IEditorOnly
    {
        public bool EnableBuild = true;
    }
}