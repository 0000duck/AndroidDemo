using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data;
//using System.ComponentModel.DataAnnotations.Schema;
using HYZK.FrameWork.Common;
using System.Threading.Tasks;


namespace HYZK.FrameWork.DAL
{
    /// <summary>
    /// DAL基类，实现Repository通用泛型数据访问模式
    /// </summary>
    public class DbContextBase : DbContext, IDataRepository, IDisposable
    {
        public DbContextBase(string connectionString)
        {
            this.Database.Connection.ConnectionString = connectionString;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

    

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

     

        public override int SaveChanges()
        {
            var result = base.SaveChanges();
            return result;
        }

       
    }
}
