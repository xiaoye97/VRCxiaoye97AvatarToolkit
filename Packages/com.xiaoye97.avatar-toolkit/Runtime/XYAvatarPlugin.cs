using System.Collections.Generic;
using AnimatorAsCode.V1;
using AnimatorAsCode.V1.ModularAvatar;
using AnimatorAsCode.V1.VRC;
using nadena.dev.modular_avatar.core;
using nadena.dev.ndmf;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using VRC.SDK3.Avatars.ScriptableObjects;
using VRC.SDKBase;
using xiaoye97.AvatarToolkit;

[assembly: ExportsPlugin(typeof(XYAvatarPlugin))]

namespace xiaoye97.AvatarToolkit
{
    public class XYAvatarPlugin : Plugin<XYAvatarPlugin>
    {
        public override string QualifiedName => "xiaoye97.XYAvatarPlugin";
        public override string DisplayName => "XYAvatar";
        private const string SystemName = "XYAvatar";
        private const bool UseWriteDefaults = true;
        private const string DefaultArmatureName = "Armature";

        protected override void Configure()
        {
            InPhase(BuildPhase.Generating).Run($"Generate {DisplayName}", Generate);
        }

        public BuildContext CTX;
        public AacFlBase AAC;
        public MaAc MA;
        public XYMenuManager XYMenuManager;
        public AacFlController FxController;
        public Dictionary<string, AacFlBoolParameter> BoolParamDict;
        public Dictionary<string, AacFlIntParameter> IntParamDict;
        public Dictionary<string, AacFlFloatParameter> FloatParamDict;
        public Dictionary<string, bool> BoolParamDefaultDict;
        public Dictionary<string, int> IntParamDefaultDict;
        public Dictionary<string, float> FloatParamDefaultDict;
        public List<string> ArmatureGameobjectList;

        public void RegisterBoolParam(string paramName, bool defaultValue, AacFlBoolParameter parameter)
        {
            BoolParamDefaultDict[paramName] = defaultValue;
            BoolParamDict[paramName] = parameter;
            var maacParameter = MA.NewParameter(parameter);
            maacParameter.WithDefaultValue(defaultValue);
        }

        public void RegisterIntParam(string paramName, int defaultValue, AacFlIntParameter parameter)
        {
            IntParamDefaultDict[paramName] = defaultValue;
            IntParamDict[paramName] = parameter;
            var maacParameter = MA.NewParameter(parameter);
            maacParameter.WithDefaultValue(defaultValue);
        }

        public void RegisterFloatParam(string paramName, float defaultValue, AacFlFloatParameter parameter)
        {
            FloatParamDefaultDict[paramName] = defaultValue;
            FloatParamDict[paramName] = parameter;
            var maacParameter = MA.NewParameter(parameter);
            maacParameter.WithDefaultValue(defaultValue);
        }

        private void Generate(BuildContext ctx)
        {
            Debug.Log("宵夜插件开始生成工作");
            CTX = ctx;
            // 先处理打包组
            var buildGroups = ctx.AvatarRootTransform.GetComponentsInChildren<XYBuildGroup>(true);
            var buildItems = ctx.AvatarRootTransform.GetComponentsInChildren<XYBuildItem>(true);
            if (buildItems.Length > 0)
            {
                for (int i = 0; i < buildGroups.Length; i++)
                {
                    var group = buildGroups[i];
                    // 如果此组不打包,则将属于此组的物体移除
                    if (group.EnableBuild == false)
                    {
                        for (int j = buildItems.Length - 1; j >= 0; j--)
                        {
                            if (buildItems[j] != null)
                            {
                                var buildItem = buildItems[j];
                                if (buildItem.Group == group)
                                {
                                    Debug.Log($"忽略了打包组{group.name}的所属物体:{buildItem.name}");
                                    GameObject.DestroyImmediate(buildItem.gameObject);
                                }
                            }
                        }
                    }
                }
            }

            var xyComponents = ctx.AvatarRootTransform.GetComponentsInChildren<XYBaseComponent>(true);
            // 如果没有宵夜组件,则跳过
            if (xyComponents.Length == 0)
            {
                Debug.Log("宵夜插件没有找到所需组件,忽略本次生成");
                return;
            }

            XYMenuManager = ctx.AvatarRootTransform.GetComponentInChildren<XYMenuManager>();
            if (XYMenuManager == null)
            {
                Debug.LogError("宵夜插件没有找到XYMenuManager,请先右键角色 MA->Extract menu,然后把XYMenuManager放到新生成的物体上");
                return;
            }

            BoolParamDict = new Dictionary<string, AacFlBoolParameter>();
            IntParamDict = new Dictionary<string, AacFlIntParameter>();
            FloatParamDict = new Dictionary<string, AacFlFloatParameter>();
            BoolParamDefaultDict = new Dictionary<string, bool>();
            IntParamDefaultDict = new Dictionary<string, int>();
            FloatParamDefaultDict = new Dictionary<string, float>();
            ArmatureGameobjectList = new List<string>();
            // 统计Armature
            SearchArmature();
            // 初始化 Animator As Code.
            AAC = AacV1.Create(new AacConfiguration
            {
                SystemName = SystemName,
                AnimatorRoot = ctx.AvatarRootTransform,
                DefaultValueRoot = ctx.AvatarRootTransform,
                AssetKey = GUID.Generate().ToString(),
                AssetContainer = ctx.AssetContainer,
                ContainerMode = AacConfiguration.Container.OnlyWhenPersistenceRequired,
                DefaultsProvider = new AacDefaultsProvider(UseWriteDefaults)
            });
            // 给模型创建一个MA子物体
            MA = MaAc.Create(new GameObject(SystemName)
            {
                transform = { parent = ctx.AvatarRootTransform }
            });
            // 创建新的FX层控制器
            FxController = AAC.NewAnimatorController();

            // 遍历宵夜组件
            for (var index = 0; index < xyComponents.Length; index++)
            {
                var component = xyComponents[index];
                component.Generate(this);
            }

            // 预设
            GeneratePreset(ctx);

            // 混合控制器
            MA.NewMergeAnimator(FxController.AnimatorController, VRCAvatarDescriptor.AnimLayerType.FX);
            Debug.Log("宵夜插件工作完毕");
        }

