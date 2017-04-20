using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJWater.Contract.Model
{
    public class PaikeUpload : ModelBase
    {
        [Key]
        public override int ID { get; set; }
        public string Uploadperson { get; set; }        ///上传人
        public int ids { get; set; }                 ///上传图片ID
        public string Address { get; set; }         ///上传地址      
        public string Latitude { get; set; }       ///经度  
        public string Longitude { get; set; }      ///纬度  

    }
}
