using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Data;

namespace NetSchool.BLL
{
    public class Main
    {
        public static Dictionary<string, DataTable> GetAllInfo()
        {
            Dictionary<string, DataTable> dic = null;
            Common.Info.PageInfo pageInfo = new Common.Info.PageInfo();
            int pageindex = 1;
            int pagesize = 5;
            pageInfo.PageSize = pagesize;
            pageInfo.CurrentPageIndex = pageindex - 1;
            pageInfo.IsPage = true;
            pageInfo.SortField1 = "CreateTime";
            pageInfo.SortType1 = NetSchool.Common.Enums.SortType.DESC;
            DataTable Law = BLL.Law.Search(pageInfo: pageInfo);
            DataTable News = BLL.News.Search(pageInfo: pageInfo);
            DataTable Notice = BLL.Notice.Search(pageInfo: pageInfo);
            dic = new Dictionary<string, DataTable>();
            dic.Add("Law", Law);
            dic.Add("Notice", Notice);
            dic.Add("News", News);
            return dic;
        }
    }
}
