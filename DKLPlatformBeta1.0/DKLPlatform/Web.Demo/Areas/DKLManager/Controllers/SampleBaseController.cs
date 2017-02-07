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
                
                var model = new SampleRegisterTable();
            
                var projectNumber = this.IDKLManagerService.GetProjectInfoList().Select(c => new { projectNumber = c.ProjectNumber }).Distinct();               
                ViewData.Add("ProjectNumber", new SelectList(projectNumber, "projectNumber", "projectNumber"));           
                ViewData.Add("SaveCondition", new SelectList(EnumHelper.GetItemValueList<EnumSaveCondition>(), "Key", "Value"));
                ViewData.Add("SampleState", new SelectList(EnumHelper.GetItemValueList<EnumSampleState>(), "Key", "Value"));
                ViewData.Add("AnalyzePeople", new SelectList(EnumHelper.GetItemValueList<EnumAnalyzePeople>(), "Key", "Value"));
                model.SampleName = collection["SampleRegister.SampleName"];
                model.SampleNumBer = collection["SampleRegister.SampleNumBer"];
                model.SampleRegisterNumber =collection["ProjectNumber"] + "-" +collection["SampleRegister.SampleRegisterNumber"];
                model.SampleState = collection["SampleState"];
                model.SaveCondition = collection["SaveCondition"];
                model.AnalyzePeople = collection["AnalyzePeople"];
                model.Remark = collection["SampleRegister.Remark"];
                int Condition = int.Parse(model.SaveCondition);
                int Analy = int.Parse(model.AnalyzePeople);
                int state = int.Parse(model.SampleState);
                model.AnalyzePeople = EnumHelper.GetEnumTitle((EnumAnalyzePeople)Analy);
                model.SaveCondition = EnumHelper.GetEnumTitle((EnumSaveCondition)Condition);
                model.SampleState = EnumHelper.GetEnumTitle((EnumSampleState)state);
                model.ParameterName = collection["ParameterName"];
                //model.ProjectNumber = collection["SampleRegister.ProjectNumber"];
                model.ProjectNumber = collection["ProjectNumber"];
                if (model != null)
                {
                    if ((DateTime.TryParse(collection["SamplingDay"], out sampling)))
                    {
                        //int number;
                        //if ((int.TryParse(collection["SampleRegister.SampleRegisterNumber"], out number)) && (number > 0))
                        //{
                        //    int Samp = int.Parse(model.SampleRegisterNumber);
                        //    Samp = number;
                        //}
                        if (!IsExitParameter(model.ParameterName))
                        {
                            model.SamplingDay = sampling;
                            DetectionData.Param.SampleRegisterNumber = model.SampleRegisterNumber;
                            var parame = this.IDKLManagerService.GetParameterList().Select(c => new { Name = c.ParameterName }).Distinct();
                            ViewData.Add("ParameterName", new SelectList(parame, "Name", "Name"));
                            DetectionData.DetectionList.Add(model);
                            return View("Create", DetectionData);
                        }
                        else
                        {
                            return Back(GlobalData.warningInfo8);
                        }
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
            catch (Exception e)
            {
                return Back(GlobalData.warningInfo4 + e.Message);
            }
        }
        protected void SaveOrderInfo(bool update = false)
        {
            SampleRegisterTable models = new SampleRegisterTable();
            ArgumentValue argu = new ArgumentValue();
            models.SamplingDay = sampling;
            models.SampleStates = (int)EnumSampleStates.NewSample;
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
                models.AnalyzePeople= DetectionData.DetectionList.First().AnalyzePeople;
                models.SampleState = DetectionData.DetectionList.First().SampleState;
                models.SampleNumBer = DetectionData.DetectionList.First().SampleNumBer;
                models.ProjectNumber = DetectionData.DetectionList.First().ProjectNumber;
                models.ParameterName = DetectionData.DetectionList.First().ParameterName;
                this.IDKLManagerService.InsertSampleRegister(models);
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