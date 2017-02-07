using DKLManager.Contract.Model;
using HYZK.Core.Upload;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Areas.DKLManager.Models;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ProjectConsultPersonController : AdminControllerBase
    {
        //
        // GET: /DKLManager/ProjectAdvisoryCharge/
        /// <summary>
        /// 咨询部员工
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            request.ProjectStatus = (int)EnumProjectSatus.ConsultHasPersonInCharge;
            request.ProjectCategory = (int)EnumProjectCategory.Consult;
            var users = this.AccountService.GetUserList(3).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("ProjectCheif", new SelectList(users, "Name", "Name"));
            ViewData["Name"] = user.Name;
            var result = this.IDKLManagerService.GetProjectInfoList(request);
            return View(result);
        }
        public ActionResult Edit(int id)
        {
            var model = new ProjectWholeInfoViewModel();
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (model.projectBasicinfo != null)
            {
                string fileName = CreateBarCode(model.projectBasicinfo.ProjectNumber);
                if (fileName != null)
                {
                    string filePath = fileName.Substring(fileName.IndexOf("Upload") - 1);
                    ViewData.Add("ProjectBarCodeImg", filePath);
                }
                model.projectBasicImgFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);
                model.projectConsultBasicinfo = this.IDKLManagerService.GetConsultBasicInfo(model.projectBasicinfo.ProjectNumber);
            }
            this.RenderMyViewData(model);
            return View(model);
        }
        //
        // POST: /Account/User/Create
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
           // 上传doc文件
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
                model.projectBasicImgFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);
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
            this.IDKLManagerService.UpdateProjectFile(model.projectBasicImgFile);
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
                string fileName = CreateBarCode(model.projectBasicinfo.ProjectNumber);
                if (fileName != null)
                {
                    string filePath = fileName.Substring(fileName.IndexOf("Upload") - 1);
                    ViewData.Add("ProjectBarCodeImg", filePath);
                }
                model.projectBasicImgFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);
                model.projectTestBasicinfo = this.IDKLManagerService.GetProjectTestBasicInfo(model.projectBasicinfo.ProjectNumber);
                model.projectValueBasicinfo = this.IDKLManagerService.GetVlaueProjectBasicInfo(model.projectBasicinfo.ProjectNumber);
            }
            this.RenderMyViewData(model);
            return View("Edit", model);

        }

        [HttpPost]
        public ActionResult Submit(int id, FormCollection collection)
        {
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
                model.projectBasicinfo.ProjectStatus = (int)EnumProjectSatus.ProjectVerifyTwo;
                var projectBasicDocFile = new ProjectDocFile();
                projectBasicDocFile.CreateTime = DateTime.Now;
                projectBasicDocFile.FilePath = fileName;
                projectBasicDocFile.ProjectNumber = model.projectBasicinfo.ProjectNumber;
                projectBasicDocFile.Status = model.projectBasicinfo.ProjectStatus;
                this.IDKLManagerService.AddProjectDocFile(projectBasicDocFile);
                this.IDKLManagerService.UpdateProjectInfo(model.projectBasicinfo);
            }
            return this.RefreshParent();
        }
        public ActionResult View(int id)
        {
            var model = new ProjectWholeInfoViewModel();
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (model.projectBasicinfo != null)
            {
                string fileName = CreateBarCode(model.projectBasicinfo.ProjectNumber);
                if (fileName != null)
                {
                    string filePath = fileName.Substring(fileName.IndexOf("Upload") - 1);
                    ViewData.Add("ProjectBarCodeImg", filePath);
                }
                model.projectBasicFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);
                model.projectConsultBasicinfo = this.IDKLManagerService.GetConsultBasicInfo(model.projectBasicinfo.ProjectNumber);
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
            return View(model);
        }

        [HttpPost]
        public ActionResult View(FormCollection collection)
        {
            return this.RefreshParent();
        }
        private string CreateBarCode(string barcodeStrData)
        {
            string ret = null;
            System.Drawing.Image barcodeImage = null;
            try
            {
                BarcodeLib.Barcode barcode = new BarcodeLib.Barcode();
                barcode.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                barcode.IncludeLabel = true;

                //===== Encoding performed here =====
                barcodeImage = barcode.Encode(BarcodeLib.TYPE.CODE128, barcodeStrData.Trim(), System.Drawing.ColorTranslator.FromHtml("#000000"), System.Drawing.ColorTranslator.FromHtml("#FFFFFF"), 300, 150);
                //===================================
                string subFolder = "month_" + DateTime.Now.ToString("yyMM");

                string barImgFileFolder = Path.Combine(UploadConfigContext.UploadPath,
                    "barCodeFolder",
                    subFolder
                    );
                if (!System.IO.Directory.Exists(barImgFileFolder))
                {
                    Directory.CreateDirectory(barImgFileFolder);
                }
                string barImgFile = barImgFileFolder + "\\" + barcodeStrData + ".jpg";
                barcodeImage.Save(barImgFile, ImageFormat.Png);
                ret = barImgFile;
            }
            catch (Exception)
            {
                //TODO: find a way to return this to display the encoding error message
            }
            finally
            {
                if (barcodeImage != null)
                {
                    //Clean up / Dispose...
                    barcodeImage.Dispose();
                }
            }//finally

            return ret;
        }
    }
}