using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetSchool.Controllers
{
    public class LawController : Controller
    {
        // GET: Law
        public ActionResult LawList()
        {
            return View();
        }
        public ActionResult LawManage(Guid id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult LawShow(Guid id)
        {
            NetSchool.Model.Law LawInfo = NetSchool.BLL.Law.GetModel(LawID: id);
            LawInfo.ViewNum++;
            NetSchool.BLL.Law.Update(LawInfo);

            return View(LawInfo);
        }

        public ActionResult LawListN()
        {
            return View();
        }

        public ActionResult LawShowN(Guid id)
        {
            NetSchool.Model.Law LawInfo = NetSchool.BLL.Law.GetModel(LawID: id);
            LawInfo.ViewNum++;
            NetSchool.BLL.Law.Update(LawInfo);

            return View(LawInfo);
        }
    }
}