using System;
using System.Collections.Generic;
using System.Data;

namespace NetSchool.BLL
{
    public class EduCompany
    {
        private static readonly IDAL.IEduCompany dal = DALFactory.DataAccess.CreateEduCompany();
        public static bool Add(Model.EduCompany model)
        {
            return dal.Add(model);
        }
        public static bool Update(Model.EduCompany model)
        {
            return dal.Update(model);
        }
        public static bool DeleteListByID(List<Guid> idList)
        {
            return dal.DeleteListByID(idList);
        }
        public static Model.EduCompany GetModel(Guid ID)
        {
            var dt = SearchList<Model.EduCompany>(ID: ID);
            if (dt.Length > 0)
                return dt[0];
            else
                return null;
        }
        public static DataTable Search(Guid? ID = null,bool IsSearchCompany=false,string company=null, bool IsSearchTerritory = false, string territory = null, int? startTime = null, int? endTime = null, Common.Info.PageInfo pageInfo = null)
        {
            return dal.Search(ID,IsSearchCompany,company,IsSearchTerritory,territory,startTime,endTime,pageInfo);
        }
        public static T[] SearchList<T>(Guid? ID = null,bool IsSearchCompany=false,string company=null, bool IsSearchTerritory = false, string territory = null, int? startTime = null, int? endTime = null, Common.Info.PageInfo pageInfo = null) where T : Model.EduCompany, new()
        {
            return dal.SearchList<T>(ID, IsSearchCompany,company, IsSearchTerritory, territory, startTime, endTime, pageInfo);
        }
        public static DataTable SearchTerritory(string territory = null, Common.Info.PageInfo pageInfo = null)
        {
            return dal.SearchTerritory(territory,pageInfo);
        }
        public static DataTable GetCountListByStamp(int Year, string terrtiroy)
        {
            bool isSearchTerritory = false;
            if (string.IsNullOrEmpty(terrtiroy))
            {
                isSearchTerritory = true;
            }
            DataTable talbe = SearchStamp(GetStampParas(Year),isSearchTerritory,terrtiroy);
            DataTable tableTemp = new DataTable();
            DataColumn dc1 = new DataColumn("Month", Type.GetType("System.String"));
            DataColumn dc2 = new DataColumn("Count", Type.GetType("System.Int32"));
            tableTemp.Columns.Add(dc1);
            tableTemp.Columns.Add(dc2);
            for (int i = 0; i < 12;i++ )
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
                stamps[i].name =months[i];
                int st = i + 1;
                int et = i + 2;
                if(et>12)
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
        private static DataTable SearchStamp(Common.Enums.StampStruct[] StampPara,bool IsSearchTerritory,string terrtiroy)
        {
            return dal.SearchStamp(StampPara, IsSearchTerritory, terrtiroy);
        }
        public static DataTable SearchCount(bool IsCountByTerritory = true, bool IsCountByTime = true, Common.Enums.StatisticalTime countStyle=Common.Enums.StatisticalTime.Year, bool IsSearchTerritory=false, string territory=null, Common.Info.PageInfo pageInfo=null)
        {
            return dal.SearchCount(IsCountByTerritory, IsCountByTime, countStyle, IsSearchTerritory, territory, pageInfo);
        }
    }
}
