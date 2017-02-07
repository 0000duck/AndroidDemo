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
    [System.ComponentModel.DataAnnotations.Schema.Table("DeviceUseRecordModel")]

    public class DeviceUseRecordModel : ModelBase
    {
       [Key]
        public override int ID { set; get; }
       [Required(ErrorMessage = "不能为空")]
       public int DeviceUseId { get; set; }
       [Required(ErrorMessage = "不能为空")]
       public string UserPerson { get;set;}
       [Required(ErrorMessage = "不能为空")]
       public DateTime UserTime { get; set; }
       [Required(ErrorMessage = "不能为空")]
       public string Remark { get; set; }
    }
}
