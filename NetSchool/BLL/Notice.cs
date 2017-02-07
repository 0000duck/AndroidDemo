using System;
using System.Collections.Generic;
using System.Data;

namespace NetSchool.BLL
{
    public class Notice
    {
        private static readonly IDAL.INotice dal = DALFactory.DataAccess.CreateNotice();
        public static bool Add(Model.Notice model)
        {
            return dal.Add(model);
        }
        public static bool Update(Model.Notice model)
        {
            return dal.Update(model);
        }
        public static bool DeleteListByID(List<Guid> idList)
        {
            return dal.DeleteListByID(idList);
        }
        public static Model.Notice GetModel(Guid NoticeID)
        {
            var dt = SearchList<Model.Notice>(ID: NoticeID);
            if (dt.Length > 0)
                return dt[0];
            else
                return null;
        }
        public static DataTable Search(Guid? ID = null, bool IsSearchTitle = false, string Title = null, bool IsSearchContent = false, string Content = null, int? Type = null, bool IsSearchAuthor = false, string Author = null, DateTime? StartTime = null, DateTime? EndTime = null, Common.Info.PageInfo pageInfo = null)
        {
            return dal.Search(ID, IsSearchTitle, Title, IsSearchContent, Content, Type, IsSearchAuthor, Author, StartTime, EndTime, pageInfo);
        }
        public static T[] SearchList<T>(Guid? ID = null, bool IsSearchTitle = false, string Title = null, bool IsSearchContent = false, string Content = null, int? Type = null, bool IsSearchAuthor = false, string Author = null, DateTime? StartTime = null, DateTime? EndTime = null, Common.Info.PageInfo pageInfo = null) where T : Model.Notice, new()
        {
            return dal.SearchList<T>(ID, IsSearchTitle, Title, IsSearchContent, Content, Type, IsSearchAuthor, Author, StartTime, EndTime, pageInfo);
        }
    }
}
