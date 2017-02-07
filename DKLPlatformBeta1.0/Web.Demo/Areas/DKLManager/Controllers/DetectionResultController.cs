using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class DetectionResultController : AdminControllerBase
    {
        //
        // GET: /DKLManager/DetectionResult/
        public ActionResult Index(ProjectInfoRequest request)
        {
            var result = this.IDKLManagerService.GetDetectionResultList(request);
            return View(result);
        }
        public ActionResult Create()
        {
            var model = new DetectionResult();
            ViewData.Add("Physicochemical", new SelectList(EnumHelper.GetItemValueList<EnumPhysicochemical>(), "Key", "Value"));
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            
            var model = new DetectionResult();
            try
            {
                this.TryUpdateModel<DetectionResult>(model);
                this.IDKLManagerService.InsertDetectionResult(model);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
            return this.RefreshParent();
        }
        public ActionResult Edit(int id)
        {
            var model = this.IDKLManagerService.SelectDetectionResult(id);
            
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.SelectDetectionResult(id);
            if (model != null)
            {
                try
                {
                    this.TryUpdateModel<DetectionResult>(model);
                    this.IDKLManagerService.UpDateDetectionResult(model);
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
                    this.IDKLManagerService.DeleteDetectionResult(ids);
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