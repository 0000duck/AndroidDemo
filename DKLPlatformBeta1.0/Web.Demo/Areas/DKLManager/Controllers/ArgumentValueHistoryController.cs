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
                request.ParameterState = (int)EnumParameterState.NewParameter;
                var ArgumentList = this.IDKLManagerService.GetArgumentValueList(request);
                return View(ArgumentList);
        }
        public ActionResult Edit(int id)
        {
            var model = this.IDKLManagerService.GetArgumentValue(id);
            return View(model);
        }

        //
        // POST: /Account/User/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.GetArgumentValue(id);
            if (model != null)
            {
                try
                {
                    this.TryUpdateModel<ArgumentValue>(model);
                }
                catch (Exception ex)
                {
                    return Back(ex.Message);
                }
            }
            model.ParameterState = (int)EnumParameterState.OldParameter;
            try
            {
                this.IDKLManagerService.UpDateArgumentValue(model);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
            return this.RefreshParent();
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            if (ids != null)
            {
                try
                {
                    this.IDKLManagerService.DeleteArgumentValue(ids);
                }
                catch (Exception ex)
                {
                    return Back(ex.Message);
                }
            }
            return RedirectToAction("Index");

        }
	}
}