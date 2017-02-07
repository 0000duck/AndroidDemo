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
    public class SampleHistory : ModelBase
    {
         [Key]        
         public override int ID { get; set; }
         public string SampleRegisterNumber { get; set; }
         public DateTime SamplingDay { get; set; }
         public string SampleName { get; set; }
         public string SampleState { get; set;}
         public int SampleNumBer { get; set; }
         public string SaveCondition { get; set; }
         public string Remark { get; set; } 
    
    }
}
