using DKLManager.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class DeviceCalibrationRemarkController : AdminControllerBase
    {
        //
        /// <summary>
        /// 设备校准表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
            var result = this.IDKLManagerService.GetCalibretionRemarkList(request);
            return View(result);
        }
        public ActionResult Create()
        {
            var model = new DeviceCalibrationRemarkModel();
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Create(FormCollection coolection)
        {
            var model = new DeviceCalibrationRemarkModel();
            model.CalibrationTime = DateTime.Now;
            try
            {
                this.TryUpdateModel<DeviceCalibrationRemarkModel>(model);
                this.IDKLManagerService.InsertCalibrationRemark(model);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
            return this.RefreshParent();
        }
        public ActionResult Edit(int id)
        {
            var model = this.IDKLManagerService.SelectCalibrationRemark(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.SelectCalibrationRemark(id);
            if (model != null)
            {
                try
                {
                    this.TryUpdateModel<DeviceCalibrationRemarkModel>(model);
                    this.IDKLManagerService.UpdateCalibrationRemark(model);
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
                    this.IDKLManagerService.DelectCalibrationRemark(ids);
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