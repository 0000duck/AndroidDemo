using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web.Demo.Areas.DKLManager.Models;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class SampleRegisterTableController : SampleBaseController
    {
        //
        // GET: /DKLManager/SampleRegisterTable/
        /// <summary>
        /// 样品表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
            request.SampleState = (int)EnumSampleStates.NewSample;
            var result = this.IDKLManagerService.GetSampleRegisterTableList(request);
            return View(result);
        }
        public ActionResult IndexPhysics(ProjectInfoRequest request)
        {
            request.SampleState = (int)EnumSampleStatus.newsample;
            var result = this.IDKLManagerService.GetProjectTestBasicInfoListBySampleStatus(request);
            return View(result);
        }
        public ActionResult EditPhysics(ProjectInfoRequest request,string ProjectNumber)
        {
            request.ProjectNumber = ProjectNumber;
            var result = this.IDKLManagerService.GetProjectTestBasicInfoList(request);
            return View(result);
        }

        /// <summary>
        /// 添加样品界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Create(string ProjectNumber)
        {

            DetectionData = new DetectionParamenterView();
            var model = new DetectionParamenterView();
            DetectionData.cookies = this.IDKLManagerService.GetCookies();
            if (ProjectNumber != null)
                {
                    DetectionData.cookies.ProjectNumber = ProjectNumber;
                    this.IDKLManagerService.UpdateCookies(DetectionData.cookies);
                }
            var projectNumber = this.IDKLManagerService.GetProjectInfoList().Select(c => new { projectNumber = c.ProjectNumber }).Distinct();
            ViewData.Add("ProjectNumber", new SelectList(projectNumber, "projectNumber", "projectNumber"));           
            AddDataToView();       
            ViewData.Add("SaveCondition", new SelectList(EnumHelper.GetItemValueList<EnumSaveCondition>(), "Key", "Value"));
            ViewData.Add("SampleState", new SelectList(EnumHelper.GetItemValueList<EnumSampleState>(), "Key", "Value"));
            ViewData.Add("AnalyzePeople", new SelectList(EnumHelper.GetItemValueList<EnumAnalyzePeople>(), "Key", "Value"));
           
            return View(DetectionData);
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                    var model = new DetectionParamenterView();
                    var models = new SampleRegisterTable();
                    var users = this.AccountService.GetUserList(3).Select(c => new { Id = c.ID, Name = c.Name });
                    ViewData.Add("AnalyzePeople", new SelectList(users, "Name", "Name"));
                  
                    SaveOrderInfo();
                    var result = this.IDKLManagerService.GetSampleRegisterTableList();                  
                    AddDataToView();
                    
                    return View("Index", result);           
            }
            catch (Exception e)
            {
                return Back(e.Message);
            }
        
        }
        public ActionResult CreatePhysics(string ProjectNumber)
        {

            DetectionData = new DetectionParamenterView();
            var model = new DetectionParamenterView();
            DetectionData.cookies = this.IDKLManagerService.GetCookies();
            if (ProjectNumber != null)
            {
                DetectionData.cookies.ProjectNumber = ProjectNumber;
                this.IDKLManagerService.UpdateCookies(DetectionData.cookies);
            }
            var projectNumber = this.IDKLManagerService.GetProjectInfoList().Select(c => new { projectNumber = c.ProjectNumber }).Distinct();
            ViewData.Add("ProjectNumber", new SelectList(projectNumber, "projectNumber", "projectNumber"));
            AddDataToView();
            ViewData.Add("SaveCondition", new SelectList(EnumHelper.GetItemValueList<EnumSaveCondition>(), "Key", "Value"));
            ViewData.Add("SampleState", new SelectList(EnumHelper.GetItemValueList<EnumSampleState>(), "Key", "Value"));
            ViewData.Add("AnalyzePeople", new SelectList(EnumHelper.GetItemValueList<EnumAnalyzePeople>(), "Key", "Value"));

            return View(DetectionData);
        }
        [HttpPost]
        public ActionResult CreatePhysics(FormCollection collection)
        {
            try
            {

                this.IDKLManagerService.UpdateUserInputList((int)EnmuUserInputType.PhysicsParameterName, collection["PhysicsModel.TestContent"]);
                var model = new TestBasicInfo();
                var projectNumber = this.IDKLManagerService.GetProjectInfoList().Select(c => new { projectNumber = c.ProjectNumber }).Distinct();
                ViewData.Add("ProjectNumber", new SelectList(projectNumber, "projectNumber", "projectNumber"));
                //ViewData.Add("SaveCondition", new SelectList(EnumHelper.GetItemValueList<EnumSaveCondition>(), "Key", "Value"));
                //ViewData.Add("SampleState", new SelectList(EnumHelper.GetItemValueList<EnumSampleState>(), "Key", "Value"));
                //ViewData.Add("AnalyzePeople", new SelectList(EnumHelper.GetItemValueList<EnumAnalyzePeople>(), "Key", "Value"));
                string NumberBegin = collection["select1"];
                string NumberEnd = collection["select1End"];
                string SampleNormalNumber = "";
                string SampleNumber;
                int Last = NumberBegin.LastIndexOf("00");
                SampleNormalNumber = NumberBegin.Substring(0, Last + "00".Length);
                int BeginNumber = int.Parse(NumberBegin.Substring(Last, NumberBegin.Length - Last));
                int EndNumber = int.Parse(NumberEnd.Substring(Last, NumberEnd.Length - Last));
                int Poor = EndNumber - BeginNumber;

                var cookies = this.IDKLManagerService.GetCookies();
                cookies.SampleLetter = collection["textbox2"];
                cookies.SampleNumber = collection["select1End"];
                cookies.SampleQuantity = collection["textbox1"];
                this.IDKLManagerService.UpdateCookies(cookies);
                for (int i = BeginNumber; i <= EndNumber; i++)
                {
                    SampleNumber = "";
                    model = new TestBasicInfo();
                    SampleNumber = SampleNormalNumber + i.ToString();
               //     model.TestContent = collection["SampleRegister.SampleName"];
                //    model.SampleNumber = collection["textbox1"];
                    model.SampleNumber = SampleNumber;
                    model.SampleStatus = 0;
                    //model.SaveCondition = collection["SaveCondition"];
                    //model.AnalyzePeople = collection["AnalyzePeople"];
                    //model.Remark = collection["SampleRegister.Remark"];
                    //int Condition = int.Parse(model.SaveCondition);                    
                    //int state = int.Parse(model.SampleState);
                   
                    //model.SaveCondition = EnumHelper.GetEnumTitle((EnumSaveCondition)Condition);
                    //model.SampleState = EnumHelper.GetEnumTitle((EnumSampleState)state);
                    model.TestContent = collection["PhysicsModel.TestContent"];
                  
                    model.ProjectNumber = collection["ProjectNumber1"];
                    model.WordShop = collection["PhysicsModel.WordShop"];
                    model.Job = collection["PhysicsModel.Job"];
                    model.Location = collection["PhysicsModel.Location"];
                    model.TouchTime = collection["PhysicsModel.TouchTime"];
                    model.NoiseStrength = collection["PhysicsModel.NoiseStrength"];
                    model.Lex8hLexw = collection["PhysicsModel.Lex8hLexw"];
                 
                    if (model != null)
                    {
                        this.IDKLManagerService.AddProjectTestBasicInfo(model);
                
                    }
                    else
                    {
                        return Back(GlobalData.warningInfo5);
                    }
                }
            }
            catch (Exception e)
            {
                return Back(GlobalData.warningInfo4 + e.Message);
            }
            return RedirectToAction("IndexPhysics", "SampleRegisterTable", new { Area = "DKLManager" });

        }
        [HttpPost]
        public ActionResult UpdateAllPhysics(ProjectInfoRequest request, FormCollection collection)
        {
            request.SampleState = (int)EnumSampleStatus.newsample;
            request.PageIndex = Convert.ToInt16(collection["currentIndex"]);
            var result = this.IDKLManagerService.GetProjectTestBasicInfoListBySampleStatus(request);
            foreach (var item in result)
            {
                item.Job = collection[item.ID.ToString() + "-Job"];
                item.Location = collection[item.ID.ToString() + "-Location"];
                item.WordShop = collection[item.ID.ToString() + "-WordShop"];
                item.TouchTime = collection[item.ID.ToString() + "-TouchTime"];
                item.NoiseStrength = collection[item.ID.ToString() + "-NoiseStrength"];
                item.Lex8hLexw = collection[item.ID.ToString() + "-Lex8hLexw"];
                item.SampleStatus = (int)EnumSampleStatus.old;
                this.IDKLManagerService.UpdateProjectTestBasicInfo(item);
            }
            request.PageIndex = 1;
            return RedirectToAction("IndexPhysics", "SampleRegisterTable", request);

        }
        [HttpPost]
        public ActionResult EditAllPhysics(ProjectInfoRequest request, FormCollection collection)
        {
          //  request.SampleState = (int)EnumSampleStatus.newsample;
            request.PageIndex = Convert.ToInt16(collection["currentIndex"]);
            request.ProjectNumber = collection["projectNumber"];
            var result = this.IDKLManagerService.GetProjectTestBasicInfoList(request);
            foreach (var item in result)
            {
                item.Job = collection[item.ID.ToString() + "-Job"];
                item.Location = collection[item.ID.ToString() + "-Location"];
                item.WordShop = collection[item.ID.ToString() + "-WordShop"];
                item.TouchTime = collection[item.ID.ToString() + "-TouchTime"];
                item.NoiseStrength = collection[item.ID.ToString() + "-NoiseStrength"];
                item.Lex8hLexw = collection[item.ID.ToString() + "-Lex8hLexw"];
                item.SampleStatus = (int)EnumSampleStatus.old;
                this.IDKLManagerService.UpdateProjectTestBasicInfo(item);
            }
          //  request.PageIndex = 1;
            return RedirectToAction("EditPhysics", "SampleRegisterTable", new { ProjectNumber = request.ProjectNumber });

        }
        [HttpPost]
        public ActionResult Submit(int id)
        {
            var model = this.IDKLManagerService.GetSampleRegisterTable(id);
            this.TryUpdateModel<SampleRegisterTable>(model);
            model.SampleStates = (int)EnumSampleStates.OldSample;
            this.IDKLManagerService.UpDateSampleRegister(model);
            return Content("返回一个字符串");
        }
        
     //   [HttpPost]
        public ActionResult Delete(ProjectInfoRequest request,List<int> ids)
        {
            if (ids!= null)
            {
                this.IDKLManagerService.DeleteSampleRegister(ids);
            }
            return this.RefreshParent();
        }
        public ActionResult DeletePhysics(ProjectInfoRequest request, List<int> ids)
        {
            if (ids != null)
            {
                this.IDKLManagerService.DeleteProjectTestBasicInfo(ids);
            }
            return this.RefreshParent();
        }
        [HttpPost]
        public ActionResult UpdateAll(ProjectInfoRequest request,FormCollection collection)
        {
            request.SampleState = (int)EnumSampleStates.NewSample;
            request.PageIndex = Convert.ToInt16(collection["currentIndex"]);
            var result = this.IDKLManagerService.GetSampleRegisterTableList(request);
            foreach (var item in result)
            {
                item.Job = collection[item.ID.ToString() + "-Job"];
                item.Location = collection[item.ID.ToString() + "-Location"];
                item.WorkShop = collection[item.ID.ToString() + "-WorkShop"];
                item.CSTEL = collection[item.ID.ToString() + "-CSTEL"];
                item.CTWA = collection[item.ID.ToString() + "-CTWA"];
                item.CMAC = collection[item.ID.ToString() + "-CMAC"];
                this.IDKLManagerService.UpDateSampleRegister(item);
            }
            return RedirectToAction("Index","SampleRegisterTable",request);
        }
	}
}