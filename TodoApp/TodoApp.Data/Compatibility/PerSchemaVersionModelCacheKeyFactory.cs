using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace TodoApp.Data.Compatibility
{
    public class PerSchemaVersionModelCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context)
        {
            return (context as TodoDataContext).SchemaVersion;
        }
    }
}
