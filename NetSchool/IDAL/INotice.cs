using System;
using System.Collections.Generic;
using System.Data;

namespace NetSchool.IDAL
{
    public interface INotice
    {
        bool Add(NetSchool.Model.Notice model);
        bool Update(NetSchool.Model.Notice model);
        bool DeleteListByID(List<Guid> idList);
        DataTable Search(Guid? ID, bool IsSearchTitle, string Title, bool IsSearchContent, string Content, int? Type, bool IsSearchAuthor, string Author,DateTime? StartTime, DateTime? EndTime, Common.Info.PageInfo pageInfo);
        T[] SearchList<T>(Guid? ID, bool IsSearchTitle, string Title, bool IsSearchContent, string Content, int? Type, bool IsSearchAuthor, string Author, DateTime? StartTime, DateTime? EndTime, Common.Info.PageInfo pageInfo) where T : Model.Notice, new();
    }
}
