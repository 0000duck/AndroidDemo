using Account.Contract;
using bjwaterAPI.Areas.EduInfo.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MnonitorBack.Areas.EduInfo.Controllers
{
    public class UserController : BaseController
    {
        public HttpResponseMessage Get()
        {
            var users = this.IAccountService.SelectAllUser();
            return Request.CreateResponse(HttpStatusCode.OK, users);

        }
        public HttpResponseMessage Get(int currentPage, int itemsPerPage)                     //分页
        {
            var model = this.IAccountService.SelectAllUser();
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
        public HttpResponseMessage Post([FromBody]User model)
        {
            if ((string.IsNullOrEmpty(model.HttpMethod)) || (model.HttpMethod.ToUpper() == "POST"))
            {
                try
                {
                    this.IAccountService.InsertUserInfo(model);
                }
                catch
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified);
                }
            }
            else if (model.HttpMethod.ToUpper() == "PUT")
            {
                return PUT(model);
            }
            else if (model.HttpMethod.ToUpper() == "DELETE")
            {
                return DELETE(model);
            }
            return Request.CreateResponse(HttpStatusCode.MethodNotAllowed);
        }
        public HttpResponseMessage DELETE(User model)                                         //删除图片，并且删除历史操作中的上传记录
        {
            if (model != null && model.Remark != null)
            {
                try
                {
                    List<int> ids = Array.ConvertAll<string, int>(model.Remark.Split(new char[] { ',' }), s => int.Parse(s)).ToList();
                    this.IAccountService.DeleteUser(ids);
                }
                catch
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "删除成功！");
            }
            else
                return Request.CreateResponse(HttpStatusCode.NotImplemented, "请确认输入正确！");
        }

        public HttpResponseMessage DELETE(string itemids)                                         //删除图片，并且删除历史操作中的上传记录
        {
            if (itemids != null)
            {
                try
                {
                    List<int> ids = Array.ConvertAll<string, int>(itemids.Split(new char[] { ',' }), s => int.Parse(s)).ToList();
                    this.IAccountService.DeleteUser(ids);
                }
                catch
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "删除成功！");
            }
            else
                return Request.CreateResponse(HttpStatusCode.NotImplemented, "请确认输入正确！");
        }
        public HttpResponseMessage PUT(User model)
        {
            if (model != null && model.ID != 0)
            {
                var user = this.IAccountService.SelectUser(model.ID);
                if (user != null)
                {
                    try
                    {
                        this.IAccountService.UpdateUser(user);
                    }
                    catch
                    {
                        return Request.CreateResponse(HttpStatusCode.NotModified);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "更新成功！");
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "未能找到该信息片！");
            }
            else
                return Request.CreateResponse(HttpStatusCode.NotImplemented, "操作错误！");
        }
    }
}