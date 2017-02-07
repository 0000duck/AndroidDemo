using DKLManager.Contract.Model;
using HYZK.Account.Contract;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ParameterController : AdminControllerBase
    {
        //
        // GET: /DKLManager/Parameterontroller/
        /// <summary>
        /// 添加参数表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
            var result = this.IDKLManagerService.GetParameterList(request);
            return View(result);
        }
        public ActionResult Create()
        {
            var model = new Parameter();
            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Parameter();
            this.TryUpdateModel<Parameter>(model);
            this.IDKLManagerService.InsertParameter(model);
            return this.RefreshParent();
        }

        public ActionResult Edit(int id)
        {
            var model = this.IDKLManagerService.SelectParameter(id);
            return View(model);
        }

        //
        // POST: /Account/User/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.SelectParameter(id);
            this.TryUpdateModel<Parameter>(model);
            this.IDKLManagerService.UpDateParameter(model);
            return this.RefreshParent();
        }
        [HttpPost]
        public ActionResult Delete(List<int> id)
        {
            this.IDKLManagerService.DeleteParameter(id);
            return RedirectToAction("Index");

        }
	}
}