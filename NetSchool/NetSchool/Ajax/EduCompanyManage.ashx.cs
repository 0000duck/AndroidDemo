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
    public class EduCompanyManage : AjaxBase
    {
        protected override string SwitchCmd(HttpRequest Request)
        {
            switch (NetSchool.Common.Library.GetPostBack.GetPostBackStr("cmd"))
            {
                case "getList":
                    return GetList();
                case "getInfo":
                    return GetInfo();
                case "addCompany":
                    return AddCompany();
                case "editCompany":
                    return EditCompany();
                case "deleteCompany":
                    return DeleteCompany();
                case "getListByTerritory":
                    return GetListByTerritory();
                case "getListByTime":
                    return GetListByTime();
                case "getListByStamp":
                    return GetListByStamp();
                default:
                    return base.SwitchCmd(Request);
            }
        }
        private static string GetList()
        {
            string strreturn;
            string serBeginTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serBeginTime");
            string serEndTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serEndTime");
            string strSearch = NetSchool.Common.Library.GetPostBack.GetPostBackStr("strSearch");
            string strCompany = NetSchool.Common.Library.GetPostBack.GetPostBackStr("strCompany");
            int pageindex = NetSchool.Common.Library.GetPostBack.GetPostBackInt("pageindex", 1);
            int pagesize = NetSchool.Common.Library.GetPostBack.GetPostBackInt("pagesize", 20);
            NetSchool.Common.Info.PageInfo pageInfo = new Common.Info.PageInfo();
            pageInfo.PageSize = pagesize;
            pageInfo.CurrentPageIndex = pageindex - 1;
            pageInfo.IsPage = true;
            pageInfo.SortField1 = "createdTime";
            pageInfo.SortType1 = NetSchool.Common.Enums.SortType.DESC;
            bool isSearch = false;
            if (strSearch != "")
            {
                isSearch = true;
            }
            bool isSearchCompany = false;
            if (strCompany != "")
            {
                isSearchCompany = true;
            }
            Common.Enums.StampStruct stamp = new Common.Enums.StampStruct();
            stamp.name = "统计时间";
            if (string.IsNullOrEmpty(serBeginTime))
            {
                stamp.startTime = Common.Library.ChangeTime.GetStamp("1970-01-01 08:00:00");
            }
            else
            {
                stamp.startTime = Common.Library.ChangeTime.GetStamp(serBeginTime + " 00:00:00");
            }
            if (string.IsNullOrEmpty(serEndTime))
            {
                stamp.endTime = Common.Library.ChangeTime.GetStamp(DateTime.Now.ToString());
            }
            else
            {
                stamp.endTime = Common.Library.ChangeTime.GetStamp(serEndTime + " 23:59:59");
            }
            var eduCompanyList = NetSchool.BLL.EduCompany.Search(IsSearchCompany: isSearchCompany, company: strCompany, IsSearchTerritory: isSearch, territory: strSearch, startTime: stamp.startTime, endTime: stamp.endTime, pageInfo: pageInfo);
            packageDatatable(eduCompanyList);
            strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", count = pageInfo.RecordCount, list = eduCompanyList }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strreturn;
        }
        private static string GetInfo()
        {
            string strreturn;
            Guid eduCompanyId = NetSchool.Common.Library.GetPostBack.GetPostBackGuid("id", Guid.Empty);
            if (eduCompanyId != Guid.Empty)
            {
                NetSchool.Model.EduCompany eduCompanyInfo = NetSchool.BLL.EduCompany.GetModel(ID: eduCompanyId);
                NetSchool.BLL.EduCompany.Update(eduCompanyInfo);
                strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", companyInfo = eduCompanyInfo }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "获取ID失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            return strreturn;
        }
        private static string AddCompany()
        {
            string strreturn;
            string strCompany = NetSchool.Common.Library.GetPostBack.GetPostBackStr("company");
            string strTerritory = NetSchool.Common.Library.GetPostBack.GetPostBackStr("territory");
            NetSchool.Model.EduCompany eduCompanyInfo = new Model.EduCompany();
            eduCompanyInfo.Id = Guid.NewGuid();
            eduCompanyInfo.Company = strCompany;
            eduCompanyInfo.Territory = strTerritory;
            eduCompanyInfo.CreateTime = DateTime.Now;
            eduCompanyInfo.CreatedTime = Common.Library.ChangeTime.GetStamp(DateTime.Now.ToString());
            if (NetSchool.BLL.EduCompany.Add(eduCompanyInfo))
            {
                strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "插入失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

            }
            return strreturn;
        }
        private static string EditCompany()
        {
            string strreturn;
            Guid newsID = NetSchool.Common.Library.GetPostBack.GetPostBackGuid("id", Guid.Empty);
            string strCompany = NetSchool.Common.Library.GetPostBack.GetPostBackStr("company");
            string strTerritory = NetSchool.Common.Library.GetPostBack.GetPostBackStr("territory");
            NetSchool.Model.EduCompany eduCompanyInfo = BLL.EduCompany.GetModel(newsID);
            eduCompanyInfo.Company = strCompany;
            eduCompanyInfo.Territory = strTerritory;
            if (NetSchool.BLL.EduCompany.Update(eduCompanyInfo))
            {
                strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "更新失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

            }
            return strreturn;
        }
        private static string DeleteCompany()
        {
            string strreturn;
            string ids = NetSchool.Common.Library.GetPostBack.GetPostBackStr("id");
            List<Guid> idList = new List<Guid>();
            foreach (string item in ids.Split(','))
            {
                idList.Add(new Guid(item));
            }
            if (NetSchool.BLL.EduCompany.DeleteListByID(idList))
            {
                strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "删除失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }

            return strreturn;
        }
        private static string GetListByTerritory()
        {
            string strreturn;
            string strSearch = NetSchool.Common.Library.GetPostBack.GetPostBackStr("strSearch");
            var eduCompanyList = NetSchool.BLL.EduCompany.SearchTerritory();
            List<object> lists = new List<object>();
            foreach (DataRow dr in eduCompanyList.Rows)
            {
                var obj = new { name = dr["territory"], value = dr["Count"] };
                lists.Add(obj);
            }
            strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", list = lists }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strreturn;
        }
        private static string GetListByTime()
        {
            string strreturn;
            bool isCountByTime = Convert.ToBoolean(NetSchool.Common.Library.GetPostBack.GetPostBackStr("isCountByTime"));
            bool isCountByTerrtiory = Convert.ToBoolean(NetSchool.Common.Library.GetPostBack.GetPostBackStr("isCountByTerrtiory"));
            bool isSearchTerritory = false;
            string territory = NetSchool.Common.Library.GetPostBack.GetPostBackStr("strSearch");
            if (!string.IsNullOrEmpty(territory))
            {
                isSearchTerritory = true;
            }
            string countStyle = NetSchool.Common.Library.GetPostBack.GetPostBackStr("countStyle");
            Common.Enums.StatisticalTime style = new Common.Enums.StatisticalTime();
            switch (countStyle)
            {
                case "Y":
                    style = Common.Enums.StatisticalTime.Year;
                    break;
                case "M":
                    style = Common.Enums.StatisticalTime.Month;
                    break;
                case "D":
                    style = Common.Enums.StatisticalTime.Day;
                    break;
                case "H":
                    style = Common.Enums.StatisticalTime.Hour;
                    break;
                case "mi":
                    style = Common.Enums.StatisticalTime.Minuter;
                    break;
                default:
                    style = Common.Enums.StatisticalTime.Second;
                    break;
            }

            var eduCompanyList = NetSchool.BLL.EduCompany.SearchCount(IsCountByTerritory: isCountByTerrtiory, IsCountByTime: isCountByTime, countStyle: style, IsSearchTerritory: isSearchTerritory, territory: territory);
            List<object> lists = new List<object>();
            if (isCountByTerrtiory)
            {
                if (isCountByTime)
                {

                }
                else
                {
                    foreach (DataRow dr in eduCompanyList.Rows)
                    {
                        var obj = new { name = dr["Territory"], value = dr["Count"] };
                        lists.Add(obj);
                    }
                }
            }
            else
            {
                if (isCountByTime)
                {
                    foreach (DataRow dr in eduCompanyList.Rows)
                    {
                        var obj = new { name = dr["Time"], value = dr["Count"] };
                        lists.Add(obj);
                    }
                }
            }
            strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", list = lists }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strreturn;
        }
        private static string GetListByStamp()
        {
            string strreturn;
            int year = NetSchool.Common.Library.GetPostBack.GetPostBackInt("strSearch", 2014);
            string territory = NetSchool.Common.Library.GetPostBack.GetPostBackStr("strSearchTerritory");
            var eduCompanyList = NetSchool.BLL.EduCompany.GetCountListByStamp(year, territory);
            List<object> lists = new List<object>();
            foreach (DataRow dr in eduCompanyList.Rows)
            {
                var obj = new { name = dr["Month"], value = dr["Count"] };
                lists.Add(obj);
            }
            strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", year = year, list = lists }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strreturn;
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
    }
}