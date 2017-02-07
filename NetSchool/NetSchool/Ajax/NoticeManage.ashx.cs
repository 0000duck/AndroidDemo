using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSchool.Common.WebBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NetSchool.Ajax
{
    public class NoticeManage : AjaxBase
    {
        protected override string SwitchCmd(HttpRequest Request)
        {
            switch (NetSchool.Common.Library.GetPostBack.GetPostBackStr("cmd"))
            {
                case "getList":
                    return GetList();
                case "getInfo":
                    return GetInfo();
                case "addNotice":
                    return AddNotice();
                case "editNotice":
                    return EditNotice();
                case "deleteNotice":
                    return DeleteNotice();
                default:
                    return base.SwitchCmd(Request);
            }
        }
        private static string GetList()
        {
            string strreturn;
            string strSearch = NetSchool.Common.Library.GetPostBack.GetPostBackStr("strSearch");
            int pageindex = NetSchool.Common.Library.GetPostBack.GetPostBackInt("pageindex", 1);
            int pagesize = NetSchool.Common.Library.GetPostBack.GetPostBackInt("pagesize", 20);
            NetSchool.Common.Info.PageInfo pageInfo = new Common.Info.PageInfo();
            pageInfo.PageSize = pagesize;
            pageInfo.CurrentPageIndex = pageindex - 1;
            pageInfo.IsPage = true;
            pageInfo.SortField1 = "CreateTime";
            pageInfo.SortType1 = NetSchool.Common.Enums.SortType.DESC;
            bool isSearch = false;
            if (strSearch != "")
            {
                isSearch = true;
            }
            var noticeInfo = NetSchool.BLL.Notice.SearchList<NetSchool.Model.Notice>(IsSearchTitle: isSearch, Title: strSearch, pageInfo: pageInfo);
            strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", count = pageInfo.RecordCount, list = noticeInfo }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

            return strreturn;
        }

        private static string GetInfo()
        {
            string strreturn;
            Guid NoticeId = NetSchool.Common.Library.GetPostBack.GetPostBackGuid("id", Guid.Empty);
            if (NoticeId != Guid.Empty)
            {
                NetSchool.Model.Notice noticeInfo = NetSchool.BLL.Notice.GetModel(NoticeID: NoticeId);
                noticeInfo.ViewNum++;
                NetSchool.BLL.Notice.Update(noticeInfo);
                strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", noticeInfo = noticeInfo }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "获取ID失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            return strreturn;
        }
        private static string AddNotice()
        {
            string strreturn;
            string strTitle = NetSchool.Common.Library.GetPostBack.GetPostBackStr("title");
            string strContent = Microsoft.JScript.GlobalObject.unescape(NetSchool.Common.Library.GetPostBack.GetPostBackStr("content"));

            NetSchool.Model.Notice noticeInfo = new Model.Notice();
            noticeInfo.NoticeID = Guid.NewGuid();
            noticeInfo.Title = strTitle;
            noticeInfo.Content = strContent;
            noticeInfo.ViewNum = 0;
            noticeInfo.Author = NetSchool.Common.Info.CurUserInfo.Username;
            noticeInfo.CreateTime = DateTime.Now;
            noticeInfo.EditTime = DateTime.Now;

            if (NetSchool.BLL.Notice.Add(noticeInfo))
            {
                strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "插入失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

            }
            return strreturn;
        }
        private static string EditNotice()
        {
            string strreturn;
            Guid NoticeID = NetSchool.Common.Library.GetPostBack.GetPostBackGuid("id", Guid.Empty);
            string strTitle = NetSchool.Common.Library.GetPostBack.GetPostBackStr("title");
            string strContent = Microsoft.JScript.GlobalObject.unescape(NetSchool.Common.Library.GetPostBack.GetPostBackStr("content"));
            NetSchool.Model.Notice noticeInfo = BLL.Notice.GetModel(NoticeID);
            noticeInfo.Title = strTitle;
            noticeInfo.Content = strContent;
            noticeInfo.Editor = NetSchool.Common.Info.CurUserInfo.Username;
            noticeInfo.EditTime = DateTime.Now;

            if (NetSchool.BLL.Notice.Update(noticeInfo))
            {
                strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "更新失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

            }
            return strreturn;
        }
        private static string DeleteNotice()
        {
            string strreturn;
            string ids = NetSchool.Common.Library.GetPostBack.GetPostBackStr("id");
            List<Guid> idList = new List<Guid>();
            foreach (string item in ids.Split(','))
            {
                idList.Add(new Guid(item));
            }
            if (NetSchool.BLL.Notice.DeleteListByID(idList))
            {
            }
            else
            {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "删除失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strreturn;
        }
    }
}