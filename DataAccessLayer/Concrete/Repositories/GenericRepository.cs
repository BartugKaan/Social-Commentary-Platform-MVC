using DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        Context context = new Context();
        DbSet<T> _dbSet;


        public GenericRepository()
        {
            _dbSet = context.Set<T>();

        }


        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            context.SaveChanges();
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity);
            context.SaveChanges();
        }

        public List<T> List()
        {
            return _dbSet.ToList();
        }

        public List<T> List(Expression<Func<T, bool>> filter)
        {
            return _dbSet.Where(filter).ToList();
        }

        public void Update(T entity)
        {
            context.SaveChanges();
        }
    }
}
