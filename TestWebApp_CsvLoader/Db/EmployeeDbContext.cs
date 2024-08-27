using EmployeeImportApp.Models;
using Microsoft.EntityFrameworkCore;


namespace EmployeeImportApp.Db
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
            : base(options)
        {
        }
        public DbSet<Employee>? Employees { get; set; }
    }
}
