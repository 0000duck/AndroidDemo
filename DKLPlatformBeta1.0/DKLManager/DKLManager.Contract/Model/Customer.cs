using System.ComponentModel.DataAnnotations;
using HYZK.FrameWork.Utility;
using HYZK.FrameWork.Common;
using System;

namespace DKLManager.Contract.Model
{
    [Serializable]
    [System.ComponentModel.DataAnnotations.Schema.Table("Customer")]
    public class CustomerModel : ModelBase
    {
        [StringLength(20, ErrorMessage = "客户名不能超过20个字")]
        [Required(ErrorMessage = "客户名不能为空")]
        public string CustomerName { get; set; }

        [StringLength(20, ErrorMessage = "电话不能超过20个字")]
        [Required(ErrorMessage = "电话不能为空")]
        public string Tel { get; set; }

        [StringLength(30)]

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "电子邮件地址无效")]
        public string Email { get; set; }
        /// <summary>
        /// 通讯地址
        /// </summary>
        [StringLength(50, ErrorMessage = "地址不能超过50个字")]
        public string Address { get; set; }
        /// <summary>
        /// 职业
        /// </summary>
        public int Profession { get; set; }
        public int Gender { get; set; }
    }

    public enum EnumProfession
    {
        [EnumTitle("无", IsDisplay = false)]
        None = 0,

        [EnumTitle("政府机关")]
        Government = 1,

        [EnumTitle("事业单位")]
        Institution = 2,

        [EnumTitle("金融业")]
        BankingBusiness = 3,

        [EnumTitle("个体私营")]
        PrivateEnterprises = 4,

        [EnumTitle("服务业")]
        ServicingBusiness = 5,

        [EnumTitle("广告传媒")]
        NewElement = 6,

        [EnumTitle("制造业")]
        Manufacturing = 7,

        [EnumTitle("运输业")]
        TransportService = 8,

        [EnumTitle("商贸")]
        Trade = 9,

        [EnumTitle("军警")]
        MilitaryPolice = 10,

        [EnumTitle("退休")]
        Retirement = 11,

        [EnumTitle("IT通讯业")]
        Komuniko = 12,

        [EnumTitle("医疗卫生教育")]
        MedicalTreatment = 13,

        [EnumTitle("房地产建筑业")]
        Realty = 14,

        [EnumTitle("其他职业")]
        Others = 15,
    }
    public enum EnumGender
    {
        [EnumTitle("男")]
        Male = 1,

        [EnumTitle("女")]
        Female = 2,
    }
}
