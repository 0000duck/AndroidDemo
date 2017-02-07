using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetSchool.Controllers
{
    public class NoticeController : Controller
    {
        // GET: Notice
        public ActionResult NoticeList()
        {
            return View();
        }
        public ActionResult NoticeManage(Guid id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult NoticeShow(Guid id)
        {
            NetSchool.Model.Notice NoticeInfo = NetSchool.BLL.Notice.GetModel(NoticeID: id);
            NoticeInfo.ViewNum++;
            NetSchool.BLL.Notice.Update(NoticeInfo);

            return View(NoticeInfo);
        }

        public ActionResult NoticeListN()
        {
            return View();
        }

        public ActionResult NoticeShowN(Guid id)
        {
            NetSchool.Model.Notice NoticeInfo = NetSchool.BLL.Notice.GetModel(NoticeID: id);
            NoticeInfo.ViewNum++;
            NetSchool.BLL.Notice.Update(NoticeInfo);

            return View(NoticeInfo);
        }
    }
}