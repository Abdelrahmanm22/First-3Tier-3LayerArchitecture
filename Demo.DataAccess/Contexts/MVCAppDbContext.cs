using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.DataAccess.Contexts
{
    public class MVCAppDbContext:DbContext
    {
        public MVCAppDbContext(DbContextOptions<MVCAppDbContext> options):base(options)
        {
             
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server = .; Database = MVCAppDb; Trusted_Connection = True");
        //}

    }
}
