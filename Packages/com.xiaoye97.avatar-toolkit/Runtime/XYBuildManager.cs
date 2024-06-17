#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;

namespace xiaoye97.AvatarToolkit
{
    [RequireComponent(typeof(VRC_AvatarDescriptor))]
    public class XYBuildManager : MonoBehaviour, IEditorOnly
    {
        private PipelineManager _pipelineManager;
        public List<XYBuildConfig> Configs = new List<XYBuildConfig>();

        public int UsedConfigIndex;

        private void Awake()
        {
        }

        public void ChangeConfig()
        {
            _pipelineManager = GetComponent<PipelineManager>();
            if (UsedConfigIndex >= 0 && UsedConfigIndex < Configs.Count)
            {
                var config = Configs[UsedConfigIndex];
                _pipelineManager.blueprintId = config.BlueprintID;
                foreach (var groupConfig in config.GroupConfigs)
                {
                    groupConfig.Group.EnableBuild = groupConfig.Enable;
                }
            }
        }
    }
}
#endif