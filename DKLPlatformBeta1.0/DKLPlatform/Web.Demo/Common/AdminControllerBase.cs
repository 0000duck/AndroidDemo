using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web;
using Newtonsoft.Json;
using HYZK.Core.Config;
using HYZK.FrameWork.Common;
using HYZK.FrameWork.Utility;
using HYZK.Account.Contract;
using Web.Common;
using HYZK.FrameWork.Web;
using Web.Demo.Areas.DKLManager.Models;
using DKLManager.Contract.Model;
using System.IO;

namespace Web.Demo.Common
{
    public abstract class AdminControllerBase : Web.Common.ControllerBase
    {
        public AdminCookieContext CookieContext
        {
            get
            {
                return AdminCookieContext.Current;
            }
        }

        public AdminUserContext UserContext
        {
            get
            {
                return AdminUserContext.Current;
            }
        }

        public CachedConfigContext ConfigContext
        {
            get
            {
                return CachedConfigContext.Current;
            }
        }


        /// <summary>
        /// 重写分页Size
        /// </summary>
        public override int PageSize
        {
            get
            {
                return 12;
            }
        }

        /// <summary>
        /// 操作人，为了记录操作历史
        /// </summary>
        public override Operater Operater
        {
            get
            {
                return new Operater()
                {
                    Name = this.LoginInfo == null ? "" : this.LoginInfo.LoginName,
                    Token = this.LoginInfo == null ? Guid.Empty : this.LoginInfo.LoginToken,
                    UserId = this.LoginInfo == null ? 0 : this.LoginInfo.UserID,
                    Time = DateTime.Now,
                    IP = Fetch.UserIp
                };
            }
        }

        /// <summary>
        /// 用户Token，每次页面都会把这个UserToken标识发送到服务端认证
        /// </summary>
        public virtual Guid UserToken
        {
            get
            {
                return CookieContext.UserToken;
            }
        }

        /// <summary>
        /// 登录后用户信息
        /// </summary>
        public virtual LoginInfo LoginInfo
        {
            get
            {
                return UserContext.LoginInfo;
            }
        }

