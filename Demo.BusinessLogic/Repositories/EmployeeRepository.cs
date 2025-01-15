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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MVCAppDbContext dbContext;
        public EmployeeRepository(MVCAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public int Add(Employee employee)
        {
            dbContext.Employees.Add(employee);
            return dbContext.SaveChanges();
        }

        public int Delete(Employee employee)
        {
            dbContext.Employees.Remove(employee);
            return dbContext.SaveChanges();
        }

        public IEnumerable<Employee> GetAll()
        {
            return dbContext.Employees.ToList();
        }

        public Employee GetById(int id)
        {
            return dbContext.Employees.Find(id);
        }

        public int Update(Employee employee)
        {
            dbContext.Employees.Update(employee);
            return dbContext.SaveChanges();
        }
    }
}
