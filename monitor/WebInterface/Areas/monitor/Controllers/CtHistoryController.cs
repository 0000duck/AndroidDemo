using bjwaterAPI.Areas.EduInfo.Controllers;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace WebInterface.Areas.monitor.Controllers
{
    public class CtHistoryController : BaseController
    {
        public HttpResponseMessage Get()
        {
            var paikelists = this.IHSService.SelectAllControlerHistory();
            return Request.CreateResponse(HttpStatusCode.OK, paikelists);

        }
        public HttpResponseMessage Get(int currentPage, int itemsPerPage)                     //分页
        {
            var model = this.IHSService.SelectAllControlerHistory();
            int totalCount = model.Count;
            if (model != null)
            {
                model = model.OrderByDescending(t => t.ID).Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, new { model, totalCount });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}