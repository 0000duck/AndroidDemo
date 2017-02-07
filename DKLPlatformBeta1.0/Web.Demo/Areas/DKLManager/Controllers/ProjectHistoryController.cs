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
    public class ProjectHistoryController : AdminControllerBase
    {
        private static ProjectWholeInfoViewModelHis m_ProjectWholeInfoViewModel;
        //
        // GET: /DKLManager/ProjectHistory/
        public ActionResult Index(ProjectInfoRequest request)
        {
            var result = this.IDKLManagerService.GetProjectInfoHistoryList(request);
            //var result = this.IDKLManagerService.GetProjectInfoHistoryListPerson(request);
            ViewData.Add("ProjectCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectCategory>(), "Key", "Value"));
            return View(result);
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
        public ActionResult View(int id)
        {
            m_ProjectWholeInfoViewModel = new ProjectWholeInfoViewModelHis();
            m_ProjectWholeInfoViewModel.projectBasicinfoHistory = this.IDKLManagerService.GetProjectInfoHistory(id);
            if (m_ProjectWholeInfoViewModel.projectBasicinfoHistory != null)
            {
                m_ProjectWholeInfoViewModel.projectBasicFileHistory = this.IDKLManagerService.GetProjectFileHistory(m_ProjectWholeInfoViewModel.projectBasicinfoHistory.ProjectNumber);
                m_ProjectWholeInfoViewModel.projectTestBasicinfoListHistory = this.IDKLManagerService.GetTestBasicInfoListHistory(m_ProjectWholeInfoViewModel.projectBasicinfoHistory.ProjectNumber);
                m_ProjectWholeInfoViewModel.projectTestChemicalReportListHistory = this.IDKLManagerService.GetTestChemicalReportListHistory(m_ProjectWholeInfoViewModel.projectBasicinfoHistory.ProjectNumber);
                this.RenderMyViewDatas(m_ProjectWholeInfoViewModel);
            }
            return View(m_ProjectWholeInfoViewModel);
        }
        public ActionResult ViewForSearch(string projectNumber)
        {
            m_ProjectWholeInfoViewModel = new ProjectWholeInfoViewModelHis();
            m_ProjectWholeInfoViewModel.projectBasicinfoHistory = this.IDKLManagerService.GetProjectInfoHistory(projectNumber);
            if (m_ProjectWholeInfoViewModel.projectBasicinfoHistory != null)
            {
                m_ProjectWholeInfoViewModel.projectBasicFileHistory = this.IDKLManagerService.GetProjectFileHistory(m_ProjectWholeInfoViewModel.projectBasicinfoHistory.ProjectNumber);
                m_ProjectWholeInfoViewModel.projectTestBasicinfoListHistory = this.IDKLManagerService.GetTestBasicInfoListHistory(m_ProjectWholeInfoViewModel.projectBasicinfoHistory.ProjectNumber);
                m_ProjectWholeInfoViewModel.projectTestChemicalReportListHistory = this.IDKLManagerService.GetTestChemicalReportListHistory(m_ProjectWholeInfoViewModel.projectBasicinfoHistory.ProjectNumber);
                this.RenderMyViewDatas(m_ProjectWholeInfoViewModel);
            }
            return View("View",m_ProjectWholeInfoViewModel);
        }
        [HttpPost]
        public ActionResult View(FormCollection collection)
        {
            return this.RefreshParent();
        }
	}
}