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

    public class DeviseController : AdminControllerBase
    {

        public ActionResult Index(ProjectInfoRequest request)
        {
            var result = this.IDKLManagerService.GetDeviceList(request);
            return View(result);
        }
        public ActionResult Create()
        {
            var model = new DeviceModel();
            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new DeviceModel();
            this.TryUpdateModel<DeviceModel>(model);
            model.BuyTime = DateTime.Now;
            model.State = "";
            model.Remark = "";
            model.CheckTime = DateTime.Now;
            model.CorrectTime = DateTime.Now;
            model.CreateTime = DateTime.Now;
            model.LastCheckTime = DateTime.Now;
            model.LastCorrectTime = DateTime.Now;
            this.IDKLManagerService.InsertDevice(model);
            return this.RefreshParent();
        }

        public ActionResult Edit(int id)
        {
            var model = this.IDKLManagerService.SelectDevice(id);
            return View(model);
        }

        //
        // POST: /Account/User/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.SelectDevice(id);
            this.TryUpdateModel<DeviceModel>(model);
            this.IDKLManagerService.UpDateDevice(model);
            return this.RefreshParent();
        }
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            if (ids != null)
            {
                this.IDKLManagerService.DeleteDevice(ids);
            }
            return RedirectToAction("Index");

        }
    }
}