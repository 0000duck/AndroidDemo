using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NetSchool.Common.WebBase;
using System.Data;

namespace NetSchool.Ajax
{
    public class EduLearningManage : AjaxBase
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
                default:
                    return base.SwitchCmd(Request);
            }
        }

        private static string GetList()
        {
            string strReturn;
            string serCompany = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serCompany");
            string serIDCard = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serIDCard");
            string serTerritory = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serTerritory");
            string serBeginTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serBeginTime");
            string serEndTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serEndTime");
            int pageindex = NetSchool.Common.Library.GetPostBack.GetPostBackInt("pageindex", 1);
            int pagesize = NetSchool.Common.Library.GetPostBack.GetPostBackInt("pagesize", 20);
            NetSchool.Common.Info.PageInfo pageInfo = new Common.Info.PageInfo();
            pageInfo.PageSize = pagesize;
            pageInfo.CurrentPageIndex = pageindex - 1;
            pageInfo.IsPage = true;
            pageInfo.SortField1 = "createdTime";
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

            DataTable dtLearnInfoList = new DataTable();
            dtLearnInfoList = NetSchool.BLL.EduLearning.Search(IsSearchCompany: searchCompany, Company: serCompany, IsSearchIdcard: searchIDCard, Idcard: serIDCard, IsSearchTerritory: searchTerritory, Territory: serTerritory,  StartTime: stamp.startTime, EndTime: stamp.endTime, pageInfo: pageInfo);
            packageDatatable(dtLearnInfoList);
            strReturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", count = pageInfo.RecordCount, list = dtLearnInfoList }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
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
            dtCountStatusList = NetSchool.BLL.EduLearning.GetCountListByStamp(year, IsSearchCompany: searchCompany, Company: serCompany, IsSearchIdcard: searchIDCard, Idcard: serIDCard, IsSearchTerritory: searchTerritory, Territory: serTerritory,  StartTime: stamp.startTime, EndTime: stamp.endTime);
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
            strReturn = JsonConvert.SerializeObject(new { state = "success",year=year, msg = "OK", list = lists, countList = monthCountList, monthList = months }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strReturn;
        }
        private static readonly string[] months = { "一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月" };
        private static string CountTerritory()
        {
            string strReturn;
            string cloumn = "territory";
            string serCompany = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serCompany");
            string serIDCard = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serIDCard");
            string serTerritory = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serTerritory");
            string serBeginTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serBeginTime");
            string serEndTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serEndTime");
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
            dtCountStatusList = NetSchool.BLL.EduLearning.SearchCount(Cloumn:cloumn, IsSearchCompany: searchCompany, Company: serCompany, IsSearchIdcard: searchIDCard, Idcard: serIDCard, IsSearchTerritory: searchTerritory, Territory: serTerritory, StartTime: stamp.startTime, EndTime: stamp.endTime);
            packOutNull(dtCountStatusList);
            List<object> lists = new List<object>();
            foreach (DataRow dr in dtCountStatusList.Rows)
            {
                var obj = new { name = dr["territory"], value = dr["Count"] };
                lists.Add(obj);
            }
            int[] monthCountList = new int[months.Length];
            for (int i = 0; i < months.Length; i++)
            {
                monthCountList[i] = (int)dtCountStatusList.Rows[i]["Count"];
            }
            strReturn = JsonConvert.SerializeObject(new { state = "success", year = year, msg = "OK", list = lists, countList = monthCountList, monthList = months }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strReturn;
        }
        private static void packageDatatable(DataTable dt)
        {
            dt.Columns.Add("Time", typeof(DateTime));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["createdTime"].ToString() != "0")
                    dt.Rows[i]["Time"] = Common.Library.ChangeTime.GetTime(dt.Rows[i]["createdTime"].ToString());
            }
        }
        private static void packOutNull(DataTable dt)
        {
            int k = 1;
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