        protected void UpdateViewModel(FormCollection collection, ref ProjectWholeInfoViewModel model)
        {         
            model.projectBasicinfo.ProjectNumber = collection["projectBasicinfo.ProjectNumber"];
            model.projectBasicinfo.ProjectName = collection["projectBasicinfo.ProjectName"];
            string strCategory = (collection["projectBasicinfo.ProjectCategory"] == null) ? collection["ProjectCategory"] : collection["projectBasicinfo.ProjectCategory"];
            int category;
            if (int.TryParse(strCategory, out category))
            {
                model.projectBasicinfo.ProjectCategory = category;
            }
            model.projectBasicinfo.ZipCode = collection["projectBasicinfo.ZipCode"];
            model.projectBasicinfo.CompaneName = collection["projectBasicinfo.CompaneName"];
            model.projectBasicinfo.CompanyAddress = collection["projectBasicinfo.CompanyAddress"];
            model.projectBasicinfo.CompanyContact = collection["projectBasicinfo.CompanyContact"];
            model.projectBasicinfo.ContactTel = collection["projectBasicinfo.ContactTel"];
            model.projectBasicinfo.ProjectClosingDate =Convert.ToDateTime( collection["ProjectClosingDate"]);
            

            //缺少检测评价部的数据更新
            model.projectBasicImgFile.FilePath = collection["projectBasicImgFile.FilePath"];
            //model.projectBasicDocFile.FilePath = collection["projectBasicDocFile.FilePath"];
            //model.projectBasicDocFile.ProjectNumber = collection["projectBasicinfo.ProjectNumber"];

            //model.projectBasicFile.FilePath = collection["projectBasicFile.FilePath"];
            //model.projectBasicFile.ProjectNumber = collection["projectBasicinfo.ProjectNumber"];
        }
        protected void RenderMyViewData(ProjectWholeInfoViewModel model)
        {
            ViewData.Add("ProjectCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectCategory>(), "Key", "Value", model.projectBasicinfo.ProjectCategory));
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
        }
        protected void RenderMyViewDatas(ProjectWholeInfoViewModel model)
        {
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
        }
        protected void RenderMyViewDatas(ProjectWholeInfoViewModelHis model)
        {
            ViewData.Add("ProjectCategory", EnumHelper.GetEnumTitle((EnumProjectCategory)model.projectBasicinfoHistory.ProjectCategory));
            if ((model.projectBasicFileHistory != null) && (!string.IsNullOrEmpty(model.projectBasicFileHistory.FilePath)))
            {
                List<string> htmlFIles = new List<string>();
                model.projectBasicFileHistory.FilePath.Split(',').ToList().ForEach(f =>
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
        }

        protected bool IsProjectDone(string projectNumber)
        {
            bool bRet = false;
            var infomodel = new ProjectInfo();
            infomodel = this.IDKLManagerService.GetProjectInfo(projectNumber);
            if (infomodel.ProjectCategory == (int)EnumProjectCategory.Test)
            {
                if (infomodel.ProjectStatus == (int)EnumProjectSatus.ProjectModifyThree)
                {
                    bRet = true;
                }
            }
            else if (infomodel.ProjectCategory == (int)EnumProjectCategory.Value)
            {
                if (infomodel.ProjectStatus == (int)EnumProjectSatus.ProjectModifyFour)
                {
                    bRet = true;
                }
            }
            else if (infomodel.ProjectCategory == (int)EnumProjectCategory.Consult)
            {
                if (infomodel.ProjectStatus == (int)EnumProjectSatus.ProjectModifyOne)
                {
                    bRet = true;
                }
            }
            else 
            {
            }

            return bRet;
        }

        protected void MoveProjectData(string projectNumber)
        {
            var infomodel = new ProjectInfo();
            infomodel = this.IDKLManagerService.GetProjectInfo(projectNumber);
            if (infomodel != null)
            {
                ProjectInfoHistory history = ProjectInfoHistory.Clone(infomodel);
                history.ProjectRealClosingDate = DateTime.Now;
                this.IDKLManagerService.AddProjectInfoHistory(history);
            }
            this.IDKLManagerService.DeleteProjectInfo(projectNumber);

            var valueInfo = new ValueBasicInfo();
            valueInfo = this.IDKLManagerService.GetVlaueProjectBasicInfo(projectNumber);
            if (valueInfo != null)
            {
                ValueBasicInfoHistory history = ValueBasicInfoHistory.Clone(valueInfo);
                this.IDKLManagerService.AddVlaueProjectBasicInfoHistory(history);
            }
            this.IDKLManagerService.DeleteVlaueProjectBasicInfo(projectNumber);
            var testChemical = new TestChemicalReport();
            testChemical = this.IDKLManagerService.GetTestTestChemicalReport(projectNumber);
            if (testChemical != null)
            {
                TestChemicalReportListHistory chemicalHistory = TestChemicalReportListHistory.Clone(testChemical);
                this.IDKLManagerService.AddTestChemicalReportListHistory(chemicalHistory);
            }
            this.IDKLManagerService.DeleteTestChemicalReport(projectNumber);
            var testInfo = new TestBasicInfo();
            testInfo = this.IDKLManagerService.GetProjectTestBasicInfo(projectNumber);
            if (testInfo != null)
            {
                TestBasicInfoHistory history = TestBasicInfoHistory.Clone(testInfo);
                this.IDKLManagerService.AddProjectTestBasicInfoHistory(history);
            }
            this.IDKLManagerService.DeleteProjectTestBasicInfo(projectNumber);

            var consultInfo = new ConsultBasicInfo();
            consultInfo = this.IDKLManagerService.GetConsultBasicInfo(projectNumber);
            if (consultInfo != null)
            {
                ConsultBasicInfoHistory history = ConsultBasicInfoHistory.Clone(consultInfo);
                this.IDKLManagerService.AddConsultBasicInfoHistory(history);
            }
            this.IDKLManagerService.DeleteConsultBasicInfo(projectNumber);

            var fileInfo = new ProjectFile();
            fileInfo = this.IDKLManagerService.GetProjectFile(projectNumber);
            if (fileInfo != null)
            {
                ProjectFileHistory history = ProjectFileHistory.Clone(fileInfo);
                this.IDKLManagerService.AddProjectFileHistory(history);
            }
            this.IDKLManagerService.DeleteProjectFile(projectNumber);

            var docList = this.IDKLManagerService.GetProjectDocFileList(projectNumber);
            ProjectDocFileHistory docHistory = new ProjectDocFileHistory();
            foreach(var doc in docList)
            {
                docHistory = ProjectDocFileHistory.Clone(doc);
                this.IDKLManagerService.AddProjectDocFileHistory(docHistory);
            } 
            this.IDKLManagerService.DeleteProjectDocFile(projectNumber);
        }

        protected string GetFilePathByRawFile(string rawFile)
        {
            string ret;
            if (rawFile.LastIndexOf("\\") > -1)
            {
                rawFile = rawFile.Substring(rawFile.LastIndexOf("\\") + 1);
            }
            //if ((fileName.LastIndexOf('.') > -1 && fileName.Substring(fileName.LastIndexOf('.')).ToUpper() == "."))
            {
                string path = Server.MapPath("~//App_Data//");
                try
                {
                    string strDateTime = DateTime.Now.ToString("yyyyMMddhhmmss");
                    string filePath = path + strDateTime + "\\";
                    if (!System.IO.Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    ret = filePath + rawFile;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return ret;
        }



        #region Override controller methods
        /// <summary>
        /// 方法执行前，如果没有登录就调整到Passport登录页面，没有权限就抛出信息
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var noAuthorizeAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AuthorizeIgnoreAttribute), false);
            if (noAuthorizeAttributes.Length > 0)
                return;

            base.OnActionExecuting(filterContext);

            if (this.LoginInfo == null)
            {
                filterContext.Result = RedirectToAction("Login", "Auth", new { Area = "Account" });
                return;
            }
            else
            {
                var user = this.AccountService.GetUser(this.LoginInfo.LoginName);

                Web.Demo.Common.AdminMenu.CurrentMenu = CachedConfigContext.Current.AdminMenuConfig.AdminMenuGroups[user.AccountType];
            }



            //bool hasPermission = true;
            //var permissionAttributes = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(PermissionAttribute), false).Cast<PermissionAttribute>();
            //permissionAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(PermissionAttribute), false).Cast<PermissionAttribute>().Union(permissionAttributes);
            //var attributes = permissionAttributes as IList<PermissionAttribute> ?? permissionAttributes.ToList();
            //if (permissionAttributes != null && attributes.Count() > 0)
            //{
            //    hasPermission = true;
            //    foreach (var attr in attributes)
            //    {
            //        foreach (var permission in attr.Permissions)
            //        {
            //            if (!this.LoginInfo.BusinessPermissionList.Contains(permission))
            //            {
            //                hasPermission = false;
            //                break;
            //            }
            //        }
            //    }

            //    if (!hasPermission)
            //    {
            //        if (Request.UrlReferrer != null)
            //            filterContext.Result = this.Stop("没有权限！", Request.UrlReferrer.AbsoluteUri);
            //        else
            //            filterContext.Result = Content("没有权限！");
            //    }
            //}
        }

        /// <summary>
        /// 方法后执行后注入一些视图数据
        /// </summary>
        /// <param name="filterContext">filter context</param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {   
            
            if (filterContext.ActionDescriptor.ActionName.Contains("Edit") ||
                filterContext.ActionDescriptor.ActionName.Contains("Add"))
                return;

            RenderViewData();
        }

        /// <summary>
        /// 如果是Ajax请求的话，清除浏览器缓存
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                filterContext.HttpContext.Response.Cache.SetNoStore();
            }

            base.OnResultExecuted(filterContext);
        }

        /// <summary>
        /// 注入资源，权限，城市等信息
        /// </summary>
        protected override void RenderViewData()
        {
            //var permissions = string.Join(",", this.PermissionList);
            //this.ViewData["permissions"] = permissions;
        }

        #endregion

    }
}
