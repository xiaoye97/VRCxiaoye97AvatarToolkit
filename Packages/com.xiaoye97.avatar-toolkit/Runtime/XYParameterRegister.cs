using System;
using System.Collections.Generic;
using nadena.dev.modular_avatar.core;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;
using VRC.SDKBase;

namespace xiaoye97.AvatarToolkit
{
    /// <summary>
    /// 根据文件,自动设置MA的参数
    /// </summary>
    public class XYParameterRegister : MonoBehaviour, IEditorOnly
    {
        public VRCExpressionParameters ParameterAsset;
        private ModularAvatarParameters maParams;

        private void OnValidate()
        {
            maParams = GetComponent<ModularAvatarParameters>();
            if (maParams == null)
            {
                maParams = gameObject.AddComponent<ModularAvatarParameters>();
            }

            maParams.parameters = new List<ParameterConfig>();
            if (ParameterAsset != null)
            {
                foreach (var p in ParameterAsset.parameters)
                {
                    var map = new ParameterConfig();
                    map.nameOrPrefix = p.name;
                    map.defaultValue = p.defaultValue;
                    map.saved = p.saved;
                    if (p.valueType == VRCExpressionParameters.ValueType.Bool)
                    {
                        map.syncType = ParameterSyncType.Bool;
                    }
                    else if (p.valueType == VRCExpressionParameters.ValueType.Int)
                    {
                        map.syncType = ParameterSyncType.Int;
                    }
                    else if (p.valueType == VRCExpressionParameters.ValueType.Float)
                    {
                        map.syncType = ParameterSyncType.Float;
                    }

                    map.localOnly = !p.networkSynced;

                    maParams.parameters.Add(map);
                }
            }
        }
    }
}