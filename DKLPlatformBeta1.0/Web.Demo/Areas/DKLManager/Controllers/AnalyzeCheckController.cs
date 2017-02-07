using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using OfficeDocGenerate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Areas.DKLManager.Models;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class AnalyzeCheckController : SampleBaseController
    {
        //private static ProjectWholeInfoViewModel m_ProjectWholeInfoViewModel;
        public static ProjectContract UploadModel = new ProjectContract();
        public static SampleRegisterTable UploadModels = new SampleRegisterTable();
        public ActionResult Index(ProjectInfoRequest request)
        {
            var users = this.AccountService.GetUserList(10).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Person", new SelectList(users, "Name", "Name"));
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            request.SampleStates = (int)EnumSampleStates.Fcheck;
            var parameNew = this.IDKLManagerService.GetSampleRegisterTableList(user.Name, request);

            return View(parameNew);
        }
        public ActionResult Show(ProjectInfoRequest request)            ///显示所有水质采样项目    去重  然后显示有采样的水质项目
        {
            List<ProjectInfo> modelList = new List<ProjectInfo>();
            var models = this.IDKLManagerService.SelectAllSampleRegisterList().Select(c => new { ProjectNumber = c.ProjectNumber }).Distinct();
            foreach(var item in models)
            {
                var Pro = this.IDKLManagerService.SelectProjectInfo(item.ProjectNumber);
                if (Pro != null)
                {
                    if (Convert.ToInt32(Pro.ProjectCategory) == 5)
                    {
                        modelList.Add(Pro);
                    }
                }
                //else
                //{
                //    return Back("项目中没有这个项目编号");
                //}
            }
            return View(modelList);
        }
        public ActionResult Check(int id, ProjectInfoRequest request)
        {
            var model = new SampleRegisterTable();
            var users = this.AccountService.GetUserList(10).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Person", new SelectList(users, "Name", "Name"));
            model = this.IDKLManagerService.GetSampleRegisterTable(id);
            this.IDKLManagerService.UpDateSampleRegister(model);
            model.SampleStates = (int)EnumSampleStates.NewSample;
            return View("Edit", model);

        }
        [HttpPost]
        public ActionResult Check(int id, FormCollection collection)
        {
            var model = new SampleRegisterTable();
            //model = this.IDKLManagerService.GetSampleRegisterTable(id);
            //model.AnalyzePeople = collection["Person"];
            //model.SampleStates = (int)EnumSampleStates.PersonCheck;
            //this.IDKLManagerService.UpDateSampleRegister(model);
            this.IDKLManagerService.DeleteSampleRegisterD(id);
            return this.RefreshParent();
        }
        [HttpPost]
        public ActionResult UpdateAlls(ProjectInfoRequest request, FormCollection collection, List<int> ids)
        {
            if (collection["Person"] == null || collection["Person"] == "")
            {
                return Back("请选择分析人！");
            }
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            request.SampleStates = (int)EnumSampleStates.Fcheck;
            if (ids != null)
            {
                foreach (var item in ids)
                {
                    var temp = this.IDKLManagerService.GetSampleRegisterTable(item);
                    temp.SampleStates = (int)EnumSampleStates.PersonCheck;
                    temp.AnalyzePeople = collection["Person"];
                    this.IDKLManagerService.UpDateSampleRegister(temp);
                }
            }

            return RedirectToAction("");
        }
        public ActionResult UploadFiles(int id)
        {
            UploadModels = this.IDKLManagerService.GetSampleRegisterTable(id);
            ProjectDocFile model = new ProjectDocFile();
            model.ProjectNumber = UploadModels.SampleRegisterNumber;
            return View();
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

            projectBasicDocFile.ProjectNumber = UploadModels.SampleRegisterNumber;
            this.IDKLManagerService.AddProjectDocFile(projectBasicDocFile);

            return Back("上传成功");
        }
    
        public ActionResult DownLoadTestDocFile1(int id)        ///下载水质监测报告      传入的值id为项目id
        {
            //try
            //{
            string strFileName = "";

            //1.判断是否满足下载条件
            //2.从各个数据库获取数据
            //3.调用接口生成报告

            var projectmodels = this.IDKLManagerService.GetProjectInfo(id);        ///项目信息
            List<string> strc = new List<string>();
           
            var chemicalmodels = this.IDKLManagerService.GetChemicalReport(projectmodels.ProjectNumber);
            var models = this.IDKLManagerService.GetSampleRegisterListByProjectNumber(projectmodels.ProjectNumber);

            #region 化学数据对接
            foreach (var item in models)
            {
                TestChemicalReport model = new TestChemicalReport();
                model.SampleNumber = item.SampleRegisterNumber;
                model.ProjectNumber = item.ProjectNumber;
                model.SampleProject = item.ParameterName;
                model.Factor = item.ParameterName;
                model.ID = item.ID;
                model.CreateTime = item.SamplingDay;
                model.WorkShop = item.WorkShop;
                model.Job = item.Job;
                model.Location = item.Location;
                model.CSTEL = item.CSTEL;
                model.CTWA = item.CTWA;
                model.CMAC = item.CMAC;
                chemicalmodels.Add(model);
            }
            #endregion

            foreach (var item in chemicalmodels)
            {
                strc.Add(item.SampleProject);
            }
            //strc = strc.Distinct().ToList();          
            #region 报告生成+下载
            CreatWaterTestDoc cr = new CreatWaterTestDoc(projectmodels);
            if ((projectmodels != null) && (chemicalmodels != null))
            {
                List<string> appList = new List<string>();

                appList = cr.CreateReportWord(strc, projectmodels, chemicalmodels);


                #region 判断报告生成运行状态
                if (appList[0] == "1")
                {
                    FileInfo fr = new FileInfo(appList[1]);
                    fr.Delete();
                    return Back("检测项目中缺少对应参数，请联系实验室人员添加相应参数");
                }
                if (appList[0] == "2")
                {
                    FileInfo fr = new FileInfo(appList[1]);
                    fr.Delete();
                    return Back("采样项目中缺少对应参数，请联系实验室人员添加相应参数");
                }
                if (appList[0] == "3")
                {
                    FileInfo fr = new FileInfo(appList[1]);
                    fr.Delete();
                    return Back("采样项目未检测到数据，请联系实验室人员添加相应数据");
                }

                #endregion

                strFileName = appList[1];
            }
                //报告下载



                if (!string.IsNullOrEmpty(strFileName))
                {

                    string fileNewName = strFileName.Substring(strFileName.LastIndexOf("\\") + 1);
                    Response.Clear();
                    Response.ContentType = "application/octet-stream";
                    Response.ContentEncoding = Encoding.UTF8;
                    Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileNewName, Encoding.UTF8));
                    Response.WriteFile(strFileName);
                    Response.End();
                }
                else
                {
                    return Back("下载报告失败");
                }

            #endregion
                //}
                //catch (Exception e)
                //{
                //    return Back("1"+e.Message); 
                //}
                return Back("成功");
            
        }
    }
}
