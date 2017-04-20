using HYZK.FrameWork.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BJWater.Contract.Model
{
    [Table("ControlerHistory")]
   public class ControlerHistory : ModelBase
    {
        [Key]
        public override int ID { get; set; }
        public double Temperature { get; set; }   ///温度
        public double humidity { get; set; }    //纬度
        public int eletritMacStatus { get; set; }    ///电机状态
    }
}
