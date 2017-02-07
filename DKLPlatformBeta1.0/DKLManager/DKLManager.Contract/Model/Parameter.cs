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
    [System.ComponentModel.DataAnnotations.Schema.Table("Parameter")]
    public class Parameter : ModelBase
    {
       public override int ID { get; set; }
       public string ParameterName { get; set; }
       public string ApparatusName { get; set; }
       public string ApparatusNumber { get; set; }

       public string DetectionPursuant { get; set; }
        // 短时间接触容许浓度
       public string ShorttimeTouchAllowConcentration { get; set; }
        //时间加权平均容许浓度
       public string TimeWeightingAverageAllowConcentration { get; set; }
        //最高容许浓度
       public string HighestAllowConcentration { get; set; }
        //超限倍数
       public string TransfiniteMultiple { get; set; }
       public int SampleType { get; set; }     //样品类别
       public string Limit { get; set; }   //限制
       public string AddValue { get; set; }//增加值
    }
    public enum EnumSampleType
    {
        [EnumTitle("化学")]
        fxra = 0,
        [EnumTitle("物理")]
        fxrb = 1,
        [EnumTitle("微生物")]
        Wsw = 2,

    }
}
