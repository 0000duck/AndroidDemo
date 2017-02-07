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
    public class ProjectConsultController : AdminControllerBase
    {
        //
        // GET: /DKLManager/ProjectConsult/
        public ActionResult Index(ProjectInfoRequest request)
        {
            request.ProjectStatus = (int)EnumProjectSatus.QualityControlSubmit;
            var result = this.IDKLManagerService.GetProjectInfoList(request);
            var users = this.AccountService.GetUserList(3).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("ProjectCheif", new SelectList(users, "Name", "Name"));
            return View(result);
        }
        //
        // POST: /Account/User/Edit/5
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
            //upload doc file
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
            else
            {
                return this.RefreshParent("报告文件不存在，请检查是否已上传！");;
            }
        }
        public ActionResult Submit(int id)
        {
            var model = new ProjectWholeInfoViewModel();
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (model.projectBasicinfo != null)
            {
                model.projectBasicFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);
                string fileName = CreateBarCode(model.projectBasicinfo.ProjectNumber);
                if (fileName != null)
                {
                    string filePath = fileName.Substring(fileName.IndexOf("Upload") - 1);
                    ViewData.Add("ProjectBarCodeImg", filePath);
                }
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
                model.projectBasicinfo.ProjectStatus = (int)EnumProjectSatus.ConsultHasPersonInCharge;
                this.IDKLManagerService.UpdateProjectInfo(model.projectBasicinfo);
            }
            return this.RefreshParent();
        }
        [HttpPost]
        public ActionResult Return(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.GetProjectInfo(id);
            this.TryUpdateModel<ProjectInfo>(model);
            model.ProjectStatus = (int)EnumProjectSatus.MarketSubmit;
            this.IDKLManagerService.UpdateProjectInfo(model);
            return RedirectToAction("Index");
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

        public ActionResult DownLoadDocFile(string number, int status)
        {
            var model = this.IDKLManagerService.GetProjectDocFile(number, status);
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