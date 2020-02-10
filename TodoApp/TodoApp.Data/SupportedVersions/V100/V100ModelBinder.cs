using Microsoft.EntityFrameworkCore;
using TodoApp.Data.Compatibility;

namespace TodoApp.Data.SupportedVersions.V100
{
    [SchemaBinder(100)]
    public class V100ModelBinder : IVersionedModelBinder
    {
        public void BindModel(ModelBuilder modelBuilder)
        {
        }
    }
}
