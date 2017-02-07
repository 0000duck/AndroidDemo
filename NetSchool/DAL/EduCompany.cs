using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace NetSchool.DAL
{
    public class EduCompany : IDAL.IEduCompany
    {
        public static SqlParameter[] GetModelPare(Model.EduCompany model)
        {
            SqlParameter[] parameters = {
                                        new SqlParameter("@ID",SqlDbType.UniqueIdentifier,16),
                                        new SqlParameter("@Company",SqlDbType.NVarChar),
                                        new SqlParameter("@Territory",SqlDbType.NVarChar),
                                        new SqlParameter("@CreatedTime",SqlDbType.Int,10),
                                        new SqlParameter("@CreateTime",SqlDbType.Date)
                                        };
            parameters[0].Value = model.Id;
            if (string.IsNullOrWhiteSpace(model.Company))
                parameters[1].Value = DBNull.Value;
            else
                parameters[1].Value = model.Company;
            if (string.IsNullOrWhiteSpace(model.Territory))
                parameters[2].Value = DBNull.Value;
            else
                parameters[2].Value = model.Territory;
            parameters[3].Value = model.CreatedTime;
            parameters[4].Value = model.CreateTime;

            return parameters;
        }
        public static readonly string updatesql = "update EduCompany set ID=@ID,company=@Company,territory=@Territory,createdTime=@CreatedTime,CreateTime=@CreateTime where ID=@ID";
        public static readonly string insertsql = "insert into EduCompany(ID,company,territory,createdTime,CreateTime) values (@ID,@Company,@Territory,@CreatedTime,@CreateTime)";
        public bool Add(Model.EduCompany model)
        {
            return SqlHelper.SqlHelper.ExecuteNonQuery(insertsql, GetModelPare(model)) > 0;
        }
        public bool Update(Model.EduCompany model)
        {
            return SqlHelper.SqlHelper.ExecuteNonQuery(updatesql, GetModelPare(model)) > 0;
        }
        public bool DeleteListByID(List<Guid> idList)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from EduCompany where ID in (");
            for (int i = 0; i < idList.Count; i++)
            {
                strSql.Append("'" + idList[i] + "'");
                if (i != idList.Count - 1)
                {
                    strSql.Append(",");
                }
            }
            strSql.Append(")");
            return SqlHelper.SqlHelper.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        public DataTable Search(Guid? ID, bool IsSearchCompany, string company, bool IsSearchTerritory, string territory, int? startTime, int? endTime, Common.Info.PageInfo pageInfo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from EduCompany");
            strSql.Append(" where 1=1");
            SqlParameter[] parameters ={
                                           new SqlParameter("@ID",SqlDbType.UniqueIdentifier,16),
                                           new SqlParameter("@Company",SqlDbType.NVarChar),
                                           new SqlParameter("@Territory",SqlDbType.NVarChar),
                                           new SqlParameter("@StartTime",SqlDbType.Int,10),
                                           new SqlParameter("@EndTime",SqlDbType.Int,10)
                                      };
            if (ID.HasValue)
            {
                strSql.Append(" and ID=@ID");
                parameters[0].Value = ID.Value;
            }
            else
            {
                parameters[0].Value = DBNull.Value;
            }
            if (IsSearchCompany)
            {
                strSql.Append(" and company like '%' +@Company+'%'");
                parameters[1].Value = company;
            }
            else
            {
                parameters[1].Value = DBNull.Value;
            }
            if (IsSearchTerritory)
            {
                strSql.Append(" and territory like '%' +@Territory+'%'");
                parameters[2].Value = territory;
            }
            else
            {
                parameters[2].Value = DBNull.Value;
            }
            if (startTime.HasValue)
            {
                if (endTime.HasValue)
                {
                    strSql.Append(" and createdTime > @StartTime");
                    strSql.Append(" and createdTime < @EndTime");
                    parameters[3].Value = startTime.Value;
                    parameters[4].Value = endTime.Value;
                }
                else
                {
                    parameters[3].Value = DBNull.Value;
                    parameters[4].Value = DBNull.Value;
                }
            }
            else
            {
                parameters[3].Value = DBNull.Value;
                parameters[4].Value = DBNull.Value;
            }
            string pageSql = strSql.ToString();
            if (pageInfo != null)
            {
                if (pageInfo.IsPage)
                {
                    SqlHelper.SqlHelper.getCountInfo(pageSql, pageInfo, parameters);
                    pageSql = SqlHelper.SqlHelper.PreparePageCommand(pageSql, pageInfo, parameters);
                }
                else
                {
                    pageSql = SqlHelper.SqlHelper.AppendSortString(pageSql, pageInfo);
                }
            }
            return SqlHelper.SqlHelper.ExecuteDataTable(pageSql, parameters);
        }
        public T[] SearchList<T>(Guid? ID, bool IsSearchCompany, string company, bool IsSearchTerritory, string territory, int? startTime, int? endTime, Common.Info.PageInfo pageInfo) where T : Model.EduCompany, new()
        {
            StringBuilder strSql = new StringBuilder();
            var parameters = new List<SqlParameter>();
            strSql.Append("select [ID],[company],[territory],[createdTime],[CreateTime] from EduCompany where 1=1");
            if (ID.HasValue)
            {
                strSql.Append(" and ID=@ID");
                var para = new SqlParameter("@ID", SqlDbType.UniqueIdentifier, 16);
                para.Value = ID;
                parameters.Add(para);
            }
            if (IsSearchCompany)
            {
                if (string.IsNullOrWhiteSpace(territory))
                {
                    strSql.Append(" and [company] is null ");
                }
                else
                {
                    strSql.Append(" and company like '%' +@Company+'%'");
                    var para = new SqlParameter("@Company", SqlDbType.NVarChar);
                    para.Value = company;
                    parameters.Add(para);
                }
            }
            if (IsSearchTerritory)
            {
                if (string.IsNullOrWhiteSpace(territory))
                {
                    strSql.Append(" and [territory] is null ");
                }
                else
                {
                    strSql.Append(" and territory like '%' +@Territory+'%'");
                    var para = new SqlParameter("@Territory", SqlDbType.NVarChar);
                    para.Value = territory;
                    parameters.Add(para);
                }
            }
            if (startTime.HasValue)
            {
                if (endTime.HasValue)
                {
                    strSql.Append(" and createdTime > @StartTime");
                    strSql.Append(" and createdTime < @EndTime");
                    var para = new SqlParameter("@StartTime", SqlDbType.Int);
                    para.Value = startTime;
                    parameters.Add(para);
                    var para1 = new SqlParameter("@EndTime", SqlDbType.Int);
                    para1.Value = endTime;
                    parameters.Add(para1);
                }
            }
            var paraarry = parameters.ToArray();
            string pageSql = SqlHelper.SqlHelper.DoPageInfo(strSql, paraarry, pageInfo);
            return DateTableToArray<T>(SqlHelper.SqlHelper.ExecuteDataTable(pageSql, paraarry));
        }
        public DataTable SearchTerritory(string territory, Common.Info.PageInfo pageInfo)
        {
            StringBuilder strSql = new StringBuilder();
            var parameters = new List<SqlParameter>();
            strSql.Append("select  distinct territory,count(territory)[Count]  from eduCompany where 1=1");
            if (!string.IsNullOrWhiteSpace(territory))
            {
                strSql.Append(" and territory like '%' +@Territory+'%'");
                var para = new SqlParameter("@Territory", SqlDbType.NVarChar);
                para.Value = territory;
                parameters.Add(para);
            }
            strSql.Append(" group by territory");
            var paraarry = parameters.ToArray();
            string pageSql = SqlHelper.SqlHelper.DoPageInfo(strSql, paraarry, pageInfo);
            return SqlHelper.SqlHelper.ExecuteDataTable(pageSql, paraarry);
        }
        public DataTable SearchStamp(Common.Enums.StampStruct[] StampPara,bool IsSearchTerritroy,string Territory)
        {
            StringBuilder strSql = new StringBuilder();
            var parameters = new List<SqlParameter>();
            strSql.Append("select ");
            if (IsSearchTerritroy)
            {
                for (int i = 0; i < StampPara.Length; i++)
                {
                    strSql.Append(string.Format(" (select COUNT(*) from [dbo].[EduCompany]  where createdTime>{1} and createdTime<{2} and territory like '%'+{3}+'%') as '{0}'", StampPara[i].name, StampPara[i].startTime, StampPara[i].endTime,Territory));
                    if (i != StampPara.Length - 1)
                    {
                        strSql.Append(",");
                    }
                }
            }
            else
            {
                for (int i = 0; i < StampPara.Length; i++)
                {
                    strSql.Append(string.Format(" (select COUNT(*) from [dbo].[EduCompany]  where createdTime>{1} and createdTime<{2}) as '{0}'", StampPara[i].name, StampPara[i].startTime, StampPara[i].endTime));
                    if (i != StampPara.Length - 1)
                    {
                        strSql.Append(",");
                    }
                }
            }
            var paraarry = parameters.ToArray();
            return SqlHelper.SqlHelper.ExecuteDataTable(strSql.ToString(), paraarry);
        }
        public DataTable SearchCount(bool IsCountByTerritory, bool IsCountByTime, Common.Enums.StatisticalTime countStyle, bool IsSearchTerritory, string territory, Common.Info.PageInfo pageInfo)
        {
            StringBuilder strSql = new StringBuilder();
            var parameters = new List<SqlParameter>();
            strSql.Append("select count(*) as Count ");
            if (IsCountByTerritory)
            {
                strSql.Append(",territory as Territory");
            }
            if (IsCountByTime)
            {
                strSql.Append(string.Format(",convert(varchar({0}),CreateTime,120) as Time", (int)countStyle));
            }
            //查询条件
            strSql.Append("  from eduCompany where 1=1 ");
            if (IsSearchTerritory)
            {
                if (string.IsNullOrEmpty(territory))
                {
                    strSql.Append(" and territory is null");
                }
                else
                {
                    strSql.Append(" and territory like '%' +@Territory+'%' ");
                    var para = new SqlParameter("@Territory", SqlDbType.NVarChar);
                    para.Value = territory;
                    parameters.Add(para);
                }
            }
            //统计方式
            if (IsCountByTerritory)
            {
                strSql.Append(" group by ");
                strSql.Append(" territory");
                if (IsCountByTime)
                {
                    strSql.Append(string.Format(",convert(varchar({0}),CreateTime,120)", (int)countStyle));
                }
            }
            else
            {
                if (IsCountByTime)
                {
                    strSql.Append(" group by ");
                    strSql.Append(string.Format(" convert(varchar({0}),CreateTime,120)", (int)countStyle));
                }
            }
            var paraarry = parameters.ToArray();
            string pageSql = SqlHelper.SqlHelper.DoPageInfo(strSql, paraarry, pageInfo);
            return SqlHelper.SqlHelper.ExecuteDataTable(pageSql, paraarry);
        }
        private static T[] DateTableToArray<T>(DataTable dt) where T : Model.EduCompany, new()
        {
            var modelList = new T[dt.Rows.Count];
            for (int n = 0; n < modelList.Length; n++)
            {
                modelList[n] = DataRowToModel<T>(dt.Rows[n]);
            }
            return modelList;
        }
        private static T DataRowToModel<T>(DataRow Rows) where T : Model.EduCompany, new()
        {
            var model = new T();
            model.Id = (Guid)Rows["ID"];
            if (Rows["company"] != DBNull.Value)
                model.Company = (string)Rows["company"];
            if (Rows["territory"] != DBNull.Value)
                model.Territory = (string)Rows["territory"];
            model.CreatedTime = (int)Rows["createdTime"];
            model.CreateTime = (DateTime)Rows["CreateTime"];
            return model;
        }
    }
}
