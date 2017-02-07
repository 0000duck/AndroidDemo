using DKLManager.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ReportReceiveListController : AdminControllerBase
    {

        //质管部 待接收报告的列表，接收完成的报告。
        // GET: /DKLManager/ReportReceiveList/
        public ActionResult Index(ProjectInfoRequest request)
        {
            request.ProjectStatus = (int)EnumProjectSatus.ProjectDocToZhiguan;
            request.ProjectCategory = (int)EnumProjectCategory.TestValue;
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            //var users = this.AccountService.GetUserList(7).Select(c => new { Id = c.ID, Name = c.Name });
            var result = this.IDKLManagerService.GetProjectInfoList(request);
            foreach (var i in result)
            {
                if (i.ProjectCheif!= user.Name)
                {
                    i.ProjectCheif = user.Name;
                    this.IDKLManagerService.UpdateProjectInfo(i);
                }

            }

            //ViewData.Add("ProjectCheif", new SelectList(users, "Name", "Name"));
            ViewData["Name"] = user.Name;
            ViewData["Name"] = (user != null) ? user.Name : "";
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
            //var model1 = new DeviceOrderInfo();
           // model1 = this.IDKLManagerService.GetDeviceOrderInfo(id);
            this.IDKLManagerService.UpdateProjectInfo(model);
            //if(model1!=null)
            //{
               
            //}
            if (model != null)
            {
                MoveProjectData(model.ProjectNumber);//把数据移动到历史数据库中

                var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
                var nn = this.IDKLManagerService.SelectContractInfo(model.ProjectName);
                model.SignTime = nn.ContractDate;
                var models1 = new TimeInstructions();
                models1.ProjectNumBer = model.ProjectNumber;
                models1.ProjectName = model.ProjectName;
                models1.TimeNode = DateTime.Now;
                models1.SignTime = model.SignTime.ToString();
                models1.Instructions =user.LoginName+ "数据移到历史数据库";
                this.IDKLManagerService.InsertTimeInstructions(models1);
            }
            return RedirectToAction("Index");
        }    

	}
}