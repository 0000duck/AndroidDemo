using DKLManager.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class DeviceIdentityController : AdminControllerBase
    {
        //
        // GET: /DKLManager/DeviceCalibrationRemar/
        /// <summary>
        /// 鉴定列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
            var result = this.IDKLManagerService.GetIdentityRemarkList(request);
            return View(result);
        }
        public ActionResult Create()
        {
            var model = new DeviceIdentityRemarkModel();
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Create(FormCollection coolection)
        {
            var model = new DeviceIdentityRemarkModel();
            model.DeviceIdentityTime = DateTime.Now;
            this.TryUpdateModel<DeviceIdentityRemarkModel>(model);
            this.IDKLManagerService.InsertIdentityRemark(model);
            return this.RefreshParent();
        }
        public ActionResult Edit(int id)
        {
            var model = this.IDKLManagerService.SelectIdentityRemark(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.SelectIdentityRemark(id);
            this.TryUpdateModel<DeviceIdentityRemarkModel>(model);
            this.IDKLManagerService.UpdateIdentityRemark(model);
            return this.RefreshParent();
        }
        public ActionResult Delete(List<int> ids)
        {
            if (ids != null)
            {
                this.IDKLManagerService.DeleteIdentityRemark(ids);
            }
            return RedirectToAction("Index");
        }
    }
}
