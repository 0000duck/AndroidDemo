using System;
using System.Data;

namespace NetSchool.BLL
{
    public class EduPayInfo
    {
        private static readonly IDAL.IEduPayInfo dal = DALFactory.DataAccess.CreateEduPayInfo();
        public static DataTable Search(Guid? Id = null, bool IsSearchCompany = false, string Company = null, bool IsSearchIdcard = false, string Idcard = null, bool IsSearchTerritory = false, string Territory = null, int? StartTime = null, int? EndTime = null, bool IsSearchPayment = false, string Payment = null, bool IsSearchStatus = false, string Status = null, Common.Info.PageInfo pageInfo = null)
        {
            return dal.Search(Id, IsSearchCompany, Company, IsSearchIdcard, Idcard, IsSearchTerritory, Territory, StartTime, EndTime, IsSearchPayment, Payment, IsSearchStatus, Status, pageInfo);
        }
        public static DataTable SearchCount(string Cloumn=null, bool IsSearchCompany = false, string Company = null, bool IsSearchIdcard = false, string Idcard = null, bool IsSearchTerritory = false, string Territory = null, int? StartTime = null, int? EndTime = null, bool IsSearchPayment = false, string Payment = null, bool IsSearchStatus = false, string Status = null)
        {
            return dal.SearchCount(Cloumn, IsSearchCompany, Company, IsSearchIdcard, Idcard, IsSearchTerritory, Territory, StartTime, EndTime, IsSearchPayment, Payment, IsSearchStatus, Status);
        }
    }
}
