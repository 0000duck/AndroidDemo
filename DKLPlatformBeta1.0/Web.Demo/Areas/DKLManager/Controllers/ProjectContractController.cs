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
using System.Web.Script.Serialization;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ProjectContractController : AdminControllerBase
    {
        public static ProjectContract UploadModel = new ProjectContract();
        //
        // GET: /DKLManager/ProjectContract/
        public ActionResult Index(ProjectInfoRequest request)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            request.ProjectStatus = (int)EnumProjectSatus.Begin;
            var result = this.IDKLManagerService.GetProjectContractListPerson(request);         
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
        public ActionResult UploadFiles(int id)
        {
            UploadModel = this.IDKLManagerService.GetProjectContractInfo(id);
            ProjectDocFile model = new ProjectDocFile();
            model.ProjectNumber = UploadModel.ProjectNumber;
            return View(model);
        }
        [HttpPost]
        public ActionResult UploadFiles(FormCollection collection)
        {
            if (collection["ProjectNumber"] == "" || collection["ProjectNumber"] == null)
                return this.RefreshParent("请填写项目编号！");
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
            projectBasicDocFile.ProjectNumber = collection["ProjectNumber"];
            UploadModel.ProjectNumber = collection["ProjectNumber"];
            this.IDKLManagerService.UpdateProjectContract(UploadModel);
            this.IDKLManagerService.AddProjectDocFile(projectBasicDocFile);
            return this.RefreshParent("上传成功");
            //return Back("上传成功");
        }
        public ActionResult Create()
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            var model = new CreateNew();
            model.ProjectContracts.ProjectClosingDate = Convert.ToDateTime(DateTime.Now.ToLongDateString());
            model.ProjectContracts.ContractDate = Convert.ToDateTime(DateTime.Now.ToLongDateString());
            ViewData.Add("ProjectContracts.ProjectCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectCategory>(), "Key", "Value", model.ProjectContracts.ProjectCategory));
            model.ProjectContracts.AuthorizedPersonB = "北京德康莱健康安全科技股份有限公司";
            model.ProjectContracts.AuthorizedPersonJobB = "日常监测";
            model.ProjectContracts.AddressB = "北京经济技术开发区西环南路18号B座";
            model.ProjectContracts.TelB = "010-51570158";
            model.ProjectContracts.ContactPersonB = "王辉";
            model.Costings.SignTime = Convert.ToDateTime(DateTime.Now.ToLongDateString());
            ViewData.Add("Costings.Type", new SelectList(EnumHelper.GetItemValueList<EnumType>(), "Key", "Value", model.Costings.Type));
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(ProjectInfoRequest request,FormCollection collection)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            var model = new CreateNew();
            model.Costings.ProjectName = collection["Costings.ProjectName"];
            model.Costings.ProjectSynopsis = collection["Costings.ProjectSynopsis"];
            model.Costings.HeadOfPeople = collection["Costings.HeadOfPeople"];
            model.Costings.Type = collection["Costings.Type"];
            model.Costings.CustomerCounty = collection["Costings.CustomerCounty"];
            model.Costings.ContactsPserson = collection["Costings.ContactsPserson"];
            model.Costings.Towns = collection["Costings.Towns"];
            model.Costings.Relation = collection["Costings.Relation"];
            model.Costings.Sales =Convert.ToDouble( collection["Costings.Sales"]);
            model.Costings.PromotionFee = Convert.ToDouble( collection["Costings.PromotionFee"]);
            model.Costings.OtherFees = Convert.ToInt32( collection["Costings.OtherFees"]);
            model.Costings.Free = Convert.ToDouble( collection["Costings.Free"]);
            model.Costings.PhysicalFactors = Convert.ToDouble( collection["Costings.PhysicalFactors"]);
            model.Costings.Dust = Convert.ToDouble( collection["Costings.Dust"]);
            model.Costings.Inorganic = Convert.ToDouble( collection["Costings.Inorganic"]);
            model.Costings.Organic = Convert.ToDouble( collection["Costings.Organic"]);
            model.Costings.Remark = collection["Costings.Remark"];
            model.Costings.SignTime = Convert.ToDateTime( collection["Costings.SignTime"]);
            model.Costings.Sign = collection["Costings.Sign"];

            model.ProjectContracts.ProjectCategory =Convert.ToInt32( collection["ProjectContracts.ProjectCategory"]);
            model.ProjectContracts.ProjectName = collection["ProjectContracts.ProjectName"];
            model.ProjectContracts.CompaneName = collection["ProjectContracts.CompaneName"];
            model.ProjectContracts.ContractDate =Convert.ToDateTime( collection["ProjectContracts.ContractDate"]);
            model.ProjectContracts.CompanyAddress = collection["ProjectContracts.CompanyAddress"];
            model.ProjectContracts.FaxA = collection["ProjectContracts.FaxA"];
            model.ProjectContracts.ContactPersonA = collection["ProjectContracts.ContactPersonA"];
            model.ProjectContracts.ContactTel = collection["ProjectContracts.ContactTel"];
            model.ProjectContracts.AuthorizedPersonB = collection["ProjectContracts.AuthorizedPersonB"];
            model.ProjectContracts.EmailA = collection["ProjectContracts.EmailA"];
            model.ProjectContracts.ContactPersonB = collection["ProjectContracts.ContactPersonB"];
            model.ProjectContracts.TelB = collection["ProjectContracts.TelB"];
            model.ProjectContracts.AuthorizedPersonJobB = collection["ProjectContracts.AuthorizedPersonJobB"];
            model.ProjectContracts.ProjectClosingDate = Convert.ToDateTime( collection["ProjectContracts.ProjectClosingDate"]);
            //model.ProjectContracts.ProjectClosingDate = model.ProjectContracts.ContractDate;
        
            model.ProjectContracts.ProjectStatus = -4;
            model.ProjectContracts.MarketPerson = user.Name;
            model.ProjectContracts.Person = model.ProjectContracts.MarketPerson;
            this.IDKLManagerService.InsertProjectContract(model.ProjectContracts);
            TimeInstructions ins = new TimeInstructions();
            ins.SignTime = model.ProjectContracts.ContractDate.ToString();
            ins.ProjectName = model.ProjectContracts.ProjectName;
            ins.TimeNode = DateTime.Now;
            ins.Instructions = user.LoginName+"填写合同分析表";
            this.IDKLManagerService.InsertTimeInstructions(ins);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;

            model.Costings.CostingState = (int)EnumCostingState.Check;
            //this.TryUpdateModel<Costing>(model);
            model.Costings.NumBerSum = model.Costings.PhysicalFactors + model.Costings.Organic + model.Costings.Free + model.Costings.Inorganic + model.Costings.Dust;//样品数量
            if (model.Costings.NumBerSum <= 0)
            {
                //return Content("<script>alert('样品数量的和小于等于零，请重新填写!');</script>");
                return Back("样品数量的和不能小于等于零，请重新填写!");
            }
            model.Costings.Person = request.userName;
            //技术成本
            model.Costings.PhysicalFactorsPrice = 30; //物理因素单价
            model.Costings.DustPrice = 60; //粉尘类单价
            model.Costings.InorganicPrice = 130;//无机类单价
            model.Costings.OrganicPrice = 180;//有机类单价
            model.Costings.FreePrice = 250;//游离SiO2单价
            model.Costings.WorkingHours = 11.5;//工时
            model.Costings.WorkingHoursPrice = 125;//工时单价
            model.Costings.TechnologyCosts = (model.Costings.PhysicalFactors * model.Costings.PhysicalFactorsPrice) + (model.Costings.DustPrice * model.Costings.Dust) + (model.Costings.Inorganic * model.Costings.InorganicPrice) + (model.Costings.Organic * model.Costings.OrganicPrice) + (model.Costings.Free * model.Costings.FreePrice) + (model.Costings.WorkingHours * model.Costings.WorkingHoursPrice);
            if (model.Costings.TechnologyCosts <= 0)
            {
                //return View("<script>alert('请填写有效的样品数量!');</script>");
                return Back("请填写有效的样品数量！");

            }
            double q = 0.1;
            double s = 1.03;
            double y = 0.03;
            double qq = model.Costings.Sales - model.Costings.PromotionFee;
            if (qq <= 0)
            {
                return Back("销售额减去推广费不能小于零，请重新填写！！");
                // return View("<script >alert('销售额减去推广费为不能小于零，请重新填写！');</script >", "text/html");
            }
            model.Costings.CollaborationFee = (model.Costings.Sales - model.Costings.PromotionFee) * q;//协作费
            double jj = model.Costings.Sales - model.Costings.PromotionFee - model.Costings.CollaborationFee;
            if (jj <= 0)
            {
                return Back("销售额减去推广费减去协作费不能小于零，请重新填写！！");
                //return View("<script >alert('销售额减去推广费减去写作费不能小于零，请重新填写！');</script >", "text/html");
            }
            model.Costings.Commission = (model.Costings.Sales - model.Costings.CollaborationFee - model.Costings.PromotionFee) * q;
            model.Costings.Tax = model.Costings.Sales / s * y;
            string tas = model.Costings.Tax.ToString("#0.00");
            double tass = Convert.ToDouble(tas);
            model.Costings.Tax = tass;
            if ((model.Costings.Sales - model.Costings.PromotionFee) >= 3000)
            {
                model.Costings.TrafficSubsidies = 50;
            }
            //毛利润
            model.Costings.GrossProfit = model.Costings.Sales - model.Costings.TechnologyCosts - model.Costings.PromotionFee - model.Costings.CollaborationFee - model.Costings.Commission - model.Costings.TrafficSubsidies - model.Costings.OtherFees - model.Costings.Tax;
            if (model.Costings.GrossProfit <= 0)
            {
                Response.Write("<script>alert('经计算毛利润小于等于零！！！');</script>");
                //return Back("经计算毛利润小于等于零，已返回！！！")               
            }

            string result = model.Costings.GrossProfit.ToString("#0.00");//点后面几个0就保留几位 
            double n = Convert.ToDouble(result);
            model.Costings.GrossProfit = n;

            model.Costings.GrossProfitMargin = model.Costings.GrossProfit / jj;
            if (model.Costings.GrossProfitMargin <= 0)
            {
                Response.Write("<script>alert('经计算毛利润率小于等于零！！！');</script>");
                //("经计算毛利润率小于等于零，已返回！！！");               
            }
            string results = model.Costings.GrossProfitMargin.ToString("#0.0000");
            double t = Convert.ToDouble(results);
            model.Costings.GrossProfitMargin = t * 100;
            var mod = new TimeInstructions();
            //var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            mod.ProjectName = model.Costings.ProjectName;
            mod.SignTime = model.Costings.SignTime.ToLongDateString();
            mod.TimeNode = DateTime.Now;
            mod.Instructions = user.LoginName + "填写成本分析表";
            this.IDKLManagerService.InsertTimeInstructions(mod);
            this.IDKLManagerService.InsertCosting(model.Costings);

            return this.RefreshParent();
        }
        public ActionResult Edit(int id)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            var model = new ProjectContract();
            model = this.IDKLManagerService.GetProjectContractInfo(id);
            model.PayRatioFirst = "50";
            ViewData.Add("ProjectCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectCategory>(), "Key", "Value", model.ProjectCategory));
          
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id,FormCollection collection)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            var model = this.IDKLManagerService.GetProjectContractInfo(id);
            model.MarketPerson = user.Name;
            this.TryUpdateModel(model);
            //if (model.Area != null)
            //{
            //    if (model.Area.Contains("区"))
            //    {
            //        int i = model.Area.IndexOf("区");
            //        if (i != -1)
            //            model.Area = model.Area.Remove(i, "区".Length); // 结果在返回值中
            //    }
            //    if (model.Area.Contains("县"))
            //    {
            //        int i = model.Area.IndexOf("县");
            //        if (i != -1)
            //            model.Area = model.Area.Remove(i, "县".Length); // 结果在返回值中
            //    }
            //}
            this.IDKLManagerService.UpdateUserInputList((int)EnmuUserInputType.CompanyName, model.CompaneName);
            this.IDKLManagerService.UpdateUserInputList((int)EnmuUserInputType.CompanyAddress, model.CompanyAddress);
          
            //this.IDKLManagerService.TryToUpdateArea(model.Area);
            this.IDKLManagerService.UpdateProjectContract(model);
            return this.RefreshParent();
        }

        public ActionResult Submit(int id)
        {

            var model = this.IDKLManagerService.GetProjectContractInfo(id);
            if (model.CompaneName == null || model.CompaneName == "")
                return this.RefreshParent("请点击编辑填写详细合同信息");
            ViewData.Add("ProjectCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectCategory>(), "Key", "Value", model.ProjectCategory));

            return View("Refer",model);
        }
        [HttpPost]
        public ActionResult Submit(int id,FormCollection collection)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            var model = this.IDKLManagerService.GetProjectContractInfo(id);
            if (model.ProjectCategory == 5)
            {
                model.EstimateStatus = 2;
                model.EstimateStatusProblem = "";
                this.IDKLManagerService.UpdateProjectContract(model);
            }
            model.ProjectStatus = -3;
            var info = new TimeInstructions();
            info.ProjectName = model.ProjectName;
            info.SignTime = model.ContractDate.ToString();
            info.TimeNode = DateTime.Now;
            info.Instructions =user.LoginName+ "提交到合同审核";
            this.IDKLManagerService.InsertTimeInstructions(info);
            this.IDKLManagerService.UpdateProjectContract(model);
            return this.RefreshParent();
        }
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            if (ids == null)
                return RedirectToAction("Index");
            this.IDKLManagerService.DeleteProjectContract(ids);
            return RedirectToAction("Index");
        }
        public ActionResult GetCompanyList(string search)
        {
            var list = this.IDKLManagerService.GetUserInputList((int)EnmuUserInputType.CompanyName, search);
            JavaScriptSerializer jsS = new JavaScriptSerializer();
            var result = jsS.Serialize(list);
            //  return Content("返回一个字符串");
            return Content(result);

        }
        public ActionResult GetCompanyAddressList(string search)
        {
            var list = this.IDKLManagerService.GetUserInputList((int)EnmuUserInputType.CompanyAddress, search);
            JavaScriptSerializer jsS = new JavaScriptSerializer();
            var result = jsS.Serialize(list);
            //  return Content("返回一个字符串");
            return Content(result);

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
        [HttpPost]
        public ActionResult CreateWordTOther(FormCollection collection)
        {
            string strFileName = "";
            //   CreateContractTestingAndEvaluation cr = new CreateContractTestingAndEvaluation();                       
            AttorneyBook cr = new AttorneyBook();
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
        public ActionResult CreateWordOther()
        {
            string strFileName = "";
            //   CreateContractTestingAndEvaluation cr = new CreateContractTestingAndEvaluation();                       
            AttorneyBook cr = new AttorneyBook();
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
            return this.CloseThickbox();
        }
        public ActionResult CreateWord(int id)
        {
            var money = 0;
            var model = new ProjectContract();
            model = this.IDKLManagerService.GetProjectContractInfo(id);
            money = Convert.ToInt32(model.Money);
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
            if (model.ProjectCategory == 1)
                ViewData.Add("ContractType", new SelectList(EnumHelper.GetItemValueList<EnumContractTypeEvaluate>(), "Key", "Value"));
            if (model.ProjectCategory == 0)
                ViewData.Add("ContractType", new SelectList(EnumHelper.GetItemValueList<EnumContractType>(), "Key", "Value"));
            if (model.ProjectCategory == 5)
            {
                return Back("这里没有合同");
            }

            return View(model);
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
	}
}