using DKLManager.Contract.Model;
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
        public ActionResult Index(ProjectInfoRequest request)
        {
            var result = this.IDKLManagerService.GetSeviceRemarkList(request);
            return View(result);
        }
        public ActionResult Create()
        {
            var model = new DeviceServiceRemarkModel();
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new DeviceServiceRemarkModel();
            model.DeviceServiceTime = DateTime.Now;
            this.TryUpdateModel<DeviceServiceRemarkModel>(model);
            this.IDKLManagerService.InsertServiceRemark(model);
            return this.RefreshParent();
        }
        public ActionResult Edit(int id)
        {
            var model = this.IDKLManagerService.SelectServiceRemark(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.SelectServiceRemark(id);
            this.TryUpdateModel<DeviceServiceRemarkModel>(model);
            this.IDKLManagerService.UpdateServiceRemark(model);
            return this.RefreshParent();
        }
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            if (ids!= null)
            {
                this.IDKLManagerService.DeleteServiceRemark(ids);
            }
            return RedirectToAction("Index"); ;
        }
    }
}