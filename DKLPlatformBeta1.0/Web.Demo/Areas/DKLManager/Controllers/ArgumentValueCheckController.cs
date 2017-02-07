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
    public class ArgumentValueCheckController : AdminControllerBase
    {
        public ActionResult Index(ProjectInfoRequest request)
        {
                request.ParameterState = (int)EnumParameterState.OldParameter;
                var ArgumentList = this.IDKLManagerService.GetArgumentValueList(request);
                return View(ArgumentList);
        }
    }
}



