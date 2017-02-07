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
    [System.ComponentModel.DataAnnotations.Schema.Table("DeviceOrderDetail")]

    public class DeviceOrderDetail : ModelBase
    {
        [Key]
        public override int ID { set; get; }

        [Required(ErrorMessage = "不能为空")]
        public string ProjectNumber { get; set; }
        [Required(ErrorMessage = "不能为空")]
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "不能为空")]

        public string DeviceName { get; set; }
        [Required(ErrorMessage = "不能为空")]
        [RegularExpression(@"^\+?[1-9][0-9]*$", ErrorMessage = "不是有效的个数")]
        public int OrderNumber { get; set; }
        [Required(ErrorMessage = "不能为空")]
        public int RealityOrderNumber { get; set; }
        public string DeviceNumber { get;set;}
        public int CheckState { get; set; }
        public string OrderPerson { get; set; }

    }
}
