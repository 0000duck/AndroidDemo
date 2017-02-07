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
    [System.ComponentModel.DataAnnotations.Schema.Table("SampleRegisterTable")]
    public class SampleRegisterTable : ModelBase
    {

         /// <summary>
         /// 样品登记表
         /// </summary>
        [Key]        
         public override int ID { get; set; }
        public string ProjectNumber { get; set; }
        [Required(ErrorMessage = "不能为空")]
        public string SampleRegisterNumber { get; set; }
         public DateTime SamplingDay { get; set; }
         public string SampleName { get; set; }
         public string SampleState { get; set;}
         public string SampleNumBer { get; set; }
         public string SaveCondition { get; set; } 
         public string Remark { get; set; }
         public string ParameterName { get; set; }
         public string AnalyzePeople { get; set; }
         public string Argument { get; set; }
         public int SampleStates { get; set; }
         public string ArgumentPrice { get; set; }
         public string WorkShop { get; set; }    //  车间
         public string Job { get; set; }         //职位
         public string Location { get; set; }    //地点
         public string Factor { get; set; }     //因素
         public string CSTEL { get; set; }    //短时间接触浓度
         public string CTWA { get; set; }        //检测结果
         public string CMAC { get; set; }        //检测结果


    }
    public enum EnumAnalyzePeople
    {
        [EnumTitle("fxra")]
        fxra = 1,
        [EnumTitle("fxrb")]
        fxrb = 2,
        [EnumTitle("fxrc")]
        fxrc = 3,
    }
    public enum EnumSampleStates
    {
        [EnumTitle("新样品")]
        NewSample = 1,
        [EnumTitle("旧样品")]
        OldSample = 2,
        [EnumTitle("分析人员添加完参数")]
        DoneSample = 3,
    }

    public enum EnumSampleRegisterNumber
    {
        [EnumTitle("二氧化锰")]
        ManganeseDioxide = 1,
        [EnumTitle("臭氧")]
        Ozone = 2,
        [EnumTitle("总尘")]
        TotalDust = 3,
    }
    public enum EnumSaveCondition
    {
        [EnumTitle("冷藏")]
        Refrigerate = 1,
        [EnumTitle("室温")]
        IndoorTemperature = 2,
        [EnumTitle("尽快测定")]
        FastMeasure = 3,
    }
}
