using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;

namespace HYZK.Account.Contract
{
    public class UserRequest : Request
    {
        public string LoginName { get; set; }
        public string Mobile { get; set; }
    }

    public class RoleRequest : Request
    {
        public string RoleName { get; set; }
    }
}
