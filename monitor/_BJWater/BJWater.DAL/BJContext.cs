using System.Data.Entity;
using BJWater.Contract.Model;
using HYZK.FrameWork.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Linq;




namespace BJWater.DAL
{
    public class BJContext : DbContext
    {                                                                  
        public BJContext()
            : base("name=Monitor")
        {  
            //Database.SetInitializer<BJContext>(null);  //   不对数据库改动默认使用这个        
            Database.SetInitializer<BJContext>(new DropCreateDatabaseAlways<BJContext>());   //生成新字段，新表用

            Configuration.AutoDetectChangesEnabled = true;
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;

        }
        public DbSet<Controler> Controler { get; set; }         //表名必须与类名相同 
        public DbSet<ControlerHistory> ControlerHistory { get; set; }
        public DbSet<Monitor> Monitor { get; set; }
        public DbSet<MonitorHistory> MonitorHistory { get; set; }
        public DbSet<Repair> Repair { get; set; }


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