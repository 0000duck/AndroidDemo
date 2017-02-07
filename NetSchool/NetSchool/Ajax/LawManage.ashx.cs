using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSchool.Common.WebBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace NetSchool.Ajax
{
    public class LawManage : AjaxBase
    {
        protected override string SwitchCmd(HttpRequest Request)
        {
            switch (NetSchool.Common.Library.GetPostBack.GetPostBackStr("cmd"))
            {
                case "getList":
                    return GetList();
                case "getInfo":
                    return GetInfo();
                case "addLaw":
                    return AddLaw();
                case "editLaw":
                    return EditLaw();
                case "deleteLaw":
                    return DeleteLaw();
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
            var lawInfo = NetSchool.BLL.Law.SearchList<NetSchool.Model.Law>(IsSearchTitle: isSearch, Title: strSearch, pageInfo: pageInfo);
            strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", count = pageInfo.RecordCount, list = lawInfo }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

            return strreturn;
        }

        private static string GetInfo()
        {
            string strreturn;
            Guid LawId = NetSchool.Common.Library.GetPostBack.GetPostBackGuid("id", Guid.Empty);
            if (LawId != Guid.Empty)
            {
                NetSchool.Model.Law lawInfo = NetSchool.BLL.Law.GetModel(LawID: LawId);
                lawInfo.ViewNum++;
                NetSchool.BLL.Law.Update(lawInfo);
                strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", lawInfo = lawInfo }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "获取ID失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            return strreturn;
        }
        private static string AddLaw()
        {
            string strreturn;
            string strTitle = NetSchool.Common.Library.GetPostBack.GetPostBackStr("title");
            string strContent = Microsoft.JScript.GlobalObject.unescape(NetSchool.Common.Library.GetPostBack.GetPostBackStr("content"));

            NetSchool.Model.Law lawInfo = new Model.Law();
            lawInfo.LawID = Guid.NewGuid();
            lawInfo.Title = strTitle;
            lawInfo.Content = strContent;
            lawInfo.ViewNum = 0;
            lawInfo.Author = NetSchool.Common.Info.CurUserInfo.Username;
            lawInfo.CreateTime = DateTime.Now;
            lawInfo.EditTime = DateTime.Now;

            if (NetSchool.BLL.Law.Add(lawInfo))
            {
                strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "插入失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

            }
            return strreturn;
        }
        private static string EditLaw()
        {
            string strreturn;
            Guid LawID = NetSchool.Common.Library.GetPostBack.GetPostBackGuid("id", Guid.Empty);
            string strTitle = NetSchool.Common.Library.GetPostBack.GetPostBackStr("title");
            string strContent = Microsoft.JScript.GlobalObject.unescape(NetSchool.Common.Library.GetPostBack.GetPostBackStr("content"));
            NetSchool.Model.Law lawInfo = BLL.Law.GetModel(LawID);
            lawInfo.Title = strTitle;
            lawInfo.Content = strContent;
            lawInfo.Editor = NetSchool.Common.Info.CurUserInfo.Username;
            lawInfo.EditTime = DateTime.Now;

            if (NetSchool.BLL.Law.Update(lawInfo))
            {
                strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "更新失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

            }
            return strreturn;
        }
        private static string DeleteLaw()
        {
            string strreturn;
            string ids = NetSchool.Common.Library.GetPostBack.GetPostBackStr("id");
            List<Guid> idList = new List<Guid>();
            foreach (string item in ids.Split(','))
            {
                idList.Add(new Guid(item));
            }
            if (NetSchool.BLL.Law.DeleteListByID(idList))
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