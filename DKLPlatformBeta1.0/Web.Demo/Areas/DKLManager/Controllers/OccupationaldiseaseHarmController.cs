using DKLManager.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
   
    public class OccupationaldiseaseHarmController : AdminControllerBase
    {
        //
        // GET: /DKLManager/OccupationaldiseaseHarm/

        /// <summary>
        /// 职业病危害
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
            var result = this.IDKLManagerService.GetOccupationaldiseaseHarmList(request);
          
            return View(result);
        }
       
        public ActionResult Create()
        {
            var model = new OccupationaldiseaseHarm();
            var parameters = this.IDKLManagerService.GetParameterListAll().Select(c => new { Name = c.ParameterName }).Distinct();
            ViewData.Add("ParameterName", new SelectList(parameters, "Name", "Name"));
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new OccupationaldiseaseHarm();
            this.TryUpdateModel<OccupationaldiseaseHarm>(model);
            this.IDKLManagerService.InsertOccupationaldisease(model);
            return this.RefreshParent();
        }
        public ActionResult Edit(int id)
        {
            var model = this.IDKLManagerService.SelectOccupationaldisease(id);
            var parameters = this.IDKLManagerService.GetParameterListAll().Select(c => new { Name = c.ParameterName }).Distinct();
            ViewData.Add("ParameterName", new SelectList(parameters, "Name", "Name"));
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.SelectOccupationaldisease(id);
            this.TryUpdateModel<OccupationaldiseaseHarm>(model);
            this.IDKLManagerService.UpDateOccupationaldisease(model);
            return this.RefreshParent();
        }
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            if (ids!= null)
            {
                this.IDKLManagerService.DeleteOccupationaldisease(ids);
            }
            return RedirectToAction("Index");
        }
	}
}