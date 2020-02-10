using Microsoft.EntityFrameworkCore;
using TodoApp.Data.Compatibility;

namespace TodoApp.Data.SupportedVersions.V101
{
    [SchemaBinder(101)]
    public class V101ModelBinder : IVersionedModelBinder
    {
        public void BindModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().Ignore(e => e.IsComplete);
            modelBuilder.Entity<TodoItem>().Property(p => p.IsComplete).HasDefaultValue(false);
        }
    }
}
