using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DKLManager.Contract.Model;
using Web.Demo.Common;
using HYZK.FrameWork.Utility;
using System;

namespace Web.Demo.Areas.DKLManager.Controllers
{
        public class CustomerController : AdminControllerBase
        {
            public ActionResult Index(ProjectInfoRequest request)
            {
                var result = this.IDKLManagerService.GetCustomerList(request);
                return View(result);
            }
            public ActionResult Create()
            {
                var model = new CustomerModel();
                this.RenderMyViewData(model);
                var users = this.AccountService.GetUserList().Select(c => new { Id = c.ID, Name = c.Name });
                if (users != null)
                {
                    ViewData.Add("Name", new SelectList(users, "Name", "Name"));
                }

                return View("Edit", model);
            }

            [HttpPost]
            public ActionResult Create(FormCollection collection)
            {
                var model = new CustomerModel();
                try
                {
                    this.TryUpdateModel<CustomerModel>(model);
                }
                catch (Exception ex)
                {
                    return Back(ex.Message);
                }

                try
                {
                    this.IDKLManagerService.AddCustomer(model);
                }
                catch (HYZK.FrameWork.Common.BusinessException e)
                {
                    this.ModelState.AddModelError(e.Name, e.Message);
                    return View("Edit", model);
                }

                return this.RefreshParent();
            }

            public ActionResult Edit(int id)
            {
                var model = this.IDKLManagerService.GetCustomer(id);
                this.RenderMyViewData(model);
                var users = this.AccountService.GetUserList().Select(c => new { Id = c.ID, Name = c.Name });
                if (users != null)
                {
                    ViewData.Add("Name", new SelectList(users, "Name", "Name"));
                }

                return View(model);
            }

            [HttpPost]
            public ActionResult Edit(int id, FormCollection collection)
            {
                var model = this.IDKLManagerService.GetCustomer(id);
                if (model != null)
                {
                    try
                    {
                        this.TryUpdateModel<CustomerModel>(model);
                        this.IDKLManagerService.UpdateCustomer(model);
                    }
                    catch (Exception ex)
                    {
                        return Back(ex.Message);
                    }
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
                        this.IDKLManagerService.DeleteCustomer(ids);
                    }
                    catch (Exception ex)
                    {
                        return Back(ex.Message);
                    }
                }
                return RedirectToAction("Index");
            }

            private void RenderMyViewData(CustomerModel model)
            {
                ViewData.Add("Gender", new SelectList(EnumHelper.GetItemValueList<EnumGender>(), "Key", "Value", model.Gender));
                ViewData.Add("Profession", new SelectList(EnumHelper.GetItemValueList<EnumProfession>(), "Key", "Value", model.Profession));
            }
        }
    }
	
