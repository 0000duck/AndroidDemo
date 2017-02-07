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
    [System.ComponentModel.DataAnnotations.Schema.Table("CostingHistory")]
    public class CostingHistory : ModelBase
    {
        public string ProjectNumber { get; set; }
        public DateTime SignTime { get; set; }
        [Required(ErrorMessage = "不能为空")]
        public string ProjectName { get; set; }
        //项目简介
        public string ProjectSynopsis { get; set; }
        //客户区县
        public string CustomerCounty { get; set; }
        //联系人
        public string ContactsPserson { get; set; }
        //签字
        public string Sign { get; set; }
        //所属乡镇
        public string Towns { get; set; }
        //联系方式
        public string Relation { get; set; }
        //业务负责人
        public string HeadOfPeople { get; set; }
        public string Type { get; set; }
        public override int ID { get; set; }
        //销售额
        public double Sales { get; set; }
        //技术成本
        public double TechnologyCosts { get; set; }
        //推广费
        public double PromotionFee { get; set; }
        //协作费
        public double CollaborationFee { get; set; }
        //提成
        public double Commission { get; set; }
        //交通补助
        public double TrafficSubsidies { get; set; }
        //毛利润
        public double GrossProfit { get; set; }
        //其他费用
        public int OtherFees { get; set; }
        //税金
        public double Tax { get; set; }
        //毛利润率
        public double GrossProfitMargin { get; set; }
        public int CostingState { get; set; }
        public string Remark { get; set; }
        public int SamplePrice { get; set; }
        //工时
        public double WorkingHours { get; set; }
        //工时单价
        public double WorkingHoursPrice { get; set; }
        //数量和
        public double NumBerSum { get; set; }
        //物理因素
        public double PhysicalFactors { get; set; }
        //粉尘类
        public double Dust { get; set; }
        //无机类
        public double Inorganic { get; set; }
        //有机类
        public double Organic { get; set; }
        //游离SiO2
        public double Free { get; set; }
        //物理因素单价
        public double PhysicalFactorsPrice { get; set; }
        //粉尘类单价
        public double DustPrice { get; set; }
        //无机类单价
        public double InorganicPrice { get; set; }
        //有机类单价
        public double OrganicPrice { get; set; }
        //游离SiO2单价
        public double FreePrice { get; set; }
        public string Person { get; set; }
        public static CostingHistory Clone(Costing info)
        {
            CostingHistory history = new CostingHistory();
            history.Person = info.Person;
            history.Sales = info.Sales;
            history.TechnologyCosts = info.TechnologyCosts;
            history.PromotionFee = info.PromotionFee;
            history.CollaborationFee = info.CollaborationFee;
            history.Commission = info.Commission;
            history.TrafficSubsidies = info.TrafficSubsidies;
            history.GrossProfit = info.GrossProfit;
            history.OtherFees = info.OtherFees;
            history.Tax = info.Tax;
            history.GrossProfitMargin = info.GrossProfitMargin;
            history.CostingState = info.CostingState;
            history.Remark = info.Remark;
            history.CreateTime = info.CreateTime;
            history.SamplePrice = info.SamplePrice;
            history.WorkingHours = info.WorkingHours;
            history.WorkingHoursPrice = info.WorkingHoursPrice;
            history.NumBerSum = info.NumBerSum;
            history.PhysicalFactors = info.PhysicalFactors;
            history.Dust = info.Dust;
            history.Inorganic = info.Inorganic;
            history.Organic = info.Organic;
            history.Free = info.Free;
            history.PhysicalFactorsPrice = info.PhysicalFactorsPrice;
            history.DustPrice = info.DustPrice;
            history.InorganicPrice = info.InorganicPrice;
            history.OrganicPrice = info.OrganicPrice;
            history.FreePrice = info.FreePrice;
            history.Type = info.Type;

            return history;
        }
    }
    public enum EnumHistoryType
    {
        [EnumTitle("现状评价")]
        one = 0,
        [EnumTitle("控制效果评价")]
        two = 0,
        [EnumTitle("预评价")]
        three = 0,
        [EnumTitle("检测与分析")]
        four = 0,
        [EnumTitle("水质监测")]
        five = 0,
        [EnumTitle("样品检测")]
        six = 0,
        [EnumTitle("日常检测")]
        seven = 0,
    }
}
