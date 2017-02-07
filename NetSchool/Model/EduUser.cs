using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSchool.Model
{
    public partial class EduUser
    {
        public EduUser()
        {
            id = Guid.Empty;
            company = string.Empty;
            idcard = string.Empty;
            nickName = string.Empty;
            roles = string.Empty;
            gender = string.Empty;
        }
        private Guid id;
        private string nickName;
        private string idcard;
        private string company;
        private string roles;
        private string gender;
        private int createdTime;
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
        public string NickName
        {
            set { nickName = value; }
            get { return nickName; }
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
        public string Roles
        {
            set { roles = value; }
            get { return roles; }
        }
        public string Gender
        {
            set { gender = value; }
            get { return gender; }
        }
        public DateTime CreateTime
        {
            set { createTime = value; }
            get { return createTime; }
        }
    }
}
