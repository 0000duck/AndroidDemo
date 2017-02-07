using DKLManager.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class SampleRegisterCountController : AdminControllerBase
    {
        // GET: DKLManager/SampleRegisterCount
        public ActionResult Index(ProjectInfoRequest request)
        {
            var projectList = this.IDKLManagerService.SelectAllProjectInfo(request);
            return View(projectList);
        }
        public ActionResult Count(string projectNumber)
        {
            if (projectNumber.Contains("?"))
            {
                projectNumber = projectNumber.Substring(0, projectNumber.IndexOf("?"));
            }
            var SampleRegisterTotal = this.IDKLManagerService.SelectSampleRegisterListByProjectNumber(projectNumber);
            if (SampleRegisterTotal.Count ==0)
            {
                return this.RefreshParent("未检测到化学数据");
            }
            List<string> ParameterNameList = new List<string>();
            ParameterNameList = SampleRegisterTotal.Select(u => u.ParameterName).Distinct().ToList();
            Dictionary<string, int> Count = new Dictionary<string, int>();
            foreach (var item in ParameterNameList)
            {
                Count.Add(item, SampleRegisterTotal.Where(u => u.ParameterName == item).ToList().Count);
            }
            return View(Count);
        }
    }
}