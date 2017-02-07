using HYZK.FrameWork.Common;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;


namespace HYZK.Account.Contract
{
    [Auditable]
    [Table("User")]
    public partial class User : ModelBase
    {
        public User()
        {
            this.IsActive = true;
        }

        [Required(ErrorMessage = "登录名不能为空")]
        public string LoginName { get; set; }

        /// <summary>
        /// 密码，使用MD5加密
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        public string Password { get; set; }


        [Required(ErrorMessage = "账户类型不能为空")]
        public int AccountType { get; set; }


        [StringLength(100, ErrorMessage = "长度不能超过100")]
        [Required(ErrorMessage = "姓名不能为空")]
        public string Name { get; set; }

        [Required(ErrorMessage = "不能为空")]
        public int Gender { get; set; }

        public DateTime ?BirthDate { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [RegularExpression(@"^[1-9]{1}\d{10}$", ErrorMessage = "不是有效的手机号码")]
        public string Mobile { get; set; }

        [StringLength(50, ErrorMessage = "电话不能超过50个字")]
        public string Tel { get; set; }

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "电子邮件地址无效")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "地址不能超过100个字")]
        public string Address { get; set; }

        public int? BranchId { get; set; }

        public bool IsActive { get; set; }

        public bool Have { get; set; }
        [NotMapped]
        public string NewPassword { get; set; }
        public bool SecondStatus { get; set; } //副职业状态
        public string SecondTotally { get; set; }//副职业总计
        public int SecondAccountType { get; set; }//副职业
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum EnumGender
    {
        [EnumTitle("男")]
        Man = 1,

        [EnumTitle("女")]
        Woman = 2
    }

    public enum EnumAccountType
    {
        [EnumTitle("管理员")]
        Admin = 0,

        [EnumTitle("市场部")]
        Maket = 1,
        
        [EnumTitle("咨询部主管")]
        AdvisoryCharge = 2,

        [EnumTitle("授权人签字")]
        ThPerson = 3,

        [EnumTitle("质管部")]
        QualityControl = 4,

        [EnumTitle("检测主管")]
        EvaluationDetectManager = 5,

        [EnumTitle("水质主管审核")]
        SQPerson = 6,

        [EnumTitle("检测评价普通员工")]
        EvaluationDetectGeneral = 7,

        [EnumTitle("设备管理员")]
        EquipmentManager = 8,

        [EnumTitle("实验室主管编制部门")]
        EquipmentLaboratory=9,

        [EnumTitle("实验室水质专用")]
        EquipmentLaboratorys= -9,

        [EnumTitle("分析人员")]
        AnalyzePerson = 10,
        [EnumTitle("市场部主管")]
        Contract = 11,
        [EnumTitle("财务部")]
        Financial = 12,
        [EnumTitle("职评部主管")]
        CostingVicePresident = 13,
        [EnumTitle("质量监督员")]
        QualityInspector = 14,
        [EnumTitle("副总")]
        CostingDirector = 15,

        [EnumTitle("总经理")]
        Director = 16,
        [EnumTitle("技术负责人")]
        jishu = 17,
        [EnumTitle("水质采样员")]
        Waterc = 18,
        [EnumTitle("报告编制人")]
        Bg = 19,
        [EnumTitle("项目负责人")]
        JC = 20,
        [EnumTitle("评价方案审核人")]
        PJ = 21,
        [EnumTitle("评价技术负责人")]
        PJFZ = 22,
        //[EnumTitle("授权人签字")]
        //ThPerson = 23,


    }
}
