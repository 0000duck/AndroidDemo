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
    [System.ComponentModel.DataAnnotations.Schema.Table("DeviceOrderEntifyModel")]

      public  class DeviceOrderEntifyModel:ModelBase
      {
       [Key]
       public override int ID { set; get; }       
       public string PersonName { get; set; }        
       public int DeviceName { get; set; }
       [Required(ErrorMessage = "不能为空")]
       public int Count { get; set; }
      [Required(ErrorMessage = "不能为空")]
       public int DeviceType { get; set; }
       //[Required(ErrorMessage = "不能为空")]
       //public DateTime StartTime { get; set; }
       //[Required(ErrorMessage = "不能为空")]
       //public DateTime LastTime { get; set; }
      [Required(ErrorMessage = "不能为空")]
      public DateTime StartTime { get; set; }
       [Required(ErrorMessage = "不能为空")]
       public int OrderState { get; set; }
       public override DateTime  CreateTime { get; set; }
       public int DeviceState { get; set; }
    }
    public enum EnumDeviceName
    {
        [EnumTitle("鼠标")]
        Mouse = 1,
        [EnumTitle("键盘")]
        KeyBoard = 2,
        [EnumTitle("显示器")]
        DisPlay = 3,
    }

      public enum EnumOrderState
      {
          [EnumTitle("空闲")]
          AddState = 1,
          [EnumTitle("使用中")]
          OrderSucceed = 2,
      }

}
