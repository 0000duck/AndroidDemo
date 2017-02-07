using HYZK.FrameWork.Common;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKLManager.Contract.Model
{

    [Serializable]
    [System.ComponentModel.DataAnnotations.Schema.Table("ProjectFile")]

    public class ProjectFile : ModelBase
    {
        [Key]
        public override int ID { set; get; }
        [Required(ErrorMessage = "不能为空")]
        public string ProjectNumber { get; set; }
        public string FilePath { get; set; }

    }
    [Serializable]
    [System.ComponentModel.DataAnnotations.Schema.Table("ProjectDocFile")]

    public class ProjectDocFile : ModelBase
    {
        [Key]
        public override int ID { set; get; }
        [Required(ErrorMessage = "不能为空")]
        public string ProjectNumber { get; set; }
        public string FilePath { get; set; }
        public string suggenstion { get; set; }
        public int Status { get; set; }
        public string WriteName { get; set; }

    }

}
