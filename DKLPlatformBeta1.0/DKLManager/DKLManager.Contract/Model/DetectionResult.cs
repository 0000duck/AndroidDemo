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
    [System.ComponentModel.DataAnnotations.Schema.Table("DetectionResult")]
    //检测结果表
    public class DetectionResult : ModelBase
    {
        public override int ID { get; set; }
        public int  Physicochemical { get;set;}
        public string Workshop { get; set; }
        //岗位
        public string Post { get; set; }
        public string State { get; set; }
    }
    public enum EnumPhysicochemical
    {
        [EnumTitle("物理")]
        Physics = 1,
        [EnumTitle("化学")]
        Chemistry = 2,
    }
}
