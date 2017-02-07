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
    public class LabController : SampleBaseController
    {
        //
        // GET: /DKLManager/Lab/
        public ActionResult Index(ProjectInfoRequest request)
        {
            var userb = this.AccountService.GetUserListF(10).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Person", new SelectList(userb, "Name", "Name"));
            request.SampleStates = (int)EnumSampleStates.NewSample;
            var results = this.IDKLManagerService.GetProjectInfoListP(request);
            if (results != null)
            {
                foreach (var r in results)
                {
                    if (r.ProjectCategory == 5)
                    {
                        //var resuls = this.IDKLManagerService.GetSampleRegisterTableList(request);
                        var usera = this.AccountService.GetUserListF(10).Select(c => new { Id = c.ID, Name = c.Name });
                        ViewData.Add("PersonW", new SelectList(usera, "Name", "Name"));
                        request.SampleStates = (int)EnumSampleStates.NewSample;
                        var resuls = this.IDKLManagerService.GetSampleRegisterTableList(request.userName, request);
                        var users = this.AccountService.GetUser(this.LoginInfo.LoginName);
                        request.UserAccountType = users.AccountType;
                        request.userName = users.Name;
                        ViewData.Add("ProjectPersonCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectPersonCategory>(), "Key", "Value"));
                        return View("WaterIndex", resuls);
                    }
                    //else
                    //{
                    //    var userv = this.AccountService.GetUserListF(10).Select(c => new { Id = c.ID, Name = c.Name });
                    //    ViewData.Add("Person", new SelectList(userv, "Name", "Name"));
                    //    request.SampleStates = (int)EnumSampleStates.NewSample;
                    //}
                }
            }
            
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);           
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            var parameNew = this.IDKLManagerService.GetSampleRegisterTableList(request.userName, request);
            return View(parameNew);
        }
        public ActionResult DataDispose(ProjectInfoRequest request, string projectNumber)
        {
            request.SampleState = (int)EnumSampleStates.NewSample;
            ViewData.Add("AnalyzePeople", new SelectList(EnumHelper.GetItemValueList<EnumAnalyzePeople>(), "Key", "Value"));
            var result = this.IDKLManagerService.GetSampleRegisterTableList(request);
            return View(result);
        }
      
        public ActionResult Check(int id,ProjectInfoRequest request)
        {
            var model = new SampleRegisterTable();
            var users = this.AccountService.GetUserList(10).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Person", new SelectList(users, "Name", "Name"));          
            model = this.IDKLManagerService.GetSampleRegisterTable(id);
            if (model != null)
            {
                try
                {
                    this.IDKLManagerService.UpDateSampleRegister(model);
                }
                catch (Exception ex)
                {
                    return Back(ex.Message);
                }
            }
            model.SampleStates = (int)EnumSampleStates.NewSample;
            return View("Edit", model);

        }
        [HttpPost]
        public ActionResult Check(int id, FormCollection collection)
        {
            var model = new SampleRegisterTable();
           // model = this.IDKLManagerService.GetSampleRegisterTable(id);  
            try
            {
                this.IDKLManagerService.DeleteSampleRegisterD(id);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }       
            //model.SampleStates = (int)EnumSampleStates.OldSample;
            //this.IDKLManagerService.UpDateSampleRegister(model);
            return this.RefreshParent();
        }

        public ActionResult CheckW(int id, ProjectInfoRequest request)
        {
            var model = new SampleRegisterTable();
            var users = this.AccountService.GetUserLists(10).Select(c => new { Id = c.ID, Name = c.Name });
            if (users != null)
            {
                ViewData.Add("Person", new SelectList(users, "Name", "Name"));
            }
            model = this.IDKLManagerService.GetSampleRegisterTable(id);
            if (model != null)
            {
                try
                {
                    this.IDKLManagerService.UpDateSampleRegister(model);
                }
                catch (Exception ex)
                {
                    return Back(ex.Message);
                }
            }
            model.SampleStates = (int)EnumSampleStates.NewSample;
            return View("EditW", model);

        }
        [HttpPost]
        public ActionResult CheckW(int id, FormCollection collection)
        {
            var model = new SampleRegisterTable();
            //model = this.IDKLManagerService.GetSampleRegisterTable(id);
            //model.AnalyzePeople = collection["Person"];
            //model.SampleStates = (int)EnumSampleStates.OldSample;
            try
            {
                this.IDKLManagerService.DeleteSampleRegisterD(id);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
            //this.IDKLManagerService.UpDateSampleRegister(model);
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
            request.SampleStates = (int)EnumSampleStates.NewSample;
            if (ids != null)
            {
                foreach (var item in ids)
                {
                    var temp = this.IDKLManagerService.GetSampleRegisterTable(item);
                    temp.SampleStates = (int)EnumSampleStates.OldSample;
                    temp.AnalyzePeople = collection["Person"];
                    try
                    {
                        this.IDKLManagerService.UpDateSampleRegister(temp);
                    }
                    catch (Exception ex)
                    {
                        return Back(ex.Message);
                    }
                }
            }

            return RedirectToAction("");
        }
        [HttpPost]
        public ActionResult UpdateAll(ProjectInfoRequest request, FormCollection collection, List<int> ids)
        {
            if (collection["Person"] == null || collection["Person"] == "")
            {
                return Back("请选择分析人！");
            }
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            request.SampleStates = (int)EnumSampleStates.NewSample;
            //request.PageIndex = Convert.ToInt16(collection["currentIndex"]);
            if (ids != null)
            {
                foreach (var item in ids)
                {
                    var temp = this.IDKLManagerService.GetSampleRegisterTable(item);
                    temp.SampleStates = (int)EnumSampleStates.OldSample;
                    temp.AnalyzePeople = collection["Person"];
                    try
                    {
                        this.IDKLManagerService.UpDateSampleRegister(temp);
                    }
                    catch (Exception ex)
                    {
                        return Back(ex.Message);
                    }
                }       
            }

            return RedirectToAction("");
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
        public ActionResult Delete(List<int> ids)
        {
            if (ids != null)
            {
                try
                {
                    this.IDKLManagerService.DeleteSampleRegister(ids);
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