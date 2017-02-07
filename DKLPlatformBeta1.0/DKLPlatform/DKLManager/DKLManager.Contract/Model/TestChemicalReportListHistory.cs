using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKLManager.Contract.Model
{
    [Serializable]
    [System.ComponentModel.DataAnnotations.Schema.Table("TestChemicalReportListHistory")]
    public class TestChemicalReportListHistory : ModelBase
    {
        [Key]
        public override int ID { set; get; }
        [Required(ErrorMessage = "不能为空")]
        public string ProjectNumber { get; set; }

        public int paraCategory { get; set; } //化学还是物理因素，0，物理，1.化学
        public string TestContent { get; set; } //检测内容，检测因素名称
        public string WordShop { get; set; } //车间
        public string Job { get; set; } //岗位
        public string Location { get; set; } //测量地点
        public string SampleNumber { get; set; }//样品编号
        public string CSTEL { get; set; }    //短时间接触浓度
        public string CTWA { get; set; }        //检测结果
        public string CMAC { get; set; }        //检测结果
        public static TestChemicalReportListHistory Clone(TestChemicalReport info)
        {
            TestChemicalReportListHistory history = new TestChemicalReportListHistory();
            history.ProjectNumber = info.ProjectNumber;
            history.TestContent = info.SampleProject;
            history.WordShop = info.WorkShop;
            history.Job = info.Job;
            history.Location = info.Location;
            history.SampleNumber = info.SampleNumber;
            history.CMAC = info.CMAC;
            history.CTWA = info.CTWA;
            history.CMAC = info.CMAC;
            return history;
        }
    }
}
