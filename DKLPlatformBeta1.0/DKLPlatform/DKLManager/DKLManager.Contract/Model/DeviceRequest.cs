using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKLManager.Contract.Model
{
    public class DeviceRequest : Request
    {
        public string ProjectNumber { get; set; }

        public string OrderPerson { get; set; }

        public int OrderState = 0;
        public DateTime OrderDate { get; set; }
    }
}
