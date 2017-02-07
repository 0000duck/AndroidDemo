using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Areas.DKLManager.Models;
using Web.Demo.Common;
using OfficeDocGenerate;
using System.Data;
using System.Text;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ProjectResultCheckController : SampleBaseController
    {
        public static IEnumerable<SampleRegisterTable> Sample;
        public ActionResult Index(ProjectInfoRequest request)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            request.SampleStates = (int)EnumSampleStates.Sumbit;
            var parameNew = this.IDKLManagerService.GetSampleRegisterTableList(user.Name, request);
            Sample = parameNew;
            return View(parameNew);
        }
        public ActionResult ExportExcelFile(FormCollection collection)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", Type.GetType("System.Int32"));
            dt.Columns[0].AutoIncrement = true;
            dt.Columns[0].AutoIncrementSeed = 1;
            dt.Columns[0].AutoIncrementStep = 1;
            dt.Columns.Add("样品登记号", Type.GetType("System.String"));
            dt.Columns.Add("采样日期", Type.GetType("System.DateTime"));
            dt.Columns.Add("样品状态", Type.GetType("System.String"));
            dt.Columns.Add("保存条件", Type.GetType("System.String"));
            dt.Columns.Add("备注", Type.GetType("System.String"));
            dt.Columns.Add("状态", Type.GetType("System.String"));
            dt.Columns.Add("审核人", Type.GetType("System.String"));
            dt.Columns.Add("样品结果", Type.GetType("System.String"));




            foreach (var m in Sample)
            {
                DataRow dtRow = dt.NewRow();

                dtRow["样品登记号"] = "" + m.SampleRegisterNumber;
                dtRow["采样日期"] = "" + m.SamplingDay;
                dtRow["样品状态"] = "" + m.SampleState;
                dtRow["保存条件"] = "" + m.SaveCondition;
                dtRow["备注"] = "" + m.Remark;
                dtRow["状态"] = "" + HYZK.FrameWork.Utility.EnumHelper.GetEnumTitle((EnumSampleStates)@m.SampleStates);
                dtRow["审核人"] = "" + m.AnalyzePeople;
                dtRow["样品结果"] = "" + m.ArgumentPrice;
                dt.Rows.Add(dtRow);
            }


            string fileName = "d://DKLdownload" + Web.Demo.Common.AdminUserContext.Current.LoginInfo.LoginName + "(" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + ").xls";
            string tabName = "采样结果";
            string reMsg = string.Empty;
            bool result = ExcelOP.DataTableExportToExcel(dt, fileName, tabName, ref reMsg);



            string strFileName = fileName;
            if (result && !string.IsNullOrEmpty(strFileName))
            {

                string fileNewName = strFileName.Substring(strFileName.LastIndexOf("\\") + 1);
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = Encoding.UTF8;
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileNewName, Encoding.UTF8));
                Response.WriteFile(strFileName);
                Response.End();
                return Back("成功");
            }
            else
            {
                return Back("导出失败");
            }
        }
    }
}
