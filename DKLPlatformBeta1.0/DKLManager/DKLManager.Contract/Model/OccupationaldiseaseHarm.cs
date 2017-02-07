using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKLManager.Contract.Model
{
    //职业病危害
    public class OccupationaldiseaseHarm:ModelBase
    {
        public override int ID { get; set; }
        public string Endanger { get; set; }
        public string Describe { get; set; }
        public string Disease { get; set; }         //可能产生的疾病
        public string Contraindication { get; set; }    //职业禁忌症
    }
}
