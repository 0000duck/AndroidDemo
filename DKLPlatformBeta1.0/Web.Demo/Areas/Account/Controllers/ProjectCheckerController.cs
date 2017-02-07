using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;

namespace Web.Demo.Areas.Account.Controllers
{
    public class ProjectCheckerController : AdminControllerBase
    {
        public ActionResult Index()
        {
            var result = this.IDKLManagerService.GetProjectCheckerList();

            return View(result);
        }


        public ActionResult Create()
        {
            var model = new ProjectChecker();
            this.RenderMyViewData(model);
            var users = this.AccountService.GetUserList().Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Name", new SelectList(users, "Name", "Name"));

            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new ProjectChecker();
            this.TryUpdateModel<ProjectChecker>(model);

            try
            {
                this.IDKLManagerService.AddProjectChecker(model);
            }
            catch (HYZK.FrameWork.Common.BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                return View("Edit", model);
            }

            return this.RefreshParent();
        }

        public ActionResult Edit(int id)
        {
            var model = this.IDKLManagerService.GetProjectChecker(id);
            var users = this.AccountService.GetUserList().Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Name", new SelectList(users, "Name", "Name"));
            this.RenderMyViewData(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.GetProjectChecker(id);
            this.TryUpdateModel<ProjectChecker>(model);
            this.IDKLManagerService.UpdateProjectChecker(model);

            return this.RefreshParent();
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.IDKLManagerService.DeleteProjectChecker(ids);
            return RedirectToAction("Index");
        }

        private void RenderMyViewData(ProjectChecker model)
        {
            ViewData.Add("CheckLevel", new SelectList(EnumHelper.GetItemValueList<EnumCheckLevel>(), "Key", "Value", model.CheckLevel));
            ViewData.Add("ProjectCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectCategory>(), "Key", "Value", model.ProjectCategory));
         
        }
	}
}