using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKLManager.Contract.Model
{
    public class UserInputList : ModelBase
    {
        [Key]
        public override int ID { get; set; }
        public string CompaneName { get; set; }                 //公司名称
        public string CompanyAddress { get; set; }              //公司地址
    }
    public enum EnmuUserInputType
    {
        All = 0,
        CompanyName = 1,
        CompanyAddress = 2,
    }
}
