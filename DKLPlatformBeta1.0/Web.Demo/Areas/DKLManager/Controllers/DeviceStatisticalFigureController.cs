using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;
using HYZK.FrameWork.Common;
using Web.Demo.Areas.DKLManager.Models;
using System.Drawing.Imaging;
using System.IO;
using HYZK.Core.Upload;
using OfficeDocGenerate;
using System.Web.Script.Serialization;
using System.Data;
using System.Text;


namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class DeviceStatisticalFigureController : AdminControllerBase
    {
        public static List< DeviceOrderDetail> Info;
        //
        // GET: /DKLManager/DeviceStatisticalFigure/
        public ActionResult Index(DeviceRequest request)
        {
            var devices = this.IDKLManagerService.GetDeviceOrderDetailList().Select(c => new { Name = c.DeviceName }).Distinct();
            ViewData.Add("DeviceName", new SelectList(devices, "Name", "Name"));
            List<string> list3 = new List<string>();
            int year = DateTime.Now.Year - 10;
            for (int i = 10; i > 0; i--)
                list3.Add((year + i).ToString());
            ViewData.Add("Year", new SelectList(list3));
            List<string> Month = new List<string>();
            for (int i = 1; i <= 12; i++)
                Month.Add(i.ToString());
            ViewData.Add("BeginMonth", new SelectList(Month));
            ViewData.Add("EndMonth", new SelectList(Month));
            return View();
        }
        [HttpPost]
        public ActionResult SelectDevice(FormCollection collection)
        {
            var devices = this.IDKLManagerService.GetDeviceOrderDetailList().Select(c => new { Name = c.DeviceName }).Distinct();
            ViewData.Add("DeviceName", new SelectList(devices, "Name", "Name"));

            List<string> list3 = new List<string>();
            int year = DateTime.Now.Year - 10;
            for (int i = 10; i > 0; i--)
                list3.Add((year + i).ToString());
            ViewData.Add("Year", new SelectList(list3));

            List<string> Month = new List<string>();
            for (int i = 1; i <= 12; i++)
                Month.Add(i.ToString());
            ViewData.Add("BeginMonth", new SelectList(Month));
            ViewData.Add("EndMonth", new SelectList(Month));
            string DeviceName = collection["DeviceName"];        
            string BeginMonth = collection["BeginMonth"];
            string EndMonth = collection["EndMonth"];
            string Year = collection["Year"];
            var list = this.IDKLManagerService.GeDeviceOrderDetailq(DeviceName, Year, BeginMonth, EndMonth);
            Info = list;
            return View(list);

        }
        [HttpPost]
        public ActionResult ExportExcelFile(FormCollection collection)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", Type.GetType("System.Int32"));
            dt.Columns[0].AutoIncrement = true;
            dt.Columns[0].AutoIncrementSeed = 1;
            dt.Columns[0].AutoIncrementStep = 1;
            dt.Columns.Add("项目编号", Type.GetType("System.String"));
            dt.Columns.Add("预约个数", Type.GetType("System.String"));
            dt.Columns.Add("预约人", Type.GetType("System.String"));
            dt.Columns.Add("预约时间", Type.GetType("System.String"));
            int i = 0;
            foreach (var m in Info)
            {
                ++i;
                DataRow dtRow = dt.NewRow();
                dtRow["项目编号"] = "" + m.ProjectNumber;
                dtRow["预约个数"] = "" + m.RealityOrderNumber;
                dtRow["预约人"] = "" + m.OrderPerson;
                dtRow["预约时间"] = "" + m.OrderDate;
                dt.Rows.Add(dtRow);
            }
            string fileName = "d://DKLdownload" + Web.Demo.Common.AdminUserContext.Current.LoginInfo.LoginName + "(" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + ").xls";
            string tabName = "市场统计信息";
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