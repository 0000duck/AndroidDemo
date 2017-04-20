using HYZK.Account.BLL;
using HYZK.Account.Contract;
using BJWater.Contract;
using System;
using System.Web.Http;
using BJWater.BLL;
using HYZK.Account.Contract.Model;

namespace bjwaterAPI.Areas.EduInfo.Controllers
{
    public class BaseController : ApiController
    {

        public IAccountService IAccountService = new AccountService();
        public IHSManager IHSService = new Service();
        public Guid token;
        
        public BaseController(Guid Token)
        {
            this.token = Token;
        }
        public BaseController()
        {

        }

	}
}