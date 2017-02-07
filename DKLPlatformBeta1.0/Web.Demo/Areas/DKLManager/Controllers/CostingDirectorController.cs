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
    public class CostingDirectorController : AdminControllerBase
    {
        //
        // GET: /DKLManager/CostingDirector/
        public ActionResult Index(CostingRequest request)
        {
            request.CostingState = (int)EnumCostingState.Director;
                var result = this.IDKLManagerService.GetCostingList(request);
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
        public ActionResult Return(int id)
        {

            var model = this.IDKLManagerService.SelectCosting(id);
            if (model != null)
            {
                ViewData.Add("Type", new SelectList(EnumHelper.GetItemValueList<EnumType>(), "Key", "Value", model.Type));
            }
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Return(int id, FormCollection collection)
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
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            var mod = new TimeInstructions();
            mod.ProjectName = model.ProjectName;
            mod.SignTime = model.SignTime.ToString();
            mod.TimeNode = DateTime.Now;
            mod.Instructions =user.LoginName+ "退回";
            try
            {
                this.IDKLManagerService.InsertTimeInstructions(mod);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
            model.CostingState = (int)EnumCostingState.Check;
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
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            var mod = new TimeInstructions();
            mod.ProjectName = model.ProjectName;
            mod.SignTime = model.SignTime.ToString();
            mod.TimeNode = DateTime.Now;
            mod.Instructions =user.LoginName+ "审核通过";
            try
            {
                this.IDKLManagerService.InsertTimeInstructions(mod);
                model.CostingState = (int)EnumCostingState.VicePresident;
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
            return View("Edit", model);

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
            var mod = new TimeInstructions();
            mod.ProjectName = model.ProjectName;
            mod.SignTime = model.SignTime.ToString();
            mod.TimeNode = DateTime.Now;
            mod.Instructions = "市场主管提交";
            try
            {
                this.IDKLManagerService.InsertTimeInstructions(mod);
                model.CostingState = (int)EnumCostingState.Director;
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
                try
                {
                    this.IDKLManagerService.DeleteCosting(ids);
                }
                catch (Exception ex)
                {
                    return Back(ex.Message);
                }
            }
            return RedirectToAction("Index");
        }
	}
}