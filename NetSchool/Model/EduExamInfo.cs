using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSchool.Model
{
    public partial class EduExamInfo
    {
        public EduExamInfo() {
            id = Guid.Empty;
            company = string.Empty;
            idcard = string.Empty;
            territory = string.Empty;
            status = string.Empty;
        }
        private Guid id;
        private string company;
        private string idcard;
        private string territory;
        private int beginTime;
        private int endTime;
        private string status;
        private DateTime createTime;
        public Guid Id
        {
            set { id = value; }
            get { return id; }
        }
        public string Company
        {
            set { company = value; }
            get { return company; }
        }
        public string Idcard
        {
            set { idcard = value; }
            get { return idcard; }
        }
        public string Territory
        {
            set { territory = value; }
            get { return territory; }
        }
        public int BeginTime
        {
            set { beginTime = value; }
            get { return beginTime; }
        }
        public int EndTime
        {
            set { endTime = value; }
            get { return endTime; }
        }
        public string Status
        {
            set { status = value; }
            get { return status; }
        }
        public DateTime CreateTime
        {
            set { createTime = value; }
            get { return createTime; }
        }
    }
}
