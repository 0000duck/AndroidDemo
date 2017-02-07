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
   
    public class Areas : ModelBase
    {

        /// <summary>
        /// 区县表
        /// </summary>
        [Key]
        public override int ID { get; set; }
        public string Area { get; set; }
        public string CompaneName { get; set; }                 //公司名称
        public string CompanyAddress { get; set; }              //公司地址
        public string ParameterName { get; set; }                   //检测参数
        public string PhysicsParameterName { get; set; }        //物理检测参数
        public string DeviceName { get; set; }                  //设备编号

    }
    public enum EnmuUserInputType
    {
        All = 0,
        CompanyName = 1,
        CompanyAddress = 2,
        ParameterName = 3,
        PhysicsParameterName =4,
        DeviceName=5,
    }

}
