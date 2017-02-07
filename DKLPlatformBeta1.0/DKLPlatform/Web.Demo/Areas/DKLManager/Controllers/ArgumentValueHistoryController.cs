using DKLManager.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;
using Web.Demo.Areas.DKLManager.Models;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ArgumentValueHistoryController : AdminControllerBase
    {
        //
        // GET: /DKLManager/ArgumentValueHistory/
        /// <summary>
        /// 历史参数表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {
            var ArgumentList = this.IDKLManagerService.GetArgumentValueList(request);
            return View(ArgumentList);
        }
        [HttpPost]
        public ActionResult Delete(List<int> id)
        {
            if (id != null)
            {
                this.IDKLManagerService.DeleteArgumentValueHistory(id);
            }
            return RedirectToAction("Index");

        }
	}
}