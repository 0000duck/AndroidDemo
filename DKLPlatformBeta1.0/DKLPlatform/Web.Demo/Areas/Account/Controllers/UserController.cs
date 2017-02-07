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
    [Permission(EnumBusinessPermission.AccountManage_User)]

    public class UserController : AdminControllerBase
    {
        //
        // GET: /Account/User/
        public ActionResult Index(UserRequest request)
        {
            var result = this.AccountService.GetUserList(request);
            return View(result);
        }

        //
        // GET: /Account/User/Create

        public ActionResult Create()
        {
            var model = new User();
            this.RenderMyViewData(model);
            model.Password = "111111";
            var users = this.AccountService.GetUserList().Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Name", new SelectList(users, "Name", "Name"));
            return View("Edit", model);
        }

        //
        // POST: /Account/User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new User();
            this.TryUpdateModel<User>(model);
            model.Password = "111111";
            //model.Password = Encrypt.MD5(model.Password);

            try
            {
                this.AccountService.SaveUser(model);
            }
            catch (HYZK.FrameWork.Common.BusinessException e)
            {
                return Back(e.Message);
                //this.ModelState.AddModelError(e.Name, e.Message);
                //return View("Edit", model);
            }

            return this.RefreshParent();
        }

        //
        // GET: /Account/User/Edit/5

        public ActionResult Edit(int id)
        {
            var model = this.AccountService.GetUser(id);
            this.RenderMyViewData(model);
            var users = this.AccountService.GetUserList().Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Name", new SelectList(users, "Name", "Name"));
            return View(model);
        }

        //
        // POST: /Account/User/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.AccountService.GetUser(id);
            this.TryUpdateModel<User>(model);
            this.AccountService.SaveUser(model);

            return this.RefreshParent();
        }

        // POST: /Account/User/Delete/5

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.AccountService.DeleteUser(ids);
            return RedirectToAction("Index");
        }

        private void RenderMyViewData(User model)
        {
            ViewData.Add("Gender", new SelectList(EnumHelper.GetItemValueList<EnumGender>(), "Key", "Value", model.Gender));
            ViewData.Add("AccountType", new SelectList(EnumHelper.GetItemValueList<EnumAccountType>(), "Key", "Value", model.AccountType));

        }
    }
}