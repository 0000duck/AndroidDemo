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
        public ActionResult Index(DeviceRequest request)                 ///查询所有DEviceOrderInfo
        {
            request.OrderState = (int)EnumOrderStateInfo.AddState;
            var result = this.IDKLManagerService.GetDeviceOrderInfoList(request);       ///应该有多条？
            foreach (var i in result)
            {
                if (i.RealityOrderNumber == 0)
                {
                    return View(result);
                }
            }
            return View("Indext",result);
        }

        public ActionResult Indext(DeviceRequest request)
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
            return RedirectToAction("Create");
        }

        #endregion
        public ActionResult Edit(int id)               ///设备编号赋值？？？？？
        {    
            var orderData1 = this.IDKLManagerService.SelectDeviceOrderDetail(id);
            var orderData = new DeviceOrderingViewModel();
            var devices = this.IDKLManagerService.GetDeviceListByDeviceName(orderData1.DeviceName);
            orderData.orderDetail = orderData1;
            List<DeviceModel> device = new List<DeviceModel>();
            if (devices != null)
            {
                foreach (var item in devices)
                {
                    var Model = this.IDKLManagerService.GetDeviceDetail(orderData.orderDetail.OrderDate, item.Number);
                    if (Model == null)
                    {
                        device.Add(item);
                        orderData1.DeviceNumber = item.Number;
                        this.IDKLManagerService.UpdateDeviceOrderDetail(orderData1);
                    }
                }
            }
            orderData.Device = device;
            return View(orderData);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, DeviceRequest request)    ///分配？？？？
        {
            var orderInfoorderData1 = this.IDKLManagerService.SelectDeviceOrderDetail(id);

            string str = orderInfoorderData1.ProjectNumber;
            var orderInfo = this.IDKLManagerService.GetDeviceOrderInfoByProjectNumberAndCreateTime(str,orderInfoorderData1.CreateTime);
            orderInfo.OrderState = (int)EnumOrderStateInfo.OrderSucceed;
            this.IDKLManagerService.UpdateDeviceOrderInfo(orderInfo);
            orderInfo.DeviceName = orderInfoorderData1.DeviceName;
            orderInfoorderData1.OrderPerson = orderInfo.OrderPerson;
          //  orderInfo.ID = orderInfoorderData1.ID;
            //orderInfo.OrderState = (int)EnumOrderStateInfo.OrderSucceed;
            var orderData = this.IDKLManagerService.GetDeviceOrderDetaislList(id);
            string t1 =collection["textbox"];
            string num= t1.Substring(0, t1.Length);
            orderInfo.DeviceNumber = num;
            string N = collection["Number"];
            N=N.Replace(",","");
            List<string> DeviceNumber =  t1.Substring(0, t1.Length - Convert.ToInt32(N)).Split(new char[] { ',' }).ToList();
            orderData.RealityOrderNumber = Convert.ToInt32(N);
            if (orderData.RealityOrderNumber <= orderData.OrderNumber)
            {
                try
                {
                    this.IDKLManagerService.UpdateDeviceOrderDetail(orderData);
                }
                catch (Exception ex)
                {
                    return Back(ex.Message);
                }
            }
            else
            {
                return Back("实际数不能大于预约数");
            }
            DeviceDetail model = new DeviceDetail();
            foreach (var item in DeviceNumber)
            {
                if (item == ""||item==null)
                {
                   
                    model.DeviceNumber = item;///
                    model.OrderTime = orderData.OrderDate;
                    try
                    {
                        this.IDKLManagerService.AddDeviceDetail(model);
                    }
                    catch (Exception ex)
                    {
                        return Back(ex.Message);
                    }
                }
            }

                orderInfo.RealityOrderNumber = orderData.RealityOrderNumber;
                try
                {
                    if(orderInfo.ProjectNumber==null||orderInfo.ProjectNumber=="")
                    {
                        orderInfo.OrderState = (int)EnumOrderStateInfo.AddState;
                    }
                    
                    this.IDKLManagerService.AddDeviceOrderInfo(orderInfo);
                }
                 
                catch (Exception ex)
                {
                    return Back(ex.Message);
                }
                return this.RefreshParent();
        }
        public ActionResult EditDele(int id, FormCollection collection)
        {
            var orderdata = GetDeviceOrderingInfoById(id);
            if (orderdata != null)
            {
                orderdata.orderDetailList = orderdata.orderDetailList.Where(u => u.CreateTime == orderdata.orderInfo.CreateTime).ToList();
            }
            return View(orderdata);
        }

        public ActionResult Submit(int id)  
        {
            ViewData.Add("projectID", id);
            var orderData = GetDeviceOrderingInfoById(id);
            if (orderData != null)
            {
                orderData.orderDetailList = orderData.orderDetailList.Where(u => u.CreateTime == orderData.orderInfo.CreateTime).ToList();
            }
            return View(orderData);
        }

        [HttpPost]
        public ActionResult Submit(int id, FormCollection collection, DeviceRequest request)
        {

            var orderInfo = this.IDKLManagerService.GetDeviceOrderInfo(id);
            orderInfo.OrderState = (int)EnumOrderStateInfo.OrderSucceed;
            var orderData = this.IDKLManagerService.GetDeviceOrderDetaislList(id);
            string t1 = collection["orderDetail.RealityOrderNumber"];         
            int s = int.Parse(t1);
            orderData.RealityOrderNumber = s;
            if (orderData.RealityOrderNumber <= orderData.OrderNumber)
            {
                try
                {
                    this.IDKLManagerService.AddDeviceOrderDetail(orderData);
                }
                catch (Exception ex)
                {
                    return Back(ex.Message);
                }
            }
            else
            {
                return Back("实际数不能大于预约数");
            }
            try
            {
                this.IDKLManagerService.UpdateDeviceOrderInfo(orderInfo);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
            return this.RefreshParent();
        }
        [HttpPost]
        public ActionResult GoBack(int id, FormCollection collection)
        {
            var orderInfo = this.IDKLManagerService.GetDeviceOrderInfo(id);
            orderInfo.OrderState = (int)EnumOrderStateInfo.OrderFailed;
            try
            {
                this.IDKLManagerService.UpdateDeviceOrderInfo(orderInfo);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
            var orderDetailsList = this.IDKLManagerService.GetDeviceOrderDetailsListByProjectNumber(orderInfo.ProjectNumber);
            orderDetailsList = orderDetailsList.Where(u => u.CreateTime == orderInfo.CreateTime).ToList();
            List<int> ids = new List<int>();
            foreach (var item in orderDetailsList)
            {
                ids.Add(item.ID);
            }
            try
            {
                this.IDKLManagerService.DeleteDeviceOrderDetail(ids);
            }
            catch (Exception ex)
            {
                return Back(ex.Message);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Deletes(FormCollection collection, List<int> ids)
        {
            string itemID = collection["id"];
            List<int> newList = new List<int>();
            newList.Add(Convert.ToInt32(itemID));
            try 
	          {
                  this.IDKLManagerService.DeleteDeviceOrderDetail(newList);
               }
	            catch (Exception)
	           {     
	           }
            return RedirectToAction("Submit", "DeviceOrdering", new { id = collection["projectID"] });
        }

    }
}