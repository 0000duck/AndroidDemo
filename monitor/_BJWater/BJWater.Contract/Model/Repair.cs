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
    [Table("Repair")]
    public class Repair : ModelBase
    {
        [Key]
        public override int ID { get; set; }

        public int closefan1 { get; set; }  ///风扇1状态
        public int closefan2 { get; set; }  ///风扇2状态
        public int closefan3 { get; set; }  ///风扇3状态
        public int closefan4 { get; set; }  ///风扇4状态
    }
}
