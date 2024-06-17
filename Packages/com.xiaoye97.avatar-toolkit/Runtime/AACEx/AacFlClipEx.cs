#if UNITY_EDITOR
using System.Reflection;
using AnimatorAsCode.V1;
using UnityEngine;

namespace xiaoye97.AvatarToolkit.AACEx
{
    public static class AacFlClipEx
    {
        public static AacFlClip MaterialProperty(this AacFlClip aacFlClip, Renderer renderer, string propertyName,
            MaterialPropertyValueType valueType, MaterialPropertyValue value)
        {
            FieldInfo _componentInfo =
                typeof(AacFlClip).GetField("_component", BindingFlags.NonPublic | BindingFlags.Instance);

            AacConfiguration _component = (AacConfiguration)_componentInfo.GetValue(aacFlClip);
            if (valueType == MaterialPropertyValueType.Color)
            {
                var bindingR = AacInternalsCopy.Binding(_component, renderer.GetType(), renderer.transform,
                    $"material.{propertyName}.r");
                var bindingG = AacInternalsCopy.Binding(_component, renderer.GetType(), renderer.transform,
                    $"material.{propertyName}.g");
                var bindingB = AacInternalsCopy.Binding(_component, renderer.GetType(), renderer.transform,
                    $"material.{propertyName}.b");
                var bindingA = AacInternalsCopy.Binding(_component, renderer.GetType(), renderer.transform,
                    $"material.{propertyName}.a");
                AacInternalsCopy.SetCurve(aacFlClip.Clip, bindingR, AacInternalsCopy.OneFrame(value.ColorValue.r));
                AacInternalsCopy.SetCurve(aacFlClip.Clip, bindingG, AacInternalsCopy.OneFrame(value.ColorValue.g));
                AacInternalsCopy.SetCurve(aacFlClip.Clip, bindingB, AacInternalsCopy.OneFrame(value.ColorValue.b));
                AacInternalsCopy.SetCurve(aacFlClip.Clip, bindingA, AacInternalsCopy.OneFrame(value.ColorValue.a));
            }
            else
            {
                var binding = AacInternalsCopy.Binding(_component, renderer.GetType(), renderer.transform,
                    $"material.{propertyName}");
                AnimationCurve curve = null;
                if (valueType == MaterialPropertyValueType.Bool)
                {
                    curve = AacInternalsCopy.OneFrame(value.BoolValue ? 1 : 0);
                }
                else if (valueType == MaterialPropertyValueType.Int)
                {
                    curve = AacInternalsCopy.OneFrame(value.IntValue);
                }
                else if (valueType == MaterialPropertyValueType.Float)
                {
                    curve = AacInternalsCopy.OneFrame(value.FloatValue);
                }

                AacInternalsCopy.SetCurve(aacFlClip.Clip, binding, curve);
            }


            return aacFlClip;
        }

        public static AacFlClip MaterialPropertyRadial(this AacFlClip aacFlClip, Renderer renderer, string propertyName,
            float minValue, float maxValue)
        {
            FieldInfo _componentInfo =
                typeof(AacFlClip).GetField("_component", BindingFlags.NonPublic | BindingFlags.Instance);

            AacConfiguration _component = (AacConfiguration)_componentInfo.GetValue(aacFlClip);
            var binding = AacInternalsCopy.Binding(_component, renderer.GetType(), renderer.transform,
                $"material.{propertyName}");
            AnimationCurve curve = AnimationCurve.Linear(0, minValue, 1, maxValue);
            AacInternalsCopy.SetCurve(aacFlClip.Clip, binding, curve);
            return aacFlClip;
        }
    }
}
#endif