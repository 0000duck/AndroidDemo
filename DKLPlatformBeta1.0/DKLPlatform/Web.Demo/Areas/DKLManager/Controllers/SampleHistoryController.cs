using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Areas.DKLManager.Models;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class SampleHistoryController : AdminControllerBase
    {
        //
        // GET: /DKLManager/SampleHistory/
        /// <summary>
        /// 样品历史表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(ProjectInfoRequest request)
        {         
            var result = this.IDKLManagerService.GetSampleRegisterTableList(request);
            request.SampleState = (int)EnumSampleStates.DoneSample;
            return View(result);
        }
	}
}