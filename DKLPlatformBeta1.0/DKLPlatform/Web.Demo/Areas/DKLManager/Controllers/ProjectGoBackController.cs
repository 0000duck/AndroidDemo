using DKLManager.Contract.Model;
using HYZK.Account.Contract;
using System.Linq;
using System.Web.Mvc;
using Web.Demo.Areas.DKLManager.Models;
using Web.Demo.Common;
using HYZK.FrameWork.Utility;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ProjectGoBackController : AdminControllerBase
    {
        //
        // GET: /DKLManager/ProjectGoBack/
        /// <summary>
        /// 咨询部的退回列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
            request.ProjectStatus = (int)EnumProjectSatus.ConsultManagerReview;
            request.ProjectCategory = (int)EnumProjectCategory.Consult;
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            var users = this.AccountService.GetUserList(3).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("ProjectCheif", new SelectList(users, "Name", "Name"));
            ViewData["Name"] = user.Name;
            var result = this.IDKLManagerService.GetProjectInfoList(request);
            return View(result);
        }

        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var model = new ProjectWholeInfoViewModel();
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (model.projectBasicinfo != null)
            {
                model.projectBasicImgFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);
                model.projectConsultBasicinfo = this.IDKLManagerService.GetConsultBasicInfo(model.projectBasicinfo.ProjectNumber);
            }
            ViewData.Add("ProjectCategory", EnumHelper.GetEnumTitle((EnumProjectCategory)model.projectBasicinfo.ProjectCategory));
            if ((model.projectBasicImgFile != null) && (!string.IsNullOrEmpty(model.projectBasicImgFile.FilePath)))
            {
                List<string> htmlFIles = new List<string>();
                model.projectBasicImgFile.FilePath.Split(',').ToList().ForEach(f =>
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
            return View(model);
        }
        //
        // POST: /Account/User/Create
        /// <summary>
        /// 对编辑操作增加文件图片等
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            //上传doc文件
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = files["docFile"];
            string fileName = "";
            if (file != null && file.ContentLength > 0)
            {
                fileName = GetFilePathByRawFile(file.FileName);
                file.SaveAs(fileName);
            }


            var model = new ProjectWholeInfoViewModel();
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (model.projectBasicinfo != null)
            {
                model.projectConsultBasicinfo = this.IDKLManagerService.GetConsultBasicInfo(model.projectBasicinfo.ProjectNumber);
            }
            UpdateViewModel(collection, ref model);
            var projectBasicDocFile = new ProjectDocFile();
            var projectConsultBasicInfo = new ConsultBasicInfo();
            projectConsultBasicInfo.ProjectNumber = model.projectBasicinfo.ProjectNumber;
            projectBasicDocFile.FilePath = fileName;
            projectBasicDocFile.ProjectNumber = model.projectBasicinfo.ProjectNumber;
            projectBasicDocFile.Status = model.projectBasicinfo.ProjectStatus;
            this.IDKLManagerService.UpdateProjectInfo(model.projectBasicinfo);
            this.IDKLManagerService.AddProjectDocFile(projectBasicDocFile);
            this.IDKLManagerService.UpdateConsultBasicInfo(model.projectConsultBasicinfo);
            return this.RefreshParent();
        }
        public ActionResult Submit(int id)
        {
            var model = new ProjectWholeInfoViewModel();
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (model.projectBasicinfo != null)
            {
                model.projectBasicImgFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);
                model.projectTestBasicinfo = this.IDKLManagerService.GetProjectTestBasicInfo(model.projectBasicinfo.ProjectNumber);
                model.projectValueBasicinfo = this.IDKLManagerService.GetVlaueProjectBasicInfo(model.projectBasicinfo.ProjectNumber);
            }
            ViewData.Add("ProjectCategory", EnumHelper.GetEnumTitle((EnumProjectCategory)model.projectBasicinfo.ProjectCategory));
            if ((model.projectBasicImgFile != null) && (!string.IsNullOrEmpty(model.projectBasicImgFile.FilePath)))
            {
                List<string> htmlFIles = new List<string>();
                model.projectBasicImgFile.FilePath.Split(',').ToList().ForEach(f =>
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
                model.projectConsultBasicinfo = this.IDKLManagerService.GetConsultBasicInfo(model.projectBasicinfo.ProjectNumber);
            }
            UpdateViewModel(collection, ref model);
            var docModel = this.IDKLManagerService.GetProjectDocFile(model.projectBasicinfo.ProjectNumber, model.projectBasicinfo.ProjectStatus);
            if (docModel != null)
            {
                model.projectBasicinfo.ProjectStatus = (int)EnumProjectSatus.ConsultManagerReview; ;
                docModel.Status = model.projectBasicinfo.ProjectStatus;
                this.IDKLManagerService.UpdateProjectDocFile(docModel);
                this.IDKLManagerService.UpdateProjectInfo(model.projectBasicinfo);
                this.IDKLManagerService.UpdateConsultBasicInfo(model.projectConsultBasicinfo);
                return this.RefreshParent();
            }
            else
            {
                return this.RefreshParent(GlobalData.warningInfo1);
            }

        }

        public ActionResult CheckDoc(int id)
        {
            var infomodel = new ProjectInfo();
            var model = new ProjectDocFile();
            infomodel = this.IDKLManagerService.GetProjectInfo(id);
            if (infomodel != null)
            {
                //根据项目编号和状态查找文件Doc
                model = this.IDKLManagerService.GetProjectDocFile(infomodel.ProjectNumber, infomodel.ProjectStatus,infomodel.ID);
                this.IDKLManagerService.UpdateProjectDocFile(model);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult CheckDoc(int id, FormCollection collection)
        {
            //upload doc file
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = files["DocFileForUpload"];
            string fileName = "";
            if (file != null && file.ContentLength > 0)
            {
                fileName = GetFilePathByRawFile(file.FileName);
                file.SaveAs(fileName);
            }

            //更新项目信息状态
            var infomodel = new ProjectInfo();
            infomodel = this.IDKLManagerService.GetProjectInfo(id);
            infomodel.ProjectStatus = (int)EnumProjectSatus.ProjectVerifyTwo;
            this.IDKLManagerService.UpdateProjectInfo(infomodel);

            //上传审核修改后doc文件记录
            var model = new ProjectDocFile();
            this.TryUpdateModel<ProjectDocFile>(model);
            model.ProjectNumber = infomodel.ProjectNumber;
            model.Status = infomodel.ProjectStatus;
            model.FilePath = fileName;
            this.IDKLManagerService.AddProjectDocFile(model);
            return this.RefreshParent();
        }
        public ActionResult DownLoadDocFile(string number, int status,int id)
        {
            var model = this.IDKLManagerService.GetProjectDocFile(number, status,id);
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
                return this.RefreshParent(GlobalData.warningInfo1); ;
            }
        }
	}
}