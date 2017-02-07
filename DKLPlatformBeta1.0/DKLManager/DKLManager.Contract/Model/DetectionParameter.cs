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
    [System.ComponentModel.DataAnnotations.Schema.Table("DetectionParameter")]
    /// <summary>
    /// 检测参数表
    /// </summary>
    public class DetectionParameter : ModelBase
    {
        [Key]
        public override int ID { get; set; }
        public string SampleRegisterNumber { get; set; }
        public DateTime SamplingDate { get; set; }
        public string SampleName { get; set; }
        public string SampleState { get; set; }
        public string DetectionParameters { get; set; }
        public string SampleNumBer { get; set; }
        public string SaveCondition { get; set; }
        public string Remark { get; set; }
        public string ParameterName { get; set; }
        
    }
    public enum EnumDetectionNumber
    {
        [EnumTitle("二氧化锰")]
        ManganeseDioxide = 1,
        [EnumTitle("臭氧")]
        Ozone = 2,
        [EnumTitle("总尘")]
        TotalDust = 3,
    }
    public enum EnumSampleState
    {
        [EnumTitle("气态")]
        Gaseous = 1,
        [EnumTitle("液态")]
        Liquid = 2,
        [EnumTitle("固态")]
        Solidity = 3,
    }
}
