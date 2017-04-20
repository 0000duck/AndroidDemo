using Account.Contract;
using HYZK.Account.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYZK.Account.Contract
{
    public interface IAccountService
    {

        #region 用户增删改查
        void InsertUserInfo(User user);                                     //插入一条用户数据
        void UpdateUser(User model);                                        //更新用户信息
        void DeleteUser(List<int> ids);                                            //删除用户
        User SelectUser(int id);                                            //查找用户
        User SelectUserByLoginName(string LoginName);                       //根据用户名查找
        List<User> SelectAllUser();                                         //查找全部用户
        #endregion
       


        

        #region 操作记录Log
        void InsertLog(UserLog model);
        List<UserLog> SelectAllUserLog();
        void DeleteUserLog(List<int> ids);
        void UpdateUserLog(UserLog model);
        #endregion      
        
    }
}
