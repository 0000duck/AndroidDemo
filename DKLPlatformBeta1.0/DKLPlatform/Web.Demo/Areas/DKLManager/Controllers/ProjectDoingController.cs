using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HYZK.FrameWork.Common;
using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using Web.Demo.Common;
using Web.Demo.Areas.DKLManager.Models;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ProjectDoingController :   AdminControllerBase
    {
        private static ProjectWholeInfoViewModel m_ProjectWholeInfoViewModel;
        //
        // GET: /DKLManager/ProjectDoing/
        /// <summary>
        /// 正在进行的项目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
            var result = this.IDKLManagerService.GetProjectInfoList(request);
            ViewData.Add("ProjectAlarmStatus", new SelectList(EnumHelper.GetItemValueList<EnumProjectAlarmStatus>(), "Key", "Value"));
            ViewData.Add("ProjectCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectCategory>(), "Key", "Value"));
            return View(result);
        }

        public ActionResult View(int id)
        {
            m_ProjectWholeInfoViewModel = new ProjectWholeInfoViewModel();
            m_ProjectWholeInfoViewModel.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (m_ProjectWholeInfoViewModel.projectBasicinfo != null)
            {
                m_ProjectWholeInfoViewModel.projectBasicImgFile = this.IDKLManagerService.GetProjectFile(m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber);
                m_ProjectWholeInfoViewModel.projectTestBasicinfoList = this.IDKLManagerService.GetTestBasicInfoList(m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber);
            }
            this.RenderMyViewDatas(m_ProjectWholeInfoViewModel);
            return View(m_ProjectWholeInfoViewModel);
        }        
           

        [HttpPost]
        public ActionResult View(FormCollection collection)
        {
            var result = this.IDKLManagerService.GetProjectInfoList();
            ViewData.Add("ProjectAlarmStatus", new SelectList(EnumHelper.GetItemValueList<EnumProjectAlarmStatus>(), "Key", "Value"));
            ViewData.Add("ProjectCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectCategory>(), "Key", "Value"));
            return View(result);
        }

    }
}