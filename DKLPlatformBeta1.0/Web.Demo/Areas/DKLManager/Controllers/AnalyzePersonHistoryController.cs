using DKLManager.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;
using Web.Demo.Areas.DKLManager.Models;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class AnalyzePersonHistoryController : AdminControllerBase
    {
        //
        // GET: /DKLManager/AnalyzePersonHistory/
        public ActionResult Index(ProjectInfoRequest request)
        {
            

                request.SampleState = (int)EnumSampleStates.OldSample;
                 var ArgumentList = this.IDKLManagerService.GetSampleRegisterTableList(request);
                 return View(ArgumentList);

            

        }
	}
}