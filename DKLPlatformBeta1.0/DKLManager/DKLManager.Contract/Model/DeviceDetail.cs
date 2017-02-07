using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKLManager.Contract.Model
{
    public class DeviceDetail : ModelBase
    {
        [Key]
        public override int ID { get; set; }
        public string DeviceNumber { get; set; }
        public DateTime OrderTime { get; set; }
    }
}
