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
    [System.ComponentModel.DataAnnotations.Schema.Table("DeviceOrderInfo")]

    public class DeviceOrderInfo : ModelBase
    {
        [Key]
        public override int ID { set; get; }
        [Required(ErrorMessage = "不能为空")]
        public string ProjectNumber { get; set; }
        [Required(ErrorMessage = "不能为空")]
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "不能为空")]

        //申请人
        public string OrderPerson { get; set; }
        public int OrderState { get; set; }
        public string DeviceName { get; set; }
        public string DeviceNumber { get; set; }
        public int RealityOrderNumber { get; set; }

    }
    public enum EnumOrderStateInfo
    {
        [EnumTitle("无", IsDisplay = false)]
        None = 0,

        [EnumTitle("预约申请")]
        AddState = 1,
        [EnumTitle("同意预约")]
        OrderSucceed = 2,
        [EnumTitle("退回预约")]
        OrderFailed = 3,
    }


}
