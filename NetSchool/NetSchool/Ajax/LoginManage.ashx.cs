using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace NetSchool.Ajax
{
    public class LoginManage : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string returnstring;
            try
            {
                returnstring = SwitchCmd(context.Request);

            }
            catch (Exception ex)
            {
                returnstring = string.Format(@"{{""state"":""error"",""msg"":""发生错误"",""catch1"":""{0}"",""catch2"":""{1}""}}", HttpUtility.JavaScriptStringEncode(ex.Message), HttpUtility.JavaScriptStringEncode(ex.StackTrace));
            }

            context.Response.Clear();
            context.Response.AddHeader("Pragma", "no-cache");
            context.Response.ContentType = "text/json";
            context.Response.Write(returnstring);
            context.Response.End();
        }

        protected virtual string SwitchCmd(HttpRequest Request)
        {
            switch (NetSchool.Common.Library.GetPostBack.GetPostBackStr("cmd"))
            {
                case "login_in":
                    return LoginIn(Request);
                case "login_out":
                    return LoginOut();
                default:
                    return @"{""state"":""error"",""msg"":""未知操作""}";
            }
        }

        private static string LoginIn(HttpRequest Request)
        {
            StringBuilder strJson = new StringBuilder();
            strJson.Append("{");

            string strDogError = string.Empty;
            string strUsername = Request["user"];
            string strPassword = Request["pwd"];
            string strRes = BLL.UserLogin.LoginIn(strUsername, strPassword);
            strJson.Append(string.Format("\"msg\":\"{0}\"", strRes));
            if (strRes == "ok")
            {
                strJson.Append(string.Format(",\"user\":\"{0}\"", NetSchool.Common.Info.CurUserInfo.RealName));
                strJson.Append(string.Format(",\"loginTime\":\"{0}\"", NetSchool.Common.Info.CurUserInfo.LoginTime.Value.ToShortTimeString()));
            }
            strJson.Append("}");

            return strJson.ToString();
        }
        private static string LoginOut()
        {
            return "{\"msg\":\"" + (BLL.UserLogin.LoginOut() ? "ok" : "error") + "\"}";
        }
        protected virtual bool NeedLogin
        {
            get
            {
                return true;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}