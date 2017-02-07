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
    [System.ComponentModel.DataAnnotations.Schema.Table("ProjectInfo")]

    public class ProjectInfo : ModelBase
    {
        public ProjectInfo()
        {
            ProjectCategory = -1;
        }
        [Key]
        public override int ID { set; get; }
        [Required(ErrorMessage = "不能为空")]
        public string ProjectNumber { get; set; }
        [Required(ErrorMessage = "不能为空")]
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "不能为空")]
        public string CompaneName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyContact { get; set; }
        public string ContactTel { get; set; }
        public string ZipCode { get; set; }
        public int ProjectStatus { get; set; }
        public int ProjectCategory { get; set; }
        public DateTime ProjectClosingDate { get; set; }
        public string ProjectCheif { get; set; }//项目负责人
        public string ProjectLeader { get; set; }//项目组长
        public string ProjectPerson { get; set; }//组员
        public string Remarks { get; set; }//组员
        public int ProjectPersonCategory { get; set; }      //员工状态
       
    }
    public enum EnumProjectPersonCategory
    {
        [EnumTitle("待接收任务")]
        Uncheck = 0,

        [EnumTitle("收到任务，待执行")]
        Received = 1,

        [EnumTitle("执行任务")]
        Doing = 2,

        [EnumTitle("任务完成，即将提交")]
        Complete = 3,
        [EnumTitle("遇到问题，待解决")]
        Return = 4,
    }
   
    public enum EnumProjectAgree
    {
        [EnumTitle("同意")]
        Agree = 31,
        [EnumTitle("不同意")]
        DisAgree =30,
    }
    public enum EnumProjectAlarmStatus
    {
        [EnumTitle("警告")]
        Alert = 0,
        [EnumTitle("预警")]
        Warning = 1,
    }
    public enum EnumProjectCategory
    {
        [EnumTitle("咨询项目")]
        Consult = 0,

        [EnumTitle("评价项目")]
        Value = 1,

        [EnumTitle("检测项目")]
        Test = 2,

        [EnumTitle("监测评价项目", IsDisplay = false)]
        TestValue=3,
    }


    public enum EnumProjectSatus
    {
        [EnumTitle("项目开始")]
        Begin = 1,

        [EnumTitle("市场部提交")]
        MarketSubmit = 2,


        [EnumTitle("质管部提交")]//提交到项目主管
        QualityControlSubmit = 3,

        [EnumTitle("项目主管提交")]
        ProjectChargeSubmit = 5,

        [EnumTitle("项目组长提交")]
        ProjectLeaderSubmit = 6,

        [EnumTitle("项目进行中")]
        ProjectWorking = 7,

        [EnumTitle("项目报告已完成")]
        ProjectWordFinish = 8,

        [EnumTitle("检测组长提交")]
        ProjectVerifyOne = 9,

        [EnumTitle("检测评价一审退回")]
        ProjectModifyOne = 10,

        [EnumTitle("咨询部员工提交")]
        ProjectVerifyTwo = 11,

        [EnumTitle("检测评价二审退回")]
        ProjectModifyTwo = 12,

        [EnumTitle("项目三审后")]
        ProjectVerifyThree = 13,

        [EnumTitle("检测评价二审通过")]
        ProjectModifyThree = 14,

        [EnumTitle("检测评价三审退回")]
        ProjectVerifyFour = 15,

        [EnumTitle("检测员工提交")]
        ProjectModifyFour = 16,

        [EnumTitle("检测评价一审通过")]
        ProjectWorkFinish = 17,

        [EnumTitle("报告给质管部")]//wyq
        ProjectDocToZhiguan = 18,


        [EnumTitle("咨询部主管提交")]
        ConsultHasPersonInCharge = 21,

        [EnumTitle("咨询部审核通过")]
        ConsultWorking = 22,

        [EnumTitle("咨询部提交")]
        ConsultSubmit = 23,

        [EnumTitle("咨询部审核退回")]
        ConsultManagerReview = 24,

        [EnumTitle("检测评价三审提交")]
        ConsultModifyDone = 25,
        [EnumTitle("不存在")]
        ProjectNone = -1,
        [EnumTitle("一审，二审，三审", IsDisplay = false)]
        OneTwoThreeAssessor =30,
    }

}
