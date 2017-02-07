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
    public class AssessmentGoBackController : AdminControllerBase
    {
        //
       // GET: /DKLManager/AssessmentGoBack/
        public static ProjectInfo UploadModel = new ProjectInfo();
        public ActionResult Index(ProjectInfoRequest request)
        {
            
            request.ProjectStatus = (int)EnumProjectSatus.OneTwoThreeAssessor;
            request.ProjectCategory = (int)EnumProjectCategory.TestValue;
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            
            //var users = this.AccountService.GetUserList(7).Select(c => new { Id = c.ID, Name = c.Name });
            //ViewData.Add("ProjectCheif", new SelectList(users, "Name", "Name"));
            var result = this.IDKLManagerService.GetProjectInfoListT(request);
            ViewData["Name"] = user.Name;
            ViewData["Name"] = (user != null) ? user.Name : "";
            return View(result); 
        }
        public ActionResult WriteOption(int id)
        {
            var model = this.IDKLManagerService.GetProjectInfo(id);
            return View("WriteOption", model);
        }
        [HttpPost]
        public ActionResult WriteOption(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.GetProjectInfo(id);
            this.TryUpdateModel<ProjectInfo>(model);
            this.IDKLManagerService.UpdateProjectInfo(model);
            return this.RefreshParent();
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
        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var model = new ProjectWholeInfoViewModel();
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (model.projectBasicinfo != null)
            {
                model.projectBasicImgFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);
                model.projectConsultBasicinfo = this.IDKLManagerService.GetConsultBasicInfo(model.projectBasicinfo.ProjectNumber);
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
            //上传doc文件
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
                model.projectBasicImgFile = this.IDKLManagerService.GetProjectFile(model.projectBasicinfo.ProjectNumber);
                model.projectTestBasicinfo = this.IDKLManagerService.GetProjectTestBasicInfo(model.projectBasicinfo.ProjectNumber);
                model.projectValueBasicinfo = this.IDKLManagerService.GetVlaueProjectBasicInfo(model.projectBasicinfo.ProjectNumber);
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
            var model = new ProjectWholeInfoViewModel();
            model.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (model.projectBasicinfo != null)
            {
                model.projectConsultBasicinfo = this.IDKLManagerService.GetConsultBasicInfo(model.projectBasicinfo.ProjectNumber);
            }
            UpdateViewModel(collection, ref model);
            var docModel = this.IDKLManagerService.GetProjectDocFile(model.projectBasicinfo.ProjectNumber, model.projectBasicinfo.ProjectStatus);
            if (docModel != null)
            {
                model.projectBasicinfo.ProjectStatus = (int)EnumProjectSatus.ConsultManagerReview; ;
                docModel.Status = model.projectBasicinfo.ProjectStatus;
                this.IDKLManagerService.UpdateProjectDocFile(docModel);
                this.IDKLManagerService.UpdateProjectInfo(model.projectBasicinfo);
                this.IDKLManagerService.UpdateConsultBasicInfo(model.projectConsultBasicinfo);
                return this.RefreshParent();
            }
            else
            {
                return this.RefreshParent(GlobalData.warningInfo1);
            }

        }
        /// <summary>
        /// 上传下载页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CheckDoc(int id)
        {
            var infomodel = new ProjectInfo();
            var model = new ProjectDocFile();
            infomodel = this.IDKLManagerService.GetProjectInfo(id);
            if (infomodel != null)
            {
                //根据项目编号和状态查找文件Doc
                var users = this.AccountService.GetUserListAu(13).Select(c => new { Id = c.ID, Name = c.Name });
                ViewData.Add("Person", new SelectList(users, "Name", "Name"));
                ViewData.Add("Status", new SelectList(EnumHelper.GetItemValueList<EnumProjectAgree>(), "Key", "Value"));
                model = this.IDKLManagerService.GetProjectDocFile(infomodel.ProjectNumber, infomodel.ProjectStatus, infomodel.ID);
                this.IDKLManagerService.UpdateProjectDocFile(model);
            }
            return View(model);
        }
        /// <summary>
        /// 上传报告
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckDoc(int id, FormCollection collection)
        {
            ProjectInfo projectModel = this.IDKLManagerService.GetProjectInfo(id);
            var fileList = this.IDKLManagerService.GetProjectDocFileLists(projectModel.ProjectNumber);
            int flag = 0;
            foreach (var item in fileList)
            {
                if (item.Status == 15)
                {
                    flag = 1;
                    break;
                }
                if (item.Status == 12)
                {
                    flag = 1;
                    break;
                }
                if (item.Status == 10)
                {
                    flag = 1;
                    break;
                }
            }
            ////upload doc file
            //HttpFileCollectionBase files = Request.Files;
            //HttpPostedFileBase file = files["DocFileForUpload"];
            //string fileName = "";
            //if (file != null && file.ContentLength > 0)
            //{
            //    fileName = GetFilePathByRawFile(file.FileName);
            //    file.SaveAs(fileName);
            if (flag != 0)
            {

                //更新项目信息状态
                var infomodel = new ProjectInfo();
                infomodel = this.IDKLManagerService.GetProjectInfo(id);
                var models = new ProjectWholeInfoViewModel();
                if (infomodel.ProjectStatus == (int)EnumProjectSatus.ProjectModifyOne)
                {
                    infomodel.ProjectStatus = (int)EnumProjectSatus.ProjectModifyFour;
                    var nn = this.IDKLManagerService.SelectContractInfo(infomodel.ProjectName);
                    infomodel.SignTime = nn.ContractDate;
                    infomodel.Person = collection["Person"];
                    var models1 = new TimeInstructions();
                    models1.ProjectNumBer = infomodel.ProjectNumber;
                    models1.ProjectName = infomodel.ProjectName;
                    models1.TimeNode = DateTime.Now;
                    models1.SignTime = infomodel.SignTime.ToString();
                    models1.Instructions = "退回一审提交";
                    this.IDKLManagerService.InsertTimeInstructions(models1);
                }
                else if (infomodel.ProjectStatus == (int)EnumProjectSatus.ProjectModifyTwo)
                {
                    infomodel.ProjectStatus = (int)EnumProjectSatus.ProjectWorkFinish;
                    var nn = this.IDKLManagerService.SelectContractInfo(infomodel.ProjectName);
                    infomodel.SignTime = nn.ContractDate;
                    infomodel.Person = collection["Person"];
                    var models1 = new TimeInstructions();
                    models1.ProjectNumBer = infomodel.ProjectNumber;
                    models1.ProjectName = infomodel.ProjectName;
                    models1.TimeNode = DateTime.Now;
                    models1.SignTime = infomodel.SignTime.ToString();
                    models1.Instructions = "退回二审提交";
                    this.IDKLManagerService.InsertTimeInstructions(models1);
                }
                else if (infomodel.ProjectStatus == (int)EnumProjectSatus.ProjectVerifyFour)
                {
                    infomodel.ProjectStatus = (int)EnumProjectSatus.PersonW;
                    var nn = this.IDKLManagerService.SelectContractInfo(infomodel.ProjectName);
                    infomodel.SignTime = nn.ContractDate;
                    infomodel.Person = collection["Person"];
                    var models1 = new TimeInstructions();
                    models1.ProjectNumBer = infomodel.ProjectNumber;
                    models1.ProjectName = infomodel.ProjectName;
                    models1.TimeNode = DateTime.Now;
                    models1.SignTime = infomodel.SignTime.ToString();
                    models1.Instructions = "退回三审提交";
                    this.IDKLManagerService.InsertTimeInstructions(models1);
                }
                this.IDKLManagerService.UpdateProjectInfo(infomodel);

                //上传审核修改后doc文件记录
                //var model = new ProjectDocFile();
                //this.TryUpdateModel<ProjectDocFile>(model);
                //model.ProjectNumber = infomodel.ProjectNumber;
                //model.Status = infomodel.ProjectStatus;
                //model.FilePath = fileName;
                //this.IDKLManagerService.AddProjectDocFile(model);
                return this.RefreshParent();
            }
            else
            {
                return this.RefreshParent(GlobalData.warningInfo1); ;
            }
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
            projectBasicDocFile.Status = UploadModel.ProjectStatus;
            this.IDKLManagerService.AddProjectDocFile(projectBasicDocFile);

            return Back("上传成功");
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