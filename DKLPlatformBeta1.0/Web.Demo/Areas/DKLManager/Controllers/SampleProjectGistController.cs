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
    public class SampleProjectGistController : AdminControllerBase
    {
        //
        // GET: /DKLManager/SampleProjectGist/
        //采样项目、采样依据

        public ActionResult Index(ProjectInfoRequest request)
        {
            var result = this.IDKLManagerService.GetSampleProjectGistList(request);
            return View(result);
        }
        public ActionResult Create()
        {
            var model = new SampleProjectGist();
            var parameters = this.IDKLManagerService.GetParameterList().Select(c => new { Name = c.ParameterName }).Distinct();
            ViewData.Add("ParameterName", new SelectList(parameters, "Name", "Name"));
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new SampleProjectGist();
            this.TryUpdateModel<SampleProjectGist>(model);
            this.IDKLManagerService.InsertSampleProject(model);
            return this.RefreshParent();
        }
        public ActionResult Edit(int id)
        {
            var model = this.IDKLManagerService.SelectSampleProject(id);
            var parameters = this.IDKLManagerService.GetParameterList().Select(c => new { Name = c.ParameterName }).Distinct();
            ViewData.Add("ParameterName", new SelectList(parameters, "Name", "Name"));
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.SelectSampleProject(id);
            this.TryUpdateModel<SampleProjectGist>(model);
            this.IDKLManagerService.UpDateSampleProject(model);
            return this.RefreshParent();
        }
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            if (ids != null)
            {
                this.IDKLManagerService.DeleteSampleProject(ids);
            }
            return RedirectToAction("Index");
        }
	}
}