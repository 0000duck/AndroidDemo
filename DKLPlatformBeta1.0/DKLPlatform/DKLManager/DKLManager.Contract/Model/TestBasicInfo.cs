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
    [System.ComponentModel.DataAnnotations.Schema.Table("TestBasicInfo")]

    public class TestBasicInfo : ModelBase
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
        public string TouchTime { get; set; }//接触时间
        public string NoiseStrength { get; set; }//噪声强度
        public string Lex8hLexw { get; set; } //专业术语

            

    }
}
