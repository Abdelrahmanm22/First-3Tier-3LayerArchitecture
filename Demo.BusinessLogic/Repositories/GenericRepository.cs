using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.BusinessLogic.Interfaces;
using Demo.DataAccess.Contexts;

namespace Demo.BusinessLogic.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MVCAppDbContext dbContext;

        public GenericRepository(MVCAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public int Add(T entity)
        {
            dbContext.Add(entity);
            return dbContext.SaveChanges();
        }

        public int Delete(T entity)
        {
            dbContext.Remove(entity);
            return dbContext.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            return dbContext.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return dbContext.Set<T>().Find(id);
        }

        public int Update(T entity)
        {
            dbContext.Update(entity);
            return dbContext.SaveChanges();
        }
    }
}
