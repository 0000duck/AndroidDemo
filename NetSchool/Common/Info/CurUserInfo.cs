using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace NetSchool.Common.Info
{
    public static class CurUserInfo//currentlyUserInfo:当前用户信息
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public static string Username
        {
            get { return HttpContext.Current.Session["Username"] as string; }
        }
        public static string RealName
        {
            get { return HttpContext.Current.Session["Realname"] as string; }
        }
        public static DateTime? LoginTime
        {
            get { return HttpContext.Current.Session["LoginTime"] as DateTime?; }
        }
        public static string Permissions
        {
            get { return HttpContext.Current.Session["Permissions"] as string; }
        }
        public static string IP
        {
            get { return HttpContext.Current.Request.UserHostAddress; }
        }
    }
}
