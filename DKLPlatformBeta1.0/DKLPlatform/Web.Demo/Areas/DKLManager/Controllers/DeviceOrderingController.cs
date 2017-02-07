using DKLManager.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Areas.DKLManager.Models;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class DeviceOrderingController : DeviceOrderBaseController
    {
        //
        // GET: /DKLManager/DeviceOrdering/
        public ActionResult Index(DeviceRequest request)
        {
            request.OrderState = (int)EnumOrderStateInfo.AddState;
            var result = this.IDKLManagerService.GetDeviceOrderInfoList(request);
            return View(result);
        }

        #region 预约申请-增加
        public ActionResult Create(string projectNumber = null)
        {
            deviceOrderData = new DeviceOrderingViewModel();
            if (!string.IsNullOrEmpty(projectNumber))
                deviceOrderData.orderInfo.ProjectNumber = projectNumber;

            AddDataToView();
            return View(deviceOrderData);
        }
           
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                if (deviceOrderData.orderDetailList.Count > 0)
                {
                    if (IsAllDeviceNumEnough())
                    {
                        SaveOrderDetails();
                        SaveOrderInfo();
                        var result = this.IDKLManagerService.GetDeviceOrderInfoList();
                        return View("Index", result);
                    }
                    else
                    {
                        return Back(GlobalData.warningInfo3);
                    }
                }
                else
                {
                    return Back(GlobalData.warningInfo6);
                }
            }
            catch (HYZK.FrameWork.Common.BusinessException e)
            {
                return Back(e.Message);
            }
            catch(Exception e)
            {
                return Back(e.Message);
            }
        }

        [HttpPost]
        public ActionResult Delete(List<string> DeviceNames)
        {
            if (DeviceNames != null)
            {
                foreach (var num in DeviceNames)
                {
                    for (int i = 0; i < deviceOrderData.orderDetailList.Count;++i )
                    {
                        if (deviceOrderData.orderDetailList[i].DeviceName == num)
                        {
                            deviceOrderData.orderDetailList.Remove(deviceOrderData.orderDetailList[i]);
                        }
                    }
                }
            }
            return RedirectToAction("Create");
        }

        #endregion

        public ActionResult Submit(int id)
        {
            var orderData = GetDeviceOrderingInfoById(id);
            return View(orderData);
        }

        [HttpPost]
        public ActionResult Submit(int id, FormCollection collection)
        {
            var orderInfo = this.IDKLManagerService.GetDeviceOrderInfo(id);
            orderInfo.OrderState = (int)EnumOrderStateInfo.OrderSucceed;
            this.IDKLManagerService.UpdateDeviceOrderInfo(orderInfo);
            return this.RefreshParent(); 
        }

        [HttpPost]
        public ActionResult GoBack(int id, FormCollection collection)
        {
            var orderInfo = this.IDKLManagerService.GetDeviceOrderInfo(id);
            orderInfo.OrderState = (int)EnumOrderStateInfo.OrderFailed;
            this.IDKLManagerService.UpdateDeviceOrderInfo(orderInfo);
            return RedirectToAction("Index");
        }

    }
}