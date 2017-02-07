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
    public class ProjectContract:ModelBase
    {
        [Key]
        public override int ID { set; get; }
        public DateTime ContractDate  { get; set; }                             //签约日期
        public string Area { get; set; }                                        //区域
        [RegularExpression(@"^-?\d+\.?\d*$", ErrorMessage = "请填写数字")]
        public string Money { get; set; }                                       //合同额
        public string MarketPerson { get;set;}
        [RegularExpression(@"^-?\d+\.?\d*$", ErrorMessage = "请填写数字")]
        public string PayRatioFirst { get; set; }                                   //首次百分比     

        #region 甲方联系信息
        public string RepresentativeA { get; set; }                              //甲方法定代表人
        public string RepresentativeATel { get; set; }                          //甲方法定代表人电话
        public string FaxA { get; set; }                                        //甲方传真
        public string ContactPersonA { get; set; }                              //甲方联系人
        public string TelA { get; set; }                                        //甲方电话
        public string AddressA { get; set; }                                    //甲方地址
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "电子邮件地址无效")]
        public string EmailA { get; set; }                                      //甲方邮箱
        public string ZipCodeA { get; set; }                                    //甲方邮编
        public string AuthorizedPersonA { get; set; }                           //甲方授权人姓名
        public string AuthorizedPersonJobA { get; set; }                        //甲方授权人职务

        #endregion
        #region 乙方联系信息
        public string RepresentativeB { get; set; }                              //乙方法定代表人
        public string RepresentativeBTel { get; set; }                          //乙方法定代表人电话
        public string FaxB { get; set; }                                        //乙方传真
        public string ContactPersonB { get; set; }                              //乙方联系人
        public string TelB { get; set; }                                        //乙方电话
        public string AddressB { get; set; }                                    //乙方地址
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "电子邮件地址无效")]
        public string EmailB { get; set; }                                      //乙方邮箱
        public string ZipCodeB { get; set; }                                    //乙方邮编
        public string AuthorizedPersonB { get; set; }                           //乙方授权人姓名
        public string AuthorizedPersonJobB { get; set; }                        //乙方授权人职务
        #endregion
        #region 项目基本信息
        public string ProjectNumber { get; set; }
        public string ProjectName { get; set; }
        public string CompaneName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyContact { get; set; }
        public string ContactTel { get; set; }
        public string ZipCode { get; set; }
        public int ProjectStatus { get; set; }
        public int ProjectCategory { get; set; }
        public DateTime ProjectClosingDate { get; set; }
#endregion
        #region 审核状态
        public int WorkerStatus { get; set; }                                   //技术负责人
        public string WorkerStatusProblem { get; set; }                            //未通过条目
        public int GeneralAccountingDepartmentStatus { get; set; }                   //财务部
        public string GeneralAccountingDepartmentStatusProblem { get; set; }
        public int QualityStatus { get; set; }                                       //质管部
        public string QualityStatusProblem { get; set; }
        public int EstimateStatus { get; set; }                                     //评价部
        public string EstimateStatusProblem { get; set; }     
        public int TestStatus { get; set; }                                         //检测部
        public string TestStatusProblem { get; set; }   
        public int MarketStatus { get; set; }                                       //市场部    
        public string MarketStatusProblem { get; set; }
        public string Person { get; set; }
        public string ParameterName { get; set; }
        #endregion

    }
    public enum EnumContractType
    {
        [EnumTitle("职业病危害控制效果评价")]
        Normal = 0,

        [EnumTitle("职业病危害现状评价")]
        Personal = 1,

        [EnumTitle("职业病危害预评价")]
        Company = 2,

        [EnumTitle("工作场所职业危害因素检测与评价")]
        Test = 3,
      
    }
    public enum EnumContractTypeCheck1
    {
        [EnumTitle("工作场所职业危害因素检测与评价")]
        Normal = 0,
    }
    public enum EnumContractTypeCheck2
    {
        [EnumTitle("检测与评价委托协议书")]
        Normal = 1,  
    }
    public enum EnumContractTypeEvaluate
    {
        [EnumTitle("职业病危害控制效果评价")]
        Normal = 0,

        [EnumTitle("职业病危害现状评价")]
        Personal = 1,

        [EnumTitle("职业病危害预评价")]
        Company = 2,

      

    }
   
}
