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
    public class EduUserManage : AjaxBase
    {

        protected override string SwitchCmd(HttpRequest Request)
        {
            switch (NetSchool.Common.Library.GetPostBack.GetPostBackStr("cmd"))
            {
                case "getList":
                    return GetList();
                case "getInfo":
                    return GetInfo();
                case "addUser":
                    return AddUser();
                case "editUser":
                    return EditUser();
                case "deleteUser":
                    return DeleteUser();
                case "getListByCompany":
                    return GetListByCompany();
                case "getListByIdcard":
                    return GetListByIdcard();
                case "getListByRoles":
                    return GetListByRoles();
                case "getListByGender":
                    return GetListByGender();
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
            string strCompanySearch = NetSchool.Common.Library.GetPostBack.GetPostBackStr("searchCompanyTxt");
            string strIdcardSearch = NetSchool.Common.Library.GetPostBack.GetPostBackStr("strIdcardSearch");
            string strRolesSearch = NetSchool.Common.Library.GetPostBack.GetPostBackStr("strRolesSearch");
            string strGenderSearch = NetSchool.Common.Library.GetPostBack.GetPostBackStr("strGenderSearch");
            string strNicknameSearch = NetSchool.Common.Library.GetPostBack.GetPostBackStr("strNicknameSearch");
            int pageindex = NetSchool.Common.Library.GetPostBack.GetPostBackInt("pageindex", 1);
            int pagesize = NetSchool.Common.Library.GetPostBack.GetPostBackInt("pagesize", 20);
            NetSchool.Common.Info.PageInfo pageInfo = new Common.Info.PageInfo();
            pageInfo.PageSize = pagesize;
            pageInfo.CurrentPageIndex = pageindex - 1;
            pageInfo.IsPage = true;
            pageInfo.SortField1 = "createdTime";
            pageInfo.SortType1 = NetSchool.Common.Enums.SortType.DESC;
            bool isSearchByCompany = false;
            bool isSearchByIdcard = false;
            bool isSearchByGender = false;
            bool isSearchByRoles = false;
            bool isSearchByNickname = false;
            if (!string.IsNullOrEmpty(strNicknameSearch))
            {
                isSearchByNickname = true;
            }
            if (!string.IsNullOrEmpty(strCompanySearch))
            {
                isSearchByCompany = true;
            }
            if (!string.IsNullOrEmpty(strIdcardSearch))
            {
                isSearchByIdcard = true;
            }
            if (!string.IsNullOrEmpty(strGenderSearch))
            {
                isSearchByGender = true;
            }
            if (!string.IsNullOrEmpty(strRolesSearch))
            {
                isSearchByRoles = true;
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
            var allRoles = NetSchool.BLL.EduUser.GetALLRolesDistinct();
            var eduUserList = NetSchool.BLL.EduUser.Search(IsSearchCompany: isSearchByCompany, Company: strCompanySearch, IsSearchNickName: isSearchByNickname, NickName: strNicknameSearch, IsSearchIdcard: isSearchByIdcard, Idcard: strIdcardSearch, IsSearchRoles: isSearchByRoles, Roles: strRolesSearch, IsSearchGender: isSearchByGender, Gender: strGenderSearch, StartTime: stamp.startTime, EndTime: stamp.endTime, pageInfo: pageInfo);
            packageDatatable(eduUserList);
            strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", count = pageInfo.RecordCount, list = eduUserList, rolesList = allRoles }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strreturn;
        }
        private static string GetInfo()
        {
            string strreturn;
            Guid eduUserId = NetSchool.Common.Library.GetPostBack.GetPostBackGuid("id", Guid.Empty);
            if (eduUserId != Guid.Empty)
            {
                NetSchool.Model.EduUser eduUserInfo = NetSchool.BLL.EduUser.GetModel(ID: eduUserId);
                NetSchool.BLL.EduUser.Update(eduUserInfo);
                strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", userInfo = eduUserInfo }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "获取ID失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            return strreturn;
        }
        private static string AddUser()
        {
            string strreturn;
            string strNickname = NetSchool.Common.Library.GetPostBack.GetPostBackStr("nickname");
            string strIdcard = NetSchool.Common.Library.GetPostBack.GetPostBackStr("idcard");
            string strCompany = NetSchool.Common.Library.GetPostBack.GetPostBackStr("company");
            string strRoles = NetSchool.Common.Library.GetPostBack.GetPostBackStr("roles");
            string strGender = NetSchool.Common.Library.GetPostBack.GetPostBackStr("gender");
            NetSchool.Model.EduUser eduUserInfo = new Model.EduUser();
            eduUserInfo.Id = Guid.NewGuid();
            eduUserInfo.NickName = strNickname;
            eduUserInfo.Idcard = strIdcard;
            eduUserInfo.Roles = strRoles;
            eduUserInfo.Gender = strGender;
            eduUserInfo.Company = strCompany;
            eduUserInfo.CreateTime = DateTime.Now;
            eduUserInfo.CreatedTime = Common.Library.ChangeTime.GetStamp(DateTime.Now.ToString());
            if (NetSchool.BLL.EduUser.Add(eduUserInfo))
            {
                strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "插入失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

            }
            return strreturn;
        }
        private static string EditUser()
        {
            string strreturn;
            Guid newsID = NetSchool.Common.Library.GetPostBack.GetPostBackGuid("id", Guid.Empty);
            string strNickname = NetSchool.Common.Library.GetPostBack.GetPostBackStr("nickname");
            string strIdcard = NetSchool.Common.Library.GetPostBack.GetPostBackStr("idcard");
            string strCompany = NetSchool.Common.Library.GetPostBack.GetPostBackStr("company");
            string strRoles = NetSchool.Common.Library.GetPostBack.GetPostBackStr("roles");
            string strGender = NetSchool.Common.Library.GetPostBack.GetPostBackStr("gender");
            NetSchool.Model.EduUser eduUserInfo = BLL.EduUser.GetModel(newsID);
            eduUserInfo.NickName = strNickname;
            eduUserInfo.Idcard = strIdcard;
            eduUserInfo.Roles = strRoles;
            eduUserInfo.Gender = strGender;
            eduUserInfo.Company = strCompany;
            if (NetSchool.BLL.EduUser.Update(eduUserInfo))
            {
                strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "更新失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

            }
            return strreturn;
        }
        private static string DeleteUser()
        {
            string strreturn;
            string ids = NetSchool.Common.Library.GetPostBack.GetPostBackStr("id");
            List<Guid> idList = new List<Guid>();
            foreach (string item in ids.Split(','))
            {
                idList.Add(new Guid(item));
            }
            if (NetSchool.BLL.EduUser.DeleteListByID(idList))
            {
            }
            else
            {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "删除失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strreturn;
        }
        private static string GetListByCompany()
        {
            string strreturn;
            string strSearch = NetSchool.Common.Library.GetPostBack.GetPostBackStr("strSearch");
            var eduUserList = NetSchool.BLL.EduUser.SearchCount("company");
            List<object> lists = new List<object>();
            foreach (DataRow dr in eduUserList.Rows)
            {
                if (dr["company"] != DBNull.Value)
                {
                    var obj = new { name = dr["company"], value = dr["Count"] };
                    lists.Add(obj);
                }
            }
            strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", list = lists }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strreturn;
        }
        private static string GetListByIdcard()
        {
            string strreturn;
            string strSearch = NetSchool.Common.Library.GetPostBack.GetPostBackStr("strSearch");
            var eduUserList = NetSchool.BLL.EduUser.SearchCount("idcard");
            List<object> lists = new List<object>();
            foreach (DataRow dr in eduUserList.Rows)
            {
                var obj = new { name = dr["idcard"], value = dr["Count"] };
                lists.Add(obj);
            }
            strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", list = lists }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strreturn;
        }
        private static string GetListByRoles()
        {
            string strreturn;
            string strSearch = NetSchool.Common.Library.GetPostBack.GetPostBackStr("strSearch");
            var eduUserList = NetSchool.BLL.EduUser.SearchCount("roles");
            List<object> lists = new List<object>();
            foreach (DataRow dr in eduUserList.Rows)
            {
                var obj = new { name = dr["roles"], value = dr["Count"] };
                lists.Add(obj);
            }
            strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", list = lists }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strreturn;
        }
        private static string GetListByGender()
        {
            string strreturn;
            string strSearch = NetSchool.Common.Library.GetPostBack.GetPostBackStr("strSearch");
            var eduUserList = NetSchool.BLL.EduUser.SearchCount("gender");
            List<object> lists = new List<object>();
            foreach (DataRow dr in eduUserList.Rows)
            {
                var obj = new { name = dr["gender"], value = dr["Count"] };
                lists.Add(obj);
            }
            strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", list = lists }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strreturn;
        }
        private static string GetListByTime()
        {
            string strreturn;
            bool isCountByTime = Convert.ToBoolean(NetSchool.Common.Library.GetPostBack.GetPostBackStr("isCountByTime"));
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

            var eduUserList = NetSchool.BLL.EduUser.GetCountListByDateTime(style: style);
            List<object> lists = new List<object>();
            if (eduUserList.Rows.Count > 0)
            {
                foreach (DataRow dr in eduUserList.Rows)
                {
                    var obj = new { name = dr["Time"], value = dr["Count"] };
                    lists.Add(obj);
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
            var eduUserList = NetSchool.BLL.EduUser.GetCountListByStamp(Year:year, company:territory);
            List<object> lists = new List<object>();
            foreach (DataRow dr in eduUserList.Rows)
            {
                var obj = new { name = dr["Month"], value = dr["Count"] };
                lists.Add(obj);
            }
            List<object> monthList = new List<object>();
            foreach (string month in months)
            {
                var obj = new { name = month };
                monthList.Add(obj);
            }
            int[] monthCountList = new int[months.Length];
            for (int i = 0; i < months.Length; i++)
            {
                    monthCountList[i] = (int)eduUserList.Rows[i]["Count"];
            }
            strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", year = year, list = lists, countList = monthCountList, monthList = months }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strreturn;
        }
        private static readonly string[] months = { "一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月" };
        private static void packageDatatable(DataTable dt)
        {
            dt.Columns.Add("Time", typeof(DateTime));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["createdTime"].ToString() != "0")
                    dt.Rows[i]["Time"] = Common.Library.ChangeTime.GetTime(dt.Rows[i]["createdTime"].ToString());
            }
            packageGenderDatatable(dt);
        }
        private static void packageGenderDatatable(DataTable dt)
        {
            dt.Columns.Add("Sex", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                switch (dt.Rows[i]["gender"].ToString())
                {
                    case "secret":
                        dt.Rows[i]["Sex"] = "保密";
                        break;
                    case "male":
                        dt.Rows[i]["Sex"] = "男性";
                        break;
                    case "female":
                        dt.Rows[i]["Sex"] = "女性";
                        break;
                    default:
                        dt.Rows[i]["Sex"] = "其他";
                        break;
                }
                if (dt.Rows[i]["createdTime"].ToString() != "0")
                    dt.Rows[i]["Time"] = Common.Library.ChangeTime.GetTime(dt.Rows[i]["createdTime"].ToString());
            }
        }
    }
}