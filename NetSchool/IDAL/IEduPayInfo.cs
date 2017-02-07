using System;
using System.Data;

namespace NetSchool.IDAL
{
    public interface IEduPayInfo
    {
        DataTable Search(Guid? Id, bool IsSearchCompany, string Company, bool IsSearchIdcard, string Idcard, bool IsSearchTerritory, string Territory, int? StartTime, int? EndTime, bool IsSearchPayment, string Payment, bool IsSearchStatus, string Status, Common.Info.PageInfo pageInfo);
        DataTable SearchCount(string Cloumn, bool IsSearchCompany, string Company, bool IsSearchIdcard, string Idcard, bool IsSearchTerritory, string Territory, int? StartTime, int? EndTime, bool IsSearchPayment, string Payment, bool IsSearchStatus, string Status);
    }
}
