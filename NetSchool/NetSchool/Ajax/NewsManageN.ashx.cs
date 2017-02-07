using System.Web;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NetSchool.Common.WebBase;
using Microsoft.JScript;

namespace NetSchool.Ajax
{
    public class NewsManageN : AjaxBaseN
    {

        protected override string SwitchCmdN(HttpRequest Request)
        {
            switch (NetSchool.Common.Library.GetPostBack.GetPostBackStr("cmd"))
            {
                case "getList":
                    return GetList();
                default:
                    return base.SwitchCmdN(Request);
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
            var newsList = NetSchool.BLL.News.SearchList<NetSchool.Model.News>(IsSearchTitle: isSearch, Title: strSearch, pageInfo: pageInfo);
            strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", count = pageInfo.RecordCount, list = newsList }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

            return strreturn;
        }
    }
}
