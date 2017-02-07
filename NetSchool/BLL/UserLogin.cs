using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace NetSchool.BLL
{
    public static class UserLogin
    {
        public static string LoginIn(string Username, string Password)
        {
            Model.People user;
            string result = UserCheck(Username, Password, out user);
            if (result == "ok")
            {
                AddLoginState(user);
            }
            return result;
        }
        public static bool LoginOut()
        {
            var Session = HttpContext.Current.Session;
            Session.Clear();
            Session.Abandon();
            return true;
        }
        private static string UserCheck(string username, string password, out Model.People user)
        {
            user = null;
            if (string.IsNullOrWhiteSpace(username))
            {
                return "用户名不能为空";
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                return "密码不能为空";
            }
            user = BLL.People.GetModel(username);
            if (user == null)
            {
                return "用户不存在！";
            }
            if (user.Password != Common.EnDeCryption.Base64.Encode(Common.EnDeCryption.JCode.Encode(password)))
            {
                return "用户密码错误！";
            }
            return "ok";
        }
        private static void AddLoginState(Model.People user)
        {
            var Session = HttpContext.Current.Session;
            Session.Timeout = 1441;
            Session["Username"] = user.UserName;
            Session["Realname"] = user.RealName;
            Session["Permissions"] = user.Permission;
            Session["LoginTime"] = DateTime.Now;
        }
    }
}