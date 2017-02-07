using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSchool.Model
{
    public partial class VerifyCode
    {
        public VerifyCode() {
            id = Guid.Empty;
            _guid = Guid.Empty;
            verifyText = string.Empty;
        }
        private Guid id;
        private Guid _guid;
        private string verifyText;
        private DateTime createTime;
        public Guid Id
        {
            set { id = value; }
            get { return id; }
        }
        public Guid _Guid {
            set { _guid = value; }
            get { return _guid; }
        }
        public string VerifyText {
            set { verifyText = value; }
            get { return verifyText; }
        }
        public DateTime CreateTime {
            set { createTime = value; }
            get { return createTime; }
        }
    }
}
