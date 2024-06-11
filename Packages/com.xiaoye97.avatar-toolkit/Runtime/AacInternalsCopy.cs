#if UNITY_EDITOR
using System;
using AnimatorAsCode.V1;
using UnityEditor;
using UnityEngine;

namespace xiaoye97.AvatarToolkit
{
    public static class AacInternalsCopy
    {
        internal static EditorCurveBinding Binding(AacConfiguration component, Type type, Transform transform,
            string propertyName)
        {
            return new EditorCurveBinding
            {
                path = ResolveRelativePath(component.AnimatorRoot, transform),
                type = type,
                propertyName = propertyName
            };
        }

        internal static void SetCurve(AnimationClip clip, EditorCurveBinding binding, AnimationCurve curve)
        {
            // https://forum.unity.com/threads/new-animationclip-property-names.367288/#post-2384172
            clip.SetCurve(binding.path, binding.type, binding.propertyName, curve);
        }

        internal static AnimationCurve OneFrame(float desiredValue)
        {
            return AnimationCurve.Constant(0f, 1 / 60f, desiredValue);
        }

        internal static AnimationCurve ConstantSeconds(float seconds, float desiredValue)
        {
            return AnimationCurve.Constant(0f, seconds, desiredValue);
        }

        internal static string ResolveRelativePath(Transform avatar, Transform item)
        {
            if (avatar == item)
            {
                // TODO: Is this correct??
                return "";
            }

            if (item.parent != avatar && item.parent != null)
            {
                return ResolveRelativePath(avatar, item.parent) + "/" + item.name;
            }

            return item.name;
        }

        internal static EditorCurveBinding ToSubBinding(EditorCurveBinding binding, string suffix)
        {
            return new EditorCurveBinding
                { path = binding.path, type = binding.type, propertyName = binding.propertyName + "." + suffix };
        }
    }
}
#endif