using HYZK.Account.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;

namespace Web.Demo.Areas.Account.Controllers
{
    public class ChangeAccountTypeController : AdminControllerBase
    {
        // GET: Account/ChangeAccountType
        public static Dictionary<string, int> SecondAccount = new Dictionary<string, int>();
        public ActionResult Index()
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            return View(user);
        }
        public ActionResult Edit(int id)
        {
            var model = this.AccountService.GetUser(id);
            if (model.SecondTotally == null || model.SecondTotally == "")
            {
                return RefreshParent("没有找到第二职位信息！");
            }
            List<string> list = new List<string>(model.SecondTotally.Split(','));
            var listInt = list.Select<string, int>(q => Convert.ToInt32(q));
        //    List<string> SecondAccount = new List<string>();
            SecondAccount.Clear();
            foreach (var item in listInt)
            {
                SecondAccount.Add(HYZK.FrameWork.Utility.EnumHelper.GetEnumTitle((EnumAccountType)@item),item);
            }
            List<string> SecondStatus = new List<string>();
            SecondStatus.Add("关闭");
            SecondStatus.Add("开启");          
            ViewData.Add("SecondStatus", new SelectList(SecondStatus));
            ViewData.Add("SecondAccountType", new SelectList(SecondAccount.Keys));
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id ,FormCollection collection)
        {
           
            var model = this.AccountService.GetUser(id);
            if (collection["SecondStatus"] == "关闭")
            {
                model.SecondStatus = false;
                this.AccountService.SaveUser(model);
                return this.RefreshParent();
            }
            else
            {
                model.SecondStatus = true;
                model.SecondAccountType = SecondAccount[collection["SecondAccountType"]];
                this.AccountService.SaveUser(model);
                return this.RefreshParent();
            }
           
        }
    }
}