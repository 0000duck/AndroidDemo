
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
    public class StaySubmitController : AdminControllerBase
    {
        //
        // GET: /DKLManager/StaySubmit/
        /// <summary>
        /// 咨询部审核完毕待提交报告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
            request.ProjectStatus = (int)EnumProjectSatus.ConsultWorking;
            request.ProjectCategory = (int)EnumProjectCategory.Consult;
            var result = this.IDKLManagerService.GetProjectInfoList(request);
            return View(result);
        }

        public ActionResult CheckDoc(int id)
        {
            var infomodel = new ProjectInfo();
            var model = new ProjectDocFile();
            infomodel = this.IDKLManagerService.GetProjectInfo(id);
            if (infomodel != null)
            {
                //根据项目编号和状态查找文件Doc
                model = this.IDKLManagerService.GetProjectDocFile(infomodel.ProjectNumber, infomodel.ProjectStatus, infomodel.ID);
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
            infomodel.ProjectStatus = infomodel.ProjectStatus + 1;
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
        public ActionResult DownLoadDocFile(string number, int status, int id)
        {
            var model = this.IDKLManagerService.GetProjectDocFile(number, status, id);
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