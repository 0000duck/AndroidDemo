using System;
using System.Data;
using System.Collections.Generic;

namespace NetSchool.IDAL
{
    public interface INews
    {
        bool Add(NetSchool.Model.News model);
        bool Update(NetSchool.Model.News model);
        bool DeleteListByID(List<Guid> idList);
        DataTable Search(Guid? ID, bool IsSearchTitle, string Title, bool IsSearchContent, string Content, int? Type, bool IsSearchAuthor, string Author,DateTime? StartTime, DateTime? EndTime, Common.Info.PageInfo pageInfo);
        T[] SearchList<T>(Guid? ID, bool IsSearchTitle, string Title, bool IsSearchContent, string Content, int? Type, bool IsSearchAuthor, string Author, DateTime? StartTime, DateTime? EndTime, Common.Info.PageInfo pageInfo) where T : Model.News, new();
    }
}
