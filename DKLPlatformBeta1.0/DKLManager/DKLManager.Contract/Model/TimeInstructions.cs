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
    [System.ComponentModel.DataAnnotations.Schema.Table("TimeInstructions")]
    public class TimeInstructions : ModelBase
    {
       public override int ID { get; set; }
       public string ProjectNumBer { get; set; }
       //时间节点
       public DateTime TimeNode { get; set; }
       //说明
       public string Instructions { get; set; }

       public string SignTime { get; set; }

       public string ProjectName { get; set; }

        //成本分析表ID
       public string costingID { get; set; }
    }
}
