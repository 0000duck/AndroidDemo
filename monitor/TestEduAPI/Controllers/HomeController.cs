using HYZK.Account.Contract;
using HYZK.FrameWork.Utility;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestEduAPI.Controllers
{
    public class HomeController : Controller
    {

        private static string key = "147594e57b986c39450a1325829567ad";
        private static string url = "http://123.57.152.229/";/// "http://biofgt.com/DklEdu";
        public ActionResult Index()
        {

            //var client1 = new RestClient(url);
            //var request1 = new RestRequest("api/UserApi/{idcard}", Method.GET);
            //request1.AddParameter("idcard", "123654", ParameterType.QueryString);
            //request1.AddParameter("Content-Type", "application/json; charset=utf-8", ParameterType.HttpHeader);
            //string strSignTemp1 = url + request1.Resource + "&key=" + key;
            //string vSign1 = Encrypt.MD5(strSignTemp1);
            //request1.AddParameter("sign", vSign1, ParameterType.QueryString);


            //var jasondata = new List<EduUser>();
            //var queryData = client1.Execute<List<EduUser>>(request1);
            //jasondata = queryData.Data;
            //jasondata = (jasondata == null) ? new List<EduUser>() : jasondata;





            var client = new RestClient(url);
            var request = new RestRequest("api/UserApi", Method.GET);
            request.AddParameter("Content-Type", "application/json; charset=utf-8", ParameterType.HttpHeader);
            string strSignTemp = url + request.Resource + "&key=" + key;
            string vSign = Encrypt.MD5(strSignTemp);
            request.AddParameter("sign", vSign, ParameterType.QueryString);

            var jasondata = new List<EduUser>();
            var queryData = client.Execute<List<EduUser>>(request);
            jasondata = queryData.Data;
            jasondata = (jasondata == null) ? new List<EduUser>() : jasondata;


            return View(jasondata);
        }

        public ActionResult Create()
        {
            var model = new EduUser();
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new EduUser(); 
            this.TryUpdateModel<EduUser>(model);
            var client = new RestClient(url);
            var request = new RestRequest("api/UserApi", Method.POST);
            request.AddParameter("Content-Type", "application/json; charset=utf-8",ParameterType.HttpHeader);
            string strSignTemp = url + request.Resource + "&key=" + key;
            string vSign = Encrypt.MD5(strSignTemp);
            request.AddParameter("sign", vSign,ParameterType.QueryString);

            Random rdata = new Random();
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new EduUser
            {
                idcard = "2154886323632325",
                CreateTime = DateTime.Now
            });
            var ret = client.Execute(request);

            var retsult = ret;
            //var ret = client.Execute(request);

            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            var client = new RestClient(url);
            var request = new RestRequest("api/UserApi/{id}", Method.GET);
            request.AddParameter("Content-Type", "application/json; charset=utf-8");
            request.AddParameter("id", id);
            string strSignTemp = url + request.Resource + "&key=" + key;
            string vSign = Encrypt.MD5(strSignTemp);
            request.AddParameter("sign", vSign, ParameterType.QueryString);

            var queryData = client.Execute<EduUser>(request);
            var jasondata = queryData.Data;

            return View(jasondata);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = new EduUser();
            this.TryUpdateModel<EduUser>(model);

            var client = new RestClient(url);
            var request = new RestRequest("api/UserApi/", Method.POST);
            string strSignTemp = url + request.Resource + "&key=" + key;
            string vSign = Encrypt.MD5(strSignTemp);
            request.AddParameter("sign", vSign, ParameterType.QueryString);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(new EduUser
            {
                CreateTime = DateTime.Now
            });
            client.Execute(request);


            string alert = null;
            var script = string.Format("<script>{0}; parent.location.reload(1)</script>", string.IsNullOrEmpty(alert) ? string.Empty : "alert('" + alert + "')");
            return this.Content(script);
        }
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            foreach (var id in ids)
            {
                var item = new EduUser();
                var client = new RestClient(url);
                var request = new RestRequest("api/UserApi/{id}", Method.DELETE);
                request.AddParameter("Content-Type", "application/json; charset=utf-8", ParameterType.HttpHeader);
                request.AddParameter("id", id, ParameterType.QueryString);
                string strSignTemp = url + request.Resource + "&key=" + key;
                string vSign = Encrypt.MD5(strSignTemp);
                request.AddParameter("sign", vSign, ParameterType.QueryString);
                var resutl = client.Execute(request);
                string dd = resutl.Content;
            }
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}