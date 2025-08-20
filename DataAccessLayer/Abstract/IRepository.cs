using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAccessLayer.Abstract
{
    public interface IRepository<T> where T : class
    {
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        T Get(Expression<Func<T, bool>> filter);
        List<T> List();
        List<T> List(Expression<Func<T, bool>> filter);

    }
}
