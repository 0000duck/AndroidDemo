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
    public class ProjectContractDoingController : AdminControllerBase
    {
        //
        // GET: /DKLManager/ProjectContractDoing/
        public ActionResult Index(ProjectInfoRequest request)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            ViewData.Add("ContractType", new SelectList(EnumHelper.GetItemValueList<EnumContractType>(), "Key", "Value"));

            var result = this.IDKLManagerService.GetProjectAllContractListDoingPerson();//request
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
        public ActionResult Edit(int id)
        {
            var model = new ProjectContract();
            model = this.IDKLManagerService.GetProjectContractInfo(id);
            var problem = new ContractProblem();
            problem.MarketStatus = model.MarketStatus;
            problem.WorkerStatus = model.WorkerStatus;
            problem.TestStatus = model.TestStatus;
            problem.QualityStatus = model.QualityStatus;
            problem.GeneralAccountingDepartmentStatus = model.GeneralAccountingDepartmentStatus;
            problem.EstimateStatus = model.EstimateStatus;
            if (model.ProjectCategory != 5)
            {
                if (model.EstimateStatusProblem == null || model.GeneralAccountingDepartmentStatusProblem == null || model.MarketStatusProblem == null || model.TestStatusProblem == null || model.QualityStatusProblem == null || model.WorkerStatusProblem == null)
                {
                    string alert = "本合同未审核的部门有：";
                    if (model.EstimateStatus == 0)
                        alert += "职评部  ";
                    if (model.GeneralAccountingDepartmentStatus == 0)
                        alert += "财务部  ";
                    if (model.MarketStatus == 0)
                        alert += "市场部  ";
                    if (model.TestStatus == 0)
                        alert += "检测部  ";
                    if (model.QualityStatus == 0)
                        alert += "质管部  ";
                    if (model.WorkerStatus == 0)
                        alert += "技术负责人  ";
                    alert += "请联系相关部门参与审核";
                    return this.RefreshParent(alert);
                }
            }
            else
            {
                if ( model.GeneralAccountingDepartmentStatusProblem == null || model.MarketStatusProblem == null || model.TestStatusProblem == null || model.QualityStatusProblem == null || model.WorkerStatusProblem == null)
                {
                    string alert = "本合同未审核的部门有：";
                    if (model.EstimateStatus == 0)
                        alert += "职评部  ";
                    if (model.GeneralAccountingDepartmentStatus == 0)
                        alert += "财务部  ";
                    if (model.MarketStatus == 0)
                        alert += "市场部  ";
                    if (model.TestStatus == 0)
                        alert += "检测部  ";
                    if (model.QualityStatus == 0)
                        alert += "质管部  ";
                    if (model.WorkerStatus == 0)
                        alert += "技术负责人  ";
                    alert += "请联系相关部门参与审核";
                    return this.RefreshParent(alert);
                }
            }
            problem.listMarket = new List<string>(model.MarketStatusProblem.Split(','));
            problem.listTest = new List<string>(model.TestStatusProblem.Split(','));
            problem.listWorker = new List<string>(model.WorkerStatusProblem.Split(','));
            problem.listQuality = new List<string>(model.QualityStatusProblem.Split(','));
            problem.listGeneralAccountingDepartment = new List<string>(model.GeneralAccountingDepartmentStatusProblem.Split(','));
            problem.listEstimate = new List<string>(model.EstimateStatusProblem.Split(','));  
            return View(problem);
        }
        [HttpPost]
        public ActionResult Edit()
        {
            return this.RefreshParent();
        }
        public ActionResult Submit(int id)
        {

            var model = this.IDKLManagerService.GetProjectContractInfo(id);
            ViewData.Add("ProjectCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectCategory>(), "Key", "Value", model.ProjectCategory));

            if(model.WorkerStatus == 2&&model.TestStatus ==2&&model.QualityStatus ==2&&model.MarketStatus==2&&model.EstimateStatus==2&&model.GeneralAccountingDepartmentStatus ==2)
            {
               
                return View("Refer",model);
            }
            else
            return this.RefreshParent("审核工作未完成或未通过审核，请点击查询审核状态了解更多");
        }
        [HttpPost]
        public ActionResult Submit(int id, FormCollection collection)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            var model = this.IDKLManagerService.GetProjectContractInfo(id);         
            model.ProjectStatus = -2;
            ProjectInfo project = new ProjectInfo();
            project.ProjectCategory = model.ProjectCategory;
            project.ProjectNumber = model.ProjectNumber;
            project.ProjectName = model.ProjectName;
            project.CompaneName = model.CompaneName;
            project.CompanyAddress = model.CompanyAddress;
            project.CompanyContact = model.CompanyContact;
            project.ContactTel = model.ContactTel;
            project.ZipCode = model.ZipCode;
            project.ProjectClosingDate = model.ProjectClosingDate;
            project.CreateTime = DateTime.Now;
            project.ProjectStatus = 1;
            project.Area = model.Area;
            project.MakeOutAnInvoiceTime = DateTime.Now;
            project.Person = user.Name;



            var nn = this.IDKLManagerService.SelectContractInfo(model.ProjectName);
            project.SignTime = nn.ContractDate;
            project.ProjectName = nn.ProjectName;
            project.ProjectNumber = nn.ProjectNumber;
            var models = new TimeInstructions();
            models.SignTime = project.SignTime.ToString();
            models.ProjectNumBer = project.ProjectNumber;
            models.ProjectName = project.ProjectName;
            models.Instructions = user.LoginName+"项目已创建成功到市场部";
            models.SignTime = project.SignTime.ToLongDateString();
            models.TimeNode = DateTime.Now;
            this.IDKLManagerService.InsertTimeInstructions(models);
            this.IDKLManagerService.AddProjectInfo(project);
            ProjectFile file = new ProjectFile();
            file.ProjectNumber = project.ProjectNumber;
            file.FilePath = "";
            file.CreateTime = project.CreateTime;
            

            this.IDKLManagerService.AddProjectFile(file);
            this.IDKLManagerService.UpdateProjectContract(model);
            return this.RefreshParent("提交成功,项目已创建,如需上传文件请通知市场部");
        }
        public ActionResult WORD0(ProjectContract Model)
        {
            string strFileName = "";
            //   CreateContractTestingAndEvaluation cr = new CreateContractTestingAndEvaluation();                       
            CreateContractControlResultEvaluation cr = new CreateContractControlResultEvaluation(Model);
            List<string> appList = new List<string>();
            appList = cr.CreateReportWord();
            #region 判断报告生成运行状态
            if (appList[0] == "1")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }
            if (appList[0] == "2")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }
            if (appList[0] == "3")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }

            #endregion

            strFileName = appList[1];
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
            return Back("成功");
        }
        public ActionResult WORD1(ProjectContract Model)
        {
            string strFileName = "";
            //   CreateContractTestingAndEvaluation cr = new CreateContractTestingAndEvaluation();                       
            CreateContractHazardAssessment cr = new CreateContractHazardAssessment(Model);
            List<string> appList = new List<string>();
            appList = cr.CreateReportWord();
            #region 判断报告生成运行状态
            if (appList[0] == "1")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }
            if (appList[0] == "2")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }
            if (appList[0] == "3")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }

            #endregion

            strFileName = appList[1];
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
            return Back("成功");
        }
        public ActionResult WORD2(ProjectContract Model)
        {
            string strFileName = "";
            //   CreateContractTestingAndEvaluation cr = new CreateContractTestingAndEvaluation();                       
            CreateContractPre_Assessment cr = new CreateContractPre_Assessment(Model);
            List<string> appList = new List<string>();
            appList = cr.CreateReportWord();
            #region 判断报告生成运行状态
            if (appList[0] == "1")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }
            if (appList[0] == "2")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }
            if (appList[0] == "3")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }

            #endregion

            strFileName = appList[1];
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
            return Back("成功");
        }
       
        public ActionResult WORD3(ProjectContract Model)
        {
            string strFileName = "";
            //   CreateContractTestingAndEvaluation cr = new CreateContractTestingAndEvaluation();                       
            CreateContractTestingAndEvaluation cr = new CreateContractTestingAndEvaluation(Model);
            List<string> appList = new List<string>();
            appList = cr.CreateReportWord();
            #region 判断报告生成运行状态
            if (appList[0] == "1")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }
            if (appList[0] == "2")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }
            if (appList[0] == "3")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }

            #endregion

            strFileName = appList[1];
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
            return Back("成功");
        }
         public ActionResult WORD4(ProjectContract Model)
        {
            string strFileName = "";
            //   CreateContractTestingAndEvaluation cr = new CreateContractTestingAndEvaluation();                       
            Testingandevaluationreport cr = new Testingandevaluationreport(Model);
            List<string> appList = new List<string>();
            appList = cr.CreateReportWord();
            #region 判断报告生成运行状态
            if (appList[0] == "1")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }
            if (appList[0] == "2")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }
            if (appList[0] == "3")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }

            #endregion

            strFileName = appList[1];
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
            return Back("成功");
        }
     
      
        public ActionResult WordTest(ProjectContract Model)
        {
            string strFileName = "";
            //   CreateContractTestingAndEvaluation cr = new CreateContractTestingAndEvaluation();                       
            CreateOccupationalHarmDoc cr = new CreateOccupationalHarmDoc();
            List<string> appList = new List<string>();
            appList = cr.CreateReportWord();
            #region 判断报告生成运行状态
            if (appList[0] == "1")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }
            if (appList[0] == "2")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }
            if (appList[0] == "3")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }

            #endregion

            strFileName = appList[1];
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
            return Back("成功");
        }
        public ActionResult CreateWord(int id)
        {                
            var money=0;
            var model = new ProjectContract();
            model = this.IDKLManagerService.GetProjectContractInfo(id);
            money=Convert.ToInt32(model.Money);
            //if (model.MarketStatus != 2 || model.TestStatus != 2 || model.QualityStatus != 2 || model.WorkerStatus != 2||model.GeneralAccountingDepartmentStatus!=2||model.EstimateStatus!=2)
            //{
            //    return this.RefreshParent("本合同审核工作尚未完成，请稍后再试");
            //}
            if (model.ProjectCategory == 2)

                if (money >= 10000)
                {
                    ViewData.Add("ContractType", new SelectList(EnumHelper.GetItemValueList<EnumContractTypeCheck1>(), "Key", "Value"));
                }
                else
                {
                    ViewData.Add("ContractType", new SelectList(EnumHelper.GetItemValueList<EnumContractTypeCheck2>(), "Key", "Value"));
                }
            if(model.ProjectCategory == 1)
                ViewData.Add("ContractType", new SelectList(EnumHelper.GetItemValueList<EnumContractTypeEvaluate>(), "Key", "Value"));
            if(model.ProjectCategory == 0)
                ViewData.Add("ContractType", new SelectList(EnumHelper.GetItemValueList<EnumContractType>(), "Key", "Value"));
            if (model.ProjectCategory == 5)
            {
                return this.RefreshParent("当前不需要生成合同！");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateWord(int id, FormCollection collection)
        {
            var model = new ProjectContract();
            model = this.IDKLManagerService.GetProjectContractInfo(id);
            if (model.ProjectCategory == 2)
            { 
              if (collection["ContractType"] == "0")
                  WORD3(model);
              if (collection["ContractType"] == "1")
                  WORD4(model);
              
            }
            if (model.ProjectCategory == 1)
            {
                if (collection["ContractType"] == "0")
                    WORD0(model);
                if (collection["ContractType"] == "1")
                    WORD1(model);
                if (collection["ContractType"] == "2")
                    WORD2(model);

            }
            if (model.ProjectCategory == 0)
            {
                if (collection["ContractType"] == "0")
                    WORD0(model);
                if (collection["ContractType"] == "1")
                    WORD1(model);
                if (collection["ContractType"] == "2")
                    WORD2(model);
                if (collection["ContractType"] == "3")
                    WORD3(model);
            }
            
            return this.RefreshParent();
        }
        public ActionResult CreateWordTest(FormCollection collection)
        {
            var model = new ProjectContract();
            WordTest(model);
            return this.RefreshParent();
        }
	}

}