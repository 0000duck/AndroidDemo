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
    public class DeviceOrderBaseController : AdminControllerBase
    {
        protected static DeviceOrderingViewModel deviceOrderData;
        protected static DateTime orderDate = new DateTime();

        [HttpPost]
        public ActionResult AddOrder(FormCollection collection)
        {
            try
            {
                var model = new DeviceOrderDetail();
                model.ProjectNumber = collection["orderInfo.ProjectNumber"];
                model.DeviceName = collection["DeviceName"];

                if ((DateTime.TryParse(collection["OrderDate"], out orderDate)) && (orderDate.CompareTo( DateTime.Today) > 0))
                {
                    model.OrderDate = orderDate;
                    int number;
                    if ((int.TryParse(collection["orderDetail.OrderNumber"], out number)) && (number > 0))
                    {
                        model.OrderNumber = number;
                    }
                    if (!IsExitOrderDevice(model.DeviceName))
                    {
                        if (IsDeviceNumEnough(model.DeviceName, orderDate, model.OrderNumber))
                        {
                            deviceOrderData.orderInfo.ProjectNumber = model.ProjectNumber;
                            deviceOrderData.orderInfo.OrderDate = orderDate;
                            deviceOrderData.orderDetailList.Add(model);
                            var devices = this.IDKLManagerService.GetDeviceList().Select(c => new { Name = c.DeivceName }).Distinct();
                            ViewData.Add("DeviceName", new SelectList(devices, "Name", "Name"));
                            return View("Create", deviceOrderData);
                        }
                        else
                        {
                            return Back(GlobalData.warningInfo3);
                        }
                    }
                    else
                    {
                        return Back(GlobalData.warningInfo2);
                    }
                }
                else
                {
                    return Back(GlobalData.warningInfo5);
                }
            }
            catch (Exception e)
            {
                return Back(GlobalData.warningInfo4 + e.Message);
            }
        }

        protected bool IsDeviceNumEnough(string deviceName, DateTime orderDate, int orderNum)
        {
            bool bRet = false;
            int canOrderNumber = this.IDKLManagerService.GetDeviceCanOrderNumber(deviceName, orderDate);
            if (orderNum <= canOrderNumber)
            {
                bRet = true;
            }
            return bRet;
        }

        protected bool IsExitOrderDevice(string deviceName)
        {
            bool bRet = false;
            foreach (var data in deviceOrderData.orderDetailList)
            {
                if (data.DeviceName == deviceName)
                {
                    bRet = true;
                    break;
                }
            }
            return bRet;
        }

        protected bool IsAllDeviceNumEnough()
        {
            bool bRet = true;
            foreach (var data in deviceOrderData.orderDetailList)
            {
                data.OrderDate = orderDate;//统一列表中时间，防止时间不一致
                if (!IsDeviceNumEnough(data.DeviceName, orderDate, data.OrderNumber))
                {
                    bRet = false;
                    break;
                }
            }
            return bRet;
        }
        protected void SaveOrderDetails(bool update = false)
        {
            if(update)
            {
                this.IDKLManagerService.DeleteDeviceOrderDetail(deviceOrderData.orderInfo.ProjectNumber);
            }
            foreach (var data in deviceOrderData.orderDetailList)
            {
                this.IDKLManagerService.AddDeviceOrderDetail(data);
            }
        }

        protected void SaveOrderInfo(bool update = false)
        {
            DeviceOrderInfo info = new DeviceOrderInfo();
            info.OrderDate = orderDate;
            if (deviceOrderData.orderDetailList.Count > 0)
            {
                info.ProjectNumber = deviceOrderData.orderDetailList.First().ProjectNumber;
            }
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            if (user != null)
            {
                info.OrderPerson = user.Name;
            }
            info.OrderState = (int)EnumOrderStateInfo.AddState;
            if (update)
            {
                DeviceRequest request = new DeviceRequest();
                request.ProjectNumber = info.ProjectNumber;
                if (this.IDKLManagerService.GetDeviceOrderInfoList(request).Count() > 0)
                {
                    this.IDKLManagerService.DeleteDeviceOrderInfo(info.ProjectNumber);
                }
            }
            this.IDKLManagerService.AddDeviceOrderInfo(info);
        }


        protected DeviceOrderingViewModel GetDeviceOrderingInfoById(int id)
        {
            var orderData = new DeviceOrderingViewModel();
            orderData.orderInfo = this.IDKLManagerService.GetDeviceOrderInfo(id);
            if (orderData.orderInfo != null)
            {
                DeviceRequest request = new DeviceRequest();
                request.ProjectNumber = orderData.orderInfo.ProjectNumber;
                foreach (var detail in this.IDKLManagerService.GetDeviceOrderDetailList(request))
                {
                    orderData.orderDetailList.Add(detail);
                }
            }
            return orderData;
        }

        protected void AddDataToView()
        {
            var devices = this.IDKLManagerService.GetDeviceList().Select(c => new {Name = c.DeivceName }).Distinct();
            ViewData.Add("DeviceName", new SelectList(devices, "Name", "Name"));
        }
    }
}