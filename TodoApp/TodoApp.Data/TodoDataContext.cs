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
            modelBuilder.Entity("TodoApp.Data.TodoItem", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();
                b.Property<string>("Description");
                b.Property<string>("Assignee");
                b.Property<bool>("IsComplete");
                b.HasKey("Id");
                b.ToTable("TodoItems");
            });

            // To support backwards compatibility
            var versionModelBinder = _versionedModelBinderFactory.CreateBinder(_tenant.SchemaVersion);
            if(versionModelBinder != null)
            {
                versionModelBinder.BindModel(modelBuilder);
            }
        }
    }
}
