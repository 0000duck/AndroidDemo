using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Configuration;
using System.Xml.Linq;
using System.Data.OleDb;
using System.Data.SqlClient;
namespace NetSchool.Common.Library
{
    public class ExcelOP
    {
        public static bool didExport(DataTable dt, string strSheetName, string filename,string Address)
        {
            try
            {
                Excel.Application excel = new Excel.Application();  
                Excel.Workbook bookDest = (Excel.Workbook)excel.Workbooks.Add(Missing.Value);
                Excel.Worksheet sheetDest = (Excel.Worksheet)bookDest.Worksheets[1];
                int rowIndex = 1;
                int colIndex = 0;
                foreach (DataColumn col in dt.Columns)
                {
                    colIndex++;
                    sheetDest.Cells[1, colIndex] = col.ColumnName;
                }
                foreach (DataRow row in dt.Rows)
                {
                    rowIndex++;
                    colIndex = 0;
                    foreach (DataColumn col in dt.Columns)
                    {
                        colIndex++;
                        sheetDest.Cells[rowIndex, colIndex] = row[col.ColumnName].ToString();
                    }
                }
                bookDest.Saved = true;
                bookDest.SaveAs(Address + filename + ".xls", Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);//方式一
                excel.Quit();
                excel = null;
                GC.Collect();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public static bool DataTableExportToExcel(DataTable dt, string filename, String tabName, ref String reMsg)
        {
            if (dt.Rows.Count <= 0)
            {
                reMsg = "目前无数据不需要导出";
                return false;
            }
            int rows = dt.Rows.Count;
            int cols = dt.Columns.Count;
            StringBuilder sb = new StringBuilder();
            string connString = String.Empty;
            connString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;", filename);
            sb.Append("CREATE TABLE " + tabName + " (");
            String colName = String.Empty;
            String colNames = String.Empty;
            String colNamePramas = String.Empty;
            String colType = String.Empty;
            for (int i = 0; i < cols; i++)
            {
                colName = dt.Columns[i].ColumnName.ToString();
                colType = dt.Columns[i].DataType.ToString();
                colType = NetDataTypeToDataBaseType(colType);
                if (i == 0)
                {
                    sb.Append(colName + "  " + colType);
                    colNames += colName;
                    colNamePramas += "@" + colName;
                }
                else
                {
                    sb.Append(", " + colName + "  " + colType);
                    colNames += "," + colName;
                    colNamePramas += ",@" + colName;
                }
            }
            sb.Append(" )");
            if (colNames == String.Empty)
            {
                reMsg = "数据集的列数必须大于0";
                return false;
            }
            using (OleDbConnection objConn = new OleDbConnection(connString))
            {
                OleDbCommand objCmd = new OleDbCommand();
                objCmd.Connection = objConn;
                objCmd.CommandText = sb.ToString();
                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    reMsg = "在Excel中创建表失败，错误信息：" + e.Message;
                    return false;
                }
                sb.Remove(0, sb.Length);
                sb.Append(" insert into " + tabName + " (" + colNames + ") values(" + colNamePramas + " )");
                objCmd.CommandText = sb.ToString();
                OleDbParameterCollection param = objCmd.Parameters;
                for (int i = 0; i < cols; i++)
                {
                    colType = dt.Columns[i].DataType.ToString();
                    colName = dt.Columns[i].ColumnName.ToString();
                    if (colType == "System.String")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.VarChar));
                    }
                    else if (colType == "System.DateTime")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Date));

                    }
                    else if (colType == "System.Boolean")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Boolean));

                    }
                    else if (colType == "System.Decimal")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Decimal));

                    }
                    else if (colType == "System.Double")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Double));

                    }
                    else if (colType == "System.Single")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Single));

                    }
                    else if (colType == "System.Single")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Single));
                    }
                    else
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Integer));
                    }

                }
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < param.Count; i++)
                    {
                        param[i].Value = row[i];
                    }

                    objCmd.ExecuteNonQuery();
                }
            }
            reMsg = "数据成功导出";
            return true;
        }
        private static String NetDataTypeToDataBaseType(String DataType)
        {
            String reType = String.Empty;
            if (DataType.ToString() == "System.String")
            {
                reType = "varchar";
            }
            else if (DataType.ToString() == "System.Decimal")
            {
                reType = "number";
            }
            else if (DataType.ToString() == "System.DateTime")
            {
                reType = "datetime";

            }
            else
            {
                reType = "int";

            }
            return reType;

        }
    }
}
