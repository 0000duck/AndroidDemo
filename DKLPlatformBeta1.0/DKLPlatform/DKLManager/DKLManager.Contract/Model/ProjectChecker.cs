using HYZK.FrameWork.Common;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKLManager.Contract.Model
{
    [Auditable]
    [Table("ProjectChecker")]
    public class ProjectChecker : ModelBase
    {
        [Required(ErrorMessage = "姓名不能为空")]
        public string Name { get; set; }
        public int ProjectCategory { get; set; }

        public int CheckLevel { get; set; }
    }

    public enum EnumCheckLevel
    {
        [EnumTitle("一审")]
        levelOne = 0,

        [EnumTitle("二审")]
        levelTwo = 1,

        [EnumTitle("三审")]
        levelThree =2,

        [EnumTitle("四审")]
        levelFour = 3,
    }

}
