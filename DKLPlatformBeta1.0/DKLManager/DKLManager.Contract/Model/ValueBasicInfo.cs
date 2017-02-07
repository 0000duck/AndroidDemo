using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKLManager.Contract.Model
{
    [Serializable]
    [System.ComponentModel.DataAnnotations.Schema.Table("ValueBasicInfo")]

    public class ValueBasicInfo : ModelBase
    {
        [Key]
        public override int ID { set; get; }
        [Required(ErrorMessage = "不能为空")]
        public string ProjectNumber { get; set; }
    }
}
