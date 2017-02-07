using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;
using HYZK.FrameWork.Common;
using Web.Demo.Areas.DKLManager.Models;
using System.Drawing.Imaging;
using System.IO;
using HYZK.Core.Upload;
using OfficeDocGenerate;
using System.Text;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ProjectMaketController : AdminControllerBase
    {

        public ActionResult Index(ProjectInfoRequest request)
        {
            ProjectNewMarket projectInfoWhole = new ProjectNewMarket();
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            var resultHistory = this.IDKLManagerService.GetProjectInfoHistoryListPerson(request);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            request.ProjectStatus = (int)EnumProjectSatus.Begin;
            var resultDoing = this.IDKLManagerService.GetProjectInfoListPerson(request);
            

            projectInfoWhole.projectInfoList = resultDoing.ToList(); ;
            projectInfoWhole.projectHistoryList = resultHistory.ToList();

            return View(projectInfoWhole);
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
        public ActionResult Create()
        {
            var model = new ProjectWholeInfoViewModel();

            this.RenderMyViewData(model);
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new ProjectWholeInfoViewModel();
            UpdateViewModel(collection, ref model);
            model.projectBasicinfo.ProjectStatus = (int)EnumProjectSatus.Begin;
            try
            {

                this.IDKLManagerService.AddProjectInfo(model.projectBasicinfo);
                model.projectBasicImgFile.ProjectNumber = model.projectBasicinfo.ProjectNumber;
                this.IDKLManagerService.AddProjectFile(model.projectBasicImgFile);
            }
            catch (HYZK.FrameWork.Common.BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                this.RenderMyViewData(model);
                return View("Edit", model);
            }
            return this.RefreshParent();
        }
        public ActionResult Edit(int id)
        {
            var model = new ProjectWholeInfoViewModel();
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (model.projectBasicinfo != null)
            {
                model.projectBasicImgFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);
                string fileName = CreateBarCode(model.projectBasicinfo.ProjectNumber);
                if (fileName != null)
                {
                    string filePath = fileName.Substring(fileName.IndexOf("Upload") - 1);
                    ViewData.Add("ProjectBarCodeImg", filePath);
                }

            }
            this.RenderMyViewData(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = new ProjectWholeInfoViewModel();
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);

            if (model.projectBasicinfo != null)
            {
                model.projectBasicImgFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);

            }
            UpdateViewModel(collection, ref model);
            this.IDKLManagerService.UpdateProjectInfo(model.projectBasicinfo);
            this.IDKLManagerService.UpdateProjectFile(model.projectBasicImgFile);
            return this.RefreshParent();
        }
        [HttpPost]
        public ActionResult Submit(int id, FormCollection collection)
        {
            var model = new ProjectWholeInfoViewModel();
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);

            if (model.projectBasicinfo != null)
            {
                var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
                //       model.projectBasicImgFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);
                var nn = this.IDKLManagerService.SelectContractInfo(model.projectBasicinfo.ProjectName);
                model.projectBasicinfo.SignTime = nn.ContractDate;
                var models = new TimeInstructions();
                models.ProjectNumBer = model.projectBasicinfo.ProjectNumber;
                models.ProjectName = model.projectBasicinfo.ProjectName;
                models.TimeNode = DateTime.Now;
                models.SignTime = model.projectBasicinfo.SignTime.ToString();
                models.Instructions = user.LoginName + "提交";
                this.IDKLManagerService.InsertTimeInstructions(models);
                model.projectBasicinfo.ProjectStatus = (int)EnumProjectSatus.MarketSubmit;
                model.projectBasicinfo.ProjectPersonCategory = 0;
                model.projectBasicinfo.Remarks = "";
                this.IDKLManagerService.UpdateProjectInfo(model.projectBasicinfo);
            }

            return this.RefreshParent();
        }
        public ActionResult Submit(int id)
        {
            var model = new ProjectWholeInfoViewModel();
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (model.projectBasicinfo != null)
            {
                model.projectBasicImgFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);
                this.IDKLManagerService.UpdateProjectFile(model.projectBasicImgFile);
                string fileName = CreateBarCode(model.projectBasicinfo.ProjectNumber);
                if (fileName != null)
                {
                    string filePath = fileName.Substring(fileName.IndexOf("Upload") - 1);
                    ViewData.Add("ProjectBarCodeImg", filePath);
                }
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
            return View("Refer", model);
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
        // POST: /Account/User/Delete/5
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            if (ids == null)
                return RedirectToAction("Index");
            foreach (var id in ids)
            {
                var projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
                if (!string.IsNullOrEmpty(projectBasicinfo.ProjectNumber))
                {
                    this.IDKLManagerService.DeleteProjectFile(projectBasicinfo.ProjectNumber);
                }
            }
            this.IDKLManagerService.DeleteProjectInfo(ids);
            return RedirectToAction("Index");
        }
    }
}