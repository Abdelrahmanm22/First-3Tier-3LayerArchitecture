using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.DataAccess.Contexts
{
    internal class MVCAppDbContext:DbContext
    {
        public DbSet<Department> Departments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = .; Database = MVCAppDb; Trusted_Connection = True");
        }

    }
}
