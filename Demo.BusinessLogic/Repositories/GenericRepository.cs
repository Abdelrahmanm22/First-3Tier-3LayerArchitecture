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
        public async Task AddAsync(T entity)
        {
            await dbContext.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            dbContext.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            ///da mosken l7d mnst5dm specification design pattern 
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>)await dbContext.Employees.Include(e=>e.Department).ToListAsync();
            }
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public void Update(T entity)
        {
            dbContext.Update(entity);
        }
    }
}
