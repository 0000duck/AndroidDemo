using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetSchool.Controllers
{
    [UserAttribute]
    public class EduLearningInfoController : Controller
    {
        // GET: EduLearningInfo
        public ActionResult LearningInfoList()
        {
            return View();
        }
    }
}