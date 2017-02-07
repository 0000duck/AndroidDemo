using System;
using System.Data;

namespace NetSchool.BLL
{
    public class EduLearning
    {
        private static readonly IDAL.IEduLearning dal = DALFactory.DataAccess.CreateEduLearning();
        public static DataTable Search(Guid? Id = null, bool IsSearchCompany = false, string Company = null, bool IsSearchIdcard = false, string Idcard = null, bool IsSearchTerritory = false, string Territory = null, int? StartTime = null, int? EndTime = null, Common.Info.PageInfo pageInfo = null)
        {
            return dal.Search(Id, IsSearchCompany, Company, IsSearchIdcard, Idcard, IsSearchTerritory, Territory, StartTime, EndTime, pageInfo);
        }
        public static DataTable SearchCount(string Cloumn=null, Guid? Id=null, bool IsSearchCompany=false, string Company=null, bool IsSearchIdcard=false, string Idcard=null, bool IsSearchTerritory=false, string Territory=null, int? StartTime=null, int? EndTime=null)
        {
            return dal.SearchCount(Cloumn,Id,IsSearchCompany,Company,IsSearchIdcard,Idcard,IsSearchTerritory,Territory,StartTime,EndTime);
        }
        public static DataTable GetCountListByStamp(int Year, Guid? Id = null, bool IsSearchCompany = false, string Company = null, bool IsSearchIdcard = false, string Idcard = null, bool IsSearchTerritory = false, string Territory = null, int? StartTime = null, int? EndTime = null)
        {
            DataTable talbe = SearchStamp(GetStampParas(Year), Id: Id, IsSearchCompany: IsSearchCompany, Company: Company, IsSearchIdcard: IsSearchIdcard, Idcard: Idcard, IsSearchTerritory: IsSearchTerritory, Territory: Territory, StartTime: StartTime, EndTime: EndTime);
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
        public static DataTable SearchStamp(Common.Enums.StampStruct[] StampPara ,Guid? Id = null, bool IsSearchCompany = false, string Company = null, bool IsSearchIdcard = false, string Idcard = null, bool IsSearchTerritory = false, string Territory = null, int? StartTime = null, int? EndTime = null)
        {
            return dal.SearchStamp(StampPara, Id, IsSearchCompany, Company, IsSearchIdcard, Idcard, IsSearchTerritory, Territory, StartTime, EndTime);
        }
    }
}
