using DKLManager.Contract.Model;
using System.Web.Mvc;
using Web.Demo.Areas.DKLManager.Models;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class DeviceOrderSuccessController : DeviceOrderBaseController
    {
        //
        // GET: /DKLManager/DeviceOrderSuccess/
        public ActionResult Index(DeviceRequest request)
        {
            request.OrderState = (int)EnumOrderStateInfo.OrderSucceed;
            var result = this.IDKLManagerService.GetDeviceOrderInfoList(request);
            return View(result);
        }
        public ActionResult View(int id)
        {
            var orderData = GetDeviceOrderingInfoById(id);
            return View(orderData);
        }        
	}
}