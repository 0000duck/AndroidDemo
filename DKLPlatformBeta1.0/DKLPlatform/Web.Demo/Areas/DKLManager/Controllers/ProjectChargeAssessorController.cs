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
        public ActionResult Index(ProjectInfoRequest request)
        {
            request.ProjectStatus = (int)EnumProjectSatus.ProjectWorkFinish;
            request.ProjectCategory = (int)EnumProjectCategory.TestValue;
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            var users = this.AccountService.GetUserList(7).Select(c => new { Id = c.ID, Name = c.Name });
            var result = this.IDKLManagerService.GetProjectInfoList(request);
            ViewData.Add("ProjectCheif", new SelectList(users, "Name", "Name"));
            request.userName = user.Name;
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
            ViewData.Add("Status", new SelectList(EnumHelper.GetItemValueList<EnumProjectAgree>(), "Key", "Value"));
            infomodel = this.IDKLManagerService.GetProjectInfo(id);
            if (infomodel != null)
            {
                //根据项目编号和状态查找文件Doc
                model = this.IDKLManagerService.GetProjectDocFile(infomodel.ProjectNumber, infomodel.ProjectStatus, infomodel.ID);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult CheckDoc(int id, FormCollection collection, ProjectInfoRequest request)
        {
            //upload doc file
            string retInfo = null;
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = files["DocFileForUpload"];
            string fileName = "";
            if (file != null && file.ContentLength > 0)
            {
                fileName = GetFilePathByRawFile(file.FileName);
                file.SaveAs(fileName);
                //更新项目信息状态
                var infomodel = new ProjectInfo();
                infomodel = this.IDKLManagerService.GetProjectInfo(id);
                if (request.Status == (int)EnumProjectAgree.DisAgree)
                {
                    infomodel.ProjectStatus = (int)EnumProjectSatus.ProjectModifyTwo;
                }
                else if (request.Status == (int)EnumProjectAgree.Agree)
                {
                    infomodel.ProjectStatus = (int)EnumProjectSatus.ProjectModifyThree;
                }
                this.IDKLManagerService.UpdateProjectInfo(infomodel);
                //上传审核修改后doc文件记录
                var model = new ProjectDocFile();
                this.TryUpdateModel<ProjectDocFile>(model);
                model.ProjectNumber = infomodel.ProjectNumber;
                model.Status = infomodel.ProjectStatus;
                model.FilePath = fileName;
                this.IDKLManagerService.AddProjectDocFile(model);
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
	}
}