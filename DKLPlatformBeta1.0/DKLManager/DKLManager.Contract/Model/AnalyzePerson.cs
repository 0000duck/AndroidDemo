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
    [System.ComponentModel.DataAnnotations.Schema.Table("AnalyzePerson")]
    public class AnalyzePerson : ModelBase
    {
        public override int ID { get; set; }
        public string SampleRegisterNumber { get; set; }
        public DateTime SamplingDay { get; set; }
        public string SampleName { get; set; }
        public string SampleState { get; set; }
        public string DetectionParameters { get; set; }
        public string SampleNumBer { get; set; }
        public string SaveCondition { get; set; }
        public string Remark { get; set; }
        public string ParameterName { get; set; }
        public string Argument { get; set; }
        public string State { get; set; }
        public string AnalyzePeople { get; set; }
    }
    public enum EnumState
    {
        [EnumTitle("未填写")]
        NoWrite = 1,
        [EnumTitle("已填写")]
        YesWrite = 2,
    }
}