        private void SearchArmature()
        {
            Transform armature = null;
            var aName = CTX.AvatarRootTransform.GetComponentInChildren<XYArmatureName>();
            if (aName != null)
            {
                armature = aName.transform;
            }
            else
            {
                var animator = CTX.AvatarRootTransform.GetComponent<Animator>();
                if (animator != null && animator.isHuman)
                {
                    var hips = animator.GetBoneTransform(HumanBodyBones.Hips);
                    armature = hips.parent;
                }
                else
                {
                    for (int i = 0; i < CTX.AvatarRootTransform.childCount; i++)
                    {
                        var t = CTX.AvatarRootTransform.GetChild(i);
                        if (t.name == DefaultArmatureName)
                        {
                            armature = t;
                            break;
                        }
                    }
                }
            }

            if (armature == null)
            {
                Debug.LogError("没有找到角色的Armature,请使用XYArmatureName组件进行指定");
                return;
            }

            var list = armature.GetAllChildTransforms();
            foreach (var t in list)
            {
                ArmatureGameobjectList.Add(t.name);
            }
        }

        /// <summary>
        /// 生成预设
        /// </summary>
        private void GeneratePreset(BuildContext ctx)
        {
            var presetComponents = ctx.AvatarRootTransform.GetComponentsInChildren<XYPreset>(true);
            if (presetComponents != null && presetComponents.Length > 0)
            {
                var layer = FxController.NewLayer("预设");
                var presetIndexParam = layer.IntParameter("预设");
                var maacParameter = MA.NewParameter(presetIndexParam);
                maacParameter.WithDefaultValue(0);
                var noneState = layer.NewState("空");
                layer.AnyTransitionsTo(noneState).When(presetIndexParam.IsEqualTo(0));
                for (int i = 0; i < presetComponents.Length; i++)
                {
                    int index = i;
                    var preset = presetComponents[index];
                    string stateName = $"预设{index + 1}";
                    var state = layer.NewState(stateName);
                    // 先收集此预设用到的所有参数名
                    List<string> presetParamNames = new List<string>();
                    foreach (var ps in preset.BoolPresets)
                    {
                        if (BoolParamDict.TryGetValue(ps.ParamName, out var p))
                        {
                            presetParamNames.Add(ps.ParamName);
                        }
                    }

                    foreach (var ps in preset.IntPresets)
                    {
                        if (IntParamDict.TryGetValue(ps.ParamName, out var p))
                        {
                            presetParamNames.Add(ps.ParamName);
                        }
                    }

                    foreach (var ps in preset.FloatPresets)
                    {
                        if (FloatParamDict.TryGetValue(ps.ParamName, out var p))
                        {
                            presetParamNames.Add(ps.ParamName);
                        }
                    }


                    // 先将所有参数恢复默认
                    foreach (var kv in BoolParamDefaultDict)
                    {
                        if (presetParamNames.Contains(kv.Key)) continue;
                        if (BoolParamDict.TryGetValue(kv.Key, out var p))
                        {
                            state = state.Drives(p, kv.Value);
                        }
                    }

                    foreach (var kv in IntParamDefaultDict)
                    {
                        if (presetParamNames.Contains(kv.Key)) continue;
                        if (IntParamDict.TryGetValue(kv.Key, out var p))
                        {
                            state = state.Drives(p, kv.Value);
                        }
                    }

                    foreach (var kv in FloatParamDefaultDict)
                    {
                        if (presetParamNames.Contains(kv.Key)) continue;
                        if (FloatParamDict.TryGetValue(kv.Key, out var p))
                        {
                            state = state.Drives(p, kv.Value);
                        }
                    }

                    // 再设置预设的值
                    foreach (var ps in preset.BoolPresets)
                    {
                        if (BoolParamDict.TryGetValue(ps.ParamName, out var p))
                        {
                            state = state.Drives(p, ps.Value);
                        }
                    }

                    foreach (var ps in preset.IntPresets)
                    {
                        if (IntParamDict.TryGetValue(ps.ParamName, out var p))
                        {
                            state = state.Drives(p, ps.Value);
                        }
                    }

                    foreach (var ps in preset.FloatPresets)
                    {
                        if (FloatParamDict.TryGetValue(ps.ParamName, out var p))
                        {
                            state = state.Drives(p, ps.Value);
                        }
                    }

                    layer.AnyTransitionsTo(state).When(presetIndexParam.IsEqualTo(index + 1));

                    // 创建菜单
                    if (preset.RegisterMenuData.RegisterMenu)
                    {
                        MA.EditMenuItem(XYMenuManager.SubMenuByPath(preset.RegisterMenuData.MenuPath)
                                .MenuItem(string.IsNullOrWhiteSpace(preset.RegisterMenuData.MenuName)
                                    ? stateName
                                    : preset.RegisterMenuData.MenuName).gameObject)
                            .ButtonSets(presetIndexParam, index + 1)
                            .WithIcon(preset.RegisterMenuData.MenuIcon);
                    }
                }
            }
        }
    }
}