using System;
using System.Collections.Generic;
using System.Data;

namespace NetSchool.IDAL
{
    public interface IEduCompany
    {
        bool Add(NetSchool.Model.EduCompany model);
        bool Update(NetSchool.Model.EduCompany model);
        bool DeleteListByID(List<Guid> idList);
        DataTable Search(Guid? ID,bool IsSearchCompany,string company, bool IsSearchTerritory, string territory, int? startTime, int? endTime, Common.Info.PageInfo pageInfo);
        T[] SearchList<T>(Guid? ID, bool IsSearchCompany, string company, bool IsSearchTerritory, string territory, int? startTime, int? endTime, Common.Info.PageInfo pageInfo) where T : Model.EduCompany, new();
        DataTable SearchTerritory(string territory, Common.Info.PageInfo pageInfo);
        DataTable SearchStamp(Common.Enums.StampStruct[] StampPara,bool IsSearchTerritroy,string Territory);
        DataTable SearchCount(bool IsCountByTerritory, bool IsCountByTime, Common.Enums.StatisticalTime countStyle, bool IsSearchTerritory, string territory, Common.Info.PageInfo pageInfo);
    }
}
