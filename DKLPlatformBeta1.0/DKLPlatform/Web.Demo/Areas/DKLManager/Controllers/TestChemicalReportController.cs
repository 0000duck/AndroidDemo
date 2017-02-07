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
    public class TestChemicalReportController : AdminControllerBase
    {
        //
        // GET: /DKLManager/TestChemicalReport/
        /// <summary>
        /// 物理报告测试
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            request.ProjectStatus = (int)EnumProjectSatus.Begin;
            var result = this.IDKLManagerService.GetTestTestChemicalReportList(request);
            return View(result);
        }
        /// <summary>
        /// 添加新增
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var model = new TestChemicalReport();
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new TestChemicalReport();
            this.TryUpdateModel<TestChemicalReport>(model);
            this.IDKLManagerService.InsertTestChemicalReport(model);
            return RefreshParent();
        }
        public ActionResult Edit(int id)
        {
            var model = this.IDKLManagerService.SelectTestChemicalReport(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.SelectTestChemicalReport(id);
            this.TryUpdateModel<TestChemicalReport>(model);
            this.IDKLManagerService.UpdateTestChemicalReport(model);
            return RefreshParent();

        }
        public ActionResult Delete(List<int> ids)
        {
            if (ids == null)
                return RedirectToAction("Index");
            this.IDKLManagerService.DeleteTestChemicalReport(ids);
            return RedirectToAction("Index");
        }
	}
}