#if UNITY_EDITOR
using System;
using UnityEngine;
using VRC.SDKBase;

namespace xiaoye97.AvatarToolkit
{
    /// <summary>
    /// 当角色的Armature名称不为默认的"Armature"时,使用此组件指定Armature
    /// </summary>
    [Obsolete("从1.0.2版本开始,可以不再使用此组件定位Armature")]
    public class XYArmatureName : MonoBehaviour, IEditorOnly
    {
    }
}
#endif