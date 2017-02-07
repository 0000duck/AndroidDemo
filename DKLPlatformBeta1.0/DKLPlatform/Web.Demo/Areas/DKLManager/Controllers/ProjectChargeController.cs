using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DKLManager.Contract.Model;
using Web.Demo.Common;
using HYZK.FrameWork.Utility;
using HYZK.Account.Contract;
using Web.Common;
using Web.Demo.Areas.DKLManager.Models;
using System.IO;
using HYZK.Core.Upload;
using System.Drawing.Imaging;


namespace Web.Demo.Areas.DKLManager.Controllers
{
    /// <summary>
    /// 评价监测部主管
    /// </summary>
    public class ProjectChargeController : AdminControllerBase
    {
        //
        // GET: /DKLManager/ProjectChargeControl/
        public ActionResult Index(ProjectInfoRequest request)
        {
            ViewData.Add("ProjectPersonCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectPersonCategory>(), "Key", "Value"));
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            
            request.ProjectStatus = (int)EnumProjectSatus.QualityControlSubmit;
            request.ProjectCategory = (int)EnumProjectCategory.TestValue;
       //     ViewData.Add("ProjectPersonCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectPersonCategory>(), "Key", "Value"));
            var result = this.IDKLManagerService.GetProjectInfoList(request);
            var users = this.AccountService.GetUserList(6).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("ProjectLeader", new SelectList(users, "Name", "Name"));
            return View(result);
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
            this.IDKLManagerService.UpdateProjectFile(model.projectBasicImgFile);
            return this.RefreshParent();
        }
        public ActionResult Return(int id)
        {
            var model = new ProjectWholeInfoViewModel();
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (model.projectBasicinfo.ProjectLeader == null)
                return Back("请指定负责人,并点击更新按钮更新状态");
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
        public ActionResult Return(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.GetProjectInfo(id);
            this.TryUpdateModel<ProjectInfo>(model);
            model.ProjectStatus = (int)EnumProjectSatus.ProjectChargeSubmit;
            model.Remarks = "";
            model.ProjectPersonCategory = 0;
            this.IDKLManagerService.UpdateProjectInfo(model); 
            return this.RefreshParent();
        }
         //public ActionResult Return(int id, FormCollection collection)
         //{
         //    var model = this.IDKLManagerService.GetProjectInfo(id);
         //    this.TryUpdateModel<ProjectInfo>(model);
         //    model.ProjectStatus = (int)EnumProjectSatus.MarketSubmit;
         //    this.IDKLManagerService.UpdateProjectInfo(model);
         //    return RedirectToAction("Index");
         //}
      
        public ActionResult Submit(int id)
        {          
          
            var model = new ProjectInfo();
       
           // this.IDKLManagerService.UpdateProjectInfo(model.projectBasicinfo);
            model = this.IDKLManagerService.GetProjectInfo(id);
            return View("Submit", model);
       //     return this.RefreshParent();

        }

        [HttpPost]
        public ActionResult Submit(int id, FormCollection collection)
        {
            //var model = new ProjectWholeInfoViewModel();
            //model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            //this.TryUpdateModel<ProjectInfo>(model.projectBasicinfo);
            //if (model.projectBasicinfo != null)
            //{


            //    model.projectBasicinfo.ProjectStatus = (int)EnumProjectSatus.MarketSubmit;
            //    this.IDKLManagerService.UpdateProjectInfo(model.projectBasicinfo);
            //}
            var model = new ProjectInfo();
            model = this.IDKLManagerService.GetProjectInfo(id);
            model.ProjectStatus = (int)EnumProjectSatus.MarketSubmit;
            model.ProjectPersonCategory = 4;
            this.TryUpdateModel<ProjectInfo>(model);
            this.IDKLManagerService.UpdateProjectInfo(model);
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
    
    }
}