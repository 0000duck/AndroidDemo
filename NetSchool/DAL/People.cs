using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace NetSchool.DAL
{
    public class People :IDAL.IPeople
    {
        public static SqlParameter[] GetModelPare(Model.People model)
        {
            SqlParameter[] parameters = {
                                        new SqlParameter("@UserName",SqlDbType.NVarChar,20),
                                        new SqlParameter("@RealName",SqlDbType.NVarChar,20),
                                        new SqlParameter("@Telephone",SqlDbType.NVarChar,20),
                                        new SqlParameter("@Email",SqlDbType.NVarChar,20),
                                        new SqlParameter("@Permission",SqlDbType.NVarChar,10),
                                        new SqlParameter("@Password",SqlDbType.NVarChar,100)
                                        };
            #region
            if (string.IsNullOrWhiteSpace(model.UserName))
                parameters[0].Value = DBNull.Value;
            else
                parameters[0].Value = model.UserName;
            if (string.IsNullOrWhiteSpace(model.RealName))
                parameters[1].Value = DBNull.Value;
            else
                parameters[1].Value = model.RealName;
            if (string.IsNullOrWhiteSpace(model.TelePhone))
                parameters[2].Value = DBNull.Value;
            else
                parameters[2].Value = model.TelePhone;
            if (string.IsNullOrWhiteSpace(model.Email))
                parameters[3].Value = DBNull.Value;
            else
                parameters[3].Value = model.Email;
            if (string.IsNullOrWhiteSpace(model.Permission))
                parameters[4].Value = DBNull.Value;
            else
                parameters[4].Value = model.Permission;
            if (string.IsNullOrWhiteSpace(model.Password))
                parameters[5].Value = DBNull.Value;
            else
                parameters[5].Value = model.Password;
            #endregion
            return parameters;
        }
        public static readonly string updatesql = "update People set UserName=@UserName,RealName=@RealName,Telephone=@Telephone,Email=@Email,Permission=@Permission,Password=@Password where UserName=@UserName";
        public static readonly string insertsql = "insert into People(UserName,RealName,Telephone,Email,Permission,Password) values (@UserName,@RealName,@Telephone,@Email,@Permission,@Password)";
        public bool Exists(string UserName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from People");
            strSql.Append(" where UserName=@UserName");
            SqlParameter[] parameters = {
					new SqlParameter("@UserName", SqlDbType.NVarChar,20)
						};
            parameters[0].Value = UserName;
            return SqlHelper.SqlHelper.Exists(strSql.ToString(), parameters);
        }
        public bool Add(Model.People model)
        {
            return SqlHelper.SqlHelper.ExecuteNonQuery(insertsql, GetModelPare(model)) > 0;
        }
        public bool Update(Model.People model)
        {
            return SqlHelper.SqlHelper.ExecuteNonQuery(updatesql, GetModelPare(model)) > 0;
        }
        public bool DeleteListByUserName(List<string> userNameList)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from People where UserName in (");
            for (int i = 0; i < userNameList.Count; i++)
            {
                strSql.Append("'" + userNameList[i] + "'");
                if (i != userNameList.Count - 1)
                {
                    strSql.Append(",");
                }
            }
            strSql.Append(")");
            return SqlHelper.SqlHelper.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        public DataTable Search(bool IsSearchUserName, string UserName, bool IsSearchRealName, string RealName, bool IsSearchTelephone, string Telephone, bool IsSearchEmail, string Email, bool IsSearchPermission, string Permission, bool IsSearchPassword, string Password, Common.Info.PageInfo pageInfo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from People  ");
            strSql.Append(" where 1=1");
            SqlParameter[] parameters ={
                                           new SqlParameter("@UserName",SqlDbType.NVarChar,20),
                                           new SqlParameter("@RealName",SqlDbType.NVarChar,20),
                                           new SqlParameter("@Telephone",SqlDbType.NVarChar),
                                           new SqlParameter("@Email",SqlDbType.NVarChar,50),
                                           new SqlParameter("@Permission",SqlDbType.NVarChar,10),
                                           new SqlParameter("@Password",SqlDbType.NVarChar,100)
                                      };
            if (IsSearchUserName)
            {
                strSql.Append(" and UserName = @UserName");
                parameters[0].Value = UserName;
            }
            else
            {
                parameters[0].Value = DBNull.Value;
            }
            if (IsSearchRealName)
            {
                strSql.Append(" and  RealName like '%'+ @RealName+'%'");
                parameters[1].Value = RealName;
            }
            else
            {
                parameters[1].Value = DBNull.Value;
            }
            if (IsSearchTelephone)
            {
                strSql.Append(" and Telephone like  '%'+@Telephone+'%'");
                parameters[2].Value = Telephone;
            }
            else
            {
                parameters[2].Value = DBNull.Value;
            }
            if (IsSearchEmail)
            {
                strSql.Append(" and Email like  '%'+@Email+'%'");
                parameters[3].Value = Email;
            }
            else
            {
                parameters[3].Value = DBNull.Value;
            }
            if (IsSearchPermission)
            {
                strSql.Append(" and Permission = @Permission");
                parameters[4].Value = Permission;
            }
            else
            {
                parameters[4].Value = DBNull.Value;
            }
            if (IsSearchPassword)
            {
                strSql.Append(" and Password = @Password");
                parameters[5].Value = Password;
            }
            else
            {
                parameters[5].Value = DBNull.Value;
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
        public  T[] SearchList<T>(bool IsSearchUserName, string UserName, bool IsSearchRealName, string RealName, bool IsSearchTelephone, string Telephone, bool IsSearchEmail, string Email, bool IsSearchPermission, string Permission, bool IsSearchPassword, string Password, Common.Info.PageInfo pageInfo) where T : Model.People, new()
        {
            var strSql = new StringBuilder();
            var parameters = new List<SqlParameter>();
            strSql.Append("select [UserName],[RealName],[Telephone],[Email],[Permission],[Password] FROM People where 1=1 ");
            if (IsSearchUserName)
            {
                if (string.IsNullOrWhiteSpace(UserName))
                {
                    strSql.Append(" and [UserName] is null ");
                }
                else
                {
                    strSql.Append(" and [UserName] like  '%'+@UserName+'%' ");
                    var para = new SqlParameter("@UserName", SqlDbType.NVarChar,20);
                    para.Value = UserName;
                    parameters.Add(para);
                }
            }
            if (IsSearchRealName)
            {
                if (string.IsNullOrWhiteSpace(RealName))
                {
                    strSql.Append(" and [RealName] is null ");
                }
                else
                {
                    strSql.Append(" and [RealName] like  '%'+@RealName+'%' ");
                    var para = new SqlParameter("@RealName", SqlDbType.NVarChar,20);
                    para.Value = RealName;
                    parameters.Add(para);
                }
            }
            if (IsSearchTelephone)
            {
                if (string.IsNullOrWhiteSpace(Telephone))
                {
                    strSql.Append(" and [Telephone] is null ");
                }
                else
                {
                    strSql.Append(" and [Telephone] like  '%'+@Telephone+'%' ");
                    var para = new SqlParameter("@Telephone", SqlDbType.NVarChar);
                    para.Value = Telephone;
                    parameters.Add(para);
                }
            }
            if (IsSearchEmail)
            {
                if (string.IsNullOrWhiteSpace(Email))
                {
                    strSql.Append(" and [Email] is null ");
                }
                else
                {
                    strSql.Append(" and [Email] like  '%'+@Email +'%'");
                    var para = new SqlParameter("@Email", SqlDbType.NVarChar,50);
                    para.Value = Email;
                    parameters.Add(para);
                }
            }
            if (IsSearchPermission)
            {
                if (string.IsNullOrWhiteSpace(Permission))
                {
                    strSql.Append(" and [Permission] is null ");
                }
                else
                {
                    strSql.Append(" and [Permission] like '%'+@Permission+'%' ");
                    var para = new SqlParameter("@Permission", SqlDbType.NVarChar,10);
                    para.Value = Permission;
                    parameters.Add(para);
                }
            }
            if (IsSearchPassword)
            {
                strSql.Append(" and [Password] = @Password ");
                var para = new SqlParameter("@Password", SqlDbType.NVarChar, 100);
                para.Value = Password;
                parameters.Add(para);
            }

            var paraarray = parameters.ToArray();
            string pageSql = NetSchool.DAL.SqlHelper.SqlHelper.DoPageInfo(strSql, paraarray, pageInfo);

            var resulttable = NetSchool.DAL.SqlHelper.SqlHelper.ExecuteDataTable(pageSql, paraarray);
            return DataTableToArray<T>(resulttable);
        }
        public  T[] DataTableToArray<T>(DataTable dt) where T : Model.People, new()
        {
            var modelList = new T[dt.Rows.Count];
            for (int n = 0; n < modelList.Length; n++)
                modelList[n] = DataRowToModel<T>(dt.Rows[n]);
            return modelList;
        }
        public  T DataRowToModel<T>(DataRow Rows) where T : Model.People, new()
        {
            var model = new T();
            if (Rows["UserName"] != DBNull.Value)
                model.UserName = (string)Rows["UserName"];
            if (Rows["RealName"] != DBNull.Value)
                model.RealName = (string)Rows["RealName"];
            if (Rows["Telephone"] != DBNull.Value)
                model.TelePhone = (string)Rows["Telephone"];
            if (Rows["Email"] != DBNull.Value)
                model.Email = (string)Rows["Email"];
            if (Rows["Permission"] != DBNull.Value)
                model.Permission = (string)Rows["Permission"];
            if (Rows["Password"] != DBNull.Value)
                model.Password = (string)Rows["Password"];
            return model;
        }
    }
}
