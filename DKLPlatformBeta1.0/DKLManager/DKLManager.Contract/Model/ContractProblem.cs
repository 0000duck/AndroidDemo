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
  public   class ContractProblem : ModelBase
    {
        public int WorkerStatus { get; set; }                                   //技术负责人
        public List<string> listWorker { get; set; }                            //未通过条目
      

        public int GeneralAccountingDepartmentStatus { get; set; }                   //财务部
        public List<string> listGeneralAccountingDepartment { get; set; }
        public int QualityStatus { get; set; }                                       //质管部
        public List<string> listQuality { get; set; }
       
        public int EstimateStatus { get; set; }                                     //评价部
        public List<string> listEstimate { get; set; }

        public int TestStatus { get; set; }                                         //检测部
        public List<string> listTest { get; set; }
    
        public int MarketStatus { get; set; }                                       //市场部    
        public List<string> listMarket { get; set; }
      
    }
  public enum EnumContractMarket
  {
      [EnumTitle("没问题")]
      Normal = 0,

      [EnumTitle("1.	技术服务报价是否符合有关收费规定和标准。")]
      Personal = 1,

      [EnumTitle("2.	评定合同的综合条款。")]
      Company = 2,
      [EnumTitle("3.    审核技术服务合同的整体合理性。")]
      test3 = 3,
     
  }
  public enum EnumContractTest
  {
      [EnumTitle("没问题")]
      Normal = 0,

      [EnumTitle("1.  审核技术服务合同的整体合理性。")]
      Personal = 1,

      [EnumTitle("2.  现有技术能力、资源是否能满足本项目的工作要求。")]
      Company = 2,
      [EnumTitle("3.  是否能在约定期限内完成。")]
      test3 = 3,
      [EnumTitle("4.  涉及的职业病危害因素检测及测量方法是否现行有效。")]
      test4 = 4,
      [EnumTitle("5.  现有仪器设备是否能满足本项目的检测需求。")]
      test5 = 5,
      [EnumTitle("6.  本项目所需实验用品是否能够及时采购。")]
      test6 = 6,
      [EnumTitle("7.  标准物质的采购是否满足要求。")]
      test7 = 7,
  }
  public enum EnumContractWorker
  {
      [EnumTitle("没问题")]
      Normal = 0,

      [EnumTitle("1．	技术要求、现有资源是否满足的要求。")]
      Personal = 1,

      [EnumTitle("2．	法律责任、保密和保护所有权是否明确。")]
      Company = 2,
     
  }
  public enum EnumContractQuality
  {
      [EnumTitle("没问题")]
      Normal = 0,

      [EnumTitle("1.	服务要求是否符合国家有关政策、法律及标准。")]
      Personal = 1,

      [EnumTitle("2.	服务范围、检测项目是否符合本公司资质范围。")]
      Company = 2,
      [EnumTitle("3.    检测项目及指标是否有需要分包的内容。")]
      test3 = 3,
    
  }
  public enum EnumContractFinancial
  {
      [EnumTitle("没问题")]
      Normal = 0,

      [EnumTitle("1．	合同付款信息是否正确。")]
      Personal = 1,

      [EnumTitle("2．	付款方式等和财务制度有关条款是否符合要求。")]
      Company = 2,
     

  }
  public enum EnumContractJob
  {
      [EnumTitle("没问题")]
      Normal = 0,

      [EnumTitle("1．	服务范围是否明确。")]
      Personal = 1,

      [EnumTitle("2．	 现有技术能力、资源是否能满足本项目的工作要求。")]
      Company = 2,
      [EnumTitle("3．	 是否能在约定期限内完成。")]
      test = 3,


  }
}
