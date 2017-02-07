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

    public class AnalyzePersonController : SampleBaseController
    {
        private static string m_sampleRegisterNumber = "";
        //
        // GET: /DKLManager/AnalyzePerson/
        /// <summary>
        /// 分析人列表页面
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
             var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
             request.UserAccountType = user.AccountType;
             request.userName = user.Name;           
             var parameNew = this.IDKLManagerService.GetSampleRegisterTableList(request.userName, request);          
             return View(parameNew);
            
        }
        /// <summary>
        /// 添加参数值的页面
        /// </summary>
        /// <param name="sampleRegisterNumber"></param>
        /// <returns></returns>
        public ActionResult Edit(string sampleRegisterNumber)
        {
            if(sampleRegisterNumber.Contains("?"))
            {
                sampleRegisterNumber = sampleRegisterNumber.Substring(0, sampleRegisterNumber.LastIndexOf("?"));
            }
            m_sampleRegisterNumber = sampleRegisterNumber;
            AnalyzePersonViewModel valueList = new AnalyzePersonViewModel();
            valueList.ArgumentValueList = this.IDKLManagerService.GetArgumentValueList(sampleRegisterNumber).ToList();
            return View(valueList);
        }
        /// <summary>
        /// 对参数值进行操作
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            var valuelist = this.IDKLManagerService.GetArgumentValueList(m_sampleRegisterNumber).ToList();
            foreach(var value in valuelist)
            {
                value.ArgumentPrice = collection[value.ID.ToString()];
                this.IDKLManagerService.UpDateArgumentValue(value);
            }
            var sample = this.IDKLManagerService.GetSampleRegisterTable(m_sampleRegisterNumber);
            if(sample != null)
            {
                sample.SampleStates = (int)EnumSampleStates.DoneSample;
                this.IDKLManagerService.UpDateSampleRegister(sample);
            }
            return this.RefreshParent();
        }
        [HttpPost]
        public ActionResult Delete(List<int> id)
        {
            if (id!= null)
            {
                this.IDKLManagerService.DeleteSampleRegister(id);
            }
            return RedirectToAction("Index");
        }
	}
}