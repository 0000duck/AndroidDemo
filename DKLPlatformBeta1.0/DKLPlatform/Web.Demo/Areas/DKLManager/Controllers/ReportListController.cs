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
using System.Data.SqlServerCe;
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

        //public ActionResult ReportPicture(ProjectInfoRequest request)
        //{
        //    var model = this.IDKLManagerService.GetProjectInfoList(request);
        //    DataSet ds = new DataSet();
        //    SqlCeDataAdapter da = new SqlCeDataAdapter();
        //    JavaScriptSerializer jsS = new JavaScriptSerializer();
        //    List<object> lists = new List<object>();
        //    foreach (var m in model)
        //    {
        //        var obj=new {}
        //    }
        //    return View("TEST", model);

        //}


        [HttpPost]
        public ActionResult Submit(int id, FormCollection collection)
        {
            var model = new ProjectInfo();
            model = this.IDKLManagerService.GetProjectInfo(id);
            if (model != null)
            {
                model.ProjectStatus = (int)EnumProjectSatus.ProjectDocToZhiguan;
                this.IDKLManagerService.UpdateProjectInfo(model);
            }
            return RedirectToAction("Index");   
        }    
       
	}
}