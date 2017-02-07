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
    [System.ComponentModel.DataAnnotations.Schema.Table("DeviceModel")]

    public class DeviceModel : ModelBase
    {
        [Key]
        public override int ID { set; get; }
        /// <summary>
        /// 仪器名称
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        public string DeivceName { get; set; }
        /// <summary>
        /// 自编号
        /// </summary>
        public string Number { get; set; }
        public string States { get; set; }
        public string ModelNum { get; set; }
        public string Manufacturer { get; set; }
        public string SkillParameter { get; set; }
        public DateTime BuyTime { get; set; }
        public double Price { get; set; }
        public string State { get; set; }
        public string DeviceIdentityPerson { get; set; }
        public DateTime DeviceIdentityTime { get; set; }
        public DateTime CheckTime { get; set; }
        public DateTime LastCheckTime { get; set; }
        public DateTime CorrectTime { get; set; }
        public DateTime LastCorrectTime { get; set; }
        //鉴定日期
        public DateTime CalibrationTime { get; set; }
        //设备种类
        public int DeviceMold { get; set; }
        //鉴定周期
        public string CalibrationPeriod { get; set; }
        public string Remark { get; set; }
        public int CheckState { get; set; }
        public int DeviceIdentityid { get; set; }
        public string DeviceServicePerson { get; set; }
        public DateTime DeviceServiceTime { get; set; }
        public string ProjectProblemDescribe { get; set; }      //问题原因描述


    }
    public enum EnumDeviceMold
    {
        [EnumTitle("现场检测")]
        SceneDetection = 1,
        [EnumTitle("大型设备")]
        BigDevice = 2,
        [EnumTitle("校验设备")]
        SiteDevice = 3,
        [EnumTitle("现场采用")]
        SceneUse = 4,
        [EnumTitle("大型设备", IsDisplay = false)]
        BigSiteDevice = 6,
    }
    public enum EnumCheckState
    {
        [EnumTitle("正常")]
        Normal = 0,

        [EnumTitle("待检定")]
        WaitCheck = 1,
        [EnumTitle("正常待检测", IsDisplay = false)]
        WaitcheckNormal = 3,
        [EnumTitle("维修")]
        Service=5,
        [EnumTitle("外借")]
        Services = 6,
        [EnumTitle("退回")]
        Servicess = 7,
        [EnumTitle("停用")]
        StopUse = 8,
    }
}
   