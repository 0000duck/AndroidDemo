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
    [System.ComponentModel.DataAnnotations.Schema.Table("ArgumentValueHistory")]
   public  class ArgumentValueHistory:ModelBase
    {

        public override int ID { get; set; }
        public string Argument { get; set; }
        public string SampleRegisterNumber { get; set; }
        public string ArgumentPrice { get; set; }
        public string State { get; set; }
    }
}
