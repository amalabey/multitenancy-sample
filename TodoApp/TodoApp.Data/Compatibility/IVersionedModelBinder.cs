using Microsoft.EntityFrameworkCore;

namespace TodoApp.Data.Compatibility
{
    public interface IVersionedModelBinder
    {
        void BindModel(ModelBuilder modelBuilder);
    }
}
