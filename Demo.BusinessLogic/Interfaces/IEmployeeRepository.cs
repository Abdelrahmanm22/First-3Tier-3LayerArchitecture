using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DataAccess.Models;

namespace Demo.BusinessLogic.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
        Employee GetById(int id);
        int Add(Employee employee);
        int Update(Employee employee);
        int Delete(Employee employee);
    }
}
