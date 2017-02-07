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
    public class ProjectImgFileHistoryController : AdminControllerBase
    {
        //
        // GET: /DKLManager/ProjectImgFileHistory/
        public ActionResult Index(ProjectInfoRequest request)
        {
            var result = this.IDKLManagerService.GetProjectInfoHistoryList(request);
            ViewData.Add("ProjectCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectCategory>(), "Key", "Value"));
            return View(result);
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
        public ActionResult ViewList(string pNumber)
        {
            if (pNumber.Contains("?"))
                pNumber = pNumber.Substring(0, pNumber.LastIndexOf("?"));
                        
            var result = this.IDKLManagerService.GetProjectFileHistory(pNumber);
            ProjectImgFileViewModel viewModel = new ProjectImgFileViewModel();
            viewModel.ID = result.ID;
            viewModel.ProjectNumber = result.ProjectNumber;
            if (result.FilePath != null)
            {
                viewModel.FilePaths = result.FilePath.Split(',').Where(f => f.Length > 5).ToList();
            }
            return View(viewModel);
        }

        public ActionResult DownLoadDocFile(string fileName)
        {
            string retInfo = null;
            if (fileName != null)
            {
                string fileNewName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = Encoding.UTF8;
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileNewName, Encoding.UTF8));
                Response.WriteFile(fileName);
            }
            else
            {
                retInfo = GlobalData.warningInfo1;
            }

            return this.RefreshParent(retInfo); 

        }

    }
}