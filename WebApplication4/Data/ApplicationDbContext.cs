using ClassLibrary1;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<RegisterTable> RegisterTables { get; set; }
        public virtual DbSet<Table> Tables { get; set; }
        public DbSet<ClassLibrary1.Table1> Table1 { get; set; } = default!;
    }
}