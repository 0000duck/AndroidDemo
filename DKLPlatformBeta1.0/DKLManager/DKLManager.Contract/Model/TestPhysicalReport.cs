using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKLManager.Contract.Model
{
    public class TestPhysicalReport : ModelBase
    {
        [Key]
        public override int ID { get; set; }
        public string TestResult { get; set; } //测量结果
        public string TestCompany { get; set; } //检测单位
        public string ProjectNumber { get; set; }   //项目编号
        public string TestContent { get; set; } //检测内容
        public string WordShop { get; set; } //车间
        public string Job { get; set; } //岗位
        public string Location { get; set; } //测量地点
        public string SampleNumber { get; set; }//样品编号
        public string ContactTime { get; set; } //接触时间
        public string NoiseIntensity { get; set; } //噪音强度
        public string Lex8hLexw { get; set; } //专业术语
        public string ResultVerdict { get; set; } //结果判定
        public int LexCategory { get; set; }    //LEX类别
 

    }
}
