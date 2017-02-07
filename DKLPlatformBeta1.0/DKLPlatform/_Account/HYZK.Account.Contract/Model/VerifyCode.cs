using System;
using System.Linq;
using System.Collections.Generic;
using HYZK.FrameWork.Utility;
using HYZK.FrameWork.Common;
using System.ComponentModel.DataAnnotations.Schema;


namespace HYZK.Account.Contract
{
    [Serializable]
    [Table("VerifyCode")]
    public class VerifyCode : ModelBase
    {
        public Guid Guid { get; set; }
        public string VerifyText { get; set; }
    }

}



