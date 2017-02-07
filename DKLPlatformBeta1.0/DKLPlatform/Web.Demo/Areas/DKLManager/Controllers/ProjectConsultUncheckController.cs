using DKLManager.Contract.Model;
using HYZK.Account.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ProjectConsultUncheckController : AdminControllerBase
    {
        //
        // GET: /DKLManager/ProjectConsultUncheck/
        /// <summary>
        /// 咨询部审核页面
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;

            var result = this.IDKLManagerService.GetProjectInfoList(request);
            var users = this.AccountService.GetUserList((int)EnumAccountType.AdvisoryGeneral).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("ProjectCheif", new SelectList(users, "Name", "Name"));
            return View(result);
        }

        //报告审核
        public ActionResult CheckDoc(int id)
        {
            var infomodel = new ProjectInfo();
            var model = new ProjectDocFile();
            infomodel = this.IDKLManagerService.GetProjectInfo(id);
            if (infomodel != null)
            {
                //根据项目编号和状态查找文件Doc
                model = this.IDKLManagerService.GetProjectDocFile(infomodel.ProjectNumber, infomodel.ProjectStatus);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CheckDoc(int id, FormCollection collection)
        {
            string retInfo = null;
            //upload doc file
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = files["DocFileForUpload"];
            string fileName = "";
            if (file != null && file.ContentLength > 0)
            {
                fileName = GetFilePathByRawFile(file.FileName);
                file.SaveAs(fileName);
                //更新项目信息状态
                UpdateProjectInfo(id);
                //上传审核修改后doc文件记录
                UploadModifiedDoc(id, fileName);
            }
            else
            {
                retInfo = GlobalData.warningInfo1;
            }
            return this.RefreshParent(retInfo); ;
        }

        private void UpdateProjectInfo(int id)
        {
            //更新项目信息状态
            var infomodel = new ProjectInfo();
            infomodel = this.IDKLManagerService.GetProjectInfo(id);
            infomodel.ProjectStatus = infomodel.ProjectStatus + 1;
            this.IDKLManagerService.UpdateProjectInfo(infomodel);
        }

        private void UploadModifiedDoc(int id, string fileName)
        {
            var infomodel = new ProjectInfo();
            infomodel = this.IDKLManagerService.GetProjectInfo(id);
            var model = new ProjectDocFile();
            this.TryUpdateModel<ProjectDocFile>(model);
            model.ProjectNumber = infomodel.ProjectNumber;
            model.Status = infomodel.ProjectStatus;
            model.FilePath = fileName;
            this.IDKLManagerService.AddProjectDocFile(model);
        }

        public ActionResult DownLoadDocFile(string number, int status)
        {
            var model = this.IDKLManagerService.GetProjectDocFile(number, status);
            if (model != null)
            {
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
            }         
            return this.RefreshParent(GlobalData.warningInfo1); ;
        }

	}
}