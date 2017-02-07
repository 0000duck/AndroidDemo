using DKLManager.Contract.Model;
using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Demo.Areas.DKLManager.Models
{
    public class DeviceOrderingViewModel
    {
        public DeviceOrderingViewModel()
        {
            orderInfo = new DeviceOrderInfo();
            orderDetail = new DeviceOrderDetail();
            orderDetailList = new List<DeviceOrderDetail>();
        }

        public DeviceOrderInfo orderInfo;
        public DeviceOrderDetail orderDetail;//只是为了前端验证用
        public IList<DeviceOrderDetail> orderDetailList;
    }
}