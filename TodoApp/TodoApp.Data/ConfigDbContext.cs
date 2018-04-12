using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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
