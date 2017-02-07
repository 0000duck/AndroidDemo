using System;
using System.Collections.Generic;
using System.Data;

namespace NetSchool.BLL
{
    public class EduUser
    {
        private static readonly IDAL.IEduUser dal = DALFactory.DataAccess.CreateEduUser();
        public static bool Add(NetSchool.Model.EduUser model)
        {
            return dal.Add(model);
        }
        public static bool Update(NetSchool.Model.EduUser model)
        {
            return dal.Update(model);
        }
        public static bool DeleteListByID(List<Guid> idList)
        {
            return dal.DeleteListByID(idList);
        }
        public static Model.EduUser GetModel(Guid ID)
        {
            var dt = SearchList<Model.EduUser>(Id: ID);
            if (dt.Length > 0)
                return dt[0];
            else
                return null;
        }
        public static DataTable GetALLRolesDistinct()
        {
            return dal.GetALLRolesDistinct();
        }
        public static DataTable Search(Guid? Id = null, bool IsSearchCompany = false, string Company = null, bool IsSearchNickName = false, string NickName = null, bool IsSearchIdcard = false, string Idcard = null, bool IsSearchRoles = false, string Roles = null, bool IsSearchGender = false, string Gender = null,  int? StartTime = null,int? EndTime=null,  DateTime? CreateTime = null, Common.Info.PageInfo pageInfo = null)
        {
            return dal.Search(Id, IsSearchCompany, Company, IsSearchNickName, NickName, IsSearchIdcard, Idcard, IsSearchRoles, Roles, IsSearchGender, Gender, StartTime, EndTime, CreateTime, pageInfo);
        }
        public static T[] SearchList<T>(Guid? Id = null, bool IsSearchCompany = false, string Company = null, bool IsSearchNickName = false, string NickName = null, bool IsSearchIdcard = false, string Idcard = null, bool IsSearchRoles = false, string Roles = null, bool IsSearchGender = false, string Gender = null, bool IsSearchCreatedTime = false, int? StartTime = null, int? EndTime=null, bool IsSearchCreateTime = false, DateTime? CreateTime = null, Common.Info.PageInfo pageInfo = null) where T : Model.EduUser, new()
        {
            return dal.SearchList<T>(Id, IsSearchCompany, Company, IsSearchNickName, NickName, IsSearchIdcard, Idcard, IsSearchRoles, Roles, IsSearchGender, Gender, StartTime,EndTime, CreateTime, pageInfo);
        }
        public static DataTable SearchCount(string Cloumn)
        {
            return dal.SearchCount(Cloumn);
        }
        public static DataTable GetCountListByDateTime(Common.Enums.StatisticalTime style = Common.Enums.StatisticalTime.Year)
        {
            return dal.SearchDateTime(style);
        }
        public static DataTable GetCountListByStamp(int Year=2015, bool isSearchCompany = false, string company = null, bool isSearchIdcard = false, string idcard = null, bool isSearchRole=false, string role = null, bool isSearchGender=false, string gender = null)
        {
            DataTable talbe = SearchStamp(StampPara: GetStampParas(Year), isSearchCompany: isSearchCompany, company: company, isSearchIdcard: isSearchIdcard, idcard: idcard, isSearchRole: isSearchRole, role: role, isSearchGender: isSearchGender, gender: gender);
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
        public static DataTable SearchStamp(Common.Enums.StampStruct[] StampPara = null, bool isSearchCompany = false, string company = null, bool isSearchIdcard = false, string idcard = null, bool isSearchRole=false, string role = null, bool isSearchGender=false, string gender = null)
        {
            return dal.SearchStamp(StampPara,isSearchCompany,company,isSearchIdcard,idcard,isSearchRole,role,isSearchGender,gender);
        }
    }
}
