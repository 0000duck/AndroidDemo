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
    public class Check : ModelBase
    {
        [Key]
        public override int ID { get; set; }
        public string token { get; set; }                     ///巡检编号
        public string DeviceName { get; set; }              ///设备名称
        public string DeviceNumber { get; set; }            ///设备编号
        public int Count { get; set; }                      ///巡检设备在本列表中的位置
        public string CheckPerson { get; set; }             ///巡检人员
        public string CheckTime { get; set; }               ///巡检时间  年/月/日
        public string Deviceproblem { get; set; }           ///设备问题       
    }
}
