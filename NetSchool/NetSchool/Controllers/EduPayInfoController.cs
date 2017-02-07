using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetSchool.Controllers
{
    public class EduPayInfoController : Controller
    {
        [UserAttribute]
        // GET: EduPayInfo
        public ActionResult PayInfoList()
        {
            return View();
        }
    }
}