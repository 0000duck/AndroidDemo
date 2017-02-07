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
    public class SelectLookController : SampleBaseController
    {
        //
        // GET: /DKLManager/SelectLook/
        public ActionResult Index(ProjectInfoRequest request, FormCollection collection)
        {
            request.SampleState = (int)EnumSampleStates.Selec;
            ViewData.Add("AnalyzePeople", new SelectList(EnumHelper.GetItemValueList<EnumAnalyzePeople>(), "Key", "Value"));
            var result = this.IDKLManagerService.GetSampleRegisterTableList(request);
            return View(result);
        }
        public ActionResult Check(int id)
        {
            var model = this.IDKLManagerService.GetSampleRegisterTable(id);
            if (model.AnalyzePeople == "fxra" || model.AnalyzePeople == "fxrb" || model.AnalyzePeople=="fxrc")
            {
                return View(model);
            }
            //ViewData.Add("AnalyzePeople", new SelectList(EnumHelper.GetItemValueList<EnumAnalyzePeople>(), "Key", "Value"));
            int Analy = int.Parse(model.AnalyzePeople);

            ViewData.Add("AnalyzePeople", EnumHelper.GetEnumTitle((EnumAnalyzePeople)Analy));
            model.AnalyzePeople = EnumHelper.GetEnumTitle((EnumAnalyzePeople)Analy);
             this.TryUpdateModel(model);
            this.IDKLManagerService.UpDateSampleRegister(model);
            return View(model);

        }

	}
}