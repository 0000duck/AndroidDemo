using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace NetSchool.DAL.SqlHelper
{
    
    public static partial class SqlHelper
    {
        
        public static readonly string CONN_STRING = GetDBConnectionString();
        private static string GetDBConnectionString()
        {
            string m_DBServer = ConfigurationManager.AppSettings["DS"];
            string m_DBUser = ConfigurationManager.AppSettings["UID"];
            string m_DBPwd = ConfigurationManager.AppSettings["PWD"];
            string m_DB = ConfigurationManager.AppSettings["DATABASE"];

            m_DBUser = NetSchool.Common.EnDeCryption.Base64.Decode(m_DBUser, Encoding.UTF8);
            m_DBPwd = Common.EnDeCryption.JCode.Decode(Common.EnDeCryption.Base64.Decode(m_DBPwd, Encoding.UTF8));
            return string.Format("Data Source={0};UID={1};PWD={2};DATABASE={3};pooling=true;Max Pool Size=512;Trusted_Connection=False;Connection Lifetime=60", m_DBServer, m_DBUser, m_DBPwd, m_DB);
        }
        public static string AppendSortString(string Sql, Common.Info.PageInfo pageinfo)
        {
            if (pageinfo.IsSelectTop)
                Sql = Sql.Insert(Sql.IndexOf("select", StringComparison.OrdinalIgnoreCase) + 6, " top " + pageinfo.PageSize);

            if (!string.IsNullOrWhiteSpace(pageinfo.SortField1))
            {
                Sql += " order by " + pageinfo.SortField1 + " " + pageinfo.SortType1;
                if (!string.IsNullOrWhiteSpace(pageinfo.SortField2))
                {
                    Sql += " ," + pageinfo.SortField2 + " " + pageinfo.SortType2;
                    if (!string.IsNullOrWhiteSpace(pageinfo.SortField3))
                    {
                        Sql += " ," + pageinfo.SortField3 + " " + pageinfo.SortType3;
                    }
                }
            }
            return Sql;
        }
        private static readonly string QueryCountCommandText = "SELECT COUNT(*) FROM ({0}) AS t0";
        private static readonly string QueryPageCommandText = "SELECT * FROM (SELECT TOP {0} * FROM (SELECT TOP {1} * FROM ({2}) AS t0 ORDER BY {3} {4}) AS t1 ORDER BY {3} {5}) AS t2 ORDER BY {3} {4}";
        private static readonly string QueryPageCommandText2 = "SELECT * FROM (SELECT TOP {0} * FROM (SELECT TOP {1} * FROM ({2}) AS t0 ORDER BY {3} {4},{6} {7}) AS t1 ORDER BY {3} {5}, {6} {8}) AS t2 ORDER BY {3} {4}, {6} {7}";
        private static readonly string QueryPageCommandText3 = "SELECT * FROM (SELECT TOP {0} * FROM (SELECT TOP {1} * FROM ({2}) AS t0 ORDER BY {3} {4},{6} {7}, {9} {10}) AS t1 ORDER BY {3} {5}, {6} {8}, {9} {11}) AS t2 ORDER BY {3} {4}, {6} {7}, {9} {10}";
        public static string PreparePageCommand(string selectCommand, Common.Info.PageInfo pageInfo, SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(pageInfo.SortField1))
            {
                DataTable dataTable = ExeTableStrucBySql(selectCommand, parameters);
                DataColumn column;
                if (dataTable.PrimaryKey.Length > 0)
                    column = dataTable.PrimaryKey[0];
                else
                    column = dataTable.Columns[0];
                pageInfo.SortField1 = column.ColumnName;
            }

            pageInfo.CurrentPageIndex = pageInfo.CurrentPageIndex == -1 ? 0 : pageInfo.CurrentPageIndex;

            int itemsPerPage = pageInfo.PageSize;
            if (pageInfo.CurrentPageIndex >= (pageInfo.PageCount - 1))
                itemsPerPage = pageInfo.RecordsInLastPage;

            string cmdText;

            if (!string.IsNullOrWhiteSpace(pageInfo.SortField2))
            {
                if (!string.IsNullOrWhiteSpace(pageInfo.SortField3))
                    cmdText = string.Format(QueryPageCommandText3, new object[] { itemsPerPage, pageInfo.PageSize * (pageInfo.CurrentPageIndex + 1), selectCommand, pageInfo.SortField1, pageInfo.SortType1, GetOtherSide(pageInfo.SortType1), pageInfo.SortField2, pageInfo.SortType2, GetOtherSide(pageInfo.SortType2), pageInfo.SortField3, pageInfo.SortType3, GetOtherSide(pageInfo.SortType3) });
                else
                    cmdText = string.Format(QueryPageCommandText2, new object[] { itemsPerPage, pageInfo.PageSize * (pageInfo.CurrentPageIndex + 1), selectCommand, pageInfo.SortField1, pageInfo.SortType1, GetOtherSide(pageInfo.SortType1), pageInfo.SortField2, pageInfo.SortType2, GetOtherSide(pageInfo.SortType2) });
            }
            else
            {
                cmdText = string.Format(QueryPageCommandText, new object[] { itemsPerPage, pageInfo.PageSize * (pageInfo.CurrentPageIndex + 1), selectCommand, pageInfo.SortField1, pageInfo.SortType1, GetOtherSide(pageInfo.SortType1) });
            }

            return cmdText;
        }
        public static Common.Enums.SortType GetOtherSide(Common.Enums.SortType strOrder)
        {
            if (strOrder == Common.Enums.SortType.ASC)
                return Common.Enums.SortType.DESC;
            else
                return Common.Enums.SortType.ASC;
        }
        public static void getCountInfo(string sqlMain, Common.Info.PageInfo pageInfo, SqlParameter[] cmdParms = null, string sqlPrecondition = null)
        {
            string mySql = sqlPrecondition + String.Format(QueryCountCommandText, sqlMain);

            pageInfo.RecordCount = (int)ExecuteScalar(mySql, cmdParms);
            pageInfo.PageCount = pageInfo.RecordCount / pageInfo.PageSize;
            pageInfo.RecordsInLastPage = pageInfo.RecordCount % pageInfo.PageSize;

            if (pageInfo.RecordsInLastPage == 0)
                pageInfo.RecordsInLastPage = pageInfo.PageSize;
            else
                pageInfo.PageCount++;
        }
        public static string DoPageInfo(StringBuilder strSql, SqlParameter[] parameters, Common.Info.PageInfo pageInfo)
        {
            return DoPageInfo(strSql.ToString(), parameters, pageInfo);
        }
        public static string DoPageInfo(string strSql, SqlParameter[] parameters,  Common.Info.PageInfo pageInfo)
        {
            string pageSql = strSql.ToString();
            if (pageInfo != null)
            {
                if (pageInfo.IsPage)
                {
                    getCountInfo(pageSql, pageInfo, parameters);
                    pageSql = PreparePageCommand(pageSql, pageInfo, parameters);
                }
                else
                {
                    pageSql = AppendSortString(pageSql, pageInfo);
                }
            }
            return pageSql;
        }
    }
}