using System;
using System.Collections.Generic;
using UnityEngine;
using VRC.SDKBase;

namespace xiaoye97.AvatarToolkit
{
    /// <summary>
    /// 预设
    /// </summary>
    public class XYPreset : MonoBehaviour, IEditorOnly
    {
        public List<BoolPreset> BoolPresets = new List<BoolPreset>();
        public List<IntPreset> IntPresets = new List<IntPreset>();
        public List<FloatPreset> FloatPresets = new List<FloatPreset>();

        public XYRegisterMenuData RegisterMenuData = new XYRegisterMenuData(true, "预设", "", null);
    }

    [Serializable]
    public class BoolPreset
    {
        public string ParamName;
        public bool Value;
    }

    [Serializable]
    public class IntPreset
    {
        public string ParamName;
        public int Value;
    }

    [Serializable]
    public class FloatPreset
    {
        public string ParamName;
        public float Value;
    }
}