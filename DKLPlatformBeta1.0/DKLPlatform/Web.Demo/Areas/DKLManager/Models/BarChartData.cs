using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Demo.Areas.DKLManager.Models
{
    public class BarChartData
    {
        public List<string> lables;
        public List<BarDatasets> dataset;
    }

    public class BarDatasets
    {
        public string fillColor;
        public string strokeColor;
        public string highlightFill;
        public string highlightStroke;
        public List<int> data;
    }
}