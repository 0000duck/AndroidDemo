using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKLManager.Contract.Model
{
    [Serializable]
    [System.ComponentModel.DataAnnotations.Schema.Table("DeviceIdentityRemarkModel")]

   public class DeviceIdentityRemarkModel:ModelBase
    {
       [Key]
        public override int ID { set; get; }
       [Required(ErrorMessage = "不能为空")]
        public int DeviceIdentityid { get; set; }
       [Required(ErrorMessage = "不能为空")]
        public string DeviceIdentityPerson { get; set; }
       [Required(ErrorMessage = "不能为空")]
        public DateTime DeviceIdentityTime { get; set; }
       [Required(ErrorMessage = "不能为空")]
        public string Remark { get; set; }
    }
}
