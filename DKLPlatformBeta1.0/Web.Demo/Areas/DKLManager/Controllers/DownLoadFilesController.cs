using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class DownLoadFilesController : AdminControllerBase
    {
        //
        // GET: /DKLManager/DownLoadFiles/
        public static List<string> FilePaths;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SDownLoadFiles(int id)
        {
            var model = this.IDKLManagerService.GetSampleRegisterTable(id);
            var list = this.IDKLManagerService.GetProjectFilesByProjectNumber(model.SampleRegisterNumber);
            var selectListTemp = list.Select(c => c.FilePath).Distinct().ToList();
            List<string> selectlist = new List<string>();
            int laseIndex = 0;
            foreach (var item in selectListTemp)
            {
                laseIndex = item.LastIndexOf("\\");
                selectlist.Add(item.Substring(laseIndex + 1, item.Length - laseIndex - 1));
                laseIndex = 0;
            }
            FilePaths = list.Select(u => u.FilePath).ToList();
            ViewData.Add("FilesName", new SelectList(selectlist.Distinct()));

            return View();
        }
        public ActionResult DownLoadFiles(int id)
        {
            var model = this.IDKLManagerService.GetProjectInfo(id);
            var list = this.IDKLManagerService.GetProjectFilesByProjectNumber(model.ProjectNumber);
            var selectListTemp = list.Select(c =>c.FilePath).Distinct().ToList();
            List<string> selectlist = new List<string>();
            int laseIndex = 0;
            foreach (var item in selectListTemp)
            {
                laseIndex = item.LastIndexOf("\\");
                selectlist.Add(item.Substring(laseIndex+1, item.Length - laseIndex-1));
                laseIndex = 0;
            }
            FilePaths = list.Select(u=>u.FilePath).ToList();
            ViewData.Add("FilesName", new SelectList(selectlist.Distinct()));

            return View();
        }
        public ActionResult DownLoadProjectFiles(int id)
        {
            var model = this.IDKLManagerService.GetProjectContractInfo(id);
            var list = this.IDKLManagerService.GetProjectFilesByProjectNumber(model.ProjectNumber);
            var selectListTemp = list.Select(c => c.FilePath).Distinct().ToList();
            List<string> selectlist = new List<string>();
            int laseIndex = 0;
            foreach (var item in selectListTemp)
            {
                laseIndex = item.LastIndexOf("\\");
                selectlist.Add(item.Substring(laseIndex + 1, item.Length - laseIndex - 1));
                laseIndex = 0;
            }
            FilePaths = list.Select(u => u.FilePath).ToList();
            ViewData.Add("FilesName", new SelectList(selectlist.Distinct()));

            return View();
        }           //下载合同
        public ActionResult ReadProjectFiles(int id)
        {
            var model = this.IDKLManagerService.GetProjectInfo(id);
            var list = this.IDKLManagerService.GetProjectFilesByProjectNumber(model.ProjectNumber);
            var selectListTemp = list.Select(c => c.FilePath).Distinct().ToList();
            List<string> selectlist = new List<string>();
            int laseIndex = 0;
            foreach (var item in selectListTemp)
            {
                laseIndex = item.LastIndexOf("\\");
                selectlist.Add(item.Substring(laseIndex + 1, item.Length - laseIndex - 1));
                laseIndex = 0;
            }
            FilePaths = list.Select(u => u.FilePath).ToList();
            ViewData.Add("FilesName", new SelectList(selectlist.Distinct()));

            return View();
        }           //阅览审核报告
        [HttpPost]
        public ActionResult ReadProjectFiles(FormCollection collection)
        {
            string fileNames = null;
            var fileNameShort = collection["FilesName"];
            foreach (var item in FilePaths)
            {
                if (item.Replace(" ", "").Contains(fileNameShort.Replace(" ", "")))
                {
                    fileNames = item;
                }
            }
            if (fileNames != null && fileNames.LastIndexOf("\\") > -1)
            {
                string fileNewName = fileNames.Substring(fileNames.LastIndexOf("\\") + 1);
                return RedirectToAction("Index", "OfficeView", new { url = fileNames });
            }
            return Back("浏览失败！");
        }
        public ActionResult ReadProjectContractFiles(int id)            //阅览合同
        {
            var model = this.IDKLManagerService.GetProjectContractInfo(id);
            var list = this.IDKLManagerService.GetProjectFilesByProjectNumber(model.ProjectNumber);
            var selectListTemp = list.Select(c => c.FilePath).Distinct().ToList();
            List<string> selectlist = new List<string>();
            int laseIndex = 0;
            foreach (var item in selectListTemp)
            {
                laseIndex = item.LastIndexOf("\\");
                selectlist.Add(item.Substring(laseIndex + 1, item.Length - laseIndex - 1));
                laseIndex = 0;
            }
            FilePaths = list.Select(u => u.FilePath).ToList();
            ViewData.Add("FilesName", new SelectList(selectlist.Distinct()));

            return View();

        }
        [HttpPost]
        public ActionResult ReadProjectContractFiles(FormCollection collection)
        {
            string fileNames = null;
            var fileNameShort = collection["FilesName"];
            foreach (var item in FilePaths)
            {
                if (item.Replace(" ", "").Contains(fileNameShort.Replace(" ", "")))
                {
                    fileNames = item;
                }
            }
            if (fileNames != null && fileNames.LastIndexOf("\\") > -1)
            {
                string fileNewName = fileNames.Substring(fileNames.LastIndexOf("\\") + 1);              
                return RedirectToAction("Index", "OfficeView", new { url = fileNames });
            }
            return Back("浏览失败！");
        }
        [HttpPost]
        public ActionResult DownLoadDoc(FormCollection collection)
        {
            string fileNames = null;
            var fileNameShort = collection["FilesName"];
            foreach (var item in FilePaths)
            {
                if (item.Replace(" ", "").Contains(fileNameShort.Replace(" ", "")))
                {
                    fileNames = item;
                }
            }
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