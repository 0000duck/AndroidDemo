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
    public class ProjectLeaderAssessorController : AdminControllerBase
    {
        //
        // GET: /DKLManager/ProjectLeaderAssessor/
        /// <summary>
        /// 检测评价项目一审
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
            request.ProjectStatus = (int)EnumProjectSatus.ProjectModifyFour;
            request.ProjectCategory = (int)EnumProjectCategory.TestValue;
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            var users = this.AccountService.GetUserList(7).Select(c => new { Id = c.ID, Name = c.Name });

            var result = this.IDKLManagerService.GetProjectInfoList(request);

            ViewData.Add("ProjectCheif", new SelectList(users, "Name", "Name"));
            ViewData["Name"] = user.Name;
            ViewData["Name"] = (user != null) ? user.Name : "";
            return View(result);
        }
        public ActionResult Edit(int id)
        {
            var model = new ProjectWholeInfoViewModel();
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (model.projectBasicinfo != null)
            {
                model.projectBasicFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);
                model.projectTestBasicinfo = this.IDKLManagerService.GetProjectTestBasicInfo(model.projectBasicinfo.ProjectNumber);
                model.projectValueBasicinfo = this.IDKLManagerService.GetVlaueProjectBasicInfo(model.projectBasicinfo.ProjectNumber);
            }
            this.RenderMyViewData(model);
            return View(model);
        }
        // POST: /Account/User/Create
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = new ProjectWholeInfoViewModel();
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (model.projectBasicinfo != null)
            {
                model.projectBasicFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);
                model.projectTestBasicinfo = this.IDKLManagerService.GetProjectTestBasicInfo(model.projectBasicinfo.ProjectNumber);
                model.projectValueBasicinfo = this.IDKLManagerService.GetVlaueProjectBasicInfo(model.projectBasicinfo.ProjectNumber);
            }
            UpdateViewModel(collection, ref model);
            this.IDKLManagerService.UpdateProjectInfo(model.projectBasicinfo);
            this.IDKLManagerService.UpdateProjectFile(model.projectBasicFile);
            this.IDKLManagerService.UpdateProjectTestBasicInfo(model.projectTestBasicinfo);
            this.IDKLManagerService.UpdateVlaueProjectBasicInfo(model.projectValueBasicinfo);
            return this.RefreshParent();
        }
        public ActionResult Submit(int id)
        {
            var model = new ProjectWholeInfoViewModel();
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (model.projectBasicinfo != null)
            {
                model.projectBasicFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);
                model.projectTestBasicinfo = this.IDKLManagerService.GetProjectTestBasicInfo(model.projectBasicinfo.ProjectNumber);
                model.projectValueBasicinfo = this.IDKLManagerService.GetVlaueProjectBasicInfo(model.projectBasicinfo.ProjectNumber);
            }
            ViewData.Add("ProjectCategory", EnumHelper.GetEnumTitle((EnumProjectCategory)model.projectBasicinfo.ProjectCategory));
            if ((model.projectBasicFile != null) && (!string.IsNullOrEmpty(model.projectBasicFile.FilePath)))
            {
                List<string> htmlFIles = new List<string>();
                model.projectBasicFile.FilePath.Split(',').ToList().ForEach(f =>
                {
                    if (!string.IsNullOrEmpty(f))
                    {
                        var picHtml = "<li class=\"span2\"><a> <img src=" + f + " > </a>";
                        picHtml += "<div class=\"actions\"> <a  href=\"#\"><i class=\"icon-pencil\"></i></a>";
                        picHtml += " </div></li>";
                        htmlFIles.Add(picHtml);
                    }
                });
                ViewData["picFiles"] = htmlFIles;
            }
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Submit(int id, FormCollection collection)
        {
            var model = new ProjectWholeInfoViewModel();
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (model.projectBasicinfo != null)

            {
              model.projectBasicinfo.ProjectStatus = (int)EnumProjectSatus.ProjectModifyFour;
              this.IDKLManagerService.UpdateProjectInfo(model.projectBasicinfo);
            }
            return this.RefreshParent();
        }
        /// <summary>
        /// 审核页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CheckDoc(int id)
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
                    infomodel.ProjectStatus = (int)EnumProjectSatus.ProjectModifyOne;
                }
                else if (request.Status == (int)EnumProjectAgree.Agree)
                {
                    infomodel.ProjectStatus = (int)EnumProjectSatus.ProjectWorkFinish;
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
        public ActionResult DownLoadDocFile(string number, int status,int ids)
        {
            var model = this.IDKLManagerService.GetProjectDocFile(number, status,ids);
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