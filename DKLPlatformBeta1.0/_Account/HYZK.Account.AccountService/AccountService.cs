using HYZK.Account.Contract;
using HYZK.Account.DAL;
using HYZK.Core.Cache;
using HYZK.Core.Config;
using HYZK.FrameWork.Common;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;

namespace HYZK.Account.BLL
{
    public class AccountService : IAccountService
    {
        private readonly int _UserLoginTimeoutMinutes = CachedConfigContext.Current.SystemConfig.UserLoginTimeoutMinutes;
        private readonly string _LoginInfoKeyFormat = "LoginInfo_{0}";


        public LoginInfo GetLoginInfo(Guid token)
        {
            return CacheHelper.Get<LoginInfo>(string.Format(_LoginInfoKeyFormat, token), () =>
            {
                using (var dbContext = new AccountDbContext())
                {
                    //如果有超时的，启动超时处理
                    var timeoutList = dbContext.FindAll<LoginInfo>(p =>System.Data.Entity.DbFunctions.DiffMinutes(DateTime.Now, p.LastAccessTime) > _UserLoginTimeoutMinutes);
                    if (timeoutList.Count > 0)
                    {
                        foreach (var li in timeoutList)
                            dbContext.LoginInfos.Remove(li);
                    }

                    dbContext.SaveChanges();


                    var loginInfo = dbContext.FindAll<LoginInfo>(l => l.LoginToken == token).FirstOrDefault();
                    if (loginInfo != null)
                    {
                        loginInfo.LastAccessTime = DateTime.Now;
                        dbContext.Update<LoginInfo>(loginInfo);
                    }

                    return loginInfo;
                }
            });
        }

        public LoginInfo Login(string loginName, string password)
        {
            LoginInfo loginInfo = null;

            //password = Encrypt.MD5(password);//密码加密
            loginName = loginName.Trim();

            using (var dbContext = new AccountDbContext())
            {
                var user = dbContext.Users.Where(u => u.LoginName == loginName && u.Password == password && u.IsActive).FirstOrDefault();
                if (user != null)
                {
                    var ip = Fetch.UserIp;
                    loginInfo = dbContext.FindAll<LoginInfo>(p => p.LoginName == loginName && p.ClientIP == ip).FirstOrDefault();
                    if (loginInfo != null)
                    {
                        loginInfo.LastAccessTime = DateTime.Now;
                    }
                    else
                    {
                        loginInfo = new LoginInfo(user.ID, user.LoginName);
                        loginInfo.ClientIP = ip;
                        dbContext.Insert<LoginInfo>(loginInfo);
                    }
                }
            }

            return loginInfo;
        }

        public void Logout(Guid token)
        {
            using (var dbContext = new AccountDbContext())
            {
                var loginInfo = dbContext.FindAll<LoginInfo>(l => l.LoginToken == token).FirstOrDefault();
                if (loginInfo != null)
                {
                    dbContext.Delete<LoginInfo>(loginInfo);
                }
            }

            CacheHelper.Remove(string.Format(_LoginInfoKeyFormat, token));
        }

        public void ModifyPwd(User user)
        {
            //user.Password = Encrypt.MD5(user.Password);

            using (var dbContext = new AccountDbContext())
            {
                if (dbContext.Users.Any(l => l.ID == user.ID && user.Password == l.Password))
                {
                    //if (!string.IsNullOrEmpty(user.NewPassword))
                    //    user.Password = Encrypt.MD5(user.NewPassword);
                    user.Password = user.NewPassword;
                    dbContext.Update<User>(user);
                }
                else
                {
                    throw new BusinessException("Password", "原密码不正确！");
                }
            }
        }

        public User GetUser(string loginName)
        {
            using (var dbContext = new AccountDbContext())
            {
                return dbContext.Users.Where(u => u.LoginName == loginName).SingleOrDefault();
            }
        }
        public User GetUserN(string Name)
        {
            using (var dbContext = new AccountDbContext())
            {
                return dbContext.Users.Where(u => u.Name == Name).SingleOrDefault();
            }
        }
        public User GetUser(int id)
        {
            using (var dbContext = new AccountDbContext())
            {
                return dbContext.Users.Where(u => u.ID == id).SingleOrDefault();
            }
        }

