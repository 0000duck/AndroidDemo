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
    public class SampleCheckController : SampleBaseController
    {
        public ActionResult Index(ProjectInfoRequest request)
        {
            var users = this.AccountService.GetUserListB(10).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Person", new SelectList(users, "Name", "Name"));
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            request.SampleStates = (int)EnumSampleStates.Selec;
            var parameNew = this.IDKLManagerService.GetSampleRegisterTableList(user.Name, request);
            return View(parameNew);
        }
        public ActionResult Check(int id, ProjectInfoRequest request)
        {
            var model = new SampleRegisterTable();
            var users = this.AccountService.GetUserList(21).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Person", new SelectList(users, "Name", "Name"));
            model = this.IDKLManagerService.GetSampleRegisterTable(id);
            this.IDKLManagerService.UpDateSampleRegister(model);            
            return View("Edit", model);

        }
        [HttpPost]
        public ActionResult Check(int id, FormCollection collection)
        {
            var model = new SampleRegisterTable();
            //model = this.IDKLManagerService.GetSampleRegisterTable(id);
            //model.AnalyzePeople = collection["Person"];
            //model.SampleStates = (int)EnumSampleStates.Sumbit;
            //this.IDKLManagerService.UpDateSampleRegister(model);
            this.IDKLManagerService.DeleteSampleRegisterD(id);
            return this.RefreshParent();
        }
        [HttpPost]
        public ActionResult UpdateAlls(ProjectInfoRequest request, FormCollection collection, List<int> ids)
        {
            if (collection["Person"] == null || collection["Person"] == "")
            {
                return Back("请选择分析人！");
            }
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            request.SampleStates = (int)EnumSampleStates.Selec;
            if (ids != null)
            {
                foreach (var item in ids)
                {
                    var temp = this.IDKLManagerService.GetSampleRegisterTable(item);
                    temp.SampleStates = (int)EnumSampleStates.Sumbit;
                    temp.AnalyzePeople = collection["Person"];
                    this.IDKLManagerService.UpDateSampleRegister(temp);
                }
            }

            return RedirectToAction("");
        }
    }
}
