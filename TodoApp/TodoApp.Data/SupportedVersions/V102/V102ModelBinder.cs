using Microsoft.EntityFrameworkCore;
using TodoApp.Data.Compatibility;

namespace TodoApp.Data.SupportedVersions
{
    [SchemaBinder(102)]
    public class V102ModelBinder : IVersionedModelBinder
    {
        public void BindModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().Ignore(p => p.StoryId);
        }
    }
}