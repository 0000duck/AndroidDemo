using System.Data.Entity;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Linq;
using Account.Contract;
using HYZK.Account.Contract.Model;
using HYZK.FrameWork.Common;



namespace HYZK.Account.DAL
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext()
            : base("name=Account")
        {
            // Database.SetInitializer<AccountDbContext>(null);
             Database.SetInitializer<AccountDbContext>(new DropCreateDatabaseAlways<AccountDbContext>());

            Configuration.AutoDetectChangesEnabled = true;
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }


        public DbSet<User> User { get; set; }   
        public DbSet<UserLog> UserLog { get; set; }


        #region 增删改查基本操作定义
        public T Update<T>(T entity) where T : ModelBase
        {
            var set = this.Set<T>();
            set.Attach(entity);
            this.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
            this.SaveChanges();

            return entity;
        }

        public T Insert<T>(T entity) where T : ModelBase
        {
            this.Set<T>().Add(entity);
            this.SaveChanges();
            return entity;
        }

        public void Delete<T>(T entity) where T : ModelBase
        {
            this.Entry<T>(entity).State = System.Data.Entity.EntityState.Deleted;
            this.SaveChanges();
        }
        public override int SaveChanges()
        {

            var result = base.SaveChanges();
            return result;
        }
        public T Find<T>(params object[] keyValues) where T : ModelBase
        {
            return this.Set<T>().Find(keyValues);
        }

        public List<T> FindAll<T>(Expression<Func<T, bool>> conditions = null) where T : ModelBase
        {
            if (conditions == null)
                return this.Set<T>().ToList();
            else
                return this.Set<T>().Where(conditions).ToList();
        }


        #endregion


    }
}
