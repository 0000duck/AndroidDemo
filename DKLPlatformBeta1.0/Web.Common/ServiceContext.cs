using System;
using System.Collections.Generic;
using HYZK.Core.Cache;
using HYZK.Core.Service;
using HYZK.Account.Contract;
using DKLManager.Contract;



namespace Web.Common
{
    public class ServiceContext
    {
        public static ServiceContext Current
        {
            get
            {
                return CacheHelper.GetItem<ServiceContext>("ServiceContext", () => new ServiceContext());
            }
        }


        public IAccountService AccountService
        {
            get
            {
                return ServiceHelper.CreateService<IAccountService>();
            }
        }
        public IDKLManager DKLManagerService
        {
            get
            {
                return ServiceHelper.CreateService<IDKLManager>();

            }
        }

    }
}
