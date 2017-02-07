using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetSchool
{
    public class UserAttribute : ActionFilterAttribute
    {
        /// 验证权限（action执行前会先执行这里） 
        /// </summary> 
        /// <param name="filterContext"></param> 
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var Session = HttpContext.Current.Session;
            //如果存在身份信息 
            if (string.IsNullOrEmpty(Common.Info.CurUserInfo.Username))
            {
                ContentResult Content = new ContentResult();
                Content.Content = string.Format("<script type='text/javascript'>alert('请先登录！');window.location.href='{0}';</script>", "/");
                filterContext.Result = Content;

            }
        }
    }
}