        public IEnumerable<User> GetUserList(UserRequest request = null)
        {
            request = (request == null) ?new UserRequest(): request ;

            using (var dbContext = new AccountDbContext())
            {
                IQueryable<User> users = dbContext.Users;

                if (!string.IsNullOrEmpty(request.LoginName))
                    users = users.Where(u => u.LoginName.Contains(request.LoginName));

                if (!string.IsNullOrEmpty(request.Mobile))
                    users = users.Where(u => u.Mobile.Contains(request.Mobile));

                return users.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public IEnumerable<User> GetUserListAu(int accountType)
        {
            using (var dbContext = new AccountDbContext())
            {
                return dbContext.FindAll<User>(u => u.AccountType == accountType || u.SecondTotally.Contains("5"));
            }
        }

        public IEnumerable<User> GetUserListT(int accountType)
        {
            using (var dbContext = new AccountDbContext())
            {
                return dbContext.FindAll<User>(u => u.AccountType == accountType || u.SecondTotally.Contains("3"));
            }
        }

        public IEnumerable<User> GetUserListG(int accountType)
        {
            using (var dbContext = new AccountDbContext())
            {
                return dbContext.FindAll<User>(u => u.AccountType == accountType||u.SecondTotally.Contains("5"));
            }
        }
        public IEnumerable<User> GetUserListF(int accountType)
        {
            using (var dbContext = new AccountDbContext())
            {
                return dbContext.FindAll<User>(u => u.AccountType == accountType||u.SecondTotally.Contains("9"));
            }
        }
        public IEnumerable<User> GetUserListV(int accountType)
        {
            using (var dbContext = new AccountDbContext())
            {
                return dbContext.FindAll<User>(u => u.AccountType == accountType);
            }
        }
        public IEnumerable<User> GetUserList(int accountType)
        {
            using (var dbContext = new AccountDbContext())
            {
                return dbContext.FindAll<User>(u => u.AccountType == accountType||u.SecondTotally.Contains("20"));                
            }
        }
        public IEnumerable<User> GetUserListZ(int accountType)
        {
            using (var dbContext = new AccountDbContext())
            {
                return dbContext.FindAll<User>(u => u.AccountType == accountType);
            }
        }
        public IEnumerable<User> GetUserListB(int accountType)
        {
            using (var dbContext = new AccountDbContext())
            {
                return dbContext.FindAll<User>(u => u.AccountType == accountType || u.SecondTotally.Contains("20"));
            }
        }
        public IEnumerable<User> GetUserListQ(int accountType)
        {
            using (var dbContext = new AccountDbContext())
            {
                return dbContext.FindAll<User>(u => u.AccountType == accountType || u.SecondTotally.Contains("5"));
            }
        }
        public IEnumerable<User> GetUserLists(int accountType)
        {
            using (var dbContext = new AccountDbContext())
            {
                return dbContext.FindAll<User>(u => u.AccountType == accountType || u.SecondTotally.Contains("9"));
            }
        }



        public void SaveUser(User user)
        {
            using (var dbContext = new AccountDbContext())
            {
                if (user.ID > 0)
                {
                    dbContext.Update<User>(user);
                }
                else
                {
                    var existUser = dbContext.FindAll<User>(u => u.LoginName == user.LoginName);
                    if (existUser.Count > 0)
                    {
                        throw new BusinessException("LoginName", "此登录名已存在！");
                    }
                    else
                    {
                        dbContext.Insert<User>(user);
                    }
                }
            }
        }

        public void DeleteUser(List<int> ids)
        {
            using (var dbContext = new AccountDbContext())
            {
                dbContext.Users.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.Users.Remove(a); });
                dbContext.SaveChanges();
            }
        }



        public Guid SaveVerifyCode(string verifyCodeText)
        {
            if (string.IsNullOrWhiteSpace(verifyCodeText))
                throw new BusinessException("verifyCode", "输入的验证码不能为空！");

            using (var dbContext = new AccountDbContext())
            {
                var verifyCode = new VerifyCode() { VerifyText = verifyCodeText, Guid = Guid.NewGuid() };
                dbContext.Insert<VerifyCode>(verifyCode);
                return verifyCode.Guid;
            }
        }

        public bool CheckVerifyCode(string verifyCodeText, Guid guid)
        {
            using (var dbContext = new AccountDbContext())
            {
                var verifyCode = dbContext.FindAll<VerifyCode>(v => v.Guid == guid && v.VerifyText == verifyCodeText).LastOrDefault();
                if (verifyCode != null)
                {
                    dbContext.VerifyCodes.Remove(verifyCode);
                    dbContext.SaveChanges();

                    //清除验证码大于2分钟还没请求的
                    var expiredTime = DateTime.Now.AddMinutes(-2);
                    foreach (var code in dbContext.VerifyCodes)
                    {
                        if (code.CreateTime < expiredTime)
                            dbContext.VerifyCodes.Remove(code);
                    }
                    //dbContext.VerifyCodes.Where(v => (v.CreateTime < expiredTime)).Delete();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
