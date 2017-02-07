using DKLManager.Contract.Model;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ProjectReportNumStatisticsMothController : AdminControllerBase
    {
        //
        // GET: /DKLManager/ProjectReportNumStatistics/
        public ActionResult Index(ProjectInfoRequest request)
        {
            var result = this.IDKLManagerService.GetProjectInfoHistoryList(request);
            return View(result);
        }
        public ActionResult Create()
        {
            return View("Report");
        }
        public ActionResult Report(string Year)
        {
            var list = this.IDKLManagerService.GetProjectDocHistoryList(Year);
            List<int> lists = new List<int>();
            for (var i = 0; i < 24; i++)
            {
                lists.Add(0);
            }
            #region 循环添加
            foreach (var item in list)
            {
                if (item.CreateTime.Month == 1)
                    lists[0]++;
                if (item.CreateTime.Month == 2)
                    lists[1]++;
                if (item.CreateTime.Month == 3)
                    lists[2]++;
                if (item.CreateTime.Month == 4)
                    lists[3]++;
                if (item.CreateTime.Month == 5)
                    lists[4]++;
                if (item.CreateTime.Month == 6)
                    lists[5]++;
                if (item.CreateTime.Month == 7)
                    lists[6]++;
                if (item.CreateTime.Month == 8)
                    lists[7]++;
                if (item.CreateTime.Month == 9)
                    lists[8]++;
                if (item.CreateTime.Month == 10)
                    lists[9]++;
                if (item.CreateTime.Month == 11)
                    lists[10]++;
                if (item.CreateTime.Month == 12)
                    lists[11]++;
                if (item.CreateTime.Month == 1)
                    lists[12]++;
                if (item.CreateTime.Month == 2)
                    lists[13]++;
                if (item.CreateTime.Month == 3)
                    lists[14]++;
                if (item.CreateTime.Month == 4)
                    lists[15]++;
                if (item.CreateTime.Month == 5)
                    lists[16]++;
                if (item.CreateTime.Month == 6)
                    lists[17]++;
                if (item.CreateTime.Month == 7)
                    lists[18]++;
                if (item.CreateTime.Month == 8)
                    lists[19]++;
                if (item.CreateTime.Month == 9)
                    lists[20]++;
                if (item.CreateTime.Month == 10)
                    lists[21]++;
                if (item.CreateTime.Month == 11)
                    lists[22]++;
                if (item.CreateTime.Month == 12)
                    lists[23]++;


            }
            #endregion
            JavaScriptSerializer jsS = new JavaScriptSerializer();
            var moth = jsS.Serialize(lists);
            return Content(moth);
        }
	}
}