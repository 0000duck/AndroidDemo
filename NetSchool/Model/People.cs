using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSchool.Model
{
    public partial class People
    {
        public People()
        {
            userName = string.Empty;
            realName = string.Empty;
            telePhone = string.Empty;
            email = string.Empty;
            permission = string.Empty;
            password = string.Empty;
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        private string realName;
        public string RealName
        {
            get { return realName; }
            set { realName = value; }
        }
        private string telePhone;
        public string TelePhone
        {
            get { return telePhone; }
            set { telePhone = value; }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        private string permission;
        public string Permission
        {
            get { return permission; }
            set { permission = value; }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}
