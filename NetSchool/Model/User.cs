using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSchool.Model
{
    public partial class User
    {
        public User() {
            id = Guid.Empty;
            loginName = string.Empty;
            password = string.Empty;
            name = string.Empty;
            moblie = string.Empty;
            tel = string.Empty;
            email = string.Empty;
            address = string.Empty;
        }

        private Guid id;
        private string loginName;
        private string password;
        private int accountType;
        private string name;
        private int gender;
        private DateTime birthDate;
        private string moblie;
        private string tel;
        private string email;
        private string address;
        private int branchId;
        private int isActive;
        private DateTime createTime;
        public Guid Id
        {
            set { id = value; }
            get { return id; }
        }
        public string LoginName
        {
            get { return loginName; }
            set { loginName = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public int AccountType
        {
            get { return accountType; }
            set { accountType = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        public DateTime BirthDate
        {
            get { return birthDate; }
            set { birthDate = value; }
        }
        public string Moblie
        {
            get { return moblie; }
            set { moblie = value; }
        }
        public string Tel
        {
            get { return tel; }
            set { tel = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        public int BranchId
        {
            get { return branchId; }
            set { branchId = value; }
        }
        public int IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }
    }
}
