using HYZK.Core.Config;
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
    public class AuthController : AdminControllerBase
    {
        [AuthorizeIgnore]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AuthorizeIgnore]
        public ActionResult Login(Web.Demo.Areas.Account.Models.LoginViewModel model)
        {
            if (!VerifyCodeHelper.CheckVerifyCode(model.Verifycode, this.CookieContext.VerifyCodeGuid))
            {
                ModelState.AddModelError("error", "验证码错误");
                return View();
            }

            var loginInfo = this.AccountService.Login(model.UserName, model.Password);

            if (loginInfo != null)
            {
                this.CookieContext.UserToken = loginInfo.LoginToken;
                this.CookieContext.UserName = loginInfo.LoginName;
                this.CookieContext.UserId = loginInfo.UserID;
                var user = this.AccountService.GetUser(model.UserName);

                Web.Demo.Common.AdminMenu.CurrentMenu = CachedConfigContext.Current.AdminMenuConfig.AdminMenuGroups[user.AccountType];
                //根据用户类型不一样，返回不同的界面
                return RedirectToPageByAccountType(user.AccountType);
                //return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("error", "用户名或密码错误");
                return View();
            }
        }

        public ActionResult RedirectToPageByAccountType(int accountType)
        {
            switch (accountType) 
            {
                case 0:
                    return RedirectToAction("Index", "ProjectDoing", new { Area = "DKLManager" });
                    //return RedirectToAction("Index");
                case 1:
                    return RedirectToAction("Index", "ProjectMaket", new { Area = "DKLManager" });
                case 2:
                    return RedirectToAction("Index", "ProjectConsult", new { Area = "DKLManager" });
                case 3:
                    return RedirectToAction("Index", "ProjectConsultPerson", new { Area = "DKLManager" });
                case 4:
                    return RedirectToAction("Index", "ProjectQualityControl", new { Area = "DKLManager" });
                case 5:
                    return RedirectToAction("Index", "ProjectCharge", new { Area = "DKLManager" });
                case 6:
                    return RedirectToAction("Index", "ProjectLeader", new { Area = "DKLManager" });
                case 7:
                    return RedirectToAction("Index", "ProjectGeneral", new { Area = "DKLManager" });
                case 8:
                    return RedirectToAction("Index", "Devise", new { Area = "DKLManager" });
                case 9:
                    return RedirectToAction("Index", "SampleRegisterTable", new { Area = "DKLManager" });
                case 10:
                    return RedirectToAction("Index", "AnalyzePerson", new { Area = "DKLManager" });
                case 11:
                    return RedirectToAction("Index", "TestReport", new { Area = "DKLManager" });
                default:
                    return RedirectToAction("Login", "Auth");
            }
        }

        public ActionResult Logout()
        {
            this.AccountService.Logout(this.CookieContext.UserToken);
            this.CookieContext.UserToken = Guid.Empty;
            this.CookieContext.UserName = string.Empty;
            this.CookieContext.UserId = 0;
            return RedirectToAction("Login");
        }

        public ActionResult ModifyPwd()
        {
            var model = this.AccountService.GetUser(this.LoginInfo.UserID);
            return View(model);
        }

        [HttpPost]
        public ActionResult ModifyPwd(FormCollection collection)
        {
            var model = this.AccountService.GetUser(this.LoginInfo.UserID);
            this.TryUpdateModel<HYZK.Account.Contract.User>(model);

            try
            {
                this.AccountService.ModifyPwd(model);
            }
            catch (HYZK.FrameWork.Common.BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                return View(model);
            }

            return this.RefreshParent();
        }
        //
        // GET: /Account/Auth/
        public ActionResult Index()
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            Web.Demo.Common.AdminMenu.CurrentMenu = CachedConfigContext.Current.AdminMenuConfig.AdminMenuGroups[user.AccountType];
            //根据用户类型不一样，返回不同的界面
            if (user.AccountType == 0)
            {
                return RedirectToAction("Index", "ProjectDoing", new { Area = "DKLManager" });
                //return View();
            }
            else
            {
                return RedirectToPageByAccountType(user.AccountType);
            }
            //return View();
        }
	}
}