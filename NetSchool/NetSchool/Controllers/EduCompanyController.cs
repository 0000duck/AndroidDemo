using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text;

namespace NetSchool.Controllers
{
    [UserAttribute]
    public class EduCompanyController : Controller
    {
        public ActionResult CompanyList()
        {
            return View();
        }
        public ActionResult CompanyManage(Guid id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult CompanyShow(Guid id)
        {
            NetSchool.Model.EduCompany EduCompanyInfo = NetSchool.BLL.EduCompany.GetModel(ID: id);
            NetSchool.BLL.EduCompany.Update(EduCompanyInfo);

            return View(EduCompanyInfo);
        }
        public struct territory 
        {
            public string name;
            public int count;
        }
        private string searchTerritory()
        {
            string strJson = string.Empty;
            DataTable eduCompany = BLL.EduCompany.SearchTerritory();
            strJson = JsonConvert.SerializeObject(new { state = "success", msg = "OK",  list = eduCompany }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strJson;
        }
        public territory[] getTerritoryCount(DataTable table)
        {
            territory[] TerritoryList = new territory[table.Rows.Count];
            for (int i = 0; i < TerritoryList.Length; i++)
            {
                TerritoryList[i].name = (string)table.Rows[i]["Territory"];
                TerritoryList[i].count = (int)table.Rows[i]["Count"];
            }
            return TerritoryList;
        }
        private string[] getTerritory(DataTable table)
        {
            string[] TerritoryList = new string[table.Rows.Count];
            for (int i = 0; i < TerritoryList.Length; i++)
            {
                TerritoryList[i] = (string)table.Rows[i]["territory"];
            }
            return TerritoryList;
        }
    }
}