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
    public class WaterDetectionCheckController : AdminControllerBase
    {
        public ActionResult Index(ProjectInfoRequest request)
        {
            request.ParameterState = (int)EnumParameterState.NewParameter;
            var ArgumentList = this.IDKLManagerService.GetArgumentValueList(request);
            return View(ArgumentList);
        }
    }
}
