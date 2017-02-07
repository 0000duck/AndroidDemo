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

        public ActionResult Index(DeviceModelRequest request)
        {
            var result = this.IDKLManagerService.GetDeviceLists(request);
            if (result != null)
            {
                foreach (var i in result)
                {          
                    if (i.CalibrationTime <=DateTime.Now&&i.CheckState == 6)
                    {
                        i.CheckState = (int)EnumCheckState.Services;
                        this.IDKLManagerService.UpDateDevice(i);
                        //return View(result);
                    }
                    if (i.CalibrationTime > DateTime.Now && i.CheckState == 6)
                    {
                        i.CheckState = (int)EnumCheckState.Services;
                        this.IDKLManagerService.UpDateDevice(i);
                       // return View(result);
                    }
                    if (i.CalibrationTime <= DateTime.Now && i.CheckState == 0)
                    {
                        i.CheckState = (int)EnumCheckState.WaitCheck;
                        this.IDKLManagerService.UpDateDevice(i);
                       // return View(result);
                    
                    }
                    if (i.CalibrationTime > DateTime.Now && i.CheckState == 0)
                    {
                        i.CheckState = (int)EnumCheckState.Normal;
                        this.IDKLManagerService.UpDateDevice(i);
                        //return View(result);
                    }
                    //if (i.CheckState == 5)
                    //{
                    //    return View(result);
                    //}
                    if (i.CalibrationTime > DateTime.Now && i.CheckState == 5)
                    {

                        i.CheckState = (int)EnumCheckState.Service;
                        this.IDKLManagerService.UpDateDevice(i);
                        //return View(result);
                    }
                    if (i.CalibrationTime <= DateTime.Now && i.CheckState == 5)
                    {

                        i.CheckState = (int)EnumCheckState.Service;
                        this.IDKLManagerService.UpDateDevice(i);
                        //return View(result);
                    }
                    if (i.CalibrationTime <= DateTime.Now && i.CheckState == 7)
                    {

                        i.CheckState = (int)EnumCheckState.Servicess;
                        this.IDKLManagerService.UpDateDevice(i);
                       // return View(result);
                    }
                    if (i.CalibrationTime > DateTime.Now && i.CheckState == 7)
                    {

                        i.CheckState = (int)EnumCheckState.Servicess;
                        this.IDKLManagerService.UpDateDevice(i);
                       // return View(result);
                    }
                    if (i.CalibrationTime >= DateTime.Now && i.CheckState == 8)
                    {

                        i.CheckState = (int)EnumCheckState.StopUse;
                        this.IDKLManagerService.UpDateDevice(i);
                        //return View(result);
                    }
                    if (i.CalibrationTime > DateTime.Now && i.CheckState == 8)
                    {

                        i.CheckState = (int)EnumCheckState.StopUse;
                        this.IDKLManagerService.UpDateDevice(i);
                        //return View(result);
                    }
                }
            }
            try
            {
                this.IDKLManagerService.CheckDevice();
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
            //var result = this.IDKLManagerService.GetDeviceLists(request);
            return View(result);
        }
        public ActionResult Create()
        {
            var model = new DeviceModel();
            model.BuyTime = Convert.ToDateTime(DateTime.Now.ToLongDateString());
            model.CalibrationTime = Convert.ToDateTime(DateTime.Now.ToLongDateString());
            ViewData.Add("DeviceMold", new SelectList(EnumHelper.GetItemValueList<EnumDeviceMold>(), "Key", "Value"));
            ViewData.Add("CheckState", new SelectList(EnumHelper.GetItemValueList<EnumCheckState>(), "Key", "Value"));
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {        
            var model = new DeviceModel();
            UpdateViewModels(collection, ref model);
            model.CheckState = (int)EnumCheckState.Normal;          
            model.State = "";
            model.Remark = "";
            model.CheckTime = DateTime.Now;
            model.CorrectTime = DateTime.Now;
            model.CreateTime = DateTime.Now;
            model.LastCheckTime = DateTime.Now;
            model.LastCorrectTime = DateTime.Now;
            model.DeviceIdentityTime = DateTime.Now;
            model.DeviceServiceTime = DateTime.Now;
            var mo = this.IDKLManagerService.GetDeviceListByNumber(model.Number);
            if (mo != null)
            {
                foreach (var n in mo)
                {
                    if (model.Number.Contains(n.Number))
                    {
                        return Back("已经有相同的设备编号");
                    }
                }
            }
            try
            {
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
                try
                {
                    this.TryUpdateModel<DeviceModel>(model);
                    this.IDKLManagerService.UpDateDevice(model);
                }
                catch (Exception ex)
                {
                    return Back(ex.Message);
                }
            }
            return this.RefreshParent();
        }
        public ActionResult Submit(int id)
        {
            var model = this.IDKLManagerService.SelectDevice(id);
            ViewData.Add("DeviceMold", new SelectList(EnumHelper.GetItemValueList<EnumDeviceMold>(), "Key", "Value"));
            ViewData.Add("CheckState", new SelectList(EnumHelper.GetItemValueList<EnumCheckState>(), "Key", "Value"));
            return View("Edit",model);
        }
        [HttpPost]
        public ActionResult Submit(int id, FormCollection collection)
        {
            var model = new DeviceModel();
            model = this.IDKLManagerService.SelectDevice(id);
            if (model != null)
            {
                //model.DeviceMold = (int)EnumDeviceMold.SceneUseDetection;
                try
                {
                    this.IDKLManagerService.UpDateDevice(model);
                }
                catch (Exception ex)
                {
                    return Back(ex.Message);
                }
            }
            return this.RefreshParent();

        }
        [HttpPost]
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