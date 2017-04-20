using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HYZK.FrameWork.Common
{
    public class ModelBase
    {
        public ModelBase()
        {
            CreateTime = DateTime.Now;
            //ID = Guid.NewGuid();
        }
        
        public virtual int ID { get; set; }
        public virtual DateTime CreateTime { get; set; }

        public virtual string HttpMethod { get; set; }
        public virtual string Remark { get; set; }                  //备注 删除 等
    }
}
