using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKLManager.Contract.Model
{
    public class DeviceOder:ModelBase
    {
        [Key]
        public override int ID { get; set; }
        public string OrderDeviceName { get; set; }
        public int OrderDeviceNumBer { get; set; }
        public int State { get; set; }
    }
}
