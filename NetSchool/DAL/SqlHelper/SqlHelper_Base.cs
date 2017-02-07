using System;
using System.Data;
using System.Data.SqlClient;

namespace NetSchool.DAL.SqlHelper
{
    
    public static partial class SqlHelper
    {
        
        public static int ExecuteNonQuery(string cmdText, SqlParameter[] cmdParms = null, CommandType cmdType = CommandType.Text, string connectstr = null)
        {
            if (string.IsNullOrWhiteSpace(connectstr))
                connectstr = CONN_STRING;

            using (SqlCommand cmd = new SqlCommand())
            {
                using (SqlConnection conn = new SqlConnection(connectstr))
                {
                    try
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandText = cmdText;
                        cmd.CommandType = cmdType;

                        if (cmdParms != null && cmdParms.Length > 0)
                            cmd.Parameters.AddRange(cmdParms);
                        return cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        cmd.Parameters.Clear();
                        conn.Close();
                    }
                }
            }
        }
        public static object ExecuteScalar(string cmdText, SqlParameter[] cmdParms = null, CommandType cmdType = CommandType.Text, string connectstr = null)
        {
            if (string.IsNullOrWhiteSpace(connectstr))
                connectstr = CONN_STRING;
            using (SqlCommand cmd = new SqlCommand())
            {
                using (SqlConnection conn = new SqlConnection(connectstr))
                {
                    try
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandText = cmdText;
                        cmd.CommandType = cmdType;

                        if (cmdParms != null && cmdParms.Length > 0)
                            cmd.Parameters.AddRange(cmdParms);

                        return cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        cmd.Parameters.Clear();
                        conn.Close();
                    }
                }
            }
        }
        public static DataTable ExeTableStrucBySql(string cmdText, SqlParameter[] cmdParms = null, CommandType cmdType = CommandType.Text, string connectstr = null)
        {
            cmdText = "SET FMTONLY ON;" + cmdText + ";SET FMTONLY OFF;";
            return ExecuteDataTable(cmdText, cmdParms, cmdType, connectstr);
        }
        public static DataTable ExecuteDataTable(string cmdText, SqlParameter[] cmdParms = null, CommandType cmdType = CommandType.Text, string connectstr = null)
        {
            return ExecuteDataSet(cmdText, cmdParms, cmdType, connectstr).Tables[0];
        }
        public static DataSet ExecuteDataSet(string cmdText, SqlParameter[] cmdParms = null, CommandType cmdType = CommandType.Text, string connectstr = null)
        {
            if (string.IsNullOrWhiteSpace(connectstr))
                connectstr = CONN_STRING;// "servere=123.57.152.229;UID=sa;PWD=dkl2015!;DATABASE=EDUDKL;pooling=true;Max Pool Size=512;Trusted_Connection=False;Connection Lifetime=60";//CONN_STRING;
            using (SqlCommand cmd = new SqlCommand())
            {
                using (SqlConnection conn = new SqlConnection(connectstr))
                {
                    try
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandText = cmdText;
                        cmd.CommandType = cmdType;

                        if (cmdParms != null && cmdParms.Length > 0)
                            cmd.Parameters.AddRange(cmdParms);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            return ds;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        cmd.Parameters.Clear();
                        conn.Close();
                    }
                }
            }
        }
        public static bool Exists(string cmdText, SqlParameter[] cmdParms = null, string connectstr = null)
        {
            object obj = SqlHelper.ExecuteScalar(cmdText, cmdParms, connectstr: connectstr);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                cmdresult = 0;
            else
                cmdresult = (int)obj;
            return cmdresult != 0;
        }
    }
}