using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DKLManager.Contract.Model;
using Web.Demo.Common;
using HYZK.FrameWork.Utility;
using HYZK.Account.Contract;
using Web.Common;
using Web.Demo.Areas.DKLManager.Models;
using System.Text;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ReportFormationController : SampleBaseController
    {
        public static ProjectInfo UploadModel = new ProjectInfo();
        //
        // GET: /DKLManager/ReportFormation/
        public ActionResult Index(ProjectInfoRequest request)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
             var parameNew = this.IDKLManagerService.GetSampleRegisterTableList(request.userName, request);
             request.SampleStates= (int)EnumSampleStates.Selec;
             return View(parameNew);
        }
        public ActionResult Check(int id, ProjectInfoRequest request)
        {
            var model = new SampleRegisterTable();
            var users = this.AccountService.GetUserList(10).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Person", new SelectList(users, "Name", "Name"));
            model = this.IDKLManagerService.GetSampleRegisterTable(id);
            this.IDKLManagerService.UpDateSampleRegister(model);
            model.SampleStates = (int)EnumSampleStates.NewSample;
            return View("Edit", model);

        }
        [HttpPost]
        public ActionResult Check(int id, FormCollection collection)
        {
            var model = new SampleRegisterTable();
            model = this.IDKLManagerService.GetSampleRegisterTable(id);
            model.AnalyzePeople = collection["Person"];
            model.SampleStates = (int)EnumSampleStates.OldSample;
            this.IDKLManagerService.UpDateSampleRegister(model);
            return this.RefreshParent();
        }
        public ActionResult UploadFiles(int id)
        {
            UploadModel = this.IDKLManagerService.GetProjectInfo(id);
            ProjectDocFile model = new ProjectDocFile();
            model.ProjectNumber = UploadModel.ProjectNumber;
            return View(model);
        }
        [HttpPost]
        public ActionResult UploadFiles(FormCollection collection)
        {
            //获取报告文档
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = files["docFile"];
            string fileName = "";
            if (file != null && file.ContentLength > 0)
            {
                fileName = GetFilePathByRawFile(file.FileName);
                file.SaveAs(fileName);
            }
            if (file == null || file.ContentLength == 0)
                return Back("未检测到上传文件！");
            var projectBasicDocFile = new ProjectDocFile();
            projectBasicDocFile.FilePath = fileName;
            projectBasicDocFile.CreateTime = DateTime.Now;
            projectBasicDocFile.ProjectNumber = UploadModel.ProjectNumber;
            projectBasicDocFile.Status = UploadModel.ProjectStatus;
            this.IDKLManagerService.AddProjectDocFile(projectBasicDocFile);

            return Back("上传成功");
        }
	}
}