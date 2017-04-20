using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYZK.Account.Contract.Model
{
    [Table("UserLog")]
    public class UserLog : ModelBase
    {
        [Key]
        public override int ID { get; set; }  
                                        
        [Required]
        public DateTime Date { get; set; }               //日期
               
        [StringLength(200)]
        public string Description { get; set; }         //操作描述
            
        [StringLength(200)]
        public string UserName { get; set; }            //用户名
    }
}
