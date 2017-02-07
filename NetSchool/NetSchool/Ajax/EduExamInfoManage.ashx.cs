using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NetSchool.Common.WebBase;
using NetSchool.Common.Library;
using Microsoft.JScript;

namespace NetSchool.Ajax
{
    public class EduExamInfoManage : AjaxBase
    {
        protected override string SwitchCmd(HttpRequest Request)
        {
            switch (NetSchool.Common.Library.GetPostBack.GetPostBackStr("cmd"))
            {
                case "countTerritory":
                    return CountTerritory();
                case "getListByStamp":
                    return CountStamp();
                case "getList":
                    return GetList();
                case "countStatus":
                    return CountStatus();
                case "exportExcel":
                    return ExportExcel(Request);
                case "exportAccess":
                    return ExportAccess(Request);
                default:
                    return base.SwitchCmd(Request);
            }
        }

        private static string doExportExcel(DataTable dtExamInfoList, HttpRequest Request)
        {
            string userName = NetSchool.Common.Info.CurUserInfo.Username;
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", Type.GetType("System.Int32"));
            dt.Columns[0].AutoIncrement = true;
            dt.Columns[0].AutoIncrementSeed = 1;
            dt.Columns[0].AutoIncrementStep = 1;
            dt.Columns.Add("姓名", Type.GetType("System.String"));
            dt.Columns.Add("性别", Type.GetType("System.String"));
            dt.Columns.Add("发证日期", Type.GetType("System.String"));
            dt.Columns.Add("卡号", Type.GetType("System.String"));
            for (int i = 0; i < dtExamInfoList.Rows.Count; i++)
            {
                DataRow dtRow = dt.NewRow(); ;
                dtRow["姓名"] = "'" + dtExamInfoList.Rows[i]["EduUserName"];
                switch (dtExamInfoList.Rows[i]["gender"].ToString())
                {
                    case "female":
                        dtRow["性别"] = "男"; break;
                    case "male":
                        dtRow["性别"] = "女"; break;
                    case "secret":
                        dtRow["性别"] = "保密"; break;
                    default:
                        dtRow["性别"] = ""; break;
                }
                dtRow["发证日期"] = DateTime.Now.ToString("yyyy-MM-dd");
                dtRow["卡号"] = "'" + dtExamInfoList.Rows[i]["idcard"];
                dt.Rows.Add(dtRow);
            }
            string fileName = "ExportExcel" + userName + "(" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + ")";
            NetSchool.Common.Library.ExcelOP.didExport(dt, "", fileName, Request.PhysicalApplicationPath + "File/Excel/");
            var url = Request.Url.Authority + "/File/Excel/" + fileName + ".xls";
            return url;
        }
        private static string ExportExcel(HttpRequest Request)
        {
            string strReturn;
            string serCompany = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serCompany");
            string serIDCard = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serIDCard");
            string serTerritory = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serTerritory");
            string serBeginTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serBeginTime");
            string serEndTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serEndTime");
            string serStatus = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serStatus");
            string userName = NetSchool.Common.Info.CurUserInfo.Username;
            bool searchCompany = true;
            if ("" == serCompany)
            {
                searchCompany = false;
            }
            bool searchIDCard = true;
            if ("" == serIDCard)
            {
                searchIDCard = false;
            }
            bool searchTerritory = true;
            if ("" == serTerritory)
            {
                searchTerritory = false;
            }
            bool searchStatus = true;
            if ("all" == serStatus)
            {
                searchStatus = false;
            }

            Common.Enums.StampStruct stamp = new Common.Enums.StampStruct();
            stamp.name = "统计时间1";
            if ("" == serBeginTime)
            {
                stamp.startTime = Common.Library.ChangeTime.GetStamp("1970-01-01 08:00:00");

            }
            else
            {
                stamp.startTime = Common.Library.ChangeTime.GetStamp(serBeginTime + " 00:00:00");

            }
            if ("" == serEndTime)
            {
                stamp.endTime = Common.Library.ChangeTime.GetStamp(DateTime.Now.ToString());
            }
            else
            {
                stamp.endTime = Common.Library.ChangeTime.GetStamp(serEndTime + " 23:59:59");
            }

            DataTable dtExamInfoList = new DataTable();
            dtExamInfoList = NetSchool.BLL.EduExamInfo.Search(IsSearchCompany: searchCompany, Company: serCompany, IsSearchIdcard: searchIDCard, Idcard: serIDCard, IsSearchTerritory: searchTerritory, Territory: serTerritory, IsSearchStatus: searchStatus, Status: serStatus, StartTime: stamp.startTime, EndTime: stamp.endTime);
            packageDatatable(dtExamInfoList);

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", Type.GetType("System.Int32"));
            dt.Columns[0].AutoIncrement = true;
            dt.Columns[0].AutoIncrementSeed = 1;
            dt.Columns[0].AutoIncrementStep = 1;
            dt.Columns.Add("姓名", Type.GetType("System.String"));
            dt.Columns.Add("性别", Type.GetType("System.String"));
            dt.Columns.Add("发证日期", Type.GetType("System.String"));
            dt.Columns.Add("卡号", Type.GetType("System.String"));
            for (int i = 0; i < dtExamInfoList.Rows.Count; i++)
            {
                DataRow dtRow = dt.NewRow(); ;
                dtRow["姓名"] = "'" + dtExamInfoList.Rows[i]["EduUserName"];
                switch (dtExamInfoList.Rows[i]["gender"].ToString())
                {
                    case "female":
                        dtRow["性别"] = "男"; break;
                    case "male":
                        dtRow["性别"] = "女"; break;
                    case "secret":
                        dtRow["性别"] = "保密"; break;
                    default:
                        dtRow["性别"] = ""; break;
                }
                dtRow["发证日期"] = DateTime.Now.ToString("yyyy-MM-dd");
                dtRow["卡号"] = "'" + dtExamInfoList.Rows[i]["idcard"];
                dt.Rows.Add(dtRow);
            }
            string fileName = "/File/Excel/ExportExcel" + userName + "(" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + ").xls";
            string tabName = "CardInfo";
            string reMsg = string.Empty;
            bool result = NetSchool.Common.Library.ExcelOP.DataTableExportToExcel(dt, Request.PhysicalApplicationPath + fileName, tabName, ref reMsg);
            if (result)
                strReturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", dUrl = fileName }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            else
                strReturn = JsonConvert.SerializeObject(new { state = "success", msg = reMsg, dUrl = "/File/Excel/" + fileName + ".xls" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strReturn;
        }
        private static string ExportAccess(HttpRequest Request)
        {
            string strReturn;
            string serCompany = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serCompany");
            string serIDCard = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serIDCard");
            string serTerritory = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serTerritory");
            string serBeginTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serBeginTime");
            string serEndTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serEndTime");
            string serStatus = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serStatus");
            string userName = NetSchool.Common.Info.CurUserInfo.Username;
            //int pageindex = NetSchool.Common.Library.GetPostBack.GetPostBackInt("pageindex", 1);
            //int pagesize = NetSchool.Common.Library.GetPostBack.GetPostBackInt("pagesize", 20);
            //NetSchool.Common.Info.PageInfo pageInfo = new Common.Info.PageInfo();
            //pageInfo.PageSize = pagesize;
            //pageInfo.CurrentPageIndex = pageindex - 1;
            //pageInfo.IsPage = true;
            //pageInfo.SortField1 = "endTime";
            //pageInfo.SortType1 = NetSchool.Common.Enums.SortType.DESC;
            bool searchCompany = true;
            if ("" == serCompany)
            {
                searchCompany = false;
            }
            bool searchIDCard = true;
            if ("" == serIDCard)
            {
                searchIDCard = false;
            }
            bool searchTerritory = true;
            if ("" == serTerritory)
            {
                searchTerritory = false;
            }
            bool searchStatus = true;
            if ("all" == serStatus)
            {
                searchStatus = false;
            }

            Common.Enums.StampStruct stamp = new Common.Enums.StampStruct();
            stamp.name = "统计时间1";
            if ("" == serBeginTime)
            {
                stamp.startTime = Common.Library.ChangeTime.GetStamp("1970-01-01 08:00:00");

            }
            else
            {
                stamp.startTime = Common.Library.ChangeTime.GetStamp(serBeginTime + " 00:00:00");

            }
            if ("" == serEndTime)
            {
                stamp.endTime = Common.Library.ChangeTime.GetStamp(DateTime.Now.ToString());
            }
            else
            {
                stamp.endTime = Common.Library.ChangeTime.GetStamp(serEndTime + " 23:59:59");
            }

            DataTable dtExamInfoList = new DataTable();
            dtExamInfoList = NetSchool.BLL.EduExamInfo.Search(IsSearchCompany: searchCompany, Company: serCompany, IsSearchIdcard: searchIDCard, Idcard: serIDCard, IsSearchTerritory: searchTerritory, Territory: serTerritory, IsSearchStatus: searchStatus, Status: serStatus, StartTime: stamp.startTime, EndTime: stamp.endTime);
            packageDatatable(dtExamInfoList);

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", Type.GetType("System.Int32"));
            dt.Columns[0].AutoIncrement = true;
            dt.Columns[0].AutoIncrementSeed = 1;
            dt.Columns[0].AutoIncrementStep = 1;
            dt.Columns.Add("姓名", Type.GetType("System.String"));
            dt.Columns.Add("性别", Type.GetType("System.String"));
            dt.Columns.Add("发证日期", Type.GetType("System.String"));
            dt.Columns.Add("卡号", Type.GetType("System.String"));
            for (int i = 0; i < dtExamInfoList.Rows.Count; i++)
            {
                DataRow dtRow = dt.NewRow(); ;
                dtRow["姓名"] = dtExamInfoList.Rows[i]["EduUserName"];
                switch (dtExamInfoList.Rows[i]["gender"].ToString())
                {
                    case "female":
                        dtRow["性别"] = "男"; break;
                    case "male":
                        dtRow["性别"] = "女"; break;
                    case "secret":
                        dtRow["性别"] = "保密"; break;
                    default:
                        dtRow["性别"] = ""; break;
                }
                dtRow["发证日期"] = DateTime.Now.ToString("yyyy-MM-dd");
                dtRow["卡号"] = dtExamInfoList.Rows[i]["idcard"];
                dt.Rows.Add(dtRow);
            }
            string fileName = "/File/Access/ExportAccess" + userName + "(" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + ").accdb";//mdb
            string reMsg = string.Empty;
            string tabName = "DefaultCardInfo";
            bool result = NetSchool.Common.Library.AccesOP.DataTableExportToAccess(dt, Request.PhysicalApplicationPath + fileName, tabName, ref reMsg);
            if (result)
                strReturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", dUrl = fileName }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            else
                strReturn = JsonConvert.SerializeObject(new { state = "error", msg = reMsg }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strReturn;
        }
        private static string CountTerritory()
        {
            string strReturn;
            string serCompany = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serCompany");
            string serIDCard = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serIDCard");
            string serTerritory = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serTerritory");
            string serBeginTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serBeginTime");
            string serEndTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serEndTime");
            string serStatus = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serStatus");
            bool searchCompany = true;
            if ("" == serCompany)
            {
                searchCompany = false;
            }
            bool searchIDCard = true;
            if ("" == serIDCard)
            {
                searchIDCard = false;
            }
            bool searchTerritory = true;
            if ("" == serTerritory)
            {
                searchTerritory = false;
            }
            bool searchStatus = true;
            if ("all" == serStatus)
            {
                searchStatus = false;
            }
            Common.Enums.StampStruct stamp = new Common.Enums.StampStruct();
            stamp.name = "统计时间1";
            if ("" == serBeginTime)
            {
                stamp.startTime = Common.Library.ChangeTime.GetStamp("1970-01-01 08:00:00");

            }
            else
            {
                stamp.startTime = Common.Library.ChangeTime.GetStamp(serBeginTime + " 00:00:00");

            }
            if ("" == serEndTime)
            {
                stamp.endTime = Common.Library.ChangeTime.GetStamp(DateTime.Now.ToString());
            }
            else
            {
                stamp.endTime = Common.Library.ChangeTime.GetStamp(serEndTime + " 23:59:59");
            }
            DataTable dtCountStatusList = new DataTable();
            string cloumn = "territory";
            dtCountStatusList = NetSchool.BLL.EduExamInfo.SearchCount(Cloumn: cloumn, IsSearchCompany: searchCompany, Company: serCompany, IsSearchIdcard: searchIDCard, Idcard: serIDCard, IsSearchTerritory: searchTerritory, Territory: serTerritory, IsSearchStatus: searchStatus, Status: serStatus, StartTime: stamp.startTime, EndTime: stamp.endTime);
            packOutNull(dtCountStatusList);
            List<object> lists = new List<object>();
            foreach (DataRow dr in dtCountStatusList.Rows)
            {
                var obj = new { name = dr["territory"], value = dr["Count"] };
                lists.Add(obj);
            }
            strReturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", list = lists }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strReturn;
        }
        private static string CountStatus()
        {
            string strReturn;
            string serCompany = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serCompany");
            string serIDCard = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serIDCard");
            string serTerritory = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serTerritory");
            string serBeginTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serBeginTime");
            string serEndTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serEndTime");
            string serStatus = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serStatus");
            bool searchCompany = true;
            if ("" == serCompany)
            {
                searchCompany = false;
            }
            bool searchIDCard = true;
            if ("" == serIDCard)
            {
                searchIDCard = false;
            }
            bool searchTerritory = true;
            if ("" == serTerritory)
            {
                searchTerritory = false;
            }
            bool searchStatus = true;
            if ("all" == serStatus)
            {
                searchStatus = false;
            }


            Common.Enums.StampStruct stamp = new Common.Enums.StampStruct();
            stamp.name = "统计时间1";
            if ("" == serBeginTime)
            {
                stamp.startTime = Common.Library.ChangeTime.GetStamp("1970-01-01 08:00:00");

            }
            else
            {
                stamp.startTime = Common.Library.ChangeTime.GetStamp(serBeginTime + " 00:00:00");

            }
            if ("" == serEndTime)
            {
                stamp.endTime = Common.Library.ChangeTime.GetStamp(DateTime.Now.ToString());
            }
            else
            {
                stamp.endTime = Common.Library.ChangeTime.GetStamp(serEndTime + " 23:59:59");
            }

            DataTable dtCountStatusList = new DataTable();
            dtCountStatusList = NetSchool.BLL.EduExamInfo.countStatus(IsSearchCompany: searchCompany, Company: serCompany, IsSearchIdcard: searchIDCard, Idcard: serIDCard, IsSearchTerritory: searchTerritory, Territory: serTerritory, IsSearchStatus: searchStatus, Status: serStatus, StartTime: stamp.startTime, EndTime: stamp.endTime);
            packageStatus(dtCountStatusList);
            List<object> lists = new List<object>();
            foreach (DataRow dr in dtCountStatusList.Rows)
            {
                var obj = new { name = dr["statusName"], value = dr["Count"] };
                lists.Add(obj);
            }
            strReturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", list = lists }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strReturn;
        }
        private static string CountStamp()
        {
            string strReturn;
            string serCompany = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serCompany");
            string serIDCard = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serIDCard");
            string serTerritory = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serTerritory");
            string serBeginTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serBeginTime");
            string serEndTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serEndTime");
            string serStatus = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serStatus");
            int year = NetSchool.Common.Library.GetPostBack.GetPostBackInt("strYear", DateTime.Now.Year);
            bool searchCompany = true;
            if ("" == serCompany)
            {
                searchCompany = false;
            }
            bool searchIDCard = true;
            if ("" == serIDCard)
            {
                searchIDCard = false;
            }
            bool searchTerritory = true;
            if ("" == serTerritory)
            {
                searchTerritory = false;
            }
            bool searchStatus = true;
            if ("all" == serStatus || "" == serStatus)
            {
                searchStatus = false;
            }
            Common.Enums.StampStruct stamp = new Common.Enums.StampStruct();
            stamp.name = "统计时间1";
            if ("" == serBeginTime)
            {
                stamp.startTime = Common.Library.ChangeTime.GetStamp("1970-01-01 08:00:00");

            }
            else
            {
                stamp.startTime = Common.Library.ChangeTime.GetStamp(serBeginTime + " 00:00:00");

            }
            if ("" == serEndTime)
            {
                stamp.endTime = Common.Library.ChangeTime.GetStamp(DateTime.Now.ToString());
            }
            else
            {
                stamp.endTime = Common.Library.ChangeTime.GetStamp(serEndTime + " 23:59:59");
            }

            DataTable dtCountStatusList = new DataTable();
            dtCountStatusList = NetSchool.BLL.EduExamInfo.GetCountListByStamp(year, IsSearchCompany: searchCompany, Company: serCompany, IsSearchIdcard: searchIDCard, Idcard: serIDCard, IsSearchTerritory: searchTerritory, Territory: serTerritory, IsSearchStatus: searchStatus, Status: serStatus, StartTime: stamp.startTime, EndTime: stamp.endTime);
            List<object> lists = new List<object>();
            foreach (DataRow dr in dtCountStatusList.Rows)
            {
                var obj = new { name = dr["Month"], value = dr["Count"] };
                lists.Add(obj);
            }
            int[] monthCountList = new int[months.Length];
            for (int i = 0; i < months.Length; i++)
            {
                monthCountList[i] = (int)dtCountStatusList.Rows[i]["Count"];
            }
            strReturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", year = year, list = lists, countList = monthCountList, monthList = months }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strReturn;
        }
        private static string GetList()
        {
            string strReturn;
            string serCompany = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serCompany");
            string serIDCard = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serIDCard");
            string serTerritory = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serTerritory");
            string serBeginTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serBeginTime");
            string serEndTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serEndTime");
            string serStatus = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serStatus");
            int pageindex = NetSchool.Common.Library.GetPostBack.GetPostBackInt("pageindex", 1);
            int pagesize = NetSchool.Common.Library.GetPostBack.GetPostBackInt("pagesize", 20);
            NetSchool.Common.Info.PageInfo pageInfo = new Common.Info.PageInfo();
            pageInfo.PageSize = pagesize;
            pageInfo.CurrentPageIndex = pageindex - 1;
            pageInfo.IsPage = true;
            pageInfo.SortField1 = "endTime";
            pageInfo.SortType1 = NetSchool.Common.Enums.SortType.DESC;
            bool searchCompany = true;
            if ("" == serCompany)
            {
                searchCompany = false;
            }
            bool searchIDCard = true;
            if ("" == serIDCard)
            {
                searchIDCard = false;
            }
            bool searchTerritory = true;
            if ("" == serTerritory)
            {
                searchTerritory = false;
            }
            bool searchStatus = true;
            if ("all" == serStatus)
            {
                searchStatus = false;
            }

            Common.Enums.StampStruct stamp = new Common.Enums.StampStruct();
            stamp.name = "统计时间1";
            if ("" == serBeginTime)
            {
                stamp.startTime = Common.Library.ChangeTime.GetStamp("1970-01-01 08:00:00");

            }
            else
            {
                stamp.startTime = Common.Library.ChangeTime.GetStamp(serBeginTime + " 00:00:00");

            }
            if ("" == serEndTime)
            {
                stamp.endTime = Common.Library.ChangeTime.GetStamp(DateTime.Now.ToString());
            }
            else
            {
                stamp.endTime = Common.Library.ChangeTime.GetStamp(serEndTime + " 23:59:59");
            }

            DataTable dtExamInfoList = new DataTable();
            dtExamInfoList = NetSchool.BLL.EduExamInfo.Search(IsSearchCompany: searchCompany, Company: serCompany, IsSearchIdcard: searchIDCard, Idcard: serIDCard, IsSearchTerritory: searchTerritory, Territory: serTerritory, IsSearchStatus: searchStatus, Status: serStatus, StartTime: stamp.startTime, EndTime: stamp.endTime, pageInfo: pageInfo);
            packageDatatable(dtExamInfoList);
            //NetSchool.Common.Library.ExcelOP.didExport(dtExamInfoList, "", "ExportExcel");
            strReturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", count = pageInfo.RecordCount, list = dtExamInfoList }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strReturn;
        }
        private static readonly string[] months = { "一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月" };
        private static void packageDatatable(DataTable dt)
        {
            dt.Columns.Add("Begin", typeof(DateTime));
            dt.Columns.Add("End", typeof(DateTime));
            dt.Columns.Add("statusName", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Begin"] = Common.Library.ChangeTime.GetTime(dt.Rows[i]["beginTime"].ToString());
                if (dt.Rows[i]["endTime"].ToString() != "0")
                    dt.Rows[i]["End"] = Common.Library.ChangeTime.GetTime(dt.Rows[i]["endTime"].ToString());

                switch (dt.Rows[i]["status"].ToString())
                {
                    case "doing":
                        dt.Rows[i]["statusName"] = "进行中";
                        break;
                    case "paused":
                        dt.Rows[i]["statusName"] = "暂停";
                        break;
                    case "reviewing":
                        dt.Rows[i]["statusName"] = "审核中";
                        break;
                    case "finished":
                        dt.Rows[i]["statusName"] = "完成";
                        break;
                }

            }
        }
        private static void packageStatus(DataTable dt)
        {
            dt.Columns.Add("statusName", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                switch (dt.Rows[i]["status"].ToString())
                {
                    case "doing":
                        dt.Rows[i]["statusName"] = "进行中";
                        break;
                    case "paused":
                        dt.Rows[i]["statusName"] = "暂停";
                        break;
                    case "reviewing":
                        dt.Rows[i]["statusName"] = "审核中";
                        break;
                    case "finished":
                        dt.Rows[i]["statusName"] = "完成";
                        break;
                }
            }
        }

        private static void packOutNull(DataTable dt)
        {
            int k=1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ("" == dt.Rows[i]["territory"].ToString())
                {
                    dt.Rows[i]["territory"] = "无" + k;
                    k++;
                }
            }
        }
    }
}