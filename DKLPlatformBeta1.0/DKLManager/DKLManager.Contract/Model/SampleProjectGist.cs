using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKLManager.Contract.Model
{
    /// <summary>
    /// 采样项目依据表
    /// </summary>
   public class SampleProjectGist:ModelBase
    {
       [Key]
       public override int ID { get; set; }
       public string SampleProject { get; set; }
       public string SampleGist { get; set; }
       public string ApparatusName { get; set; }
       public string ApparatusNumber { get; set; }
    }
}
