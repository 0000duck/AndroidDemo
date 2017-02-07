using HYZK.Core.Cache;
using System;
using Web.Common;



namespace Web.Demo.Common
{
    public class AdminUserContext : UserContext
    {
        public AdminUserContext()
            : base(AdminCookieContext.Current)
        {
        }

        public AdminUserContext(IAuthCookie authCookie)
            : base(authCookie)
        {
        }

        public static AdminUserContext Current
        {
            get
            {
                return CacheHelper.GetItem<AdminUserContext>();
            }
        }
    }
}
