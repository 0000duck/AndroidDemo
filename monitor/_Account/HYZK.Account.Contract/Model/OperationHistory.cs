using HYZK.FrameWork.Common;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYZK.Account.Contract.Model
{
    [Auditable]
    [Table("OperationHistory")]
   public class OperationHistory : ModelBase
    {
        public override int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string LoginNames { get; set; }  //操作人 
        public int idc { get; set; }  //操作图片编号
        public int OperationType { get; set; }      
    }
}
