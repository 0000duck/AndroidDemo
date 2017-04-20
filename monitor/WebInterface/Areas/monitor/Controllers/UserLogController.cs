using Account.Contract;
using bjwaterAPI.Areas.EduInfo.Controllers;
using HYZK.Account.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MnonitorBack.Areas.EduInfo.Controllers
{
    public class UserLogController : BaseController
    {
        public HttpResponseMessage Get()
        {
            var logs = this.IAccountService.SelectAllUserLog();
            return Request.CreateResponse(HttpStatusCode.OK, logs);

        }
        public HttpResponseMessage Get(int currentPage, int itemsPerPage)                     //分页
        {
            var model = this.IAccountService.SelectAllUserLog();
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