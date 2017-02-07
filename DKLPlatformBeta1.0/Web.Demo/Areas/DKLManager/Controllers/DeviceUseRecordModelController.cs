using DKLManager.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class DeviceUseRecordModelController : AdminControllerBase
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
            try
            {
                this.TryUpdateModel<DeviceUseRecordModel>(model);
                this.IDKLManagerService.InsertUserRecord(model);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
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
            if (model != null)
            {
                try
                {
                    this.TryUpdateModel<DeviceUseRecordModel>(model);
                    this.IDKLManagerService.UpdateUserRecord(model);
                }
                catch (Exception ex)
                {
                    return Back(ex.Message);
                }
            }
            return this.RefreshParent();
        }
        public ActionResult Delete(List<int> ids)
        {
            if (ids!= null)
            {
                try
                {
                    this.IDKLManagerService.DeleteUserRecord(ids);
                }
                catch (Exception ex)
                {
                    return Back(ex.Message);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
