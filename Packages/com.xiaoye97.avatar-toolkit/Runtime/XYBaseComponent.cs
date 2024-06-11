#if UNITY_EDITOR
using UnityEngine;
using VRC.SDKBase;

namespace xiaoye97.AvatarToolkit
{
    /// <summary>
    /// 组件基类
    /// </summary>
    public abstract class XYBaseComponent : MonoBehaviour, IEditorOnly
    {
        public virtual void Generate(XYAvatarPlugin plugin)
        {
        }
    }
}
#endif