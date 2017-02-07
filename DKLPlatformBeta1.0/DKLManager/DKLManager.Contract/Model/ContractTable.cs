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
    [System.ComponentModel.DataAnnotations.Schema.Table("ContractTable")]
   public class ContractTable:ModelBase
    {
        [Key]
       public override int ID{ get; set; }
       public DateTime SigningTime { get; set; }
       public DateTime CompletionTime { get; set; }
       public string PaymentSchedule { get; set; }
       public string ContractMoney { get; set; }
       public DateTime AcceptTheDate { get; set; }
       public string County { get; set; }
       public int State { get; set; }
       public string Remark { get; set; }

    }
}
