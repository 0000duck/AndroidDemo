using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace NetSchool.DAL
{
    public class Notice :IDAL.INotice
    {
        public static SqlParameter[] GetModelPare(Model.Notice model)
        {
            SqlParameter[] parameters = {
                                        new SqlParameter("@NoticeID",SqlDbType.UniqueIdentifier,16),
                                        new SqlParameter("@Title",SqlDbType.NVarChar,100),
                                        new SqlParameter("@Content",SqlDbType.NText),
                                        new SqlParameter("@Type",SqlDbType.Int,10),
                                        new SqlParameter("@Author",SqlDbType.NVarChar),
                                        new SqlParameter("@CreateTime",SqlDbType.DateTime),
                                        new SqlParameter("@ViewNum",SqlDbType.Int),
                                        new SqlParameter("@Editor",SqlDbType.NVarChar),
                                        new SqlParameter("@EditTime",SqlDbType.DateTime),
                                        new SqlParameter("@Attachment",SqlDbType.NVarChar ,500)
                                        };
            parameters[0].Value = model.NoticeID;
            if (string.IsNullOrWhiteSpace(model.Title))
                parameters[1].Value = DBNull.Value;
            else
                parameters[1].Value = model.Title;
            if (string.IsNullOrWhiteSpace(model.Content))
                parameters[2].Value = DBNull.Value;
            else
                parameters[2].Value = model.Content;
            parameters[3].Value = model.Type;
            if (string.IsNullOrWhiteSpace(model.Author))
                parameters[4].Value = DBNull.Value;
            else
                parameters[4].Value = model.Author;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.ViewNum;
            if (string.IsNullOrWhiteSpace(model.Editor))
                parameters[7].Value = DBNull.Value;
            else
                parameters[7].Value = model.Editor;
            parameters[8].Value = model.EditTime;
            if (string.IsNullOrWhiteSpace(model.Attachment))
                parameters[9].Value = DBNull.Value;
            else
                parameters[9].Value = model.Attachment;
            return parameters;
        }
        public static readonly string updatesql = "update Notice set NoticeID=@NoticeID,Title=@Title,Content=@Content,Type=@Type,Author=@Author,CreateTime=@CreateTime,ViewNum=@ViewNum,Editor=@Editor,EditTime=@EditTime,Attachment=@Attachment where NoticeID=@NoticeID";
        public static readonly string insertsql = "insert into Notice(NoticeID,Title,Content,Type,Author,CreateTime,ViewNum,Editor,EditTime,Attachment) values (@NoticeID,@Title,@Content,@Type,@Author,@CreateTime,@ViewNum,@Editor,@EditTime,@Attachment)";
        public bool Add(Model.Notice model)
        {
            return SqlHelper.SqlHelper.ExecuteNonQuery(insertsql, GetModelPare(model)) > 0;
        }
        public bool Update(Model.Notice model)
        {
            return SqlHelper.SqlHelper.ExecuteNonQuery(updatesql, GetModelPare(model)) > 0;
        }
        public bool DeleteListByID(List<Guid> idList)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Notice where NoticeID in (");
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
        public DataTable Search(Guid? ID, bool IsSearchTitle, string Title, bool IsSearchContent, string Content, int? Type, bool IsSearchAuthor, string Author, DateTime? StartTime, DateTime? EndTime, Common.Info.PageInfo pageInfo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Notice ");
            strSql.Append(" where 1=1");
            SqlParameter[] parameters ={
                                           new SqlParameter("@NoticeID",SqlDbType.UniqueIdentifier,16),
                                           new SqlParameter("@Title",SqlDbType.NVarChar,100),
                                           new SqlParameter("@Content",SqlDbType.NText),
                                           new SqlParameter("@Type",SqlDbType.Int),
                                           new SqlParameter("@Author",SqlDbType.NVarChar),
                                           new SqlParameter("@StartTime",SqlDbType.DateTime),
                                           new SqlParameter("@EndTime",SqlDbType.DateTime)
                                      };
            if (ID.HasValue)
            {
                strSql.Append(" and NoticeID=@NoticeID");
                parameters[0].Value = ID.Value;
            }
            else
            {
                parameters[0].Value = DBNull.Value;
            }
            if (IsSearchTitle)
            {
                strSql.Append(" and Title like '%'+@Title+'%'");
                parameters[1].Value = Title;
            }
            else
            {
                parameters[1].Value = DBNull.Value;
            }
            if (IsSearchContent)
            {
                strSql.Append(" and  Content like '%'+@Content+'%'");
                parameters[2].Value = Content;
            }
            else
            {
                parameters[2].Value = DBNull.Value;
            }
            if (Type.HasValue)
            {
                strSql.Append(" and Type=@Type");
                parameters[3].Value = Type;
            }
            else
            {
                parameters[3].Value = DBNull.Value;
            }
            if (IsSearchAuthor)
            {
                strSql.Append(" and Author like '%'+@Author+'%'");
                parameters[4].Value = Author;
            }
            else
            {
                parameters[4].Value = DBNull.Value;
            }
            if (StartTime.HasValue)
            {
                if (EndTime.HasValue)
                {
                    strSql.Append(" and createdTime > @StartTime");
                    strSql.Append(" and createdTime < @EndTime");
                    parameters[5].Value = StartTime.Value;
                    parameters[6].Value = EndTime.Value;
                }
                else
                {
                    parameters[5].Value = DBNull.Value;
                    parameters[6].Value = DBNull.Value;
                }
            }
            else
            {
                parameters[5].Value = DBNull.Value;
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
        public T[] SearchList<T>(Guid? ID, bool IsSearchTitle, string Title, bool IsSearchContent, string Content, int? Type, bool IsSearchAuthor, string Author, DateTime? StartTime, DateTime? EndTime, Common.Info.PageInfo pageInfo) where T : Model.Notice, new()
        {
            StringBuilder strSql = new StringBuilder();
            var parameters = new List<SqlParameter>();
            strSql.Append("select [NoticeID],[Title],[Content],[Type],[Author],[CreateTime],[ViewNum],[Editor],[EditTime],[Attachment] from Notice where 1=1");
            if (ID.HasValue)
            {
                strSql.Append(" and NoticeID=@NoticeID");
                var para = new SqlParameter("@NoticeID", SqlDbType.UniqueIdentifier, 16);
                para.Value = ID;
                parameters.Add(para);
            }
            if (IsSearchTitle)
            {
                if (string.IsNullOrWhiteSpace(Title))
                {
                    strSql.Append(" and [Title] is null ");
                }
                else
                {
                    strSql.Append(" and Title like '%'+@Title+'%'");
                    var para = new SqlParameter("@Title", SqlDbType.NVarChar, 100);
                    para.Value = Title;
                    parameters.Add(para);
                }
            }
            if (IsSearchContent)
            {
                if (string.IsNullOrWhiteSpace(Content))
                {
                    strSql.Append(" and [Content] is null ");
                }
                else
                {
                    strSql.Append(" and Content like '%'+@Content+'%'");
                    var para = new SqlParameter("@Content", SqlDbType.NText);
                    para.Value = Content;
                    parameters.Add(para);
                }
            }
            if (IsSearchAuthor)
            {
                if (string.IsNullOrWhiteSpace(Author))
                {
                    strSql.Append(" and [Author] is null ");
                }
                else
                {
                    strSql.Append(" and Author like '%'+@Author+'%'");
                    var para = new SqlParameter("@Author", SqlDbType.NVarChar);
                    para.Value = Author;
                    parameters.Add(para);
                }
            }
            if (StartTime.HasValue)
            {
                if (EndTime.HasValue)
                {
                    strSql.Append(" and CreateTime > @StartTime");
                    strSql.Append(" and CreateTime < @EndTime");
                    var para = new SqlParameter("@StartTime", SqlDbType.Date);
                    para.Value = StartTime;
                    parameters.Add(para);
                    var para1 = new SqlParameter("@EndTime", SqlDbType.Date);
                    para1.Value = EndTime;
                    parameters.Add(para1);
                }
            }
            var paraarry = parameters.ToArray();
            string pageSql = SqlHelper.SqlHelper.DoPageInfo(strSql, paraarry, pageInfo);
            return DateTableToArray<T>(SqlHelper.SqlHelper.ExecuteDataTable(pageSql, paraarry));
        }
        private static T[] DateTableToArray<T>(DataTable dt) where T : Model.Notice, new()
        {
            var modelList = new T[dt.Rows.Count];
            for (int n = 0; n < modelList.Length; n++)
            {
                modelList[n] = DataRowToModel<T>(dt.Rows[n]);
            }
            return modelList;
        }
        private static T DataRowToModel<T>(DataRow Rows) where T : Model.Notice, new()
        {
            var model = new T();
            model.NoticeID = (Guid)Rows["NoticeID"];
            if (Rows["Title"] != DBNull.Value)
                model.Title = (string)Rows["Title"];
            if (Rows["Content"] != DBNull.Value)
                model.Content = (string)Rows["Content"];
            model.Type = (int)Rows["Type"];
            if (Rows["Author"] != DBNull.Value)
                model.Author = (string)Rows["Author"];
            model.CreateTime = (DateTime)Rows["CreateTime"];
            model.ViewNum = (int)Rows["ViewNum"];
            if (Rows["Editor"] != DBNull.Value)
                model.Editor = (string)Rows["Editor"];
            model.EditTime = (DateTime)Rows["EditTime"];
            if (Rows["Attachment"] != DBNull.Value)
                model.Attachment = (string)Rows["Attachment"];
            return model;
        }
    }
}
