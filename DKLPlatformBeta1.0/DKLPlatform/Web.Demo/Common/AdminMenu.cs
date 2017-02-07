using HYZK.Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Demo.Common
{
    public class AdminMenu
    {
        private static AdminMenuGroup menu;

        public static AdminMenuGroup CurrentMenu
        {
            get
            {
                return (menu == null) ? CachedConfigContext.Current.AdminMenuConfig.AdminMenuGroups[0] : menu;
            }
            set
            {
                menu = value;
            }
        }
    }
}