using Microsoft.EntityFrameworkCore;
using TodoApp.Data.Compatibility;

namespace TodoApp.Data
{
    public class TodoDataContext : DbContext
    {
        private readonly Tenant _tenant;
        private readonly VersionedModelBinderFactory _versionedModelBinderFactory;

        public TodoDataContext(Tenant tenant, 
            VersionedModelBinderFactory versionedModelBinderFactory,
            DbContextOptions<TodoDataContext> options)
            : base(options)
        {
            _versionedModelBinderFactory = versionedModelBinderFactory;
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
            var modelBinder = _versionedModelBinderFactory.CreateBinder(_tenant.SchemaVersion);
            modelBinder.BindModel(modelBuilder);
        }
    }
}
