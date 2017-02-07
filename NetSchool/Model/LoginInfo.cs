using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSchool.Model
{
    public partial class LoginInfo
    {
        public LoginInfo() {
            id = Guid.Empty;
            loginToken = Guid.Empty;
            loginName = string.Empty;
            clientIP = string.Empty;
        }
        private Guid id;
        private Guid loginToken;
        private DateTime lastAccessTime;
        private int userId;
        private string loginName;
        private string clientIP;
        private DateTime createTime;
        public Guid Id
        {
            set { id = value; }
            get { return id; }
        }
        public Guid LoginToken {
            set { loginToken = value; }
            get { return loginToken; }
        }
        public DateTime LastAccessTime {
            set { lastAccessTime = value; }
            get { return lastAccessTime; }
        }
        public int UserId {
            set { userId = value; }
            get { return userId; }
        }
        public string LoginName {
            set { loginName = value; }
            get { return loginName; }
        }
        public string ClientIP {
            set { clientIP = value; }
            get { return clientIP; }
        }
        public DateTime CreateTime {
            set { createTime = value; }
            get { return createTime; }
        }
    }
}
