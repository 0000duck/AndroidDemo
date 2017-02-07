using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYZK.Account.Contract
{
    public interface IAccountService
    {
        LoginInfo GetLoginInfo(Guid token);
        LoginInfo Login(string loginName, string password);
        void Logout(Guid token);
        void ModifyPwd(User user);


        User GetUser(string loginName);
        User GetUser(int id);
        IEnumerable<User> GetUserList(UserRequest request = null);
        void SaveUser(User user);
        void DeleteUser(List<int> ids);

        IEnumerable<User> GetUserList(int accountType);

        Guid SaveVerifyCode(string verifyCodeText);
        bool CheckVerifyCode(string verifyCodeText, Guid guid);


    }
}
