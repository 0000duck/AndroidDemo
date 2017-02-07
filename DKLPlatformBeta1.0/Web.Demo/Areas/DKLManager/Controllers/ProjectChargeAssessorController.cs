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
    public class ProjectChargeAssessorController : AdminControllerBase
    {
        // GET: /DKLManager/ProjectChargeAssessor/
        /// <summary>
        /// 检测二审
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static ProjectInfo UploadModel = new ProjectInfo();
        public ActionResult Index(ProjectInfoRequest request)
        {
            request.ProjectStatus = (int)EnumProjectSatus.ProjectWorkFinish;
            request.ProjectCategory = (int)EnumProjectCategory.TestValue;
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            //var users = this.AccountService.GetUserList(7).Select(c => new { Id = c.ID, Name = c.Name });
            var result = this.IDKLManagerService.GetProjectInfoList(request);
            //ViewData.Add("ProjectCheif", new SelectList(users, "Name", "Name"));
            ViewData["Name"] = user.Name;
            ViewData["Name"] = (user != null) ? user.Name : "";
            return View(result);
        }
        [HttpPost]
        public ActionResult Return(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.GetProjectInfo(id);
            this.TryUpdateModel<ProjectInfo>(model);
            model.ProjectStatus = (int)EnumProjectSatus.ProjectWorking;
            this.IDKLManagerService.UpdateProjectInfo(model);
            return RedirectToAction("Index");
        }
        public ActionResult CheckDoc(int id)
        {
            var infomodel = new ProjectInfo();
            var model = new ProjectDocFile();
            var users = this.AccountService.GetUserListQ(3).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Person", new SelectList(users, "Name", "Name"));
            ViewData.Add("Status", new SelectList(EnumHelper.GetItemValueList<EnumProjectAgree>(), "Key", "Value"));
            infomodel = this.IDKLManagerService.GetProjectInfo(id);
            if (infomodel != null)
            {
                //根据项目编号和状态查找文件Doc
                model = this.IDKLManagerService.GetProjectDocFile(infomodel.ProjectNumber, infomodel.ProjectStatus, infomodel.ID);
            }
            return View(model);
        }
        public ActionResult WriteOption(int id)
        {
            var model = this.IDKLManagerService.GetProjectInfo(id);
            return View("WriteOption",model);
        }
        [HttpPost]
        public ActionResult WriteOption(int id,FormCollection collection)
        {
            var model = this.IDKLManagerService.GetProjectInfo(id);
            this.TryUpdateModel<ProjectInfo>(model);
            this.IDKLManagerService.UpdateProjectInfo(model);
            return this.RefreshParent();           
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
        [HttpPost]
        public ActionResult CheckDoc(int id, FormCollection collection, ProjectInfoRequest request)
        {
            ProjectInfo projectModel = this.IDKLManagerService.GetProjectInfo(id);
            var fileList = this.IDKLManagerService.GetProjectDocFileLists(projectModel.ProjectNumber);
            int flag = 0;
            foreach (var item in fileList)
            {
                if (item.Status == 17)
                {
                    flag = 1;
                    break;
                }
            }
            ////upload doc file
            string retInfo = null;
            //HttpFileCollectionBase files = Request.Files;
            //HttpPostedFileBase file = files["DocFileForUpload"];
            //string fileName = "";
            //if (file != null && file.ContentLength > 0)
            //{
            if (flag != 0)
            {
                //fileName = GetFilePathByRawFile(file.FileName);
                //file.SaveAs(fileName);
                //更新项目信息状态
                var infomodel = new ProjectInfo();
                infomodel = this.IDKLManagerService.GetProjectInfo(id);
                if (request.Status == (int)EnumProjectAgree.DisAgree)
                {
                    var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
                    infomodel.ProjectStatus = (int)EnumProjectSatus.ProjectModifyTwo;
                    infomodel.Person = collection["Person"];
                    var nn = this.IDKLManagerService.SelectContractInfo(infomodel.ProjectName);
                    infomodel.SignTime = nn.ContractDate;
                    var models1 = new TimeInstructions();
                    models1.ProjectNumBer = infomodel.ProjectNumber;
                    models1.ProjectName = infomodel.ProjectName;
                    models1.TimeNode = DateTime.Now;
                    models1.SignTime = infomodel.SignTime.ToString();
                    models1.Instructions = user.LoginName+"未通过";
                    this.IDKLManagerService.InsertTimeInstructions(models1);
                }
                else if (request.Status == (int)EnumProjectAgree.Agree)
                {
                    var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
                    infomodel.ProjectStatus = (int)EnumProjectSatus.PersonW;
                    infomodel.Person = collection["Person"];
                    var nn = this.IDKLManagerService.SelectContractInfo(infomodel.ProjectName);
                    infomodel.SignTime = nn.ContractDate;
                    var models1 = new TimeInstructions();
                    models1.ProjectNumBer = infomodel.ProjectNumber;
                    models1.ProjectName = infomodel.ProjectName;
                    models1.TimeNode = DateTime.Now;
                    models1.SignTime = infomodel.SignTime.ToString();
                    models1.Instructions =user.LoginName +"通过";
                    this.IDKLManagerService.InsertTimeInstructions(models1);
                }
                this.IDKLManagerService.UpdateProjectInfo(infomodel);
                //上传审核修改后doc文件记录
                //var model = new ProjectDocFile();
                //this.TryUpdateModel<ProjectDocFile>(model);
                //model.ProjectNumber = infomodel.ProjectNumber;
                //model.Status = infomodel.ProjectStatus;
                //model.FilePath = fileName;
                //this.IDKLManagerService.AddProjectDocFile(model);
            }
            else
            {
                retInfo = GlobalData.warningInfo1;
            }
            return this.RefreshParent(retInfo);
        }
        public ActionResult DownLoadDocFile(string number, int status ,int ida)
        {
            var model = this.IDKLManagerService.GetProjectDocFile(number, status,ida);
            string fileNames = model.FilePath;
            if (fileNames != null && fileNames.LastIndexOf("\\") > -1)
            {
                string fileNewName = fileNames.Substring(fileNames.LastIndexOf("\\") + 1);
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = Encoding.UTF8;
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileNewName, Encoding.UTF8));
                Response.WriteFile(fileNames);
                return this.RefreshParent();
            }
            else
            {
                return this.RefreshParent("报告文件不存在，请检查是否已上传！"); ;
            }
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
            projectBasicDocFile.suggenstion = collection["suggenstion"];
            projectBasicDocFile.ProjectNumber = UploadModel.ProjectNumber;
            projectBasicDocFile.Status = UploadModel.ProjectStatus;
            this.IDKLManagerService.AddProjectDocFile(projectBasicDocFile);

            return Back("上传成功");
        }
	}
}