using HYZK.FrameWork.Utility;
using HYZK.FrameWork.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Common;
using Web.Demo.Common;

namespace Web.Demo.Areas.Account.Controllers
{
    public class CommonController : AdminControllerBase
    {
        [AuthorizeIgnore]
        public virtual ActionResult VerifyImage()
        {
            var validateCodeType = new ValidateCode_Style1();
            string code = "6666";
            byte[] bytes = validateCodeType.CreateImage(out code);
            this.CookieContext.VerifyCodeGuid = VerifyCodeHelper.SaveVerifyCode(code);

            return File(bytes, @"image/jpeg");
        }



    }
}