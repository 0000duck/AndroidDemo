using System;
using System.Data;

namespace NetSchool.IDAL
{
    public interface IEduExamInfo
    {
        DataTable countStatus(Guid? Id, bool IsSearchCompany, string Company, bool IsSearchIdcard, string Idcard, bool IsSearchTerritory, string Territory, int? StartTime, int? EndTime, bool IsSearchStatus, string Status);
        DataTable Search(Guid? Id, bool IsSearchCompany, string Company, bool IsSearchIdcard, string Idcard, bool IsSearchTerritory, string Territory, int? StartTime, int? EndTime, bool IsSearchStatus, string Status, Common.Info.PageInfo pageInfo);
        DataTable SearchCount(string Cloumn, Guid? Id, bool IsSearchCompany, string Company, bool IsSearchIdcard, string Idcard, bool IsSearchTerritory, string Territory, int? StartTime, int? EndTime, bool IsSearchStatus, string Status);
        DataTable SearchStamp(Common.Enums.StampStruct[] StampPara, Guid? Id, bool IsSearchCompany, string Company, bool IsSearchIdcard, string Idcard, bool IsSearchTerritory, string Territory, int? StartTime, int? EndTime, bool IsSearchStatus, string Status);
    }
}
