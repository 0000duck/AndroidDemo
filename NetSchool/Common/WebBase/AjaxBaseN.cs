using System;
using System.Web;
using System.Web.SessionState;

namespace NetSchool.Common.WebBase
{
    public abstract class AjaxBaseN : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string returnstring;
            try
            {
                    returnstring = SwitchCmdN(context.Request);

            }
            catch(Exception ex)
            {
                returnstring = string.Format(@"{{""state"":""error"",""msg"":""发生错误"",""catch1"":""{0}"",""catch2"":""{1}""}}", HttpUtility.JavaScriptStringEncode(ex.Message), HttpUtility.JavaScriptStringEncode(ex.StackTrace));
            }

            context.Response.Clear();
            context.Response.AddHeader("Pragma", "no-cache");
            context.Response.ContentType = "text/json";
            context.Response.Write(returnstring);
            context.Response.End();
        }

        protected virtual string SwitchCmdN(HttpRequest Request)
        {
            return @"{""state"":""error"",""msg"":""未知操作""}";
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