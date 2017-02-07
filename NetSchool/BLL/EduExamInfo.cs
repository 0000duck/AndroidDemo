using System;
using System.Data;
using System.Collections.Generic;

namespace NetSchool.BLL
{
    public class EduExamInfo
    {
        private static readonly IDAL.IEduExamInfo dal = DALFactory.DataAccess.CreateEduExamInfo();
        public static DataTable countStatus(Guid? Id = null, bool IsSearchCompany = false, string Company = null, bool IsSearchIdcard = false, string Idcard = null, bool IsSearchTerritory = false, string Territory = null, int? StartTime = null, int? EndTime = null, bool IsSearchStatus = false, string Status = null)
        {
            return dal.countStatus(Id, IsSearchCompany, Company, IsSearchIdcard, Idcard, IsSearchTerritory, Territory, StartTime, EndTime, IsSearchStatus, Status);
        }
        public static DataTable Search(Guid? Id = null, bool IsSearchCompany = false, string Company = null, bool IsSearchIdcard = false, string Idcard = null, bool IsSearchTerritory = false, string Territory = null, int? StartTime = null, int? EndTime = null, bool IsSearchStatus = false, string Status = null, Common.Info.PageInfo pageInfo = null)
        {
            return dal.Search(Id, IsSearchCompany, Company, IsSearchIdcard, Idcard, IsSearchTerritory, Territory, StartTime, EndTime, IsSearchStatus, Status, pageInfo);
        }
        public static DataTable SearchCount(string Cloumn,Guid? Id = null, bool IsSearchCompany = false, string Company = null, bool IsSearchIdcard = false, string Idcard = null, bool IsSearchTerritory = false, string Territory = null, int? StartTime = null, int? EndTime = null, bool IsSearchStatus = false, string Status = null)
        {
            return dal.SearchCount(Cloumn, Id: Id, IsSearchCompany: IsSearchCompany, Company: Company, IsSearchIdcard: IsSearchIdcard, Idcard: Idcard, IsSearchTerritory: IsSearchTerritory, Territory: Territory, StartTime: StartTime, EndTime: EndTime, IsSearchStatus: IsSearchStatus, Status: Status);
        }
        public static DataTable GetCountListByStamp(int Year,Guid? Id = null, bool IsSearchCompany = false, string Company = null, bool IsSearchIdcard = false, string Idcard = null, bool IsSearchTerritory = false, string Territory = null, int? StartTime = null, int? EndTime = null, bool IsSearchStatus = false, string Status = null)
        {
            DataTable talbe = SearchStamp(GetStampParas(Year), Id, IsSearchCompany, Company, IsSearchIdcard, Idcard, IsSearchTerritory, Territory, StartTime, EndTime, IsSearchStatus, Status);
            DataTable tableTemp = new DataTable();
            DataColumn dc1 = new DataColumn("Month", Type.GetType("System.String"));
            DataColumn dc2 = new DataColumn("Count", Type.GetType("System.Int32"));
            tableTemp.Columns.Add(dc1);
            tableTemp.Columns.Add(dc2);
            for (int i = 0; i < 12; i++)
            {
                DataRow dr = tableTemp.NewRow();
                dr["Month"] = months[i];
                dr["Count"] = talbe.Rows[0][months[i]];
                tableTemp.Rows.Add(dr);
            }
            return tableTemp;
        }
        private static readonly string[] months = { "一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月" };
        private static Common.Enums.StampStruct[] GetStampParas(int Year)
        {
            Common.Enums.StampStruct[] stamps = new Common.Enums.StampStruct[12];
            string stTime;
            string eTime;
            for (int i = 0; i < 12; i++)
            {
                stamps[i].name = months[i];
                int st = i + 1;
                int et = i + 2;
                if (et > 12)
                {
                    Year += 1;
                    et = 1;
                }
                stTime = Year.ToString() + "-" + st.ToString() + "-01 00:00:00";
                eTime = Year.ToString() + "-" + et.ToString() + "-01 00:00:00";
                stamps[i].startTime = Common.Library.ChangeTime.GetStamp(stTime);
                stamps[i].endTime = Common.Library.ChangeTime.GetStamp(eTime);
            }
            return stamps;
        }
        public static DataTable SearchStamp(Common.Enums.StampStruct[] StampPara,Guid? Id = null, bool IsSearchCompany = false, string Company = null, bool IsSearchIdcard = false, string Idcard = null, bool IsSearchTerritory = false, string Territory = null, int? StartTime = null, int? EndTime = null, bool IsSearchStatus = false, string Status = null)
        {
            return dal.SearchStamp(StampPara, Id, IsSearchCompany, Company, IsSearchIdcard, Idcard, IsSearchTerritory, Territory, StartTime, EndTime, IsSearchStatus, Status);
        }
    }
}
