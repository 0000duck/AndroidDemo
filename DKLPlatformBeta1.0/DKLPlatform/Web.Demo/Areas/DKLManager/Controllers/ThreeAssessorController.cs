﻿using DKLManager.Contract.Model;
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
    public class ThreeAssessorController : AdminControllerBase
    {
        //
        // GET: /DKLManager/ThreeAssessor/
        /// <summary>
        /// 检测三审
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
            request.ProjectStatus = (int)EnumProjectSatus.ProjectModifyThree;
            request.ProjectCategory = (int)EnumProjectCategory.TestValue;
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            var result = this.IDKLManagerService.GetProjectInfoList(request);
            return View(result);
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
                model = this.IDKLManagerService.GetProjectDocFile(infomodel.ProjectNumber, infomodel.ProjectStatus,infomodel.ID);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult CheckDoc(int id, FormCollection collection, ProjectInfoRequest request)
        {
            //upload doc file
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = files["DocFileForUpload"];
            string retInfo = null;
            string fileName = "";
            if (file != null && file.ContentLength > 0)
            {
                fileName = GetFilePathByRawFile(file.FileName);
                file.SaveAs(fileName);
                //更新项目信息状态
                var infomodel = new ProjectInfo();
                infomodel = this.IDKLManagerService.GetProjectInfo(id);
                infomodel.ProjectStatus = infomodel.ProjectStatus+1;
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
            var models = new ProjectWholeInfoViewModel();
            models.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (request.Status == (int)EnumProjectAgree.DisAgree)
            {
                models.projectBasicinfo.ProjectStatus = (int)EnumProjectSatus.ProjectVerifyFour;
            }
            else if (request.Status == (int)EnumProjectAgree.Agree)
            {
                models.projectBasicinfo.ProjectStatus = (int)EnumProjectSatus.ConsultModifyDone;
            }
            this.IDKLManagerService.UpdateProjectInfo(models.projectBasicinfo);
            return this.RefreshParent();
        }
        public ActionResult DownLoadDocFile(string number, int status,int ids)
        {
            var model = this.IDKLManagerService.GetProjectDocFile(number, status,ids);
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
            return this.RefreshParent("报告文件不存在，请检查是否已上传！"); ;
        }
	}
}