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
    public class SampleBaseController : AdminControllerBase
    {
        protected static DetectionParamenterView DetectionData;
        protected static DateTime sampling = new DateTime();
        protected static DateTime acceptTime = new DateTime();
        //
        // GET: /DKLManager/SampleBase/
        [HttpPost]
        public ActionResult AddParameter(FormCollection collection)
        {
            try
            {

                this.IDKLManagerService.UpdateUserInputList((int)EnmuUserInputType.ParameterName, collection["SampleRegister.ParameterName"]);
                var model = new SampleRegisterTable();

                var projectNumber = this.IDKLManagerService.GetProjectInfoList().Select(c => new { projectNumber = c.ProjectNumber }).Distinct();
                ViewData.Add("ProjectNumber", new SelectList(projectNumber, "projectNumber", "projectNumber"));
                ViewData.Add("SaveCondition", new SelectList(EnumHelper.GetItemValueList<EnumSaveCondition>(), "Key", "Value"));
                ViewData.Add("SampleState", new SelectList(EnumHelper.GetItemValueList<EnumSampleState>(), "Key", "Value"));
                ViewData.Add("AnalyzePeople", new SelectList(EnumHelper.GetItemValueList<EnumAnalyzePeople>(), "Key", "Value"));
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
                    model = new SampleRegisterTable();
                    model.Poor = Poor+1;
                    SampleNumber = SampleNormalNumber + i.ToString();
                    model.SampleName = collection["SampleRegister.SampleName"];
                    model.SampleNumBer = collection["textbox1"];
                    model.SampleRegisterNumber = SampleNumber;
                    model.SampleState = collection["SampleState"];
                    model.SaveCondition = collection["SaveCondition"];
                    model.AnalyzePeople = collection["AnalyzePeople"];
                    model.Remark = collection["SampleRegister.Remark"];
                    int Condition = int.Parse(model.SaveCondition);
                    //int Analy = int.Parse(model.AnalyzePeople);
                    int state = int.Parse(model.SampleState);
                    //model.AnalyzePeople = EnumHelper.GetEnumTitle((EnumAnalyzePeople)Analy);
                    model.SaveCondition = EnumHelper.GetEnumTitle((EnumSaveCondition)Condition);
                    model.SampleState = EnumHelper.GetEnumTitle((EnumSampleState)state);
                    model.ParameterName = collection["SampleRegister.ParameterName"];
                    //model.ProjectNumber = collection["SampleRegister.ProjectNumber"];
                    model.ProjectNumber = collection["ProjectNumber1"];
                    model.WorkShop = collection["SampleRegister.WorkShop"];
                    model.Job = collection["SampleRegister.Job"];
                    model.Location = collection["SampleRegister.Location"];
                    model.CSTEL = collection["SampleRegister.CSTEL"];
                    model.CTWA = collection["SampleRegister.CTWA"];
                    model.CMAC = collection["SampleRegister.CMAC"];                  
                    //var Temp = this.IDKLManagerService.GetSampleRegisterTableBySampleNumber(model.SampleRegisterNumber);
                    //if (Temp != null)
                    //{
                    //    return Back("已经存在样品编号为" + Temp.SampleRegisterNumber + "的样品，请勿重复添加");
                    //}
                    if (model != null)
                    {
                        if ((DateTime.TryParse(collection["SamplingDay"], out sampling)))
                        {
                            int number;
                            if ((int.TryParse(collection["SampleRegister.SampleRegisterNumber"], out number)) && (number > 0))
                            {
                                int Samp = int.Parse(model.SampleRegisterNumber);
                                Samp = number;
                            }
                            //if (!IsExitParameter(model.ParameterName))
                            //{
                            model.SamplingDay = sampling;
                            //          DetectionData.Param.SampleRegisterNumber = model.SampleRegisterNumber;

                            //           DetectionData.DetectionList.Add(model);
                            model.SampleStates = (int)EnumSampleStates.NewSample;
                            model.SignTime = DateTime.Now;
                          
                            this.IDKLManagerService.InsertSampleRegister(model);

                            //}
                            //else
                            //{
                            //    return Back(GlobalData.warningInfo8);
                            //}
                        }
                        else
                        {
                            return Back(GlobalData.warningInfo7);
                        }
                    }
                    else
                    {
                        return Back(GlobalData.warningInfo5);
                    }
                }
                //var parame = this.IDKLManagerService.GetParameterList().Select(c => new { Name = c.ParameterName }).Distinct();
                //ViewData.Add("ParameterName", new SelectList(parame, "Name", "Name"));
                //return View("Create",model.ProjectNumber);
                
               // var result = this.IDKLManagerService.GetSampleRegisterTableList();                
               // if (result != null)
               // {
               //     ArgumentValue ar = new ArgumentValue();
               //     this.IDKLManagerService.DeleteArgumentValue();
               //     foreach (var i in result)
               //     {
               //         ar.SampleRegisterNumber = i.SampleRegisterNumber;
               //         ar.Argument = i.ParameterName;
               //         this.IDKLManagerService.InsertArgumentValue(ar);
               //     }
               // }

               // AddDataToView();
               //// return View("Index", result);
               // return RedirectToAction("Index", "SampleRegisterTable", new { Area = "DKLManager" });

            }
            catch (Exception e)
            {
                return Back(GlobalData.warningInfo4 + e.Message);
            }
            return RedirectToAction("Index", "SampleRegisterTable", new { Area = "DKLManager" });
        }
        public ActionResult GetParameterNameList(string search)
        {
            var list = this.IDKLManagerService.GetUserInputList((int)EnmuUserInputType.ParameterName, search);
            JavaScriptSerializer jsS = new JavaScriptSerializer();
            var result = jsS.Serialize(list);
            return Content(result);

        }
        public ActionResult GetPhysicsParameterNameList(string search)
        {
            var list = this.IDKLManagerService.GetUserInputList((int)EnmuUserInputType.PhysicsParameterName, search);
            JavaScriptSerializer jsS = new JavaScriptSerializer();
            var result = jsS.Serialize(list);
            return Content(result);

        }
        protected void SaveOrderInfo(bool update = false)
        {
            SampleRegisterTable models = new SampleRegisterTable();
            ArgumentValue argu = new ArgumentValue();
            //models.SamplingDay = sampling;
            models.SamplingDay = DateTime.Now;
            models.SampleStates = (int)EnumSampleStates.Selec;

            foreach (var data in DetectionData.DetectionList)
            {
                argu.SampleRegisterNumber = data.SampleRegisterNumber;
                argu.Argument = data.ParameterName;              
                this.IDKLManagerService.InsertArgumentValue(argu);

            }
            if (DetectionData.DetectionList.Count > 0)
            {
                models.SampleRegisterNumber = DetectionData.DetectionList.First().SampleRegisterNumber;

                models.Remark = DetectionData.DetectionList.First().Remark;
                models.SampleName = DetectionData.DetectionList.First().ParameterName;
                models.SaveCondition = DetectionData.DetectionList.First().SaveCondition;
                models.AnalyzePeople = DetectionData.DetectionList.First().AnalyzePeople;
                models.SampleState = DetectionData.DetectionList.First().SampleState;
                models.SampleNumBer = DetectionData.DetectionList.First().SampleNumBer;
                models.ProjectNumber = DetectionData.DetectionList.First().ProjectNumber;
                models.ParameterName = DetectionData.DetectionList.First().ParameterName;
                models.WorkShop = DetectionData.DetectionList.First().WorkShop;
                models.Job = DetectionData.DetectionList.First().Job;
                models.Location = DetectionData.DetectionList.First().Location;
                models.CSTEL = DetectionData.DetectionList.First().CSTEL;
                models.CTWA = DetectionData.DetectionList.First().CTWA;
                models.CMAC = DetectionData.DetectionList.First().CMAC;
                models.SignTime = DateTime.Now;
                this.IDKLManagerService.InsertSampleRegister(models);
                var cookies = this.IDKLManagerService.GetCookies();
                cookies.ProjectNumber = models.ProjectNumber;
                cookies.SampleNumber = models.SampleRegisterNumber;
                cookies.SampleQuantity = models.SampleNumBer;

                this.IDKLManagerService.UpdateCookies(cookies);
            }
        }

        protected bool IsExitParameter(string parameter)
        {
            bool bRet = false;
            foreach (var data in DetectionData.DetectionList)
            {
                if (data.ParameterName == parameter)
                {
                    bRet = true;
                    break;
                }
            }
            return bRet;
        }
        protected void AddDataToView()
        {
            var parameters = this.IDKLManagerService.GetParameterList().Select(c => new { Name = c.ParameterName }).Distinct();
            #region 过滤掉样品登记表中的噪声选项
            int i = 0;
            var paraMeters = parameters.ToList();
            foreach (var item in parameters)
            {
                i++;
                if (item.Name == "噪声" || item.Name == "噪音")
                {
                    paraMeters.Remove(paraMeters[i - 1]);
                    i--;
                }
            }
            #endregion
            ViewData.Add("ParameterName", new SelectList(paraMeters, "Name", "Name"));

        }
    }
}