using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;
using HYZK.FrameWork.Common;
using Web.Demo.Areas.DKLManager.Models;
using System.Drawing.Imaging;
using System.IO;
using HYZK.Core.Upload;
using OfficeDocGenerate;
using System.Text;


namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ProjectContractHistoryController : AdminControllerBase
    {
        //
        // GET: /DKLManager/ProjectContractHistory/
        public ActionResult Index(ProjectInfoRequest request)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            request.ProjectStatus = (int)EnumProjectSatus.Begin;
            if (user.AccountType == 1)
            {
                var result = this.IDKLManagerService.GetProjectContractHistoryPerson(request);
                return View(result);
            }
            else
            {
                var result = this.IDKLManagerService.GetProjectContractHistory(request);
                return View(result);
            }
           
            
        }
        public ActionResult Course(string ProjectName, string SignTime)
        {

            if (SignTime.Contains("?"))
            {
                SignTime = SignTime.Substring(0, SignTime.LastIndexOf("?"));
            }
            List<TimeInstructions> result = new List<TimeInstructions>();
            if (ProjectName != null && SignTime != null)
            {

                result = this.IDKLManagerService.SelectTimeInstructions(ProjectName, SignTime);
            }
            return View(result);
        }
        public ActionResult Search(ProjectInfoRequest request,string projectNumber)
        {
             var model = this.IDKLManagerService.GetProjectContractByProjectNumber(projectNumber);
             if (projectNumber == "")
               model = this.IDKLManagerService.GetProjectContractHistory(request);
             return View("Index", model);
        }
        public ActionResult Edit(int id)
        {

            var model = new ProjectContract();
            ViewData.Add("ProjectCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectCategory>(), "Key", "Value", model.ProjectCategory));

            model = this.IDKLManagerService.GetProjectContractInfo(id);
            return View("Refer", model);
        }
        [HttpPost]
        public ActionResult Edit()
        {
            return this.RefreshParent();
        }
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            if (ids == null)
                return RedirectToAction("Index");
            this.IDKLManagerService.DeleteProjectContract(ids);
            return RedirectToAction("Index");
        }
	}
}