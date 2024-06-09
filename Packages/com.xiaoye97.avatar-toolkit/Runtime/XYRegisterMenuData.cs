using System;
using UnityEngine;

namespace xiaoye97.AvatarToolkit
{
    [Serializable]
    public class XYRegisterMenuData
    {
        public bool RegisterMenu;
        public string MenuPath;
        public string MenuName;
        public Texture2D MenuIcon;

        public XYRegisterMenuData()
        {
        }

        public XYRegisterMenuData(bool RegisterMenu, string MenuPath, string MenuName, Texture2D MenuIcon)
        {
            this.RegisterMenu = RegisterMenu;
            this.MenuPath = MenuPath;
            this.MenuName = MenuName;
            this.MenuIcon = MenuIcon;
        }
    }
}