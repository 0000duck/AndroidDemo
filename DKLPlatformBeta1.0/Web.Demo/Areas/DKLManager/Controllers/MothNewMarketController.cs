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
using System.Data;
using System.Text;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class MothNewMarketController : AdminControllerBase
    {
        public static MarketSearch Market;
        //
        // GET: /DKLManager/MothNewProjectNumStatistics/
        public static ProjectSearch ProjectSearch;
        //
        // GET: /DKLManager/MothNewProjectNumStatistics/
        public static ProjectSearch ProjectSearchArea;
        //
        // GET: /DKLManager/MothNewProjectNumStatistics/
        public ActionResult Index(ProjectInfoRequest request)
        {
            var result = this.IDKLManagerService.GetProjectInfoList(request);
            AddSearchViewData();
            return View("Index2");
        }
        public ActionResult MarketSearch(ProjectInfoRequest request)
        {
            AddSearchViewData();
            List<string> Month = new List<string>();
            for (int i = 1; i <= 12; i++)
                Month.Add(i.ToString());
            ViewData.Add("BeginMonth", new SelectList(Month));
            ViewData.Add("EndMonth", new SelectList(Month));
            Market = new MarketSearch();
            return View();
        }
        public ActionResult Welcome(ProjectInfoRequest request)
        {
            return View();
        }
        public ActionResult AreaSearch(ProjectInfoRequest request)
        {
            var area = this.IDKLManagerService.GetAreasList().Select(c => new { area = c.Area }).Distinct();
            ViewData.Add("Area", new SelectList(area, "area", "area"));
            AddSearchViewData();
            List<string> Month = new List<string>();
            for (int i = 1; i <= 12; i++)
                Month.Add(i.ToString());
            ViewData.Add("BeginMonth", new SelectList(Month));
            ViewData.Add("EndMonth", new SelectList(Month));
            ProjectSearchArea = new ProjectSearch();
            return View();
        }
        [HttpPost]
        public ActionResult SearchByArea(FormCollection collection)
        {
            var area = this.IDKLManagerService.GetAreasList().Select(c => new { area = c.Area }).Distinct();
            ViewData.Add("Area", new SelectList(area, "area", "area"));
            AddSearchViewData();
            ProjectSearch search1 = new ProjectSearch();
            List<string> Month = new List<string>();
            for (int i = 1; i <= 12; i++)
                Month.Add(i.ToString());
            ViewData.Add("BeginMonth", new SelectList(Month));
            ViewData.Add("EndMonth", new SelectList(Month));
            string Area = collection["Area"];
            string Year = collection["Year"];
            string BeginMonth = collection["BeginMonth"];
            string EndMonth = collection["EndMonth"];
            var list = this.IDKLManagerService.GetProjectSearchByArea(Area, Year, BeginMonth, EndMonth);
            ProjectSearchArea.projectList = list;
            return View("AreaSearch", ProjectSearchArea);
        }
        public void AddSearchViewData()
        {
            var marketperson = this.AccountService.GetUserList(1).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("MarketPerson", new SelectList(marketperson, "Name", "Name"));
            var users = this.AccountService.GetUserList(7).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Person", new SelectList(users, "Name", "Name"));
            var users1 = this.AccountService.GetUserList(6).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Leader", new SelectList(users1, "Name", "Name"));
            List<string> list = new List<string>();
            list.Add("检测评价组长");
            list.Add("检测评价普通员工");
            ViewData.Add("Select", new SelectList(list));
            List<string> list2 = new List<string>();
            list2.Add("全部项目");
            list2.Add("检测项目");
            list2.Add("评价项目");
            ViewData.Add("Type", new SelectList(list2));
            List<string> list3 = new List<string>();
            int year = DateTime.Now.Year - 10;
            for (int i = 10; i > 0; i--)
                list3.Add((year + i).ToString());
            ViewData.Add("Year", new SelectList(list3));
            ProjectSearch = new ProjectSearch();
        }
        public ActionResult Create()
        {
            return View("Graph");
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

                if (item.ProjectClosingDate.Month == 1 && item.ProjectRealClosingDate > item.ProjectClosingDate)                     //超期部分
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
        public ActionResult HistoryCompare(string Year)
        {/*
          * 检测评价报告完成对比图
          */
            var model = this.IDKLManagerService.GetProjectInfosHistory(Year);
            List<int> lists = new List<int>();
            lists.Add(0);
            lists.Add(0);
            foreach (var item in model)
            {
                if (item.ProjectCategory == 1)
                    lists[0]++;
                else
                    lists[1]++;
            }
            JavaScriptSerializer jsS = new JavaScriptSerializer();
            var result = jsS.Serialize(lists);
            return Content(result);
        }
        [HttpPost]
        public ActionResult Search(FormCollection collection)
        {
            AddSearchViewData();
            ProjectSearch search = new ProjectSearch();
            search.JobType = collection["Select"];
            if (search.JobType == "")
                return View("Index2");
            if (search.JobType == "检测评价组长")
                search.People = collection["Leader"];
            else
                search.People = collection["Person"];
            search.Year = collection["Year"];
            search.ProjectType = collection["Type"];
            var model = this.IDKLManagerService.GetProjectSearch(search.JobType, search.People, search.ProjectType, search.Year);
            search.projectList = model;
            ProjectSearch.projectList = search.projectList;
            ProjectSearch.People = search.People;
            return View("Index2", search);

        }
        [HttpPost]
        public ActionResult SearchByMarket(FormCollection collection)
        {
            AddSearchViewData();
            List<string> Month = new List<string>();
            for (int i = 1; i <= 12; i++)
                Month.Add(i.ToString());
            ViewData.Add("BeginMonth", new SelectList(Month));
            ViewData.Add("EndMonth", new SelectList(Month));
            MarketSearch search1 = new MarketSearch();
            search1.People = collection["MarketPerson"];
            search1.Year = collection["Year"];
            search1.BeginMonth = collection["BeginMonth"];
            search1.EndMonth = collection["EndMonth"];
            var model1 = this.IDKLManagerService.GetMoneySearch(search1.People, search1.Year, search1.BeginMonth, search1.EndMonth);
            search1.MarketList = model1;
            Market.MarketList = search1.MarketList;
            Market.People = search1.People;
            return View("MarketSearch", search1);
        }
        public ActionResult Detail(int id)
        {
            var model = this.IDKLManagerService.GetProjectContractInfo(id);
            ViewData.Add("ProjectCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectCategory>(), "Key", "Value", model.ProjectCategory));
            return View(model);
        }
        [HttpPost]
        public ActionResult Detail(FormCollection collection)
        {
            return this.RefreshParent();
        }
        [HttpPost]
        public ActionResult ExportExcelFile(FormCollection collection)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("ID", Type.GetType("System.Int32"));
            dt.Columns[0].AutoIncrement = true;
            dt.Columns[0].AutoIncrementSeed = 1;
            dt.Columns[0].AutoIncrementStep = 1;
            dt.Columns.Add("项目名称", Type.GetType("System.String"));
            dt.Columns.Add("项目编号", Type.GetType("System.String"));
            dt.Columns.Add("项目类别", Type.GetType("System.String"));
            dt.Columns.Add("规定完成时间", Type.GetType("System.String"));
            dt.Columns.Add("项目负责人", Type.GetType("System.String"));
            dt.Columns.Add("合同额（元）", Type.GetType("System.String"));

            dt.Columns.Add("合同额总计（万元）", Type.GetType("System.String"));
            double Sum = 0;
            foreach (var item in Market.MarketList)
            {
                Sum += (Convert.ToDouble(item.Money) / 10000);
            }
            int i = 0;
            foreach (var m in Market.MarketList)
            {
                ++i;
                DataRow dtRow = dt.NewRow();
                dtRow["项目名称"] = "" + m.ProjectName;
                dtRow["项目编号"] = "" + m.ProjectNumber;
                dtRow["项目类别"] = "" + EnumHelper.GetEnumTitle((EnumProjectCategory)m.ProjectCategory);
                dtRow["规定完成时间"] = "" + m.ProjectClosingDate.ToString();
                dtRow["项目负责人"] = "" + Market.People;
                dtRow["合同额（元）"] = "" + m.Money;
                if (i == 1)
                    dtRow["合同额总计（万元）"] = "" + Sum.ToString();

                dt.Rows.Add(dtRow);
            }





            string fileName = "d://DKLdownload" + Web.Demo.Common.AdminUserContext.Current.LoginInfo.LoginName + "(" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + ").xls";
            string tabName = "市场统计信息";
            string reMsg = string.Empty;
            bool result = ExcelOP.DataTableExportToExcel(dt, fileName, tabName, ref reMsg);



            string strFileName = fileName;
            if (result && !string.IsNullOrEmpty(strFileName))
            {

                string fileNewName = strFileName.Substring(strFileName.LastIndexOf("\\") + 1);
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = Encoding.UTF8;
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileNewName, Encoding.UTF8));
                Response.WriteFile(strFileName);
                Response.End();
                return Back("成功");
            }
            else
            {
                return Back("导出失败");
            }
        }
        public ActionResult ExportExcelFileByPerson(FormCollection collection)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", Type.GetType("System.Int32"));
            dt.Columns[0].AutoIncrement = true;
            dt.Columns[0].AutoIncrementSeed = 1;
            dt.Columns[0].AutoIncrementStep = 1;
            dt.Columns.Add("项目名称", Type.GetType("System.String"));
            dt.Columns.Add("项目编号", Type.GetType("System.String"));
            dt.Columns.Add("项目类别", Type.GetType("System.String"));
            dt.Columns.Add("公司名称", Type.GetType("System.String"));
            dt.Columns.Add("规定时间", Type.GetType("System.String"));
            dt.Columns.Add("实际时间", Type.GetType("System.String"));
            dt.Columns.Add("项目负责人", Type.GetType("System.String"));



            foreach (var m in ProjectSearch.projectList)
            {
                DataRow dtRow = dt.NewRow();

                dtRow["项目名称"] = "" + m.ProjectName;
                dtRow["项目编号"] = "" + m.ProjectNumber;
                dtRow["项目类别"] = "" + EnumHelper.GetEnumTitle((EnumProjectCategory)m.ProjectCategory);
                dtRow["公司名称"] = "" + m.CompaneName;
                dtRow["规定时间"] = "" + m.ProjectClosingDate.ToString();
                dtRow["实际时间"] = "" + m.ProjectRealClosingDate.ToString();
                dtRow["项目名称"] = "" + m.ProjectName;
                dtRow["项目负责人"] = "" + ProjectSearch.People;
                dt.Rows.Add(dtRow);
            }


            string fileName = "d://DKLdownload" + Web.Demo.Common.AdminUserContext.Current.LoginInfo.LoginName + "(" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + ").xls";
            string tabName = "负责人统计信息";
            string reMsg = string.Empty;
            bool result = ExcelOP.DataTableExportToExcel(dt, fileName, tabName, ref reMsg);



            string strFileName = fileName;
            if (result && !string.IsNullOrEmpty(strFileName))
            {

                string fileNewName = strFileName.Substring(strFileName.LastIndexOf("\\") + 1);
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = Encoding.UTF8;
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileNewName, Encoding.UTF8));
                Response.WriteFile(strFileName);
                Response.End();
                return Back("成功");
            }
            else
            {
                return Back("导出失败");
            }
        }
        public ActionResult ExportExcelFileByArea(FormCollection collection)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", Type.GetType("System.Int32"));
            dt.Columns[0].AutoIncrement = true;
            dt.Columns[0].AutoIncrementSeed = 1;
            dt.Columns[0].AutoIncrementStep = 1;
            dt.Columns.Add("项目名称", Type.GetType("System.String"));
            dt.Columns.Add("项目编号", Type.GetType("System.String"));
            dt.Columns.Add("项目类别", Type.GetType("System.String"));
            dt.Columns.Add("区域名称", Type.GetType("System.String"));
            dt.Columns.Add("规定时间", Type.GetType("System.String"));
            dt.Columns.Add("实际时间", Type.GetType("System.String"));

            foreach (var m in ProjectSearchArea.projectList)
            {
                DataRow dtRow = dt.NewRow();
                dtRow["项目名称"] = "" + m.ProjectName;
                dtRow["项目编号"] = "" + m.ProjectNumber;
                dtRow["项目类别"] = "" + EnumHelper.GetEnumTitle((EnumProjectCategory)m.ProjectCategory);
                dtRow["区域名称"] = "" + m.Area;
                dtRow["规定时间"] = "" + m.ProjectClosingDate.ToString();
                dtRow["实际时间"] = "" + m.ProjectRealClosingDate.ToString();
                dtRow["项目名称"] = "" + m.ProjectName;
                dt.Rows.Add(dtRow);
            }


            string fileName = "d://DKLdownload" + Web.Demo.Common.AdminUserContext.Current.LoginInfo.LoginName + "(" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + ").xls";
            string tabName = "区域统计信息";
            string reMsg = string.Empty;
            bool result = ExcelOP.DataTableExportToExcel(dt, fileName, tabName, ref reMsg);



            string strFileName = fileName;
            if (result && !string.IsNullOrEmpty(strFileName))
            {

                string fileNewName = strFileName.Substring(strFileName.LastIndexOf("\\") + 1);
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = Encoding.UTF8;
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileNewName, Encoding.UTF8));
                Response.WriteFile(strFileName);
                Response.End();
                return Back("成功");
            }
            else
            {
                return Back("导出失败");
            }
        }
    }
}