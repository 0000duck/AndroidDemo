using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace NetSchool.DAL
{
    public class EduExamInfo : IDAL.IEduExamInfo
    {
        public DataTable countStatus(Guid? Id, bool IsSearchCompany, string Company, bool IsSearchIdcard, string Idcard, bool IsSearchTerritory, string Territory, int? StartTime, int? EndTime, bool IsSearchStatus, string Status)
        {
            StringBuilder strSql = new StringBuilder();
            var parameters = new List<SqlParameter>();
            strSql.Append("select  distinct [status],count([status])[Count]  from [EduExamInfo] where 1=1");
            if (IsSearchCompany)
            {
                strSql.Append(" and Company like '%' +@Company+'%'");
                var para = new SqlParameter("@Company", SqlDbType.NVarChar);
                para.Value = Company;
                parameters.Add(para);
            }
            if (IsSearchIdcard)
            {
                strSql.Append(" and Idcard like '%' +@Idcard+'%'");
                var para = new SqlParameter("@Idcard", SqlDbType.NVarChar);
                para.Value = Idcard;
                parameters.Add(para);
            }
            if (IsSearchTerritory)
            {
                strSql.Append(" and Territory like '%' +@Territory+'%'");
                var para = new SqlParameter("@Territory", SqlDbType.NVarChar);
                para.Value = Territory;
                parameters.Add(para);
            }
            if (IsSearchStatus)
            {
                strSql.Append(" and Status like '%' +@Status+'%'");
                var para = new SqlParameter("@Status", SqlDbType.NVarChar);
                para.Value = Status;
                parameters.Add(para);
            }
            if (StartTime.HasValue)
            {
                strSql.Append(" and [beginTime] >= @StartTime ");
                var para = new SqlParameter("@StartTime", SqlDbType.Int, 10);
                para.Value = StartTime;
                parameters.Add(para);
            }
            if (EndTime.HasValue)
            {
                strSql.Append(" and [endTime] <= @EndTime ");
                var para = new SqlParameter("@EndTime", SqlDbType.Int, 10);
                para.Value = EndTime; 
                parameters.Add(para);
            }
            strSql.Append(" group by [status]");
            var paraarry = parameters.ToArray();
            return SqlHelper.SqlHelper.ExecuteDataTable(strSql.ToString(), paraarry);
        }
        public DataTable Search(Guid? Id, bool IsSearchCompany, string Company, bool IsSearchIdcard, string Idcard, bool IsSearchTerritory, string Territory, int? StartTime, int? EndTime, bool IsSearchStatus, string Status, Common.Info.PageInfo pageInfo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *,(select [nickname] from [EduUser] where [EduUser].[idcard] = EduExamInfo.[idcard]) as EduUserName,(select [gender] from [EduUser] where [EduUser].[idcard] = EduExamInfo.[idcard]) as gender from EduExamInfo");
            strSql.Append(" where 1=1");
            SqlParameter[] parameters ={
                                           new SqlParameter("@ID",SqlDbType.UniqueIdentifier,16),
                                           new SqlParameter("@Company",SqlDbType.NVarChar),
                                           new SqlParameter("@Idcard",SqlDbType.NVarChar),
                                           new SqlParameter("@Territory",SqlDbType.NVarChar),
                                           new SqlParameter("@StartTime",SqlDbType.Int,10),
                                           new SqlParameter("@EndTime",SqlDbType.Int,10),
                                           new SqlParameter("@Status",SqlDbType.NVarChar)
                                      };
            if (Id.HasValue)
            {
                strSql.Append(" and ID=@ID");
                parameters[0].Value = Id.Value;
            }
            else
            {
                parameters[0].Value = DBNull.Value;
            }
            if (IsSearchCompany)
            {
                strSql.Append(" and company like '%' +@Company+'%'");
                parameters[1].Value = Company;
            }
            else
            {
                parameters[1].Value = DBNull.Value;
            }
            if (IsSearchIdcard)
            {
                strSql.Append(" and idcard like '%' +@Idcard+'%'");
                parameters[2].Value = Idcard;
            }
            else
            {
                parameters[2].Value = DBNull.Value;
            }
            if (IsSearchTerritory)
            {
                strSql.Append(" and territory like '%' +@Territory+'%'");
                parameters[3].Value = Territory;
            }
            else
            {
                parameters[3].Value = DBNull.Value;
            }
            if (StartTime.HasValue)
            {
                //strSql.Append(" and ((beginTime > @StartTime and beginTime < @EndTime) or (endTime > @StartTime and endTime < @EndTime))");
                strSql.Append(" and  endTime >= @StartTime ");
                parameters[4].Value = StartTime;
            }
            else
            {
                parameters[4].Value = DBNull.Value;

            }
            if (EndTime.HasValue)
            {
                strSql.Append(" and endTime <= @EndTime ");
                parameters[5].Value = EndTime;
            }
            else
            {
                parameters[5].Value = DBNull.Value;
            }




            if (IsSearchStatus)
            {
                strSql.Append(" and status=@Status");
                parameters[6].Value = Status;
            }
            else
            {
                parameters[6].Value = DBNull.Value;
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
        public DataTable SearchCount(string Cloumn, Guid? Id, bool IsSearchCompany, string Company, bool IsSearchIdcard, string Idcard, bool IsSearchTerritory, string Territory, int? StartTime, int? EndTime, bool IsSearchStatus, string Status)
        {
            StringBuilder strSql = new StringBuilder();
            var parameters = new List<SqlParameter>();
            StringBuilder aluse = new StringBuilder();
            if (IsSearchCompany)
            {
                aluse.Append(" and Company like '%' +@Company+'%'");
                var para = new SqlParameter("@Company", SqlDbType.NVarChar);
                para.Value = Company;
                parameters.Add(para);
            }
            if (IsSearchIdcard)
            {
                aluse.Append(" and Idcard like '%' +@Idcard+'%'");
                var para = new SqlParameter("@Idcard", SqlDbType.NVarChar);
                para.Value = Idcard;
                parameters.Add(para);
            }
            if (IsSearchTerritory)
            {
                aluse.Append(" and Territory like '%' +@Territory+'%'");
                var para = new SqlParameter("@Territory", SqlDbType.NVarChar);
                para.Value = Territory;
                parameters.Add(para);
            }
            if (IsSearchStatus)
            {
                aluse.Append(" and Status like '%' +@Status+'%'");
                var para = new SqlParameter("@Status", SqlDbType.NVarChar);
                para.Value = Status;
                parameters.Add(para);
            }
            if (StartTime.HasValue)
            {
                aluse.Append(" and [endTime] >= @StartTime ");
                var para = new SqlParameter("@StartTime", SqlDbType.Int, 10);
                para.Value = StartTime;
                parameters.Add(para);
            }
            if (EndTime.HasValue)
            {
                aluse.Append(" and [endTime] <= @EndTime ");
                var para = new SqlParameter("@EndTime", SqlDbType.Int, 10);
                para.Value = EndTime;
                parameters.Add(para);
            }
            strSql.Append(string.Format("select count(*) as Count,{0} from [dbo].[EduExamInfo] where 1=1{1} group by {0}", Cloumn,aluse));

            var paraarry = parameters.ToArray();
            return SqlHelper.SqlHelper.ExecuteDataTable(strSql.ToString(), paraarry);
        }
        public DataTable SearchStamp(Common.Enums.StampStruct[] StampPara,Guid? Id, bool IsSearchCompany, string Company, bool IsSearchIdcard, string Idcard, bool IsSearchTerritory, string Territory, int? StartTime, int? EndTime, bool IsSearchStatus, string Status)
        {
            StringBuilder strSql = new StringBuilder();
            var parameters = new List<SqlParameter>();
            strSql.Append("select ");
            StringBuilder aluse = new StringBuilder();
            if (IsSearchCompany)
            {
                aluse.Append(" and Company like '%' +@Company+'%'");
                var para = new SqlParameter("@Company", SqlDbType.NVarChar);
                para.Value = Company;
                parameters.Add(para);
            }
            if (IsSearchIdcard)
            {
                aluse.Append(" and Idcard like '%' +@Idcard+'%'");
                var para = new SqlParameter("@Idcard", SqlDbType.NVarChar);
                para.Value = Idcard;
                parameters.Add(para);
            }
            if (IsSearchTerritory)
            {
                aluse.Append(" and Territory like '%' +@Territory+'%'");
                var para = new SqlParameter("@Territory", SqlDbType.NVarChar);
                para.Value = Territory;
                parameters.Add(para);
            }
            if (IsSearchStatus)
            {
                aluse.Append(" and Status =@Status");
                var para = new SqlParameter("@Status", SqlDbType.NVarChar);
                para.Value = Status;
                parameters.Add(para);
            }
            for (int i = 0; i < StampPara.Length; i++)
            {
                strSql.Append(string.Format(" (select COUNT(*) from [dbo].[EduExamInfo]  where beginTime>{1} and beginTime<{2}{3}) as '{0}'", StampPara[i].name, StampPara[i].startTime, StampPara[i].endTime,aluse));
                if (i != StampPara.Length - 1)
                {
                    strSql.Append(",");
                }
            }
            var paraarry = parameters.ToArray();
            return SqlHelper.SqlHelper.ExecuteDataTable(strSql.ToString(), paraarry);
        }
    }
}
