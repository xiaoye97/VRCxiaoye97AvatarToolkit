using System;
using UnityEngine;

namespace xiaoye97.AvatarToolkit
{
    [Serializable]
    public class BlendShapeData
    {
        public SkinnedMeshRenderer Renderer;
        public string BlendShapeName;
        public float Value;
    }
}