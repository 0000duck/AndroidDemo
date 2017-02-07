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
    /// <summary>
    /// 评价监测组长
    /// </summary>
    public class ProjectLeaderController : AdminControllerBase
    {
        private static ProjectWholeInfoViewModel m_ProjectWholeInfoViewModel;
        /// <summary>
        /// 检测评价组长
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {

            ViewData.Add("ProjectPersonCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectPersonCategory>(), "Key", "Value"));
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            var users = this.AccountService.GetUserList(7).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("ProjectCheif", new SelectList(users, "Name", "Name"));

            var results = this.IDKLManagerService.GetProjectInfoList(user.Name,request);
            request.ProjectStatus = (int)EnumProjectSatus.ProjectChargeSubmit;
            request.ProjectCheif = user.Name;
            if (request.userName == user.Name)
            {
                request.ProjectStatus = (int)EnumProjectSatus.ProjectChargeSubmit;
                request.ProjectCategory = (int)EnumProjectCategory.TestValue;
            }
            ViewData["Name"] = user.Name;
            ViewData["Name"] = (user != null) ? user.Name : "";
            return View(results);
        }

       // public ActionResult Edit(int id)
       // {
       //     var model = new ProjectWholeInfoViewModel();
       //     model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
       //     if (model.projectBasicinfo != null)
       //     {
       //         model.projectBasicFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);
       //         string fileName = CreateBarCode(model.projectBasicinfo.ProjectNumber);               
       //         if (fileName != null)
       //         {
       //             string filePath = fileName.Substring(fileName.IndexOf("Upload") - 1);
       //             ViewData.Add("ProjectBarCodeImg", filePath);
       //         }
       //     }
       //    ViewData.Add("ProjectCategory", EnumHelper.GetEnumTitle((EnumProjectCategory)model.projectBasicinfo.ProjectCategory));
       ////      this.RenderMyViewData(model);

       //     return View(model);
       // }
        //
        // POST: /Account/User/Create
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    var model = new ProjectWholeInfoViewModel();
        //    model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
        //    if (model.projectBasicinfo != null)
        //    {
        //        model.projectBasicFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);
        //    }
        //    UpdateViewModel(collection, ref model);
        //    this.IDKLManagerService.UpdateProjectInfo(model.projectBasicinfo);
        //    this.IDKLManagerService.UpdateProjectFile(model.projectBasicFile);
        //    return this.RefreshParent();
        //}
        /// <summary>
        /// 退回项目
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Return(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.GetProjectInfo(id);
            model.ProjectPersonCategory = 4;
            this.TryUpdateModel<ProjectInfo>(model);
            model.ProjectStatus = (int)EnumProjectSatus.QualityControlSubmit;
            this.IDKLManagerService.UpdateProjectInfo(model);
            //return RedirectToAction("Index");
            return this.RefreshParent();
        }
        public ActionResult Return(int id)
        {
          
            var model = new ProjectInfo();
            
            return View(model);
        }

        public ActionResult Submit(int id)
        {
            var model = new ProjectWholeInfoViewModel();
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (model.projectBasicinfo.ProjectCheif == null)
                return Back("请指定负责人,并点击更新按钮更新状态");
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
            return View("Edit", model);

        }
        [HttpPost]
        public ActionResult Submit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.GetProjectInfo(id);
          
            if (model != null)
            {
                model.ProjectPersonCategory = 0;
                this.TryUpdateModel<ProjectInfo>(model);
                model.ProjectStatus = (int)EnumProjectSatus.ProjectVerifyOne;
                this.IDKLManagerService.UpdateProjectInfo(model);
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

        [HttpPost]
        public ActionResult View(FormCollection collection)
        {
            return this.RefreshParent();
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