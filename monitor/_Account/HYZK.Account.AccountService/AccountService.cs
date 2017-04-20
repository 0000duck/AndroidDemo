using HYZK.Account.Contract;
using HYZK.Account.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using Account.Contract;
using HYZK.Account.Contract.Model;
using System.Net;

namespace HYZK.Account.BLL
{
    public class AccountService : IAccountService
    {
        public List<User> SelectAllUser()
        {
            try
            {
                using (var context = new AccountDbContext())
                {
                    return context.User.OrderByDescending(u => u.ID).ToList();
                }
            }
            catch
            {
                return null;
            }
        }
        
        public User SelectUser(int id)
        {
            try
            {
                using (var context = new AccountDbContext())
                {
                    return context.User.Where(u => u.ID == id).FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }

        public User SelectUserByLoginName(string LoginName)
        {
            try
            {
                using (var context = new AccountDbContext())
                {
                    return context.User.Where(u => u.LoginName == LoginName).FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }

        }
        public void DeleteUser(List<int> ids)
        {
            try
            {
                using (var context = new AccountDbContext())
                {
                    context.User.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { context.User.Remove(a); });
                    context.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }
        public void UpdateUser(User model)
        {
            try
            {
                using (var context = new AccountDbContext())
                {
                    context.Update<User>(model);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public void InsertUserInfo(User user)
        {
            try
            {
                using (var context = new AccountDbContext())
                {
                    context.Insert<User>(user);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }
        


        #region Log
        public List<UserLog> SelectAllUserLog()
        {
            try
            {
                using (var dbContext = new AccountDbContext())
                {
                    return dbContext.UserLog.OrderBy(u => u.ID).ToList();
                }
            }
            catch (Exception )
            {
                
                return null;

            }

        }

        public void InsertLog(UserLog model)
        {
            try
            {
                using (var context = new AccountDbContext())
                {

                    context.Insert<UserLog>(model);
                }
            }
            catch (Exception )
            {                
            }
        }
        public void DeleteUserLog(List<int> ids)
        {
            try
            {
                using (var context = new AccountDbContext())
                {
                    context.UserLog.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { context.UserLog.Remove(a); });
                    context.SaveChanges();
                }
            }
            catch (Exception exception)
            {               
                throw exception;
            }

        }

        public void UpdateUserLog(UserLog model)
        {
            try
            {
                using (var context = new AccountDbContext())
                {
                    context.Update<UserLog>(model);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion      
    }
}
