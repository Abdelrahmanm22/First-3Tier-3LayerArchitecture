using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.BusinessLogic.Interfaces;
using Demo.DataAccess.Contexts;
using Demo.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.BusinessLogic.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MVCAppDbContext dbContext;

        public GenericRepository(MVCAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Add(T entity)
        {
            dbContext.Add(entity);
        }

        public void Delete(T entity)
        {
            dbContext.Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            ///da mosken l7d mnst5dm specification design pattern 
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) dbContext.Employees.Include(e=>e.Department).ToList();
            }
            return dbContext.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return dbContext.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
            dbContext.Update(entity);
        }
    }
}
