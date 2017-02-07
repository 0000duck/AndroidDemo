using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace NetSchool.DAL
{
    public class EduPayInfo : IDAL.IEduPayInfo
    {
        public DataTable Search(Guid? Id, bool IsSearchCompany, string Company, bool IsSearchIdcard, string Idcard, bool IsSearchTerritory, string Territory, int? StartTime, int? EndTime, bool IsSearchPayment, string Payment, bool IsSearchStatus, string Status, Common.Info.PageInfo pageInfo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *,(select [nickname] from [EduUser] where [EduUser].[idcard] = [EduPayInfo].[idcard]) as EduUserName from EduPayInfo");
            strSql.Append(" where 1=1");
            SqlParameter[] parameters ={
                                           new SqlParameter("@ID",SqlDbType.UniqueIdentifier,16),
                                           new SqlParameter("@Company",SqlDbType.NVarChar),
                                           new SqlParameter("@Idcard",SqlDbType.NVarChar),
                                           new SqlParameter("@Territory",SqlDbType.NVarChar),
                                           new SqlParameter("@StartTime",SqlDbType.Int,10),
                                           new SqlParameter("@EndTime",SqlDbType.Int,10),
                                           new SqlParameter("@Payment",SqlDbType.NVarChar),
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
                strSql.Append(" and company like '%'+@Company+'%'");
                parameters[1].Value = Company;
            }
            else
            {
                parameters[1].Value = DBNull.Value;
            }
            if (IsSearchIdcard)
            {
                strSql.Append(" and idcard like '%'+@Idcard+'%'");
                parameters[2].Value = Idcard;
            }
            else
            {
                parameters[2].Value = DBNull.Value;
            }
            if (IsSearchTerritory)
            {
                strSql.Append(" and territory like '%'+@Territory+'%'");
                parameters[3].Value = Territory;
            }
            else
            {
                parameters[3].Value = DBNull.Value;
            }


            if (StartTime.HasValue)
            {
                strSql.Append(" and createdTime >= @StartTime");
                parameters[4].Value = StartTime.Value;
            }
            else
            {
                parameters[4].Value = DBNull.Value;
                parameters[5].Value = DBNull.Value;
            }
            if (EndTime.HasValue)
            {
                strSql.Append(" and createdTime <= @EndTime");
                parameters[5].Value = EndTime.Value;
            }
            else
            {
                parameters[4].Value = DBNull.Value;
                parameters[5].Value = DBNull.Value;
            }
            if (IsSearchPayment)
            {
                strSql.Append(" and payment like '%'+@Payment+'%'");
                parameters[6].Value = Payment;
            }
            else
            {
                parameters[6].Value = DBNull.Value;
            }
            if (IsSearchStatus)
            {
                strSql.Append(" and status like '%'+@Status+'%'");
                parameters[7].Value = Status;
            }
            else
            {
                parameters[7].Value = DBNull.Value;
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
        public DataTable SearchCount(string Cloumn, bool IsSearchCompany, string Company, bool IsSearchIdcard, string Idcard, bool IsSearchTerritory, string Territory, int? StartTime, int? EndTime, bool IsSearchPayment, string Payment, bool IsSearchStatus, string Status)
        {
            StringBuilder strSql = new StringBuilder();
            var parameters = new List<SqlParameter>();
             strSql.Append(string.Format("select count(*) as Count,{0} from [dbo].[EduPayInfo] where 1=1", Cloumn));
            if (IsSearchCompany)
            {
                if (string.IsNullOrWhiteSpace(Company))
                {
                    strSql.Append(" and [company] is null ");
                }
                else
                {
                    strSql.Append(" and company like '%'+@Company+'%'");
                    var para = new SqlParameter("@Company", SqlDbType.NVarChar);
                    para.Value = Company;
                    parameters.Add(para);
                }
            }
            if (IsSearchIdcard)
            {
                if (string.IsNullOrWhiteSpace(Idcard))
                {
                    strSql.Append(" and [idcard] is null ");
                }
                else
                {
                    strSql.Append(" and idcard like '%'+@Idcard+'%'");
                    var para = new SqlParameter("@Idcard", SqlDbType.NVarChar);
                    para.Value = Idcard;
                    parameters.Add(para);
                }
            }
            if (IsSearchTerritory)
            {
                if (string.IsNullOrWhiteSpace(Territory))
                {
                    strSql.Append(" and [territory] is null ");
                }
                else
                {
                    strSql.Append(" and territory like '%'+@Territory+'%'");
                    var para = new SqlParameter("@Territory", SqlDbType.NVarChar);
                    para.Value = Territory;
                    parameters.Add(para);
                }
            }
            if (StartTime.HasValue)
            {
                if (EndTime.HasValue)
                {
                    strSql.Append(" and createdTime > @StartTime");
                    strSql.Append(" and createdTime < @EndTime");
                    var para = new SqlParameter("@StartTime", SqlDbType.Int);
                    para.Value = StartTime;
                    parameters.Add(para);
                    var para1 = new SqlParameter("@EndTime", SqlDbType.Int);
                    para1.Value = EndTime;
                    parameters.Add(para1);
                }
            }
            if (IsSearchPayment)
            {
                if (string.IsNullOrWhiteSpace(Status))
                {
                    strSql.Append(" and [payment] is null ");
                }
                else
                {
                    strSql.Append(" and payment like '%'+@Payment+'%'");
                    var para = new SqlParameter("@Payment", SqlDbType.NVarChar);
                    para.Value = Payment;
                    parameters.Add(para);
                }
            }
            if (IsSearchStatus)
            {
                if (string.IsNullOrWhiteSpace(Status))
                {
                    strSql.Append(" and [status] is null ");
                }
                else
                {
                    strSql.Append(" and status like '%'+@Status+'%'");
                    var para = new SqlParameter("@Status", SqlDbType.NVarChar);
                    para.Value = Status;
                    parameters.Add(para);
                }
            }
            strSql.Append(string.Format(" group by {0}", Cloumn));
            var paraarry = parameters.ToArray();
            return SqlHelper.SqlHelper.ExecuteDataTable(strSql.ToString(), paraarry);
        }
    }
}
