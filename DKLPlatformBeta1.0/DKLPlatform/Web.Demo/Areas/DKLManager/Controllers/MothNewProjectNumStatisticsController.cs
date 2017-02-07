using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;
using HYZK.FrameWork.Common;
using Web.Demo.Areas.DKLManager.Models;
using System.Drawing.Imaging;
using System.IO;
using HYZK.Core.Upload;
using OfficeDocGenerate;
using System.Web.Script.Serialization;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class MothNewProjectNumStatisticsController : AdminControllerBase
    {
        //
        // GET: /DKLManager/MothNewProjectNumStatistics/
        public ActionResult Index(ProjectInfoRequest request)
        {
            var result = this.IDKLManagerService.GetProjectInfoList(request);
         //   return View(result);
            return View("Index");
        }
        public ActionResult Create()
        {
            return View ("Graph");
        }
        public ActionResult History()
        {
            return View("History");
        }
        public ActionResult Caption(string Year)
        {   /*对当月产生的项目数量做统计，计算为产生数
             * 对当月实际完成（并非规定完成时间）的项目做统计，计算为完成数            
             */
            var model = this.IDKLManagerService.GetProjectInfosHistory(Year);
            var modelNew = this.IDKLManagerService.GetProjectInfos(Year);
            List<int> lists = new List<int>();
            for (int i = 0; i < 24; i++)
            {
                lists.Add(0);
            }
            #region 循环统计数量
            foreach (var item in model)
            {
                if (item.ProjectRealClosingDate.Month == 1)
                    lists[0]++;
                if (item.ProjectRealClosingDate.Month == 2)
                    lists[1]++;
                if (item.ProjectRealClosingDate.Month == 3)
                    lists[2]++;
                if (item.ProjectRealClosingDate.Month == 4)
                    lists[3]++;
                if (item.ProjectRealClosingDate.Month == 5)
                    lists[4]++;
                if (item.ProjectRealClosingDate.Month == 6)
                    lists[5]++;
                if (item.ProjectRealClosingDate.Month == 7)
                    lists[6]++;
                if (item.ProjectRealClosingDate.Month == 8)
                    lists[7]++;
                if (item.ProjectRealClosingDate.Month == 9)
                    lists[8]++;
                if (item.ProjectRealClosingDate.Month == 10)
                    lists[9]++;
                if (item.ProjectRealClosingDate.Month == 11)
                    lists[10]++;
                if (item.ProjectRealClosingDate.Month == 12)
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
            foreach (var item in modelNew)
            {
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
            var result = jsS.Serialize(lists);
            //  return Content("返回一个字符串");
            return Content(result);
        }
        public ActionResult HistoryEdit(string Year)
        {
            /*
             * 对已经完成的项目进行分析，超出规定时间，临近规定时间一周的为临近超期，实际完成时间大于规定时间一周的为正常
             * 
             */
            var model = this.IDKLManagerService.GetProjectInfosHistory(Year);
            List<int> lists = new List<int>();
            for (int i = 0; i < 48; i++)
            {
                lists.Add(0);
            }
            #region 循环统计数量
            foreach (var item in model)
            {
                if (item.ProjectClosingDate.Month == 1)
                    lists[0]++;
                if (item.ProjectClosingDate.Month == 2)
                    lists[1]++;
                if (item.ProjectClosingDate.Month == 3)
                    lists[2]++;
                if (item.ProjectClosingDate.Month == 4)
                    lists[3]++;
                if (item.ProjectClosingDate.Month == 5)
                    lists[4]++;
                if (item.ProjectClosingDate.Month == 6)
                    lists[5]++;
                if (item.ProjectClosingDate.Month == 7)
                    lists[6]++;
                if (item.ProjectClosingDate.Month == 8)
                    lists[7]++;
                if (item.ProjectClosingDate.Month == 9)
                    lists[8]++;
                if (item.ProjectClosingDate.Month == 10)
                    lists[9]++;
                if (item.ProjectClosingDate.Month == 11)
                    lists[10]++;
                if (item.ProjectClosingDate.Month == 12)
                    lists[11]++;

                if (item.ProjectClosingDate.Month == 1 &&item.ProjectRealClosingDate > item.ProjectClosingDate)                     //超期部分
                    lists[12]++;
                if (item.ProjectClosingDate.Month == 2 && item.ProjectRealClosingDate > item.ProjectClosingDate)
                    lists[13]++;
                if (item.ProjectClosingDate.Month == 3 && item.ProjectRealClosingDate > item.ProjectClosingDate)
                    lists[14]++;
                if (item.ProjectClosingDate.Month == 4 && item.ProjectRealClosingDate > item.ProjectClosingDate)
                    lists[15]++;
                if (item.ProjectClosingDate.Month == 5 && item.ProjectRealClosingDate > item.ProjectClosingDate)
                    lists[16]++;
                if (item.ProjectClosingDate.Month == 6 && item.ProjectRealClosingDate > item.ProjectClosingDate)
                    lists[17]++;
                if (item.ProjectClosingDate.Month == 7 && item.ProjectRealClosingDate > item.ProjectClosingDate)
                    lists[18]++;
                if (item.ProjectClosingDate.Month == 8 && item.ProjectRealClosingDate > item.ProjectClosingDate)
                    lists[19]++;
                if (item.ProjectClosingDate.Month == 9 && item.ProjectRealClosingDate > item.ProjectClosingDate)
                    lists[20]++;
                if (item.ProjectClosingDate.Month == 10 && item.ProjectRealClosingDate > item.ProjectClosingDate)
                    lists[21]++;
                if (item.ProjectClosingDate.Month == 11 && item.ProjectRealClosingDate > item.ProjectClosingDate)
                    lists[22]++;
                if (item.ProjectClosingDate.Month == 12 && item.ProjectRealClosingDate > item.ProjectClosingDate)
                    lists[23]++;            
                //临近超期 24-35
                if (item.ProjectClosingDate.Month == 1 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days < 7)
                    lists[24]++;
                if (item.ProjectClosingDate.Month == 2 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days < 7)
                    lists[25]++;
                if (item.ProjectClosingDate.Month == 3 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days < 7)
                    lists[26]++;
                if (item.ProjectClosingDate.Month == 4 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days < 7)
                    lists[27]++;
                if (item.ProjectClosingDate.Month == 5 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days < 7)
                    lists[28]++;
                if (item.ProjectClosingDate.Month == 6 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days < 7)
                    lists[29]++;
                if (item.ProjectClosingDate.Month == 7 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days < 7)
                    lists[30]++;
                if (item.ProjectClosingDate.Month == 8 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days < 7)
                    lists[31]++;
                if (item.ProjectClosingDate.Month == 9 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days < 7)
                    lists[32]++;
                if (item.ProjectClosingDate.Month == 10 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days < 7)
                    lists[33]++;
                if (item.ProjectClosingDate.Month == 11 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days < 7)
                    lists[34]++;
                if (item.ProjectClosingDate.Month == 12 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days < 7)
                    lists[35]++;

                //正常 36-47
                //if (item.ProjectClosingDate.Month == 1 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days > 7)
                //    lists[36]++;
                //if (item.ProjectClosingDate.Month == 2 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days > 7)
                //    lists[37]++;
                //if (item.ProjectClosingDate.Month == 3 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days > 7)
                //    lists[38]++;
                //if (item.ProjectClosingDate.Month == 4 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days > 7)
                //    lists[39]++;
                //if (item.ProjectClosingDate.Month == 5 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days > 7)
                //    lists[40]++;
                //if (item.ProjectClosingDate.Month == 6 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days > 7)
                //    lists[41]++;
                //if (item.ProjectClosingDate.Month == 7 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days > 7)
                //    lists[42]++;
                //if (item.ProjectClosingDate.Month == 8 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days > 7)
                //    lists[43]++;
                //if (item.ProjectClosingDate.Month == 9 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days > 7)
                //    lists[44]++;
                //if (item.ProjectClosingDate.Month == 10 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days > 7)
                //    lists[45]++;
                //if (item.ProjectClosingDate.Month == 11 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days > 7)
                //    lists[46]++;
                //if (item.ProjectClosingDate.Month == 12 && item.ProjectRealClosingDate < item.ProjectClosingDate && item.ProjectClosingDate.Subtract(item.ProjectRealClosingDate).Days > 7)
                //    lists[47]++;
            }
            lists[36] = lists[0] - lists[12] - lists[24];
            lists[37] = lists[1] - lists[13] - lists[25];
            lists[38] = lists[2] - lists[14] - lists[26];
            lists[39] = lists[3] - lists[15] - lists[27];
            lists[40] = lists[4] - lists[16] - lists[28];
            lists[41] = lists[5] - lists[17] - lists[29];
            lists[42] = lists[6] - lists[18] - lists[30];
            lists[43] = lists[7] - lists[19] - lists[31];
            lists[44] = lists[8] - lists[20] - lists[32];
            lists[45] = lists[9] - lists[21] - lists[33];
            lists[46] = lists[10] - lists[22] - lists[34];
            lists[47] = lists[11] - lists[23] - lists[35];
            #endregion
            JavaScriptSerializer jsS = new JavaScriptSerializer();
            var result = jsS.Serialize(lists);
            //  return Content("返回一个字符串");
            return Content(result);
        }
	}
}