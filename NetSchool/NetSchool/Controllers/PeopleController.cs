using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetSchool.Controllers
{
    [UserAttribute]
    public class PeopleController : Controller
    {
        // GET: People
        public ActionResult PeopleList()
        {
            return View();
        }

        public ActionResult PeopleManage(string id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult PasswordChange()
        {
            ViewBag.username = NetSchool.Common.Info.CurUserInfo.Username;
            return View();
        }
    }
}