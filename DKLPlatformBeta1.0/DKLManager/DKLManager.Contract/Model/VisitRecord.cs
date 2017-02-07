using System;
using System.ComponentModel.DataAnnotations;
using HYZK.FrameWork.Utility;
using HYZK.FrameWork.Common;

namespace DKLManager.Contract.Model
{
    [Serializable]
    [System.ComponentModel.DataAnnotations.Schema.Table("VisitRecord")]
    public partial class VisitRecord : ModelBase
    {
        public int VisitWay { get; set; }
        public int FollowLevel { get; set; }
        public int FollowStep { get; set; }
        public string Address { get; set; }

        /// <summary>
        /// 沟通细节
        /// </summary>
        [StringLength(400, ErrorMessage = "沟通细节不能超过400个字")]
        public string Detail { get; set; }

        public DateTime? VisitTime { get; set; }

        public string CustomerName { get; set; }

       /* public virtual Customer { get; set; }*/

        [Required(ErrorMessage = "请选择")]
        public int PriceResponse { get; set; }
        public int CognitiveChannel { get; set; }

    }

    /// <summary>
    /// 认知途径
    /// </summary>
    [Flags]
    public enum EnumCognitiveChannel
    {
        [EnumTitle("无", IsDisplay = false)]
        None = 0,

        [EnumTitle("电视")]
        TV = 1,

        [EnumTitle("短信")]
        Message = 2,

        [EnumTitle("报纸")]
        Paper = 4,

        [EnumTitle("现场")]
        Scene = 8,

        [EnumTitle("房展")]
        HosueExhibition = 16,

        [EnumTitle("DM直邮")]
        DMMail = 32,

        [EnumTitle("网络")]
        Internet = 64,

        [EnumTitle("杂志")]
        Magazine = 128,

        [EnumTitle("他人介绍")]
        OthersIntroduce = 256,

        [EnumTitle("户外广告")]
        OutdoorAdvertising = 512,

        [EnumTitle("电梯广告")]
        ElevatorAdvertising = 1024,

        [EnumTitle("夹报")]
        NewspaperClipping = 2048,

        [EnumTitle("过路客")]
        Passerby = 4096,

        [EnumTitle("其他媒体")]
        Others = 8192
    }

    /// <summary>
    /// 价格反应
    /// </summary>
    public enum EnumPriceResponse
    {
        [EnumTitle("无", IsDisplay = false)]
        None = 0,

        [EnumTitle("较低")]
        Inexpensive = 1,

        [EnumTitle("尚可接受")]
        Acceptable = 2,

        [EnumTitle("较贵")]
        Expensive = 3,

        [EnumTitle("很贵，不能接受")]
        VeryExpensive = 4
    }



    /// <summary>
    /// 访问方式
    /// </summary>
    public enum EnumVisitWay
    {
        [EnumTitle("来电")]
        Tel = 1,

        [EnumTitle("来访")]
        Visit = 2
    }

    /// <summary>
    /// 跟进级别
    /// </summary>
    public enum EnumFollowLevel
    {
        [EnumTitle("一般")]
        Normal = 1,

        [EnumTitle("进一步")]
        Further = 2,

        [EnumTitle("深入")]
        Thorough = 3
    }

    /// <summary>
    /// 跟进阶段
    /// </summary>
    public enum EnumFollowStep
    {
        [EnumTitle("意思平平")]
        Insipidity = 1,

        [EnumTitle("有可能性")]
        Potentially = 2,

        [EnumTitle("已确定")]
        Positive = 3
    }


}

