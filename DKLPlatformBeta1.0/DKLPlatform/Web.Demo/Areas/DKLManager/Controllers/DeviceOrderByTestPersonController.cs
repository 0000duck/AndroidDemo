using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Areas.DKLManager.Models;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class DeviceOrderByTestPersonController : DeviceOrderBaseController
    {
        public ActionResult Create(string projectNumber)
        {
            deviceOrderData = new DeviceOrderingViewModel();
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

                        //Response.Write("<script>window.opener=null;window.close();</script>");// 不会弹出询问

                        //var result = this.IDKLManagerService.GetDeviceOrderInfoList();

                        return this.Content("<script>window.opener=null;window.close();</script>");
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

	}
}