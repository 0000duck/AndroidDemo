using DKLManager.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ProjectInfoController : AdminControllerBase
    {
        //
        // GET: /DKLManager/ValueProjectInfo/
        public ActionResult Index()
        {
            var result = this.IDKLManagerService.GetProjectFileList();
            return View(result);
        }
        public ActionResult Create()
        {
            var model = new ProjectInfo();
            return View("Edit", model);
        }
        public ActionResult Create(FormCollection collection)
        {
            var model = new ProjectInfo();
            this.TryUpdateModel<ProjectInfo>(model);
            this.IDKLManagerService.AddProjectInfo(model);
            return this.RefreshParent();
        }
    }
}