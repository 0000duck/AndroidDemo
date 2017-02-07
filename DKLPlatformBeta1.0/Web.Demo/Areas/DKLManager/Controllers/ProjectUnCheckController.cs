using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Areas.DKLManager.Models;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ProjectUnCheckController : AdminControllerBase
    {
        // GET: /DKLManager/ProjectUnCheck/
        /// <summary>
        /// 咨询部审核
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
            request.ProjectCategory = (int)EnumProjectCategory.Consult;
            request.ProjectStatus = (int)EnumProjectSatus.ProjectVerifyTwo;
            var result = this.IDKLManagerService.GetProjectInfoList(request);
            return View(result);
        }
        public ActionResult CheckDoc(int id, ProjectInfoRequest request)
        {
            var infomodel = new ProjectInfo();
            var model = new ProjectDocFile();
            ViewData.Add("Status", new SelectList(EnumHelper.GetItemValueList<EnumProjectAgree>(), "Key", "Value"));
            infomodel = this.IDKLManagerService.GetProjectInfo(id);
            if (infomodel != null)
            {
                //根据项目编号和状态查找文件Doc
                model = this.IDKLManagerService.GetProjectDocFile(infomodel.ProjectNumber, infomodel.ProjectStatus,infomodel.ID);
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
                    infomodel.ProjectStatus = (int)EnumProjectSatus.ConsultManagerReview;
                    var info = new TimeInstructions();
                    info.ProjectNumBer = infomodel.ProjectNumber;
                    info.TimeNode = DateTime.Now;
                    info.Instructions = "咨询主管未同意";
                    this.IDKLManagerService.InsertTimeInstructions(info);
                }
                else if (request.Status == (int)EnumProjectAgree.Agree)
                {
                    infomodel.ProjectStatus = (int)EnumProjectSatus.ConsultWorking;
                    var info = new TimeInstructions();
                    info.ProjectNumBer = infomodel.ProjectNumber;
                    info.TimeNode = DateTime.Now;
                    info.Instructions = "咨询主管同意";
                    this.IDKLManagerService.InsertTimeInstructions(info);
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
        public ActionResult CourseA(string ProjectNumber)
        {
            List<TimeInstructions> result = new List<TimeInstructions>();
            if (ProjectNumber.Contains("?"))
            {
                ProjectNumber = ProjectNumber.Substring(0, ProjectNumber.LastIndexOf("?"));
            }

            result = this.IDKLManagerService.SelectTimeInstruction(ProjectNumber);

            return View(result);
        }
        //public ActionResult Course(string ProjectNumber)
        //{
        //    List<TimeInstructions> result = new List<TimeInstructions>();

        //    if (ProjectNumber.Contains("?"))
        //    {
        //        ProjectNumber = ProjectNumber.Substring(0, ProjectNumber.LastIndexOf("?"));
        //    }

        //    result = this.IDKLManagerService.SelectTimeInstruction(ProjectNumber);

        //    return View(result);
        //}
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
        public ActionResult DownLoadDocFile(string projectNumber, int status)
        {
            var model = this.IDKLManagerService.GetProjectDocFile(projectNumber, status);
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