using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;
using HYZK.FrameWork.Common;
using Web.Demo.Areas.DKLManager.Models;
using System.Drawing.Imaging;
using System.IO;
using HYZK.Core.Upload;
using OfficeDocGenerate;
using System.Web.Script.Serialization;


namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class TestReportController : AdminControllerBase
    {
        //
        // GET: /DKLManager/TestReport/
        /// <summary>
        /// 测试报告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            request.ProjectStatus = (int)EnumProjectSatus.Begin;
            var result = this.IDKLManagerService.GetTestPhysicalReportList(request);
            return View(result);
        }
        public ActionResult Create()
        {
            return View("test");
            
        }
        public ActionResult CreateBar()
        {
            return View("bar");
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new TestPhysicalReport();
            this.TryUpdateModel<TestPhysicalReport>(model);
            this.IDKLManagerService.InsertTestPhysicalReport(model);
            return RefreshParent();
        }
        public ActionResult test1()         //饼图
        {
            var model = this.IDKLManagerService.GetSampleRegisterTableListEdit("1739");  //测试用的 这是项目名 需要随时改
            List<object> lists = new List<object>();
            foreach (var item in model)
            {
                var obj = new { name = item.SampleStates, value = 10 };
                lists.Add(obj);
            }
            JavaScriptSerializer jsS = new JavaScriptSerializer();
            var result = jsS.Serialize(lists);
          //  return Content("返回一个字符串");
            return Content(result);
        }
        public ActionResult test2()             //柱状图 项目分析
        {
            var model = this.IDKLManagerService.GetProjectInfos("2016");  
            List<int> lists = new List<int>();
            for (int i = 0; i < 24; i++)
            {
                lists.Add(0);
            }
            #region 循环统计数量
            foreach (var item in model)
            {
                if (item.ProjectClosingDate.Month == 1)
                    lists[0]++;
                if (item.ProjectClosingDate.Month == 2)
                    lists[1]++;
                if (item.ProjectClosingDate.Month == 3)
                    lists[2]++;
                if (item.ProjectClosingDate.Month == 4)
                    lists[3]++;
                if (item.ProjectClosingDate.Month == 5)
                    lists[4]++;
                if (item.ProjectClosingDate.Month == 6)
                    lists[5]++;
                if (item.ProjectClosingDate.Month == 7)
                    lists[6]++;
                if (item.ProjectClosingDate.Month == 8)
                    lists[7]++;
                if (item.ProjectClosingDate.Month == 9)
                    lists[8]++;
                if (item.ProjectClosingDate.Month == 10)
                    lists[9]++;
                if (item.ProjectClosingDate.Month == 11)
                    lists[10]++;
                if (item.ProjectClosingDate.Month == 12)
                    lists[11]++;
                if (item.CreateTime.Month == 1)
                    lists[12]++;
                if (item.CreateTime.Month == 2)
                    lists[13]++;
                if (item.CreateTime.Month == 3)
                    lists[14]++;
                if (item.CreateTime.Month == 4)
                    lists[15]++;
                if (item.CreateTime.Month == 5)
                    lists[16]++;
                if (item.CreateTime.Month == 6)
                    lists[17]++;
                if (item.CreateTime.Month == 7)
                    lists[18]++;
                if (item.CreateTime.Month == 8)
                    lists[19]++;
                if (item.CreateTime.Month == 9)
                    lists[20]++;
                if (item.CreateTime.Month == 10)
                    lists[21]++;
                if (item.CreateTime.Month == 11)
                    lists[22]++;
                if (item.CreateTime.Month == 12)
                    lists[23]++;


            }
            #endregion
            JavaScriptSerializer jsS = new JavaScriptSerializer();
            var result = jsS.Serialize(lists);
            //  return Content("返回一个字符串");
            return Content(result);
        }
        public ActionResult Edit(int id)
        {
            var model = this.IDKLManagerService.SelectTestPhysicalReport(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.SelectTestPhysicalReport(id);
            this.TryUpdateModel<TestPhysicalReport>(model);
            this.IDKLManagerService.UpdateTestPhysicalReport(model);
            return RefreshParent();

        }
        public ActionResult Delete(List<int> ids)
        {
            if (ids == null)
                return RedirectToAction("Index");
            this.IDKLManagerService.DeleteTestPhysicalReport(ids);
            return RedirectToAction("Index");
        }
        /*      public ActionResult test(int id)
              {
                  var model = this.IDKLManagerService.SelectTestPhysicalReport(id);
            
                  CreateWord cr = new CreateWord();
                  cr.CreateReportWord(model);
                  return RefreshParent();
              }
         * */
               public ActionResult ReportCreate(ProjectInfoRequest request)
                    {
                        var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
                        request.UserAccountType = user.AccountType;
                        request.userName = user.Name;
                        request.ProjectStatus = (int)EnumProjectSatus.Begin;
                        var result = this.IDKLManagerService.GetTestReportProjectList(request);                       
                        return View(result);
                    }
          
       
                 public ActionResult CreateWord(string projectName)
                 {
              
                     List<TestPhysicalReport> physicalmodels = new List<TestPhysicalReport>();
                     physicalmodels = this.IDKLManagerService.GetPhysicalModels(projectName);
                     List<TestChemicalReport> chemicalmodels = new List<TestChemicalReport>();
                     chemicalmodels = this.IDKLManagerService.GetChemicalReport(projectName);
                     List<ProjectInfo> projectmodelss = new List<ProjectInfo>();
                     projectmodelss = this.IDKLManagerService.GetProjectInfoModels(projectName);
                     var projectmodels = projectmodelss[0];
                     List<string> str = this.IDKLManagerService.GetSampleList(projectName);
                     List<string> strc = this.IDKLManagerService.GetTestList(projectName);

                 //    CreateWord cr = new CreateWord();
                 //    cr.CreateReportWord(str,strc,strf,projectmodels,physicalmodels,chemicalmodels);
                     return RefreshParent();
                 }
             
    }

}