using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;
using HYZK.FrameWork.Common;
using Web.Demo.Areas.DKLManager.Models;
using System.Drawing.Imaging;
using System.IO;
using HYZK.Core.Upload;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class FinancialDepartmentController : AdminControllerBase
    {
        //
        // GET: /DKLManager/FinancialDepartment/
        public ActionResult Index(ProjectInfoRequest request)
        {
            var result = this.IDKLManagerService.GetProjectInfoList(request);
            return View(result);
        }
        public ActionResult Create()
        {
            var model = new ProjectWholeInfoViewModel();
            if (model != null)
            {
                this.RenderMyViewData(model);
            }
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new ProjectWholeInfoViewModel();
            try
            {
                UpdateViewModel(collection, ref model);
                this.IDKLManagerService.AddProjectInfo(model.projectBasicinfo);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
            return this.RefreshParent();
        }
        public ActionResult Edit(int id)
        {
            var model = this.IDKLManagerService.GetProjectInfo(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.GetProjectInfo(id);
            try
            {
                this.TryUpdateModel<ProjectInfo>(model);
                this.IDKLManagerService.UpdateProjectInfo(model);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
            return this.RefreshParent();
        }
        public ActionResult Delete(List<int> ids)
        {
            if (ids != null)
            {
                try
                {
                    this.IDKLManagerService.DeleteProjectInfo(ids);
                }
                catch (Exception ex) {
                    return Back(ex.Message);
                }
            }
            return RedirectToAction("Index");
        }
	}
}