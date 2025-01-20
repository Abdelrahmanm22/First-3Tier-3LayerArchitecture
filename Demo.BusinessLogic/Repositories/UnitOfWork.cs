using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.BusinessLogic.Interfaces;
using Demo.DataAccess.Contexts;

namespace Demo.BusinessLogic.Repositories
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly MVCAppDbContext _dbContext;

        public IDepartmentRepository DepartmentRepository { get; set; }
        public IEmployeeRepository EmployeeRepository { get; set; }
        public UnitOfWork(MVCAppDbContext dbContext) //Ask CLR for Object From DbContext
        {
            DepartmentRepository = new DepartmentRepository(dbContext);
            EmployeeRepository = new EmployeeRepository(dbContext);
            _dbContext = dbContext;
        }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose() // to close connection with database ==>> w kda el CLR hy3rf lwa7do enk 3ayez t close el connection w m4 m7tag t use it in controllers
        {
            _dbContext.Dispose();
        }
    }
}
