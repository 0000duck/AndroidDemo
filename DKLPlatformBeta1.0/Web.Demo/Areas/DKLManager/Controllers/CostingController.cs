using DKLManager.Contract.Model;
using HYZK.Account.Contract;
using HYZK.FrameWork.Utility;
using OfficeDocGenerate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class CostingController : AdminControllerBase
    {
        //
        // GET: /DKLManager/Costiong/
        public ActionResult Index(CostingRequest request)
        {
            request.CostingState =(int)EnumCostingState.Check;
            var result = this.IDKLManagerService.GetCostingList(request);            
            return View(result);
        }

        public ActionResult GetCompanyList(string search)
        {
            var list = this.AccountService.GetUserList();
            List<string> NameList = new List<string>();
            foreach (var item in list)
            {
                if (item.LoginName.Contains(search))
                {
                    NameList.Add(item.LoginName);
                }
                
            }
            JavaScriptSerializer jsS = new JavaScriptSerializer();
            var result = jsS.Serialize(NameList);
            //  return Content("返回一个字符串");
            return Content(result);
           
        }
        public ActionResult Create()
        {
            var model = new Costing();
           // model.SignTime = DateTime.Now;
            model.SignTime = Convert.ToDateTime(DateTime.Now.ToLongDateString());

            ViewData.Add("Type", new SelectList(EnumHelper.GetItemValueList<EnumType>(), "Key", "Value"));
            return View("Edit", model);
        }
        public ActionResult Course(string ProjectName, string SignTime)
        {

            List<TimeInstructions> result = new List<TimeInstructions>();
            if (ProjectName != null && SignTime != null)
            {

                result = this.IDKLManagerService.SelectTimeInstructions(ProjectName, SignTime);
            }
            return View(result);
        }
        [HttpPost]
        public ActionResult Create(ProjectInfoRequest request,FormCollection collection)
        {
            var user1 = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user1.AccountType;
            request.userName = user1.Name;
            var model = new Costing();
          
            model.CostingState = (int)EnumCostingState.Check;
            this.TryUpdateModel<Costing>(model);
            model.NumBerSum = model.PhysicalFactors + model.Organic + model.Free + model.Inorganic + model.Dust;//样品数量
            if (model.NumBerSum <= 0)
            {
                //return Content("<script>alert('样品数量的和小于等于零，请重新填写!');</script>");
                return Back("样品数量的和不能小于等于零，请重新填写!");
            }
            model.Person = request.userName;
            //技术成本
            model.PhysicalFactorsPrice = 30; //物理因素单价
            model.DustPrice = 60; //粉尘类单价
            model.InorganicPrice = 130;//无机类单价
            model.OrganicPrice = 180;//有机类单价
            model.FreePrice = 250;//游离SiO2单价
            model.WorkingHours = 11.5;//工时
            model.WorkingHoursPrice = 125;//工时单价
            model.TechnologyCosts = (model.PhysicalFactors * model.PhysicalFactorsPrice) +( model.DustPrice * model.Dust) + (model.Inorganic * model.InorganicPrice) +( model.Organic * model.OrganicPrice )+ (model.Free * model.FreePrice)+(model.WorkingHours*model.WorkingHoursPrice);
            if (model.TechnologyCosts <= 0)
            {
                //return View("<script>alert('请填写有效的样品数量!');</script>");
                return Back("请填写有效的样品数量！");

            }
            
            double q = 0.1;
            double s = 1.03;
            double y = 0.03;
            double qq = model.Sales - model.PromotionFee;
            if (qq <= 0)
            {
                return Back("销售额减去推广费不能小于零，请重新填写！！");
               // return View("<script >alert('销售额减去推广费为不能小于零，请重新填写！');</script >", "text/html");
            }
            model.CollaborationFee = (model.Sales - model.PromotionFee) * q;//协作费
            double jj = model.Sales - model.PromotionFee - model.CollaborationFee;
            if (jj <= 0)
            {
                return Back("销售额减去推广费减去协作费不能小于零，请重新填写！！");
                //return View("<script >alert('销售额减去推广费减去写作费不能小于零，请重新填写！');</script >", "text/html");
            }
            model.Commission = (model.Sales - model.CollaborationFee - model.PromotionFee) * q;
            model.Tax = model.Sales / s * y;
            string tas = model.Tax.ToString("#0.00");
            double tass = Convert.ToDouble(tas);
            model.Tax = tass;
            if ((model.Sales - model.PromotionFee) >= 3000)
            {
                model.TrafficSubsidies = 50;
            }
            //毛利润
            model.GrossProfit = model.Sales - model.TechnologyCosts - model.PromotionFee - model.CollaborationFee - model.Commission - model.TrafficSubsidies - model.OtherFees - model.Tax;
            if (model.GrossProfit <= 0)
            {
                Response.Write("<script>alert('经计算毛利润小于等于零！！！');</script>"); 
                //return Back("经计算毛利润小于等于零，已返回！！！")               
            }
                
            string result = model.GrossProfit.ToString("#0.00");//点后面几个0就保留几位 
            double n = Convert.ToDouble(result);
            model.GrossProfit = n;

            model.GrossProfitMargin = model.GrossProfit / jj;
            if (model.GrossProfitMargin <= 0)
            {
                Response.Write("<script>alert('经计算毛利润率小于等于零！！！');</script>"); 
               //("经计算毛利润率小于等于零，已返回！！！");               
            }
            string results = model.GrossProfitMargin.ToString("#0.0000");
            double t = Convert.ToDouble(results);
            model.GrossProfitMargin = t*100;
            var mod = new TimeInstructions();
            //var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            mod.ProjectName = model.ProjectName;
            mod.SignTime =model.SignTime.ToLongDateString();
            mod.TimeNode = DateTime.Now;
            mod.Instructions = user1.LoginName + "填写成本分析表";
            this.IDKLManagerService.InsertTimeInstructions(mod);
            this.IDKLManagerService.InsertCosting(model);
            return this.RefreshParent();
        }


        public ActionResult Edit(int id)
        {
            var model = this.IDKLManagerService.SelectCosting(id);
            ViewData.Add("Type", new SelectList(EnumHelper.GetItemValueList<EnumType>(), "Key", "Value"));
            return View(model);
        }

        //
        // POST: /Account/User/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, ProjectInfoRequest request)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            var model = this.IDKLManagerService.SelectCosting(id);
            ViewData.Add("Type", new SelectList(EnumHelper.GetItemValueList<EnumType>(), "Key", "Value"));
            this.TryUpdateModel<Costing>(model);
            model.TechnologyCosts = (model.PhysicalFactors * model.PhysicalFactorsPrice) + (model.DustPrice * model.Dust) + (model.Inorganic * model.InorganicPrice) + (model.Organic * model.OrganicPrice) + (model.Free * model.FreePrice) + (model.WorkingHours * model.WorkingHoursPrice);
            model.NumBerSum = model.PhysicalFactors + model.Organic + model.Free + model.Inorganic + model.Dust;//样品数量
            double q = 0.1;
            double s = 1.03;
            double y = 0.03;
            model.CollaborationFee = (model.Sales - model.PromotionFee) * q;
            model.Commission = (model.Sales - model.CollaborationFee) * q;


            model.Tax = model.Sales / s * y;
            string tas = model.Tax.ToString("#0.00");
            double tass = Convert.ToDouble(tas);
            model.Tax = tass;
            if ((model.Sales - model.PromotionFee) >= 3000)
            {
                model.TrafficSubsidies = 50;
            }

            //毛利润
            model.GrossProfit = model.Sales - model.TechnologyCosts - model.PromotionFee - model.CollaborationFee - model.Commission - model.TrafficSubsidies - model.OtherFees - model.Tax;
            string result = model.GrossProfit.ToString("#0.00");//点后面几个0就保留几位 
            double n = Convert.ToDouble(result);
            model.GrossProfit = n;


            model.GrossProfitMargin = model.GrossProfit / (model.Sales - model.PromotionFee - model.CollaborationFee);
            string results = model.GrossProfitMargin.ToString("#0.0000");
            double t = Convert.ToDouble(results);
            model.GrossProfitMargin = t * 100;
            this.IDKLManagerService.UpDateCosting(model);
            return this.RefreshParent();
        }
        public ActionResult Submit(int id)
        {
            var model = this.IDKLManagerService.SelectCosting(id);
            ViewData.Add("Type", new SelectList(EnumHelper.GetItemValueList<EnumType>(), "Key", "Value", model.Type));
            return View(model);

        }
        [HttpPost]
        public ActionResult Submit(int id, FormCollection collection)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            var model = this.IDKLManagerService.SelectCosting(id);
            model.Person = user.Name;
            this.TryUpdateModel<Costing>(model);
            model.TechnologyCosts = (model.PhysicalFactors * model.PhysicalFactorsPrice) + (model.DustPrice * model.Dust) + (model.Inorganic * model.InorganicPrice) + (model.Organic * model.OrganicPrice) + (model.Free * model.FreePrice) + (model.WorkingHours * model.WorkingHoursPrice);
            model.NumBerSum = model.PhysicalFactors + model.Organic + model.Free + model.Inorganic + model.Dust;//样品数量
            double q = 0.1;
            double s = 1.03;
            double y = 0.03;
            model.CollaborationFee = (model.Sales - model.PromotionFee) * q;
            model.Commission = (model.Sales - model.CollaborationFee) * q;


            model.Tax = model.Sales / s * y;
            string tas = model.Tax.ToString("#0.00");
            double tass = Convert.ToDouble(tas);
            model.Tax = tass;
            if ((model.Sales - model.PromotionFee) >= 3000)
            {
                model.TrafficSubsidies = 50;
            }

            //毛利润
            model.GrossProfit = model.Sales - model.TechnologyCosts - model.PromotionFee - model.CollaborationFee - model.Commission - model.TrafficSubsidies - model.OtherFees - model.Tax;
            string result = model.GrossProfit.ToString("#0.00");//点后面几个0就保留几位 
            double n = Convert.ToDouble(result);
            model.GrossProfit = n;          
            model.GrossProfitMargin = model.GrossProfit / (model.Sales - model.PromotionFee - model.CollaborationFee);
            string results = model.GrossProfitMargin.ToString("#0.0000");
            double t = Convert.ToDouble(results);
            model.GrossProfitMargin = t * 100;
            model.CostingState = (int)EnumCostingState.Director;         
            var mod = new TimeInstructions();
            mod.ProjectName = model.ProjectName;
            mod.SignTime = model.SignTime.ToString();
            mod.TimeNode = DateTime.Now;
            mod.Instructions = user.LoginName+"提交";
            this.IDKLManagerService.InsertTimeInstructions(mod);
            this.IDKLManagerService.UpDateCosting(model);
            return this.RefreshParent();


        }
        // POST: /Account/User/Delete/5

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            if (ids != null)
            {
                this.IDKLManagerService.DeleteCosting(ids);
            }
            return RedirectToAction("Index");
        }
        public ActionResult CreateWord(int id)
        {
            var model = new Costing();
            model = this.IDKLManagerService.SelectCosting(id);
            ViewData.Add("Type", new SelectList(EnumHelper.GetItemValueList<EnumType>(), "Key", "Value", model.Type));
            
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateWord(int id, FormCollection collection)
        {
            var model = new Costing();
            model = this.IDKLManagerService.SelectCosting(id);
            Word(model);
            return this.RefreshParent();
        }
        public ActionResult Word(Costing Model)
        {

            if (Model.Type == "0"||Model.Type == "1"||Model.Type == "6")
            {
                string strFileName = "";
                CreateAnalysis cr = new CreateAnalysis(Model);
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
            else if (Model.Type == "2" || Model.Type == "4" || Model.Type == "5")
            {
                string strFileName = "";
                CreateJianCe cr = new CreateJianCe(Model);
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
            else
            {
                string strFileName = "";
                CreateWater cr = new CreateWater(Model);
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
            // CreateContractControlResultEvaluation cr = new CreateContractControlResultEvaluation(Model);
            
        }
	}
}