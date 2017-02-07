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
    public class DeviceServiceController : AdminControllerBase
    {
        //
        // GET: /DKLManager/DeviceService/
        /// <summary>
        /// 设备维修
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(DeviceModelRequest request)
        {
           var result = this.IDKLManagerService.GetDeviceList(request);
           return View(result);
        }
        public ActionResult Create()
        {
            var model = new DeviceModel();
            ViewData.Add("DeviceMold", new SelectList(EnumHelper.GetItemValueList<EnumDeviceMold>(), "Key", "Value"));
            ViewData.Add("CheckState", new SelectList(EnumHelper.GetItemValueList<EnumCheckState>(), "Key", "Value"));
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new DeviceModel();
            try
            {
                UpdateViewModels(collection, ref model);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
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
        public ActionResult Submit(int id)
        {
            var model = this.IDKLManagerService.SelectDevice(id);
            ViewData.Add("DeviceMold", new SelectList(EnumHelper.GetItemValueList<EnumDeviceMold>(), "Key", "Value"));
            ViewData.Add("CheckState", new SelectList(EnumHelper.GetItemValueList<EnumCheckState>(), "Key", "Value"));
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Submit(int id, FormCollection collection)
        {
            var model = new DeviceModel();
            model.ProjectProblemDescribe = "";
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