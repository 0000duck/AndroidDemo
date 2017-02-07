using DKLManager.Contract.Model;
using HYZK.Account.Contract;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class CostingTheGeneralManagerController : AdminControllerBase
    {
        //
        // GET: /DKLManager/TheGeneralManager/
        public ActionResult Index(CostingRequest request)
        {
            request.CostingState = (int)EnumCostingState.Directors;
            var result = this.IDKLManagerService.GetCostingList(request);
            return View(result);
        }
               public ActionResult Create()
        {
            var model = new Costing();
            ViewData.Add("Type", new SelectList(EnumHelper.GetItemValueList<EnumType>(), "Key", "Value", model.Type));
            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Costing();
            model.CostingState = (int)EnumCostingState.Check;
            try
            {
                this.TryUpdateModel<Costing>(model);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
            //技术成本
            model.TechnologyCosts = (model.PhysicalFactors * model.PhysicalFactorsPrice) +( model.DustPrice * model.Dust) + (model.Inorganic * model.InorganicPrice) +( model.Organic * model.OrganicPrice )+ (model.Free * model.FreePrice)+(model.WorkingHours*model.WorkingHoursPrice);
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
            model.GrossProfitMargin = t*100;


            try
            {
                this.IDKLManagerService.InsertCosting(model);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
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
        public ActionResult Edit(int id)
        {
            var model = this.IDKLManagerService.SelectCosting(id);
            if (model != null)
            {
                ViewData.Add("Type", new SelectList(EnumHelper.GetItemValueList<EnumType>(), "Key", "Value", model.Type));
            }
            return View(model);
        }

        //
        // POST: /Account/User/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.SelectCosting(id);
            if (model != null)
            {
                try
                {
                    this.TryUpdateModel<Costing>(model);
                }
                catch (Exception ex)
                {
                    return Back(ex.Message);
                }
            }
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
            try
            {
                this.IDKLManagerService.UpDateCosting(model);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
            return this.RefreshParent();
        }
        public ActionResult Submit(int id)
        {
            var model = this.IDKLManagerService.SelectCosting(id);
            if (model != null)
            {
                ViewData.Add("Type", new SelectList(EnumHelper.GetItemValueList<EnumType>(), "Key", "Value", model.Type));
            }
            return View(model);

        }
        [HttpPost]
        public ActionResult Submit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.SelectCosting(id);
            if (model != null)
            {
                try
                {
                    this.TryUpdateModel<Costing>(model);
                }
                catch (Exception ex)
                {
                    return Back(ex.Message);
                }
            }
            model.PhysicalFactorsPrice = 30; //物理因素单价
            model.DustPrice = 60; //粉尘类单价
            model.InorganicPrice = 130;//无机类单价
            model.OrganicPrice = 180;//有机类单价
            model.FreePrice = 250;//游离SiO2单价
            model.WorkingHours = 11.5;//工时
            model.WorkingHoursPrice = 125;//工时单价
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
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            var models = new TimeInstructions();           
            models.SignTime = model.SignTime.ToString();
            models.ProjectName = model.ProjectName;
            models.TimeNode = DateTime.Now;
            models.Instructions = user.LoginName+"审核通过，并提交到成本分析历史表中";
            try
            {
                this.IDKLManagerService.InsertTimeInstructions(models);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
            model.CostingState = (int)EnumCostingState.History;
            CostingHistory history = new CostingHistory();
            history.ProjectName = model.ProjectName;
            history.SignTime = model.SignTime;
            history.ProjectSynopsis = model.ProjectSynopsis;
            history.CustomerCounty = model.CustomerCounty;
            history.ContactsPserson = model.ContactsPserson;
            history.Sign = model.Sign;
            history.Towns = model.Towns;
            history.Relation = model.Relation;
            history.HeadOfPeople = model.HeadOfPeople;
            history.CostingState = model.CostingState;
            history.Sales = model.Sales;
            history.TechnologyCosts = model.TechnologyCosts;
            history.PromotionFee = model.PromotionFee;
            history.CollaborationFee = model.CollaborationFee;
            history.Commission = model.Commission;
            history.TrafficSubsidies = model.TrafficSubsidies;
            history.GrossProfit = model.GrossProfit;
            history.OtherFees = model.OtherFees;
            history.Tax = model.Tax;
            history.GrossProfitMargin = model.GrossProfitMargin;
            history.Remark = model.Remark;
            history.CreateTime = model.CreateTime;
            history.SamplePrice = model.SamplePrice;
            history.WorkingHours = model.WorkingHours;
            history.WorkingHoursPrice = model.WorkingHoursPrice;
            history.NumBerSum = model.NumBerSum;
            history.PhysicalFactors = model.PhysicalFactors;
            history.Dust = model.Dust;
            history.Inorganic = model.Inorganic;
            history.Organic = model.Organic;
            history.Free = model.Free;
            history.PhysicalFactorsPrice = model.PhysicalFactorsPrice;
            history.DustPrice = model.DustPrice;
            history.InorganicPrice = model.InorganicPrice;
            history.OrganicPrice = model.OrganicPrice;
            history.FreePrice = model.FreePrice;
            history.Person = model.Person;
            history.Type = model.Type;
            try
            {
                this.IDKLManagerService.AddCostingHistory(history);
                this.IDKLManagerService.UpDateCosting(model);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
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
	}
}
	
