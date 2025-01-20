using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BusinessLogic.Interfaces
{
    public interface IUnitOfWork
    {
        //Signature for property for each and every repositort interface
        IDepartmentRepository DepartmentRepository { get; set; }
        IEmployeeRepository EmployeeRepository { get; set; }
        Task<int> CompleteAsync();

        //void Dispose(); // to close connection with database
    }
}
