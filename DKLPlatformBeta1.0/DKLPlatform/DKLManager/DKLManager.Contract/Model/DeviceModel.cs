using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKLManager.Contract.Model
{
    [Serializable]
    [System.ComponentModel.DataAnnotations.Schema.Table("DeviceModel")]

    public class DeviceModel:ModelBase
    {
        [Key]
        public override int ID { set; get; }
        /// <summary>
        /// 仪器名称
        /// </summary>
        [Required(ErrorMessage = "不能为空")]

        public string DeivceName { get; set; }
        /// <summary>
        /// 自编号
        /// </summary>

        public string Number { get; set; }

        public string ModelNum { get; set; }
        public string Manufacturer { get; set; }
        public string SkillParameter { get; set; }

        public DateTime BuyTime { get; set; }

        public double Price { get; set; }
        public string State { get; set; }
        public DateTime CheckTime { get; set; }
        public DateTime LastCheckTime { get; set; }
        public DateTime CorrectTime { get; set; }
        public DateTime LastCorrectTime { get; set; }
        public string Remark { get; set; }
    }
}
   