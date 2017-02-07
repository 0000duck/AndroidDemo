using System;
using System.Data;
namespace NetSchool.IDAL
{
    public interface IEduLearning
    {
        DataTable Search(Guid? Id, bool IsSearchCompany, string Company, bool IsSearchIdcard, string Idcard, bool IsSearchTerritory, string Territory, int? StartTime, int? EndTime, Common.Info.PageInfo pageInfo);
        DataTable SearchCount(string Cloumn, Guid? Id, bool IsSearchCompany, string Company, bool IsSearchIdcard, string Idcard, bool IsSearchTerritory, string Territory, int? StartTime, int? EndTime);
        DataTable SearchStamp(Common.Enums.StampStruct[] StampPara, Guid? Id, bool IsSearchCompany, string Company, bool IsSearchIdcard, string Idcard, bool IsSearchTerritory, string Territory, int? StartTime, int? EndTime);
    }
}
