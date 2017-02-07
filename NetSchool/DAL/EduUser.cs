using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace NetSchool.DAL
{
    public partial class EduUser : IDAL.IEduUser
    {
        public EduUser() { }
        public static SqlParameter[] GetModelPare(Model.EduUser model)
        {
            SqlParameter[] parameters = {
                                        new SqlParameter("@ID",SqlDbType.UniqueIdentifier,16),
                                        new SqlParameter("@Nickname",SqlDbType.NVarChar),
                                        new SqlParameter("@Idcard",SqlDbType.NVarChar),
                                        new SqlParameter("@Company",SqlDbType.NVarChar),
                                        new SqlParameter("@Roles",SqlDbType.NVarChar),
                                        new SqlParameter("@Gender",SqlDbType.NVarChar), 
                                        new SqlParameter("@CreatedTime",SqlDbType.Int,10),
                                        new SqlParameter("@CreateTime",SqlDbType.Date)
                                        };
            parameters[0].Value = model.Id;
            if (string.IsNullOrWhiteSpace(model.Company))
                parameters[3].Value = DBNull.Value;
            else
                parameters[3].Value = model.Company;
            if (string.IsNullOrWhiteSpace(model.NickName))
                parameters[1].Value = DBNull.Value;
            else
                parameters[1].Value = model.NickName;
            if (string.IsNullOrWhiteSpace(model.Idcard))
                parameters[2].Value = DBNull.Value;
            else
                parameters[2].Value = model.Idcard;
            if (string.IsNullOrWhiteSpace(model.Roles))
                parameters[4].Value = DBNull.Value;
            else
                parameters[4].Value = model.Roles;
            if (string.IsNullOrWhiteSpace(model.Gender))
                parameters[5].Value = DBNull.Value;
            else
                parameters[5].Value = model.Gender;
            parameters[6].Value = model.CreatedTime;
            parameters[7].Value = model.CreateTime;

            return parameters;
        }
        public static readonly string updatesql = "update EduUser set ID=@ID,nickname=@Nickname,idcard=@Idcard,company=@Company,roles=@Roles,gender=@Gender,createdTime=@CreatedTime,CreateTime=@CreateTime where ID=@ID";
        public static readonly string insertsql = "insert into EduUser(ID,nickname,idcard,company,roles,gender,createdTime,CreateTime) values (@ID,@Nickname,@Idcard,@Company,@Roles,@Gender,@CreatedTime,@CreateTime)";
        public bool Add(Model.EduUser model)
        {
            return SqlHelper.SqlHelper.ExecuteNonQuery(insertsql, GetModelPare(model)) > 0;
        }
        public bool Update(Model.EduUser model)
        {
            return SqlHelper.SqlHelper.ExecuteNonQuery(updatesql, GetModelPare(model)) > 0;
        }
        public bool DeleteListByID(List<Guid> idList)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from EduUser where ID in (");
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
        public DataTable GetALLRolesDistinct()
        {
            string strSql = "select distinct roles from [dbo].[EduUser]";
            return SqlHelper.SqlHelper.ExecuteDataTable(strSql.ToString());
        }
        public DataTable Search(Guid? Id, bool IsSearchCompany, string Company, bool IsSearchNickName, string NickName, bool IsSearchIdcard, string Idcard, bool IsSearchRoles, string Roles, bool IsSearchGender, string Gender, int? StartTime, int? EndTime, DateTime? CreateTime, Common.Info.PageInfo pageInfo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from EduUser");
            strSql.Append(" where 1=1");
            SqlParameter[] parameters = {
                                        new SqlParameter("@ID",SqlDbType.UniqueIdentifier,16),
                                        new SqlParameter("@Company",SqlDbType.NVarChar),
                                        new SqlParameter("@NickName",SqlDbType.NVarChar),
                                        new SqlParameter("@Idcard",SqlDbType.NVarChar),
                                        new SqlParameter("@Roles",SqlDbType.NVarChar),
                                        new SqlParameter("@Gender",SqlDbType.NVarChar), 
                                        new SqlParameter("@StartTime",SqlDbType.Int,10),
                                        new SqlParameter("@EndTime",SqlDbType.Int,10),
                                        new SqlParameter("@CreateTime",SqlDbType.Date)
                                        };
            if (Id.HasValue)
            {
                strSql.Append(" and ID=@ID ");
                parameters[0].Value = Id.Value;
            }
            else
            {
                parameters[0].Value = DBNull.Value;
            }
            if (IsSearchCompany)
            {
                if (string.IsNullOrWhiteSpace(Company))
                {
                    strSql.Append(" and company is null ");
                    parameters[1].Value = DBNull.Value;
                }
                else
                {
                    strSql.Append(" and company like '%'+@Company+'%' ");
                    parameters[1].Value = Company;
                }
            }
            else
            {
                parameters[1].Value = DBNull.Value;
            }
            if (IsSearchNickName)
            {
                if (string.IsNullOrWhiteSpace(NickName))
                {
                    strSql.Append(" and nickname is null ");
                    parameters[2].Value = DBNull.Value;
                }
                else
                {
                    strSql.Append(" and nickname like '%'+@NickName+'%' ");
                    parameters[2].Value = NickName;
                }
            }
            else
            {
                parameters[2].Value = DBNull.Value;
            }
            if (IsSearchIdcard)
            {
                if (string.IsNullOrWhiteSpace(Idcard))
                {
                    strSql.Append(" and idcard is null ");
                    parameters[3].Value = DBNull.Value;
                }
                else
                {
                    strSql.Append(" and idcard like '%'+@Idcard+'%' ");
                    parameters[3].Value = Idcard;
                }
            }
            else
            {
                parameters[3].Value = DBNull.Value;
            }
            if (IsSearchRoles)
            {
                if (string.IsNullOrWhiteSpace(Roles) || Roles.Equals("null", StringComparison.CurrentCultureIgnoreCase))
                {
                    strSql.Append(" and roles is null ");
                    parameters[4].Value = DBNull.Value;
                }
                else
                {
                    strSql.Append(" and roles = @Roles ");
                    parameters[4].Value = Roles;
                }
            }
            else
            {
                parameters[4].Value = DBNull.Value;
            }
            if (IsSearchGender)
            {
                if (string.IsNullOrWhiteSpace(Gender))
                {
                    strSql.Append(" and gender is null ");
                    parameters[5].Value = DBNull.Value;
                }
                else
                {
                    strSql.Append(" and gender =@Gender ");
                    parameters[5].Value = Gender;
                }
            }
            else
            {
                parameters[5].Value = DBNull.Value;
            }
            if (StartTime.HasValue)
            {
                if (EndTime.HasValue)
                {
                    strSql.Append(" and createdTime>@StartTime ");
                    strSql.Append(" and createdTime<@EndTime ");
                    parameters[6].Value = StartTime.Value;
                    parameters[7].Value = EndTime.Value;
                }
                else
                {
                    parameters[6].Value = DBNull.Value;
                    parameters[7].Value = DBNull.Value;
                }
            }
            else
            {
                parameters[6].Value = DBNull.Value;
                parameters[7].Value = DBNull.Value;
            }
            if (CreateTime.HasValue)
            {
                strSql.Append(" and CreateTime=@CreateTime ");
                parameters[8].Value = CreateTime.Value;
            }
            else
            {
                parameters[8].Value = DBNull.Value;
            }
            string pageSql = strSql.ToString();
            if (pageInfo != null)
            {
                if (pageInfo.IsPage)
                {
                    NetSchool.DAL.SqlHelper.SqlHelper.getCountInfo(pageSql, pageInfo, parameters);
                    pageSql = NetSchool.DAL.SqlHelper.SqlHelper.PreparePageCommand(pageSql, pageInfo, parameters);
                }
                else
                {
                    pageSql = NetSchool.DAL.SqlHelper.SqlHelper.AppendSortString(pageSql, pageInfo);
                }
            }
            return NetSchool.DAL.SqlHelper.SqlHelper.ExecuteDataTable(pageSql, parameters);
        }
        public DataTable SearchCount(string Cloumn)
        {
            StringBuilder strSql = new StringBuilder();
            var parameters = new List<SqlParameter>();
            strSql.Append(string.Format("select count(*) as Count,{0} from [dbo].[EduUser]  group by {0}", Cloumn));
            var paraarry = parameters.ToArray();
            return SqlHelper.SqlHelper.ExecuteDataTable(strSql.ToString(), paraarry);
        }
        public DataTable SearchDateTime(Common.Enums.StatisticalTime style)
        {
            StringBuilder strSql = new StringBuilder();
            var parameters = new List<SqlParameter>();
            strSql.Append(string.Format("select   convert(varchar({0}),CreateTime,120) as Time,count(*) as Count  from eduUser where 1=1", (int)style));
            strSql.Append(string.Format(" group by convert(varchar({0}),CreateTime,120)", (int)style));
            var paraarry = parameters.ToArray();
            return SqlHelper.SqlHelper.ExecuteDataTable(strSql.ToString(), paraarry);
        }
        public DataTable SearchStamp(Common.Enums.StampStruct[] StampPara, bool isSearchCompany, string company, bool isSearchIdcard, string idcard, bool isSearchRole, string role, bool isSearchGender, string gender)
        {
            StringBuilder strSql = new StringBuilder();
            var parameters = new List<SqlParameter>();
            strSql.Append("select ");
            string aluse = "(select Count(*) from [dbo].[EduUser] where 1=1";
            if (isSearchCompany)
            {
                aluse += " and company like '" + company + "'";
            }
            if (isSearchGender)
            {
                aluse+=" and gender = '"+gender+"'";
            }
            if(isSearchIdcard)
            {
                aluse+=" and idcard like '"+idcard+"'";
            }
            if(isSearchRole)
            {
                if (role.Equals("null", StringComparison.CurrentCultureIgnoreCase))
                    aluse += " and roles is null";
                else
                    aluse += " and roles = '" + role + "'";
            }
            for (int i = 0; i < StampPara.Length; i++)
            {
                strSql.Append(string.Format(aluse+" and createdTime>{1} and createdTime<{2}) as '{0}'", StampPara[i].name, StampPara[i].startTime, StampPara[i].endTime));
                if (i != StampPara.Length - 1)
                {
                    strSql.Append(",");
                }
            }
            var paraarry = parameters.ToArray();
            return SqlHelper.SqlHelper.ExecuteDataTable(strSql.ToString(), paraarry);
        }
        public T[] SearchList<T>(Guid? Id, bool IsSearchCompany, string Company, bool IsSearchNickName, string NickName, bool IsSearchIdcard, string Idcard, bool IsSearchRoles, string Roles, bool IsSearchGender, string Gender, int? StartTime, int? EndTime, DateTime? CreateTime, Common.Info.PageInfo pageInfo) where T : Model.EduUser, new()
        {
            var strSql = new StringBuilder();
            var parameters = new List<SqlParameter>();
            strSql.Append("select [ID],[nickname],[idcard],[company],[roles],[gender],[createdTime],[CreateTime] FROM EduUser where 1=1 ");

            if (Id.HasValue)
            {
                strSql.Append(" and [ID] = @ID ");
                var para = new SqlParameter("@ID", SqlDbType.UniqueIdentifier, 16);
                para.Value = Id;
                parameters.Add(para);
            }


            if (IsSearchCompany)
            {
                if (string.IsNullOrWhiteSpace(Company))
                {
                    strSql.Append(" and [company] is null ");
                }
                else
                {
                    strSql.Append(" and [company] like '%'+@Company+'%' ");
                    var para = new SqlParameter("@Company", SqlDbType.NVarChar);
                    para.Value = Company;
                    parameters.Add(para);
                }
            }
            if (IsSearchNickName)
            {
                if (string.IsNullOrWhiteSpace(NickName))
                {
                    strSql.Append(" and [nickname] is null ");
                }
                else
                {
                    strSql.Append(" and [nickname] like '%'+@NickName+'%' ");
                    var para = new SqlParameter("@NickName", SqlDbType.NVarChar);
                    para.Value = NickName;
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
                    strSql.Append(" and [idcard] like '%'+@Idcard+'%' ");
                    var para = new SqlParameter("@Idcard", SqlDbType.NVarChar);
                    para.Value = Idcard;
                    parameters.Add(para);
                }
            }
            if (IsSearchRoles)
            {
                if (string.IsNullOrWhiteSpace(Roles)||Roles.Equals("null",StringComparison.CurrentCultureIgnoreCase))
                {
                    strSql.Append(" and [roles] is null ");
                }
                else
                {
                    strSql.Append(" and [roles] = @Roles ");
                    var para = new SqlParameter("@Roles", SqlDbType.NVarChar);
                    para.Value = Roles;
                    parameters.Add(para);
                }
            }
            if (IsSearchGender)
            {
                if (string.IsNullOrWhiteSpace(Gender))
                {
                    strSql.Append(" and [gender] is null ");
                }
                else
                {
                    strSql.Append(" and [gender] =@Gender ");
                    var para = new SqlParameter("@Gender", SqlDbType.NVarChar);
                    para.Value = Gender;
                    parameters.Add(para);
                }
            }
            if (StartTime.HasValue)
            {
                if (EndTime.HasValue)
                {
                    strSql.Append(" and [createdTime]> @StartTime ");
                    strSql.Append(" and [createdTime]< @EndTime ");
                    var para = new SqlParameter("@StartTime", SqlDbType.Int, 10);
                    para.Value = StartTime;
                    var para1 = new SqlParameter("@EndTime", SqlDbType.Int, 10);
                    para1.Value = EndTime;
                    parameters.Add(para);
                    parameters.Add(para1);
                }
            }
            if (CreateTime.HasValue)
            {
                strSql.Append(" and [CreateTime] = @CreateTime ");
                var para = new SqlParameter("@CreateTime", SqlDbType.DateTime);
                para.Value = CreateTime;
                parameters.Add(para);
            }

            var paraarray = parameters.ToArray();
            string pageSql = NetSchool.DAL.SqlHelper.SqlHelper.DoPageInfo(strSql, paraarray, pageInfo);

            var resulttable = NetSchool.DAL.SqlHelper.SqlHelper.ExecuteDataTable(pageSql, paraarray);
            return DataTableToArray<T>(resulttable);
        }
        public static T[] DataTableToArray<T>(DataTable dt) where T : Model.EduUser, new()
        {
            var modelList = new T[dt.Rows.Count];
            for (int n = 0; n < modelList.Length; n++)
                modelList[n] = DataRowToModel<T>(dt.Rows[n]);
            return modelList;
        }
        public static T DataRowToModel<T>(DataRow Rows) where T : Model.EduUser, new()
        {
            var model = new T();
            model.Id = (Guid)Rows["ID"];
            if (Rows["nickname"] != DBNull.Value)
                model.NickName = (string)Rows["nickname"];
            if (Rows["idcard"] != DBNull.Value)
                model.Idcard = (string)Rows["idcard"];
            if (Rows["company"] != DBNull.Value)
                model.Company = (string)Rows["company"];
            if (Rows["roles"] != DBNull.Value)
                model.Roles = (string)Rows["roles"];
            if (Rows["gender"] != DBNull.Value)
                model.Gender = (string)Rows["gender"];
            model.CreatedTime = (int)Rows["createdTime"];
            model.CreateTime = (DateTime)Rows["CreateTime"];
            return model;
        }
    }
}
