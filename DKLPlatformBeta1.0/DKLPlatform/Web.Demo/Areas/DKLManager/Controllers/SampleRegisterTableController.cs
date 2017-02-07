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
    public class SampleRegisterTableController : SampleBaseController
    {
        //
        // GET: /DKLManager/SampleRegisterTable/
        /// <summary>
        /// 样品表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request, FormCollection collection)
        {
            request.SampleState = (int)EnumSampleStates.NewSample;
            var result = this.IDKLManagerService.GetSampleRegisterTableList(request);
            //ViewData.Add("AnalyzePeople", new SelectList(EnumHelper.GetItemValueList<EnumAnalyzePeople>(), "Key", "Value"));
            //var users = this.AccountService.GetUserList(3).Select(c => new { Id = c.ID, Name = c.Name });
            //ViewData.Add("fxr", new SelectList(users, "Name", "Name"));
            return View(result);
        }
        /// <summary>
        /// 添加样品界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            DetectionData = new DetectionParamenterView();
            var model = new DetectionParamenterView();
            DetectionData.SampleRegister.SampleRegisterNumber = "请输入后半部分"; 
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
    
        [HttpPost]
        public ActionResult Submit(int id)
        {
            var model = this.IDKLManagerService.GetSampleRegisterTable(id);
            this.TryUpdateModel<SampleRegisterTable>(model);
            model.SampleStates = (int)EnumSampleStates.DoneSample;
            this.IDKLManagerService.UpDateSampleRegister(model);
            return Content("返回一个字符串");
        }
        
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            if (ids!= null)
            {
                this.IDKLManagerService.DeleteSampleRegister(ids);
            }
            return RedirectToAction("Index");
        }
	}
}