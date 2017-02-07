using DKLManager.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web.Demo.Areas.DKLManager.Models;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class DeviceOrderByTestPersonController : DeviceOrderBaseController
    {
        public ActionResult Create(string projectNumber)
        {
            deviceOrderData = new DeviceOrderingViewModel();
            AddDataToView();
            deviceOrderData.orderInfo.OrderDate = Convert.ToDateTime(DateTime.Now.ToLongDateString());
            deviceOrderData.orderInfo.ProjectNumber = projectNumber;           
            return View(deviceOrderData);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {// && deviceOrderData.orderDetailList.Count <= 1
            try
            {
                if (deviceOrderData.orderDetailList.Count > 0)
                {
                    if (IsAllDeviceNumEnough())
                    {
                        DateTimeNow = DateTime.Now;
                        SaveOrderDetails();
                        SaveOrderInfo();
                        var projectNumber=collection["Number"];
                        deviceOrderData = new DeviceOrderingViewModel();
                        AddDataToView();
                        deviceOrderData.orderInfo.ProjectNumber = projectNumber;
                        deviceOrderData.orderInfo.OrderDate = Convert.ToDateTime(DateTime.Now.ToLongDateString());
                        return View("Create", deviceOrderData);
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
        public ActionResult Edit(string DeviceName)
        {
            var model = new DeviceOrderDetail();
            if (DeviceName != null)
            {               
                    for (int i = 0; i < deviceOrderData.orderDetailList.Count; ++i)
                    {
                        if (deviceOrderData.orderDetailList[i].DeviceName == DeviceName)
                        {
                            model=deviceOrderData.orderDetailList[i]; 
                        }
                    }
                
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(List<string> DeviceNames, FormCollection collection)
        {
            var projectNumber = collection["Number"];
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
            return RedirectToAction("Create",projectNumber);
        }

	}
}