using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;
using HYZK.FrameWork.Common;
using Web.Demo.Areas.DKLManager.Models;
using System.Drawing.Imaging;
using System.IO;


namespace Web.Demo.Areas.DKLManager.Models
{
    public class ProjectSearch
    {
        public List<ProjectInfoHistory> projectList { get; set; }
        public string JobType { get; set; }
        public string People { get; set; }
        public string ProjectType { get; set; }
        public string Year { get; set; }
    }
}