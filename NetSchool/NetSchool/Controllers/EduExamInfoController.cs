using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetSchool.Controllers
{
    [UserAttribute]
    public class EduExamInfoController : Controller
    {
        // GET: ExamInfo
        public ActionResult ExamInfoList()
        {
            return View();
        }
    }
}