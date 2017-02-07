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
    [System.ComponentModel.DataAnnotations.Schema.Table("DeviceStateModel")]

    public class DeviceStateModel : ModelBase
    {
       [Key]
       public override int ID { set; get; }
       [Required(ErrorMessage="不能为空")]
       public int StateNumber { get; set; }
       [Required(ErrorMessage="不能为空")]
       public string StateDescribe { get; set; }
    }
}
