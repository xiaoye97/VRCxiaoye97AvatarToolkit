using System;
using System.Collections.Generic;
using System.Linq;
using nadena.dev.modular_avatar.core;
using UnityEngine;

namespace xiaoye97.AvatarToolkit
{
    public static class XYTool
    {
        /// <summary>
        /// 获取所有子物体
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="includeSelf">是否包含自身</param>
        /// <param name="ignoreComponents">忽略的组件,有此组件的子物体及子物体的子物体会被忽略</param>
        /// <returns></returns>
        public static GameObject[] GetAllChildGameObjects(this GameObject gameObject, bool includeSelf = true,
            List<Type> ignoreComponents = null)
        {
            var ts = GetAllChildTransforms(gameObject.transform, includeSelf, ignoreComponents);
            List<GameObject> list = new List<GameObject>();
            foreach (var t in ts)
            {
                list.Add(t.gameObject);
            }

            return list.ToArray();
        }

        /// <summary>
        /// 获取所有子物体
        /// </summary>
        /// <param name="t"></param>
        /// <param name="includeSelf">是否包含自身</param>
        /// <param name="ignoreComponents">忽略的组件,有此组件的子物体及子物体的子物体会被忽略</param>
        /// <returns></returns>
        public static Transform[] GetAllChildTransforms(this Transform t, bool includeSelf = true,
            List<Type> ignoreComponents = null)
        {
            List<Transform> tList = new List<Transform>();
            if (includeSelf)
            {
                tList.Add(t);
            }

            int len = t.childCount;
            for (int i = 0; i < len; i++)
            {
                Transform child = t.GetChild(i);
                if (ignoreComponents != null && ignoreComponents.Count > 0)
                {
                    bool needIgnore = false;
                    foreach (var type in ignoreComponents)
                    {
                        if (child.GetComponent(type) != null)
                        {
                            needIgnore = true;
                            break;
                        }
                    }

                    if (needIgnore)
                    {
                        continue;
                    }
                }

                tList.Add(child);
                if (child.childCount > 0)
                {
                    tList.AddRange(GetAllChildTransforms(child, false));
                }
            }

            return tList.ToArray();
        }

        public static GameObject[] RemoveGameObjectItemByName(GameObject[] gameObjects, List<string> names)
        {
            var list = gameObjects.ToList();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var go = list[i];
                if (names.Contains(go.name))
                {
                    list.RemoveAt(i);
                }
                // 修复根骨骼与角色根骨骼名字不一样导致的角色根骨骼错误隐藏的问题
                if (go.GetComponent<ModularAvatarMergeArmature>())
                {
                    list.RemoveAt(i);
                }
            }

            return list.ToArray();
        }
    }
}