using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using HYZK.FrameWork.Common;
using HYZK.FrameWork.Utility;


namespace HYZK.Account.Contract
{
    [Serializable]
    [Table("LoginInfo")]
    public class LoginInfo : ModelBase
    {
        public LoginInfo()
        {
            LastAccessTime = DateTime.Now;
            LoginToken = Guid.NewGuid();
        }

        public LoginInfo(int userID, string loginName)
        {
            LastAccessTime = DateTime.Now;
            LoginToken = Guid.NewGuid();

            UserID = userID;
            LoginName = loginName;
        }

        public Guid LoginToken { get; set; }
        public DateTime LastAccessTime { get; set; }
        public int UserID { get; set; }
        public string LoginName { get; set; }
        public string ClientIP { get; set; }

    }

}



