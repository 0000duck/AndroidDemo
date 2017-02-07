using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Areas.DKLManager.Models;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ProjectChartController : Controller
    {
        //
        // GET: /DKLManager/ProjectChart/
        public ActionResult Index()
        {
            var model = new BarChartData();

            BarDatasets bardataSet = new BarDatasets();
            bardataSet.fillColor = "rgba(220,220,220,0.5)";
            bardataSet.strokeColor = "rgba(220,220,220,0.8)";
            bardataSet.highlightFill = "rgba(220,220,220,0.75)";
            bardataSet.highlightStroke = "rgba(220,220,220,1)";
            bardataSet.data.Add(12);
            bardataSet.data.Add(58);
            bardataSet.data.Add(63);
            bardataSet.data.Add(67);
            bardataSet.data.Add(81);

            model.lables.Add("一月份");
            model.lables.Add("二月份");
            model.lables.Add("三月份");
            model.lables.Add("四月份");
            model.lables.Add("五月份");
            model.dataset.Add(bardataSet);


            return View(model);
        }

	}
}