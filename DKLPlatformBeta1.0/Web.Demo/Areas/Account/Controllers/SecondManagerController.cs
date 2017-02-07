using HYZK.Account.Contract;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Common;
using Web.Demo.Common;
namespace Web.Demo.Areas.Account.Controllers
{
    public class SecondManagerController : AdminControllerBase
    {
        // GET: Account/SecondManager
        public ActionResult Index(UserRequest request)
        {
            var result = this.AccountService.GetUserList(request);
            return View(result);
        }
        public ActionResult Edit(int id)
        {
            var model = this.AccountService.GetUser(id);
            
            this.RenderMyViewData(model);
            var users = this.AccountService.GetUserList().Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Name", new SelectList(users, "Name", "Name"));
            ViewData.Add("AnotherAccountTotally", GetAnotherAccountTotally(model.SecondTotally));
            return View(model);
        }
       
        //
        // POST: /Account/User/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.AccountService.GetUser(id);
            if (model.SecondTotally ==null ||model.SecondTotally == "")
            {
                model.SecondTotally += collection["AnotherAccountType"].ToString();
            }
            else
            {
                model.SecondTotally += ","+ collection["AnotherAccountType"].ToString();
            }
           
            this.TryUpdateModel<User>(model);
            this.AccountService.SaveUser(model);

            return this.RefreshParent();
        }
        private void RenderMyViewData(User model)
        {
            ViewData.Add("Gender", new SelectList(EnumHelper.GetItemValueList<EnumGender>(), "Key", "Value", model.Gender));

            ViewData.Add("AnotherAccountType", new SelectList(EnumHelper.GetItemValueList<EnumAccountType>(), "Key", "Value", model.AccountType));

        }
        private string GetAnotherAccountTotally(string total)
        {
            if (total == null ||total == "")
            {
                return "暂无第二职务";
            }
           
            List<string> list = new List<string>(total.Split(','));
            var listInt = list.Select<string, int>(q => Convert.ToInt32(q));
            string finalTotal=null;
            foreach (var item in listInt)
            {
                finalTotal += HYZK.FrameWork.Utility.EnumHelper.GetEnumTitle((EnumAccountType)@item)+" ";
            }
            return finalTotal;
        }
    }
}