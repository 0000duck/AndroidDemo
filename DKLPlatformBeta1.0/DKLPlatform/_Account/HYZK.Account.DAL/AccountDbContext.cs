using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HYZK.FrameWork.DAL;
using HYZK.Core.Config;
using HYZK.Core.Log;
using System.Data.Entity;
using HYZK.Account.Contract;


namespace HYZK.Account.DAL
{
    public class AccountDbContext : DbContextBase
    {
        public AccountDbContext()
            : base(CachedConfigContext.Current.DaoConfig.Account, new LogDbContext())
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           Database.SetInitializer<AccountDbContext>(null);
           //  Database.SetInitializer<AccountDbContext>(new DropCreateDatabaseAlways<AccountDbContext>());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<LoginInfo> LoginInfos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VerifyCode> VerifyCodes { get; set; }
    }
}
