using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSchool.Model
{
    public partial class EduLearning
    {
        public EduLearning() {
            id = Guid.Empty;
            company = string.Empty;
            idcard = string.Empty;
            territory = string.Empty;
        }
        private Guid id;
        private string company;
        private string idcard;
        private string territory;
        private int createdTime;
        private DateTime createTime;
        public Guid Id
        {
            set { id = value; }
            get { return id; }
        }
        public string Company {
            set { company = value; }
            get { return company; }
        }
        public string Territory
        {
            set { territory = value; }
            get { return territory; }
        }
        public string Idcard
        {
            set { idcard = value; }
            get { return idcard; }
        }
        public int CreatedTime
        {
            set { createdTime = value; }
            get { return createdTime; }
        }
        public DateTime CreateTime
        {
            set { createTime = value; }
            get { return createTime; }
        }
    }
}
