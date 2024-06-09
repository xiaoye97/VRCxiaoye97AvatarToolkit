﻿using System.Collections.Generic;
using AnimatorAsCode.V1;
using UnityEngine;

namespace xiaoye97.AvatarToolkit
{
    /// <summary>
    /// 开关组件基类
    /// </summary>
    public abstract class XYToggleComponent : XYBaseComponent
    {
        [Header("参数名")] public string ParameterName;
        [Header("默认值")] public bool DefaultValue;
        [Header("BlendShape")] public List<BlendShapeData> OnBlendShapeDatas;
        public List<BlendShapeData> OffBlendShapeDatas;
        [Header("菜单")] public XYRegisterMenuData RegisterMenuData;

        public virtual AacFlClip SetOnBlendShapes(AacFlClip clip)
        {
            if (OnBlendShapeDatas != null && OnBlendShapeDatas.Count > 0)
            {
                foreach (var bsd in OnBlendShapeDatas)
                {
                    clip.BlendShape(bsd.Renderer, bsd.BlendShapeName, bsd.Value);
                }
            }

            return clip;
        }

        public virtual AacFlClip SetOffBlendShapes(AacFlClip clip)
        {
            if (OffBlendShapeDatas != null && OffBlendShapeDatas.Count > 0)
            {
                foreach (var bsd in OffBlendShapeDatas)
                {
                    clip.BlendShape(bsd.Renderer, bsd.BlendShapeName, bsd.Value);
                }
            }

            return clip;
        }
    }
}