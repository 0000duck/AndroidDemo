using System.ComponentModel.DataAnnotations;
using HYZK.FrameWork.Utility;
using HYZK.FrameWork.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Account.Contract
{
    [Auditable]
    [Table("Comments")]
    public class Comments : ModelBase
    {
        [Key]
        public override int ID { get; set; }
        public int paikeID { get; set; }        ///图片ID                    
        [Required]
        [StringLength(100)]                                                
        public string LoginName { get; set; }   ///登录名                                     
        [StringLength(100)]
        public string Comment { get; set; }     ///评论信息       
    }
    


}
