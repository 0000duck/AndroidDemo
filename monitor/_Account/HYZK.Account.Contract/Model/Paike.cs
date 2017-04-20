using System.ComponentModel.DataAnnotations;
using HYZK.FrameWork.Utility;
using System.ComponentModel.DataAnnotations.Schema;
using HYZK.FrameWork.Common;

namespace Account.Contract
{
    [Auditable]
    [Table("Paike")]

    public class Paike : ModelBase
    {
        [Key]
        public override int ID { get; set; }     
        [Required]
        public string imgUrls { get; set; }       ///图片地址

        [StringLength(100)]
        public string userName { get; set; }     ///上传人名

        [StringLength(100)]
        public string address { get; set; }       ///上传地址
        public string Latitude { get; set; }       ///纬度  
        public string Longitude { get; set; }      ///经度
        public int Gender { get; set; }       //性别
        public int star { get; set; }
        public int StarStatus { get; set; }      ///点赞状态
        public enum EnumStarStatus
        {
            [EnumTitle("未点赞")]
            YStar = 0,

            [EnumTitle("已点赞")]
            NStar = 1,
        }        
    }
    


}
