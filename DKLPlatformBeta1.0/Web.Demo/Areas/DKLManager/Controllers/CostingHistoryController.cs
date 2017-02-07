using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Areas.DKLManager.Models;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class CostingHistoryController : AdminControllerBase
    {
        //
        // GET: /DKLManager/CostingHistory/
        public ActionResult Index(CostingRequest request)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            if (user != null)
            {
                request.userName = user.Name;
                request.UserAccountType = user.AccountType;
                request.CostingState = (int)EnumCostingState.History;
                if (user.AccountType == 1)
                {
                    var result = this.IDKLManagerService.GetProjectInfoHistoryListPerson(request);
                    if (result != null)
                    {
                        return View(result);
                    }
                    else
                    {
                        return Back("错误");
                    }
                }
                else
                {
                    var result = this.IDKLManagerService.GetProjectInfoHistoryList(request);
                    if (result != null)
                    {
                        return View(result);
                    }
                    else
                    {
                        return Back("错误");
                    }
                }
                
            }
            return View();
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