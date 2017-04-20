using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HYZK.FrameWork.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BJWater.Contract.Model
{
    [Table("Controler")]
    public class Controler : ModelBase
    {
        [Key]
        public override int ID { get; set; }
        public double Temperature { get; set; }   ///温度
        public double humidity { get; set; }    ///湿度

        public int eletritMacStatus { get; set; }    ///电机状态
                                             
    }
}

