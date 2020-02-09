using Microsoft.EntityFrameworkCore;

namespace TodoApp.Data
{
    public class ConfigDbContext: DbContext
    {
        public ConfigDbContext(DbContextOptions<ConfigDbContext> options)
            :base(options)
        { }

        public DbSet<Tenant> Tenants { get; set; }
    }
}
