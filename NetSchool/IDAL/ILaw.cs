using System;
using System.Collections.Generic;
using System.Data;

namespace NetSchool.IDAL
{
    public interface ILaw
    {
        bool Add(NetSchool.Model.Law model);
        bool Update(NetSchool.Model.Law model);
        bool DeleteListByID(List<Guid> idList);
        DataTable Search(Guid? ID, bool IsSearchTitle, string Title, bool IsSearchContent, string Content, int? Type, bool IsSearchAuthor, string Author, DateTime? StartTime, DateTime? EndTime, Common.Info.PageInfo pageInfo);
        T[] SearchList<T>(Guid? ID, bool IsSearchTitle, string Title, bool IsSearchContent, string Content, int? Type, bool IsSearchAuthor, string Author, DateTime? StartTime, DateTime? EndTime, Common.Info.PageInfo pageInfo) where T : Model.Law, new();
    }
}
