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
    public class CostingDoingController : AdminControllerBase
    {
        //
        // GET: /DKLManager/CostingDoing/
        public ActionResult Index(CostingRequest request)
        {
            request.CostingState = (int)EnumCostingState.Except;
            var result = this.IDKLManagerService.GetCostingList(request);
            ViewData.Add("CostingState", new SelectList(EnumHelper.GetItemValueList<EnumCostingState>(), "Key", "Value"));
            ViewData.Add("Type", new SelectList(EnumHelper.GetItemValueList<EnumType>(), "Key", "Value"));
            return View(result);
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
	}
}