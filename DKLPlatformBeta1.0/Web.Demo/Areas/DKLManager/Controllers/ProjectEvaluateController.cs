using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DKLManager.Contract.Model;
using Web.Demo.Common;
using HYZK.FrameWork.Utility;
using Web.Demo.Areas.DKLManager.Models;
using HYZK.Core.Upload;
using System.Drawing.Imaging;
using System.IO;
using HYZK.Account.Contract;
using System.Web;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ProjectEvaluateControlController : AdminControllerBase
    {
        public static ProjectInfo UploadModel = new ProjectInfo();
        public ActionResult Index(ProjectInfoRequest request)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            ViewData["Name"] = user.Name;
            request.ProjectStatus = (int)EnumProjectSatus.PJSumbit;
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            var result = this.IDKLManagerService.GetProjectInfoListPerson(request);
            ViewData.Add("ProjectPersonCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectPersonCategory>(), "Key", "Value"));
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

        [HttpPost]
        public ActionResult UpdatePersonStatus(int id, FormCollection collection)
        {
            //string str = "";
            //str = collection["ProjectPersonCategory"]; 
            var model = this.IDKLManagerService.GetProjectInfo(id);
            //model.ProjectPersonCategory = Convert.ToInt32(str);
            this.TryUpdateModel<ProjectInfo>(model);
            this.IDKLManagerService.UpdateProjectInfo(model);
            return RedirectToAction("Index");

        }
        [HttpPost]
        public ActionResult Return(int id, FormCollection collection)
        {

            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            var model = this.IDKLManagerService.GetProjectInfo(id);
            var nn = this.IDKLManagerService.SelectContractInfo(model.ProjectName);
            model.SignTime = nn.ContractDate;
            var models = new TimeInstructions();
            models.SignTime = model.SignTime.ToString();
            models.ProjectNumBer = model.ProjectNumber;
            models.ProjectName = model.ProjectName;
            models.TimeNode = DateTime.Now;
            models.Instructions = user.LoginName + "退回";
            this.IDKLManagerService.InsertTimeInstructions(models);
            model.ProjectPersonCategory = 4;
            this.TryUpdateModel<ProjectInfo>(model);
            model.ProjectStatus = (int)EnumProjectSatus.QualityControlSubmit;
            model.Person = model.ProjectCheif;
            this.IDKLManagerService.UpdateProjectInfo(model);
            //  return RedirectToAction("Index");
            return this.RefreshParent();
        }
        public ActionResult Return(int id)
        {

            var model = new ProjectInfo();

            return View(model);
        }
        public ActionResult Problem(int id)
        {
            var model = this.IDKLManagerService.GetProjectInfo(id);
            ViewData.Add("ProjectProblem", new SelectList(EnumHelper.GetItemValueList<EnumProjectProblem>(), "Key", "Value"));
            return View(model);

        }
        [HttpPost]
        public ActionResult Problem(int id, FormCollection collection)
        {

            var model = this.IDKLManagerService.GetProjectInfo(id);

            this.TryUpdateModel<ProjectInfo>(model);
            this.IDKLManagerService.UpdateProjectInfo(model);
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
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = new ProjectWholeInfoViewModel();
            ViewData.Add("ProjectCategory", EnumHelper.GetEnumTitle((EnumProjectCategory)model.projectBasicinfo.ProjectCategory));
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (model.projectBasicinfo != null)
            {
                model.projectBasicImgFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);
            }
            UpdateViewModel(collection, ref model);
            //this.IDKLManagerService.UpdateProjectInfo(model.projectBasicinfo);
            this.IDKLManagerService.UpdateProjectFile(model.projectBasicImgFile);
            return this.RefreshParent();
        }
        public ActionResult Submit(int id)
        {
            var users = this.AccountService.GetUserList(21);
            List<User> user = new List<User>();
            foreach (var item in users)
            {
                if (item.Have == true)
                {
                    user.Add(item);
                }
            }
            ViewData.Add("Person", new SelectList(user, "Name", "Name"));
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
                var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
                var nn = this.IDKLManagerService.SelectContractInfo(model.projectBasicinfo.ProjectName);
                model.projectBasicinfo.SignTime = nn.ContractDate;
                var models = new TimeInstructions();
                models.ProjectNumBer = model.projectBasicinfo.ProjectNumber;
                models.ProjectName = model.projectBasicinfo.ProjectName;
                models.TimeNode = DateTime.Now;
                models.SignTime = model.projectBasicinfo.SignTime.ToString();
                models.Instructions = user.LoginName + "提交";
                this.IDKLManagerService.InsertTimeInstructions(models);
                model.projectBasicinfo.ProjectCheif = user.Name;
                model.projectBasicinfo.ProjectStatus = (int)EnumProjectSatus.Engineer;
                model.projectBasicinfo.Remarks = "";
                model.projectBasicinfo.ProjectPersonCategory = 0;
                model.projectBasicinfo.ProjectProblem = 0;
                model.projectBasicinfo.ProjectProblemDescribe = "";
                model.projectBasicinfo.Person = collection["Person"];
                this.IDKLManagerService.UpdateProjectInfo(model.projectBasicinfo);
            }
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
        public ActionResult UploadFiles(int id)
        {
            UploadModel = this.IDKLManagerService.GetProjectInfo(id);
            ProjectDocFile model = new ProjectDocFile();
            model.ProjectNumber = UploadModel.ProjectNumber;
            return View(model);
        }
        [HttpPost]
        public ActionResult UploadFiles(FormCollection collection)
        {
            //获取报告文档
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = files["docFile"];
            string fileName = "";
            if (file != null && file.ContentLength > 0)
            {
                fileName = GetFilePathByRawFile(file.FileName);
                file.SaveAs(fileName);
            }
            if (file == null || file.ContentLength == 0)
                return Back("未检测到上传文件！");
            var projectBasicDocFile = new ProjectDocFile();
            projectBasicDocFile.FilePath = fileName;
            projectBasicDocFile.CreateTime = DateTime.Now;
            projectBasicDocFile.ProjectNumber = UploadModel.ProjectNumber;
            this.IDKLManagerService.AddProjectDocFile(projectBasicDocFile);

            return Back("上传成功");
        }
    }
}