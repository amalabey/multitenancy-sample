using Microsoft.EntityFrameworkCore;
using TodoApp.Data.Compatibility;

namespace TodoApp.Data
{
    public class TodoDataContext : DbContext
    {
        private readonly Tenant _tenant;

        public TodoDataContext(Tenant tenant, DbContextOptions<TodoDataContext> options)
            : base(options)
        {
            _tenant = tenant;
        }

        public int SchemaVersion
        {
            get
            {
                return _tenant.SchemaVersion;
            }
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var modelBinderFactory = new VersionedModelBinderFactory();
            var modelBinder = modelBinderFactory.CreateBinder(_tenant.SchemaVersion);
            modelBinder.BindModel(modelBuilder);
        }
    }
}
