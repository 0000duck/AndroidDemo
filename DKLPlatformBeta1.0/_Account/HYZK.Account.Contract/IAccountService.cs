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

        User GetUserN(string Name);
        User GetUser(string loginName);
        User GetUser(int id);
        IEnumerable<User> GetUserList(UserRequest request = null);
        void SaveUser(User user);
        void DeleteUser(List<int> ids);

        IEnumerable<User> GetUserListAu(int accountType);
        IEnumerable<User> GetUserLists(int accountType);
        IEnumerable<User> GetUserListT(int accountType);
        IEnumerable<User> GetUserListQ(int accountType);
        IEnumerable<User> GetUserList(int accountType);
        IEnumerable<User> GetUserListV(int accountType);
        IEnumerable<User> GetUserListZ(int accountType);
        IEnumerable<User> GetUserListB(int accountType);
        IEnumerable<User> GetUserListG(int accountType);
        IEnumerable<User> GetUserListF(int accountType);

        Guid SaveVerifyCode(string verifyCodeText);
        bool CheckVerifyCode(string verifyCodeText, Guid guid);


    }
}
