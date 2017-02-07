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
    [System.ComponentModel.DataAnnotations.Schema.Table("Cookies")]
    public class Cookies : ModelBase
    {

        /// <summary>
        /// 样品登记表
        /// </summary>
        [Key]
        public override int ID { get; set; }
        public string ProjectNumber { get; set; }
       
        public string SampleNumber { get; set; }

        public string SampleQuantity { get; set; }
        public string SampleLetter { get; set; }


    }
  
}
