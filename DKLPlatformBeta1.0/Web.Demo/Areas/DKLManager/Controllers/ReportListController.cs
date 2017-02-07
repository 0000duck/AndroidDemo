using DKLManager.Contract.Model;
using HYZK.Account.Contract;
using System.Linq;
using System.Web.Mvc;
using Web.Demo.Areas.DKLManager.Models;
using Web.Demo.Common;
using HYZK.FrameWork.Utility;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System;
using System.Data;

using System.Web.Script.Serialization;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ReportListController : AdminControllerBase
    {
        /// <summary>
        /// 检测三审完毕报告列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
            request.ProjectStatus = (int)EnumProjectSatus.ConsultModifyDone;
            request.ProjectCategory = (int)EnumProjectCategory.TestValue;
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            var users = this.AccountService.GetUserList(3).Select(c => new { Name = c.Name });
            ViewData.Add("ProjectCheif", new SelectList(users, "Name", "Name"));
            ViewData["Name"] = user.Name;
            var result = this.IDKLManagerService.GetProjectInfoList(request);
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

        [HttpPost]
        public ActionResult Submit(int id, FormCollection collection)
        {
            var model = new ProjectInfo();
            model = this.IDKLManagerService.GetProjectInfo(id);
            if (model != null)
            {
                var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
                model.ProjectStatus = (int)EnumProjectSatus.ProjectDocToZhiguan;
                this.IDKLManagerService.UpdateProjectInfo(model);
                var nn = this.IDKLManagerService.SelectContractInfo(model.ProjectName);
                model.SignTime = nn.ContractDate;
                var models1 = new TimeInstructions();
                models1.ProjectNumBer = model.ProjectNumber;
                models1.ProjectName = model.ProjectName;
                models1.TimeNode = DateTime.Now;
                models1.SignTime = model.SignTime.ToString();
                models1.Instructions =user.LoginName+ "审核完列表";
                this.IDKLManagerService.InsertTimeInstructions(models1);

            }
            return RedirectToAction("Index");   
        }    
       
	}
}