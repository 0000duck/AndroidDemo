using HYZK.FrameWork.Common;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJWater.Contract.Model
{
   public class Device : ModelBase
    {
        [Key]
        public override int ID { get; set; }
        public string DeviceName { get; set; }              ///设备名称
        public string DeviceNumber { get; set; }            ///设备编号
        public string factory { get; set; }                 ///生产厂家     
        public string DeviceBirthday { get; set; }          ///设备生产日期
        public string DeviceAge { get; set; }               ///设备有效年限
        public string DevicePrice { get; set; }             ///设备价格
        public int State { get; set; }                      ///设备状态

        public enum EnumState
        {
            [EnumTitle("正常使用")]
            Normal = 0,

            [EnumTitle("维护中")]
            Repair = 1,
        }
    }
}
