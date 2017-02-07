using System;
using System.Data;
using System.Collections.Generic;
namespace NetSchool.BLL
{
    public class News
    {
        private static readonly IDAL.INews dal = DALFactory.DataAccess.CreateNews();
        public static bool Add(Model.News model)
        {
            return dal.Add(model);
        }
        public static bool Update(Model.News model)
        {
            return dal.Update(model);
        }
        public static bool DeleteListByID(List<Guid> idList)
        {
            return dal.DeleteListByID(idList);
        }
        public static Model.News GetModel(Guid NewsID)
        {
            var dt = SearchList<Model.News>(ID: NewsID);
            if (dt.Length > 0)
                return dt[0];
            else
                return null;
        }
        public static DataTable Search(Guid? ID = null, bool IsSearchTitle = false, string Title = null, bool IsSearchContent = false, string Content = null, int? Type = null, bool IsSearchAuthor = false, string Author = null, DateTime? StartTime = null, DateTime? EndTime = null, Common.Info.PageInfo pageInfo = null)
        {
            return dal.Search(ID, IsSearchTitle, Title, IsSearchContent, Content, Type, IsSearchAuthor, Author, StartTime, EndTime, pageInfo);
        }
        public static T[] SearchList<T>(Guid? ID = null, bool IsSearchTitle = false, string Title = null, bool IsSearchContent = false, string Content = null, int? Type = null, bool IsSearchAuthor = false, string Author = null, DateTime? StartTime = null, DateTime? EndTime = null, Common.Info.PageInfo pageInfo = null) where T : Model.News, new()
        {
            return dal.SearchList<T>(ID, IsSearchTitle, Title, IsSearchContent, Content, Type, IsSearchAuthor, Author, StartTime, EndTime, pageInfo);
        }
    }
}
