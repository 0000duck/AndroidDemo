using DKLManager.Contract.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Web.Demo.Areas.DKLManager.Models;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class DeviceOrderSuccessController : DeviceOrderBaseController
    {
        //
        // GET: /DKLManager/DeviceOrderSuccess/
        public ActionResult Index(DeviceRequest request) ///能否直接查重？
        {
            request.OrderState = (int)EnumOrderStateInfo.OrderSucceed;
            var DeviceOrderInfo = this.IDKLManagerService.GetDeviceOrderInfoListD(request).Select(c => new { ProjectNumber = c.ProjectNumber }).Distinct();
            List<DeviceOrderInfo> deviceorderInfo = new List<DeviceOrderInfo>();
            foreach (var item in DeviceOrderInfo)
            {
                string Number = item.ProjectNumber;
                var deviceOrderInfo = this.IDKLManagerService.GetDeviceOrderInfoByNumberD(Number);
                deviceorderInfo.Add(deviceOrderInfo);
            }
            return View(deviceorderInfo);
            //request.OrderState = (int)EnumOrderStateInfo.OrderSucceed;
            //var DeviceOrderDetail = this.IDKLManagerService.GetDeviceOrderDetailList(request).Select(c => new { ProjectNumber = c.ProjectNumber }).Distinct();
            //List<DeviceOrderDetail> deviceorderdetail = new List<DeviceOrderDetail>();
            //foreach (var item in DeviceOrderDetail)
            //{
            //    string Number = item.ProjectNumber;
            //    var deviceOrderDetail = this.IDKLManagerService.GetDeviceOrderDetailByNumber(Number);                
            //    deviceorderdetail.Add(deviceOrderDetail);
            //}
            //return View(deviceorderdetail);
        }
        public ActionResult View(int id, string projectNumber)
        {
            List<DeviceOrderInfo> model = new List<DeviceOrderInfo>();
            model = this.IDKLManagerService.GetDeviceOrderInfoByProjectNumbert(projectNumber);
            return View(model);
        }

       
    }
}