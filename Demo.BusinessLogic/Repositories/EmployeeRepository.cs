using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.BusinessLogic.Interfaces;
using Demo.DataAccess.Contexts;
using Demo.DataAccess.Models;

namespace Demo.BusinessLogic.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MVCAppDbContext dbContext;

        public EmployeeRepository(MVCAppDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
            return dbContext.Employees.Where(d => d.Address == address);
        }
    }
}
