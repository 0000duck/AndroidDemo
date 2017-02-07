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
            var result = this.IDKLManagerService.GetProjectInfoList(request);
            return View(result);
        }

        [HttpPost]
        public ActionResult Submit(int id, FormCollection collection)
        {
            var model = new ProjectInfo();
            model = this.IDKLManagerService.GetProjectInfo(id);            
            this.IDKLManagerService.UpdateProjectInfo(model);
            if (model != null)
            {
                MoveProjectData(model.ProjectNumber);//把数据移动到历史数据库中
            }
            return RedirectToAction("Index");
        }    

	}
}