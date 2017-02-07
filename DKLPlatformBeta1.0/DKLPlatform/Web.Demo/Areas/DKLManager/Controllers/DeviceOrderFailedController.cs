using DKLManager.Contract.Model;
using HYZK.Account.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Areas.DKLManager.Models;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class DeviceOrderFailedController : DeviceOrderBaseController
    {
        Dictionary<string, int> deviceOrderNumberMap = new Dictionary<string,int>();
        //
        // GET: /DKLManager/DeviceOrderFailed/
        public ActionResult Index(DeviceRequest request)
        {
            return View(GetDeviceOrderRealTimeList(request));
        }

        private IEnumerable<DeviceOrderInfo> GetDeviceOrderRealTimeList(DeviceRequest request = null)
        {
            request = (request == null) ? new DeviceRequest() : request;
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            if (user.AccountType != (int)EnumAccountType.EquipmentManager)
            {
                request.OrderPerson = user.Name;
            }

            request.OrderState = (int)EnumOrderStateInfo.OrderFailed;
            return this.IDKLManagerService.GetDeviceOrderInfoList(request);
        }
        public ActionResult Create(int id)
        {
            deviceOrderData = GetDeviceOrderingInfoById(id);
            AddDataToView();
            //back up device order number history
            foreach(var item in deviceOrderData.orderDetailList)
            {
                deviceOrderNumberMap.Add(item.DeviceName, item.OrderNumber);
            }
            return View(deviceOrderData);
        }

        protected new bool IsAllDeviceNumEnough()
        {
            bool bRet = true;
            foreach (var data in deviceOrderData.orderDetailList)
            {
                int newOrderNumber = data.OrderNumber;
                if(deviceOrderNumberMap.ContainsKey(data.DeviceName))
                {
                    newOrderNumber = newOrderNumber - deviceOrderNumberMap[data.DeviceName];
                }
                data.OrderDate = orderDate;//统一列表中时间，防止时间不一致
                if (!IsDeviceNumEnough(data.DeviceName, orderDate, newOrderNumber))
                {
                    bRet = false;
                    break;
                }
            }
            return bRet;
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
                        //update
                        SaveOrderDetails(true);
                        SaveOrderInfo(true);
                        return View("Index", GetDeviceOrderRealTimeList());
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
            catch (Exception e)
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
                    for (int i = 0; i < deviceOrderData.orderDetailList.Count; ++i)
                    {
                        if (deviceOrderData.orderDetailList[i].DeviceName == num)
                        {
                            deviceOrderData.orderDetailList.Remove(deviceOrderData.orderDetailList[i]);
                        }
                    }
                }
            }
            AddDataToView(); 
            return View("Create", deviceOrderData);
        }


    }
}