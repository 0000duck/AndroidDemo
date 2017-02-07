using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetSchool.Controllers
{
    public class NewsController : Controller
    {
        public ActionResult NewsList()
        {
            ViewBag.username = NetSchool.Common.Info.CurUserInfo.Username;
            ViewBag.name = NetSchool.Common.Info.CurUserInfo.RealName;
            return View();
        }

        public ActionResult NewsManage(Guid id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult NewsShow(Guid id)
        {
            NetSchool.Model.News newsInfo = NetSchool.BLL.News.GetModel(NewsID:id);
            newsInfo.ViewNum++;
            NetSchool.BLL.News.Update(newsInfo);

            return View(newsInfo);
        }
        public ActionResult NewsListN()
        {
            return View();
        }

        public ActionResult NewsShowN(Guid id)
        {
            NetSchool.Model.News newsInfo = NetSchool.BLL.News.GetModel(NewsID: id);
            newsInfo.ViewNum++;
            NetSchool.BLL.News.Update(newsInfo);

            return View(newsInfo);
        }
    }
}