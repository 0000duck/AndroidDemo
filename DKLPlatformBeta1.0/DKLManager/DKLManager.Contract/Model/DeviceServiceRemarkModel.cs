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
    [System.ComponentModel.DataAnnotations.Schema.Table("DeviceServiceRemarkModel")]

   public class DeviceServiceRemarkModel:ModelBase
    {
       [Key]
        public override int ID { set; get; }
       [Required(ErrorMessage = "不能为空")]
        public int DeviceServiceId { get; set; }
       [Required(ErrorMessage = "不能为空")]
        public string DeviceServicePerson { get; set; }
       [Required(ErrorMessage = "不能为空")]
        public DateTime DeviceServiceTime { get; set; }
       [Required(ErrorMessage="不能为空")]
        public string Remark { get; set; }
       public DateTime CalibrationTime { get; set; }
       //鉴定周期
       public DateTime CalibrationPeriod { get; set; }
    }
}
