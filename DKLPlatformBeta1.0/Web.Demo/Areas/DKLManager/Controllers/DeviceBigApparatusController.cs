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
    public class DeviceBigApparatusController : AdminControllerBase
    {
        //
        // GET: /DKLManager/DeviceBigApparatus/
        public ActionResult Index(DeviceModelRequest request)
        {
            //this.IDKLManagerService.CheckDevice();
            //request.CheckState = (int)EnumCheckState.WaitcheckNormal;
            //request.DeviceMold = (int)EnumDeviceMold.BigDevice;
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
            ViewData.Add("DeviceMold", new SelectList(EnumHelper.GetItemValueList<EnumDeviceMold>(), "Key", "Value"));
            ViewData.Add("CheckState", new SelectList(EnumHelper.GetItemValueList<EnumCheckState>(), "Key", "Value"));
            return View(model);
        }

        //
        // POST: /Account/User/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.SelectDevice(id);
            ViewData.Add("CheckState", new SelectList(EnumHelper.GetItemValueList<EnumCheckState>(), "Key", "Value"));
            if (model != null)
            {
               try{
                this.TryUpdateModel<DeviceModel>(model);
                this.IDKLManagerService.UpDateDevice(model);}
               catch (Exception ex)
               {
                   return Back(ex.Message);
               }
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