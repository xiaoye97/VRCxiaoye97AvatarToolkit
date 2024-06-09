using nadena.dev.modular_avatar.core;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;
using VRC.SDKBase;

namespace xiaoye97.AvatarToolkit
{
    /// <summary>
    /// 菜单管理器,使用前,先右键角色 MA->Extract menu,然后把XYMenuManager放到新生成的物体上
    /// </summary>
    public class XYMenuManager : MonoBehaviour, IEditorOnly
    {
        public XYMenuNode SubMenu(string menuName)
        {
            return new XYMenuNode(gameObject).SubMenu(menuName);
        }

        public XYMenuNode SubMenuByPath(string path)
        {
            return new XYMenuNode(gameObject).SubMenuByPath(path);
        }
    }

    public class XYMenuNode
    {
        public GameObject gameObject;

        public XYMenuNode()
        {
        }

        public XYMenuNode(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        /// <summary>
        /// 通过解析路径获取对应子菜单
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public XYMenuNode SubMenuByPath(string path)
        {
            var menuNames = path.Split('/');
            XYMenuNode node = null;
            for (int i = 0; i < menuNames.Length; i++)
            {
                if (i == 0)
                {
                    node = SubMenu(menuNames[0]);
                }
                else
                {
                    node = node.SubMenu(menuNames[i]);
                }
            }

            return node;
        }

        public XYMenuNode SubMenu(string menuName)
        {
            var t = gameObject.transform.Find(menuName);
            if (t == null)
            {
                var go = new GameObject(menuName);
                go.transform.SetParent(gameObject.transform);
                var menuItem = go.GetOrAddComponent<ModularAvatarMenuItem>();
                menuItem.Control = new VRCExpressionsMenu.Control()
                {
                    name = menuName,
                    type = VRCExpressionsMenu.Control.ControlType.SubMenu
                };
                menuItem.MenuSource = SubmenuSource.Children;
                return new XYMenuNode(go);
            }
            else
            {
                return new XYMenuNode(t.gameObject);
            }
        }

        public XYMenuNode MenuItem(string itemName)
        {
            var t = gameObject.transform.Find(itemName);
            if (t == null)
            {
                var go = new GameObject(itemName);
                go.transform.SetParent(gameObject.transform);
                return new XYMenuNode(go);
            }
            else
            {
                return new XYMenuNode(t.gameObject);
            }
        }
    }
}