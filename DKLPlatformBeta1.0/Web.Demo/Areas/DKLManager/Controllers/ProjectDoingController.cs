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
using HYZK.Account.Contract;

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
        /// 
        public ActionResult Index(ProjectInfoRequest request)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            
            ViewData.Add("ProjectAlarmStatus", new SelectList(EnumHelper.GetItemValueList<EnumProjectAlarmStatus>(), "Key", "Value"));
            ViewData.Add("ProjectCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectCategory>(), "Key", "Value"));
            ViewData.Add("ProjectPersonCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectPersonCategory>(), "Key", "Value"));
            //if (user.AccountType == 11)
            //{
            //    var result = this.IDKLManagerService.GetProjectInfoListPerson(request);
            //    return View(result);
            //}
            //else
            //{
            //    var result = this.IDKLManagerService.GetProjectInfoList(request);
            //    return View(result);
            //}
            var result = this.IDKLManagerService.GetAllProjectInfoList(request);
            return View(result);
        }
        public ActionResult Problem(int id)
        {
            var model = this.IDKLManagerService.GetProjectInfo(id);
            ViewData.Add("ProjectProblem", new SelectList(EnumHelper.GetItemValueList<EnumProjectProblem>(), "Key", "Value"));
            return View(model);

        }
        [HttpPost]
        public ActionResult Problem(int id, FormCollection collection)
        {

            var model = this.IDKLManagerService.GetProjectInfo(id);

            this.TryUpdateModel<ProjectInfo>(model);
            this.IDKLManagerService.UpdateProjectInfo(model);
            return this.RefreshParent();
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
            ViewData.Add("ProjectPersonCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectPersonCategory>(), "Key", "Value"));
            return View(result);
        }

    }
}