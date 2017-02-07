using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Areas.DKLManager.Models;
using Web.Demo.Common;
namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class AnalyzePersonCheckController : SampleBaseController
    {
        public static List<string> FilePaths;
        public static ProjectContract UploadModel = new ProjectContract();
        public static SampleRegisterTable UploadModels = new SampleRegisterTable();
        public ActionResult Index(ProjectInfoRequest request)
        {
            var users = this.AccountService.GetUserListT(9).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Person", new SelectList(users, "Name", "Name"));
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            request.SampleStates = (int)EnumSampleStates.PersonCheck;
            var parameNew = this.IDKLManagerService.GetSampleRegisterTableList(user.Name, request);
            return View(parameNew);
        }
        public ActionResult Check(int id, ProjectInfoRequest request)
        {
            var model = new SampleRegisterTable();
            var users = this.AccountService.GetUserListT(9).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Person", new SelectList(users, "Name", "Name"));
            model = this.IDKLManagerService.GetSampleRegisterTable(id);
            this.IDKLManagerService.UpDateSampleRegister(model);          
            return View("Edit", model);

        }
        [HttpPost]
        public ActionResult Check(int id, FormCollection collection)
        {
            var model = new SampleRegisterTable();
            //model = this.IDKLManagerService.GetSampleRegisterTable(id);
            //model.AnalyzePeople = collection["Person"];
            //model.SampleStates = (int)EnumSampleStates.SysCheck;
            //this.IDKLManagerService.UpDateSampleRegister(model);
            this.IDKLManagerService.DeleteSampleRegisterD(id);
            return this.RefreshParent();
        }
        [HttpPost]
        public ActionResult UpdateAlls(ProjectInfoRequest request, FormCollection collection, List<int> ids)
        {
            if (collection["Person"] == null || collection["Person"] == "")
            {
                return Back("请选择分析人！");
            }
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            request.SampleStates = (int)EnumSampleStates.PersonCheck;
            if (ids != null)
            {
                foreach (var item in ids)
                {
                    var temp = this.IDKLManagerService.GetSampleRegisterTable(item);
                    temp.SampleStates = (int)EnumSampleStates.SysCheck;
                    temp.AnalyzePeople = collection["Person"];
                    this.IDKLManagerService.UpDateSampleRegister(temp);
                }
            }

            return RedirectToAction("");
        }
        public ActionResult UploadFiles(int id)
        {
            UploadModels = this.IDKLManagerService.GetSampleRegisterTable(id);
            ProjectDocFile model = new ProjectDocFile();
            model.ProjectNumber = UploadModels.SampleRegisterNumber;
            return View();
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
            projectBasicDocFile.ProjectNumber = UploadModels.SampleRegisterNumber;
            this.IDKLManagerService.AddProjectDocFile(projectBasicDocFile);

            return Back("上传成功");
        }
    }
}
