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
    public class EduPayInfoManage : AjaxBase
    {
        protected override string SwitchCmd(HttpRequest Request)
        {
            switch (NetSchool.Common.Library.GetPostBack.GetPostBackStr("cmd"))
            {
                case "countTerritory":
                    return CountTerritory();
                case "countStatus":
                    return CountStatus();
                case "countPayment":
                    return CountPayMent();
                case "getList":
                    return GetList();
                default:
                    return base.SwitchCmd(Request);
            }
        }
        private static string CountStatus()
        {
            string strReturn;
            #region 参数设置
            string cloumn="Status";
            string serCompany = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serCompany");
            string serIDCard = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serIDCard");
            string serTerritory = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serTerritory");
            string serBeginTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serBeginTime");
            string serEndTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serEndTime");
            string serPayment = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serPayment");
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
            if ("all" == serStatus || "" == serStatus)
            {
                searchStatus = false;
            }
            bool searchPaymant = true;
            if ("all" == serPayment || "" == serPayment)
            {
                searchPaymant = false;
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
            #endregion
            DataTable dtExamInfoList = NetSchool.BLL.EduPayInfo.SearchCount(Cloumn: cloumn, IsSearchCompany: searchCompany, Company: serCompany, IsSearchIdcard: searchIDCard, Idcard: serIDCard, IsSearchTerritory: searchTerritory, Territory: serTerritory, IsSearchStatus: searchStatus, Status: serStatus, IsSearchPayment: searchPaymant, Payment: serPayment, StartTime: stamp.startTime, EndTime: stamp.endTime);
            packageStatusDatatable(dtExamInfoList);
            List<object> lists = new List<object>();
            foreach (DataRow dr in dtExamInfoList.Rows)
            {
                var obj = new { name = dr["statusName"], value = dr["Count"] };
                lists.Add(obj);
            }
            strReturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", list = lists }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strReturn;
        }
        private static string CountPayMent()
        {
            string strReturn;
            #region 参数获取
            string cloumn = "payment";
            string serCompany = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serCompany");
            string serIDCard = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serIDCard");
            string serTerritory = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serTerritory");
            string serBeginTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serBeginTime");
            string serEndTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serEndTime");
            string serPayment = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serPayment");
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
            bool searchPaymant = true;
            if ("all" == serPayment)
            {
                searchPaymant = false;
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
            #endregion
            DataTable dtExamInfoList = NetSchool.BLL.EduPayInfo.SearchCount(Cloumn: cloumn, IsSearchCompany: searchCompany, Company: serCompany, IsSearchIdcard: searchIDCard, Idcard: serIDCard, IsSearchTerritory: searchTerritory, Territory: serTerritory, IsSearchStatus: searchStatus, Status: serStatus, IsSearchPayment: searchPaymant, Payment: serPayment, StartTime: stamp.startTime, EndTime: stamp.endTime);
            
            packagePaymentDatatable(dtExamInfoList);
            List<object> lists = new List<object>();
            foreach (DataRow dr in dtExamInfoList.Rows)
            {
                var obj = new { name = dr["payMentName"], value = dr["Count"] };
                lists.Add(obj);
            }
            strReturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", list = lists }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strReturn;
        }
        private static string CountTerritory()
        {
            string strReturn;
            #region 参数获取
            string cloumn = "territory";
            string serCompany = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serCompany");
            string serIDCard = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serIDCard");
            string serTerritory = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serTerritory");
            string serBeginTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serBeginTime");
            string serEndTime = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serEndTime");
            string serPayment = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serPayment");
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
            bool searchPaymant = true;
            if ("all" == serPayment)
            {
                searchPaymant = false;
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
            #endregion
            DataTable dtExamInfoList = NetSchool.BLL.EduPayInfo.SearchCount(Cloumn: cloumn, IsSearchCompany: searchCompany, Company: serCompany, IsSearchIdcard: searchIDCard, Idcard: serIDCard, IsSearchTerritory: searchTerritory, Territory: serTerritory, IsSearchStatus: searchStatus, Status: serStatus, IsSearchPayment: searchPaymant, Payment: serPayment, StartTime: stamp.startTime, EndTime: stamp.endTime);
            List<object> lists = new List<object>();
            foreach (DataRow dr in dtExamInfoList.Rows)
            {
                var obj = new { name = dr["territory"], value = dr["Count"] };
                lists.Add(obj);
            }
            strReturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", list = lists }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
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
            string serPayment = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serPayment");
            string serStatus = NetSchool.Common.Library.GetPostBack.GetPostBackStr("serStatus");
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
            bool searchStatus = true;
            if ("all" == serStatus)
            {
                searchStatus = false;
            }
            bool searchPaymant = true;
            if ("all" == serPayment)
            {
                searchPaymant = false;
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
            dtExamInfoList = NetSchool.BLL.EduPayInfo.Search(IsSearchCompany: searchCompany, Company: serCompany, IsSearchIdcard: searchIDCard, Idcard: serIDCard, IsSearchTerritory: searchTerritory, Territory: serTerritory, IsSearchStatus: searchStatus, Status: serStatus, IsSearchPayment:searchPaymant,Payment:serPayment,StartTime: stamp.startTime, EndTime: stamp.endTime, pageInfo: pageInfo);
            packageDatatable(dtExamInfoList);
            strReturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", count = pageInfo.RecordCount, list = dtExamInfoList }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strReturn;
        }
        private static void packageDatatable(DataTable dt)
        {
            dt.Columns.Add("Time", typeof(DateTime));
            dt.Columns.Add("payMentName", typeof(string));
            dt.Columns.Add("statusName", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["createdTime"].ToString() != "0")
                    dt.Rows[i]["time"] = Common.Library.ChangeTime.GetTime(dt.Rows[i]["createdTime"].ToString());
                switch (dt.Rows[i]["status"].ToString())
                {
                    case "created":
                        dt.Rows[i]["statusName"] = "创建未支付";
                        break;
                    case "paid":
                        dt.Rows[i]["statusName"] = "已支付";
                        break;
                    case "refunding":
                        dt.Rows[i]["statusName"] = "退款中";
                        break;
                    case "refunded":
                        dt.Rows[i]["statusName"] = "已退款";
                        break;
                    case "cancelled":
                        dt.Rows[i]["statusName"] = "取消";
                        break;
                    default:
                        dt.Rows[i]["statusName"] = "其他";
                        break;
                }
                switch (dt.Rows[i]["payment"].ToString())
                {
                    case "none":
                        dt.Rows[i]["payMentName"] = "无";
                        break;
                    case "alipay":
                        dt.Rows[i]["payMentName"] = "支付宝";
                        break;
                    case "tenpay":
                        dt.Rows[i]["payMentName"] = "财付通";
                        break;
                    case "coin":
                        dt.Rows[i]["payMentName"] = "一卡通";
                        break;
                    case "wxpay":
                        dt.Rows[i]["payMentName"] = "微信支付";
                        break;
                    default:
                        dt.Rows[i]["statusName"] = "其他";
                        break;
                }
            }
        }
        private static void packageStatusDatatable(DataTable dt)
        {
            dt.Columns.Add("statusName", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                switch (dt.Rows[i]["status"].ToString())
                {
                    case "created":
                        dt.Rows[i]["statusName"] = "创建未支付";
                        break;
                    case "paid":
                        dt.Rows[i]["statusName"] = "已支付";
                        break;
                    case "refunding":
                        dt.Rows[i]["statusName"] = "退款中";
                        break;
                    case "refunded":
                        dt.Rows[i]["statusName"] = "已退款";
                        break;
                    case "cancelled":
                        dt.Rows[i]["statusName"] = "取消";
                        break;
                    default:
                        dt.Rows[i]["statusName"] = "其他";
                        break;
                }
            }
        }
        private static void packagePaymentDatatable(DataTable dt)
        {
            dt.Columns.Add("payMentName", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                switch (dt.Rows[i]["payment"].ToString())
                {
                    case "none":
                        dt.Rows[i]["payMentName"] = "未支付";
                        break;
                    case "alipay":
                        dt.Rows[i]["payMentName"] = "支付宝";
                        break;
                    case "tenpay":
                        dt.Rows[i]["payMentName"] = "财付通";
                        break;
                    case "coin":
                        dt.Rows[i]["payMentName"] = "一卡通";
                        break;
                    case "wxpay":
                        dt.Rows[i]["payMentName"] = "微信支付";
                        break;
                    default:
                        dt.Rows[i]["statusName"] = "其他";
                        break;
                }
            }
        }
    }
}