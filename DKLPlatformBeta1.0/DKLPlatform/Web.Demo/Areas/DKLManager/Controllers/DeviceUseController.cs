using DKLManager.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class DeviceUseController : AdminControllerBase
    {
        //
        // GET: /DKLManager/DeviceCalibrationRemar/
        public ActionResult Index(ProjectInfoRequest request)
        {
            var result = this.IDKLManagerService.GetUserRecordList(request);
            return View(result);
        }
        public ActionResult Create()
        {
            var model = new DeviceUseRecordModel();
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Create(FormCollection coolection)
        {
            var model = new DeviceUseRecordModel();
            model.UserTime = DateTime.Now;
            this.TryUpdateModel<DeviceUseRecordModel>(model);
            this.IDKLManagerService.InsertUserRecord(model);
            return this.RefreshParent();
        }
        public ActionResult Edit(int id)
        {
            var model = this.IDKLManagerService.SelectUserRecord(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.SelectUserRecord(id);
            this.TryUpdateModel<DeviceUseRecordModel>(model);
            this.IDKLManagerService.UpdateUserRecord(model);
            return this.RefreshParent();
        }
        public ActionResult Delete(List<int> id)
        {
            if (id != null)
            {
                this.IDKLManagerService.DeleteUserRecord(id);
            }
            return RefreshParent("Index");
        }
    }
}
