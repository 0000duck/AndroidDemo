using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Demo.Areas.DKLManager.Models
{
    public class ProjectImgFileViewModel
    {
        public  int ID { set; get; }
        public string ProjectNumber { get; set; }
        public List<string> FilePaths { get; set; }

    }
}