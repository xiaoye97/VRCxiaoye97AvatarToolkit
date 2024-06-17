#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace xiaoye97.AvatarToolkit
{
    [Serializable]
    public class XYBuildConfig
    {
        public string ConfigName;
        public string BlueprintID;
        public List<XYBuildGroupConfig> GroupConfigs = new List<XYBuildGroupConfig>();
    }

    [Serializable]
    public class XYBuildGroupConfig
    {
        public XYBuildGroup Group;
        public bool Enable;
    }
}
#endif