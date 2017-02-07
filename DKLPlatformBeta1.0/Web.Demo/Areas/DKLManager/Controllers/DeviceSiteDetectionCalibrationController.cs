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
    public class DeviceSiteDetectionCalibrationController : AdminControllerBase
    {
        //
        // GET: /DKLManager/DeviceSiteDetectionCalibration/
        public ActionResult Index(DeviceModelRequest request)
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
        public ActionResult Create(FormCollection coolection)
        {
            var model = new DeviceModel();
            model.BuyTime = DateTime.Now;
            model.CheckTime = DateTime.Now;
            model.CorrectTime = DateTime.Now;
            model.CreateTime = DateTime.Now;
            model.LastCheckTime = DateTime.Now;
            model.LastCorrectTime = DateTime.Now;
            try
            {
                this.TryUpdateModel<DeviceModel>(model);
                this.IDKLManagerService.InsertDevice(model);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
            return this.RefreshParent();
        }
        public ActionResult Edit(int id)
        {
            var model = this.IDKLManagerService.SelectDevice(id);
            //ViewData.Add("States", new SelectList(EnumHelper.GetItemValueList<EnumStates>(), "Key", "Value"));
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.SelectDevice(id);
            try
            {
                this.TryUpdateModel<DeviceModel>(model);
                this.IDKLManagerService.UpDateDevice(model);
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
                    this.IDKLManagerService.DeleteDevice(ids);
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