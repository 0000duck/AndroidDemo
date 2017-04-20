using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HYZK.FrameWork.Common;
using System.Linq.Expressions;

namespace HYZK.FrameWork.DAL
{
    public interface IDataRepository
    {
        T Update<T>(T entity) where T : ModelBase;
        T Insert<T>(T entity) where T : ModelBase;
        void Delete<T>(T entity) where T : ModelBase;
        T Find<T>(params object[] keyValues) where T : ModelBase;
        List<T> FindAll<T>(Expression<Func<T, bool>> conditions = null) where T : ModelBase;
       
    }
}
