using Microsoft.EntityFrameworkCore;
using TodoApp.Data.Compatibility;

namespace TodoApp.Data.SupportedVersions.V101
{
    [SchemaBinder(101)]
    public class V102ModelBinder : IVersionedModelBinder
    {
        public void BindModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().Ignore(p => p.Assignee);
        }
    }
}
