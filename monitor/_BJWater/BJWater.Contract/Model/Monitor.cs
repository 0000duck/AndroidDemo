using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJWater.Contract.Model
{

    [Table("Monitor")]
   public class Monitor : ModelBase
    {
       [Key]
       public override int ID { get; set; }         
       public double Temperature { get; set; }   ///温度
       public double humidity { get; set; }    ///纬度
       public double current { get; set; }    ///电流

       public int fan1Status { get; set; }  ///风扇1状态
       public int fan2Status { get; set; }  ///风扇2状态
       public int fan3Status { get; set; }  ///风扇3状态
       public int fan4Status { get; set; }  ///风扇4状态
    }
    
}
