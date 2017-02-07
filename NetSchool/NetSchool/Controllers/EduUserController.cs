using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetSchool.Controllers
{
    [UserAttribute]
    public class EduUserController : Controller
    {
        // GET: User
        public ActionResult UserList()
        {
            return View();
        }
        public ActionResult UserManage(Guid id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult UserShow(Guid id)
        {
            NetSchool.Model.EduUser EduUserInfo = NetSchool.BLL.EduUser.GetModel(ID: id);
            NetSchool.BLL.EduUser.Update(EduUserInfo);

            return View(EduUserInfo);
        }
    }
}