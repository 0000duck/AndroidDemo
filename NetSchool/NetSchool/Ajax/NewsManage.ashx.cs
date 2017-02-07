using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NetSchool.Common.WebBase;
using Microsoft.JScript;

namespace NetSchool.Ajax
{
    public class NewsManege : AjaxBase
    {

        protected override string SwitchCmd(HttpRequest Request)//暂时没有做登录验证
        {
            switch (NetSchool.Common.Library.GetPostBack.GetPostBackStr("cmd"))
            {
                case "getList":
                    return GetList();
                case "getInfo":
                    return GetInfo();
                case "addNews":
                    return AddNews();
                case "editNews":
                    return EditNews();
                case "deleteNews":
                    return DeleteNews();
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
            var newsList = NetSchool.BLL.News.SearchList<NetSchool.Model.News>(IsSearchTitle:isSearch,Title:strSearch,pageInfo:pageInfo);
            strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", count = pageInfo.RecordCount, list = newsList }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            
            return strreturn;
        }

        private static string GetInfo() {
            string strreturn;
            Guid newsId = NetSchool.Common.Library.GetPostBack.GetPostBackGuid("id", Guid.Empty);
            if (newsId != Guid.Empty)
            {
                NetSchool.Model.News newsInfo = NetSchool.BLL.News.GetModel(NewsID: newsId);
                newsInfo.ViewNum++;
                NetSchool.BLL.News.Update(newsInfo);
                strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", newsInfo=newsInfo }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "获取ID失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            return strreturn;
        }
        private static string AddNews()
        {
            string strreturn;
            string strTitle = NetSchool.Common.Library.GetPostBack.GetPostBackStr("title");
            string strContent = Microsoft.JScript.GlobalObject.unescape(NetSchool.Common.Library.GetPostBack.GetPostBackStr("content"));

            NetSchool.Model.News newsInfo = new Model.News();
            newsInfo.NewsID = Guid.NewGuid();
            newsInfo.Title = strTitle;
            newsInfo.Content = strContent;
            newsInfo.ViewNum = 0;
            newsInfo.Author = NetSchool.Common.Info.CurUserInfo.Username;
            newsInfo.CreateTime = DateTime.Now;
            newsInfo.EditTime = DateTime.Now;

            if (NetSchool.BLL.News.Add(newsInfo))
            {
                strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "插入失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            
            }
            return strreturn;
        }
        private static string EditNews()
        {
            string strreturn;
            Guid newsID = NetSchool.Common.Library.GetPostBack.GetPostBackGuid("id", Guid.Empty);
            string strTitle = NetSchool.Common.Library.GetPostBack.GetPostBackStr("title");
            string strContent = Microsoft.JScript.GlobalObject.unescape(NetSchool.Common.Library.GetPostBack.GetPostBackStr("content"));
            NetSchool.Model.News newsInfo = BLL.News.GetModel(newsID);
            newsInfo.Title = strTitle;
                newsInfo.Content = strContent;
                newsInfo.Editor = NetSchool.Common.Info.CurUserInfo.Username;
            newsInfo.EditTime = DateTime.Now;

            if (NetSchool.BLL.News.Update(newsInfo))
            {
                strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "更新失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

            }
            return strreturn;
        }
        private static string DeleteNews()
        {
            string strreturn;
            string ids = NetSchool.Common.Library.GetPostBack.GetPostBackStr("id");
            List<Guid> idList = new List<Guid>();
            foreach (string item in ids.Split(','))
            {
                idList.Add(new Guid(item));
            }
            if (NetSchool.BLL.News.DeleteListByID(idList))
            {
                strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                 strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "删除失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            
            return strreturn;
        }
    }
}