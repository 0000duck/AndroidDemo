using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKLManager.Contract.Model
{
    /// <summary>
    /// 检测参数表
    /// </summary>
    public class ParameterTable : ModelBase
    {
         [Key]
        public override int ID { get; set; }
        public int StandardNumber { get; set; }
        public string MethodName { get; set; }
        public string UseApparatus { get; set; }
        public string SampleName { get; set; }
        public string SampleState { get; set; }
        public string SaveMethod { get; set; }
        public string SampleFlow { get; set; }
        public string SampleSize { get; set; }
        public string MethodDescribe { get; set; }
       // 方法解析洗脱效率
        public string MethodResolutionElutionEefficiency { get; set; }
        //解析洗脱效率
        public string ParsingTheElutionEfficiency { get; set; }
        //检出限
        public string TestLimit { get; set; }
        //最低浓度
        public string LowestConcentration { get; set; }
        //穿透容量
        public string PenetratingCapacity { get; set; }
    }
}
