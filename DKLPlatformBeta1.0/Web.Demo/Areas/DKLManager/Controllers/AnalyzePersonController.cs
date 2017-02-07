using DKLManager.Contract.Model;
using HYZK.Account.Contract;
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
             var users = this.AccountService.GetUserListG(9).Select(c => new { Id = c.ID, Name = c.Name });
             ViewData.Add("Person", new SelectList(users, "Name", "Name"));  
             var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
             request.UserAccountType = user.AccountType;
             request.userName = user.Name;
             request.SampleStates = (int)EnumSampleStates.OldSample;
             var parameNew = this.IDKLManagerService.GetSampleRegisterTableList(user.Name, request);
             return View(parameNew);
            
        }

        public ActionResult Check(int id, ProjectInfoRequest request)
        {

            var model = new SampleRegisterTable();            
            var users = this.AccountService.GetUserListG(9).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Person", new SelectList(users, "Name", "Name"));  
            model = this.IDKLManagerService.GetSampleRegisterTable(id);
            this.IDKLManagerService.UpDateSampleRegister(model);
            return View("Check", model);

        }

        [HttpPost]
        public ActionResult Check(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.GetSampleRegisterTable(id);
            model.AnalyzePeople = collection["Person"];
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);           
            //if(user.SecondAccountType<0)
            //if (model.AnalyzePeople == "冯晓青")
            //{
            //    model.SampleStates = (int)EnumSampleStates.Fcheck;
            //    this.TryUpdateModel<SampleRegisterTable>(model);
            //    //this.IDKLManagerService.UpDateSampleRegister(model);
            //    this.IDKLManagerService.DeleteSampleRegisterD(id);
            //    return this.RefreshParent();
            //}
           
            //model.SampleStates = (int)EnumSampleStates.Selec;
            //this.TryUpdateModel<SampleRegisterTable>(model);           
            //this.IDKLManagerService.UpDateSampleRegister(model);
            this.IDKLManagerService.DeleteSampleRegisterD(id);
            return this.RefreshParent();
        }
        [HttpPost]
        public ActionResult UpdateAll(ProjectInfoRequest request, FormCollection collection)
        {
        //   request.SampleState = (int)EnumSampleStates.NewSample;
            if (collection["Person"]==null || collection["Person"] =="")
            {
                return Back("请选择编制人！");
            }
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            request.SampleStates = (int)EnumSampleStates.OldSample;
            request.PageIndex = Convert.ToInt16(collection["currentIndex"]);
            var result = this.IDKLManagerService.GetSampleRegisterTableList(user.Name,request);
            var userA = this.AccountService.GetUserN(collection["Person"]);
              foreach (var item in result)
            {
                item.AnalyzePeople = collection["Person"];
                item.ArgumentPrice = collection[item.ID.ToString() + "-ArgumentPrice"];  

                if (item.ArgumentPrice != null && item.ArgumentPrice != "")
                  {
                      if (Convert.ToInt16(userA.SecondAccountType) < 0)
                      {
                          item.SampleStates = (int)EnumSampleStates.Fcheck;

                          this.IDKLManagerService.UpDateSampleRegister(item);
                      }
                      else
                      {
                          item.SampleStates = (int)EnumSampleStates.Selec;

                          this.IDKLManagerService.UpDateSampleRegister(item);
                      }
                     
 
                }  
             }
        
            return RedirectToAction("Index", "AnalyzePerson", request);
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
                if (value.ArgumentPrice == null)
                {
                    return RedirectToAction("请填写参数");
                }
                value.ParameterState = 1;
                this.IDKLManagerService.UpDateArgumentValue(value);
            }
            var sample = this.IDKLManagerService.GetSampleRegisterTable(m_sampleRegisterNumber);
            if(sample != null)
            {
                //sample.SampleStates = (int)EnumSampleStates.DoneSample;
                this.IDKLManagerService.UpDateSampleRegister(sample);
            }
            return this.RefreshParent();
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