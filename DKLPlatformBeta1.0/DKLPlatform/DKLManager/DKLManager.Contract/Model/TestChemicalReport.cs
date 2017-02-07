using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DKLManager.Contract.Model
{
    public class TestChemicalReport : ModelBase
    {
        [Key]
        public override int ID { get; set; }
        public string SampleNumber { get; set; } //样品编号
        public string ProjectNumber { get; set; } //项目编号
        public string ProjectName { get; set; }   //项目名称
        public string SampleProject { get; set; }  //采样项目名称
        public string WorkShop { get; set; }    //  车间
        public string Job { get; set; }         //职位
        public string Location { get; set; }    //地点
        public string Factor { get; set; }     //因素
        public string CSTEL { get; set; }    //短时间接触浓度
        public string CTWA { get; set; }        //检测结果
        public string CMAC { get; set; }        //检测结果
        public string Multiple { get; set; }    //超限倍数
        public string ResultVerdict { get; set; } //结果判定


    }
}
