using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DKLManager.Contract.Model;
using Web.Demo.Common;
using HYZK.FrameWork.Utility;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class VisitRecordController : AdminControllerBase
    {
        /// <summary>
        /// 访问记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
            var result = this.IDKLManagerService.GetVisitRecordList(request);
            return View(result);
        }


        public ActionResult Create()
        {
            var model = new VisitRecord();
            this.RenderMyViewData(model);
            var users = this.IDKLManagerService.GetCustomerList().Select(c => new { CustomerName = c.CustomerName });
            ViewData.Add("CustomerName", new SelectList(users, "CustomerName", "CustomerName"));

            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new VisitRecord();
            this.TryUpdateModel<VisitRecord>(model);

            try
            {
                this.IDKLManagerService.AddVisitRecord(model);
            }
            catch (HYZK.FrameWork.Common.BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                return View("Edit", model);
            }

            return this.RefreshParent();
        }

        public ActionResult Edit(int id)
        {
            var model = this.IDKLManagerService.GetVisitRecord(id);
            this.RenderMyViewData(model);
            var users = this.IDKLManagerService.GetCustomerList().Select(c => new { CustomerName = c.CustomerName });
            ViewData.Add("CustomerName", new SelectList(users, "CustomerName", "CustomerName"));

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.GetVisitRecord(id);
            this.TryUpdateModel<VisitRecord>(model);
            this.IDKLManagerService.UpdateVisitRecord(model);

            return this.RefreshParent();
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            if (ids != null)
            {
                this.IDKLManagerService.DeleteVisitRecord(ids);
            }
            return RedirectToAction("Index");
        }

        private void RenderMyViewData(VisitRecord model)
        {
            ViewData.Add("CognitiveChannel", new SelectList(EnumHelper.GetItemValueList<EnumCognitiveChannel>(), "Key", "Value", model.CognitiveChannel));
            ViewData.Add("PriceResponse", new SelectList(EnumHelper.GetItemValueList<EnumPriceResponse>(), "Key", "Value", model.PriceResponse));
            ViewData.Add("VisitWay", new SelectList(EnumHelper.GetItemValueList<EnumVisitWay>(), "Key", "Value", model.VisitWay));
            ViewData.Add("FollowStep", new SelectList(EnumHelper.GetItemValueList<EnumFollowStep>(), "Key", "Value", model.FollowStep));
            ViewData.Add("FollowLevel", new SelectList(EnumHelper.GetItemValueList<EnumFollowLevel>(), "Key", "Value", model.FollowLevel));    
        }
    }
}

