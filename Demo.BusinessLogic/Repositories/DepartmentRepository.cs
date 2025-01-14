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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly MVCAppDbContext dbContext;
        public DepartmentRepository(MVCAppDbContext dbContext) //Ask CLR for Object from DbContext (go to allow dependency injection in startup file)
        {
            this.dbContext = dbContext;
        }

        public int Add(Department department)
        {
            dbContext.Departments.Add(department);
            
            return dbContext.SaveChanges();
        }

        public int Delete(Department department)
        {
            dbContext.Departments.Remove(department);
            return dbContext.SaveChanges();
        }

        public IEnumerable<Department> GetAll()
        {
            return dbContext.Departments.ToList();
        }

        public Department GetById(int id)
        {
            //var dep = dbContext.Departments.Local.Where(d=>d.Id == id).FirstOrDefault();
            //if (dep is null) { 
            //    return dbContext.Departments.Where(d => d.Id == id).FirstOrDefault();
            //}
            //else
            //{
            //    return dep;
            //}

            return dbContext.Departments.Find(id);
        }

        public int Update(Department department)
        {
            dbContext.Departments.Update(department);
            return dbContext.SaveChanges();
        }
    }
}
