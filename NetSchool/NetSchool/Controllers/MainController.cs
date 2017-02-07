using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;

namespace NetSchool.Controllers
{
    public class MainController : Controller
    {
        private Dictionary<string, DataTable> dic = null;
        public ActionResult Index()
        {
            Dictionary<string, DataTable> dic = NetSchool.BLL.Main.GetAllInfo();
            return View(dic);
        }
        [UserAttribute]
        public ActionResult Main()
        {
            return View(NetSchool.BLL.Main.GetAllInfo());
        }
	}
}