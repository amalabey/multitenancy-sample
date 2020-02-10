using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace TodoApp.Data.Compatibility
{
    public class VersionedModelBinderFactory
    {
        public IVersionedModelBinder CreateBinder(int schemaVersion)
        {
            IEnumerable<Type> versionedBinderTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IVersionedModelBinder).IsAssignableFrom(t) && !t.IsInterface);

            var versionBinderType = versionedBinderTypes
                .FirstOrDefault(t =>
                {
                    var schemaBinderAttrib = t.GetCustomAttribute<SchemaBinderAttribute>();
                    if(schemaBinderAttrib != null && schemaBinderAttrib.SchemaVersion == schemaVersion)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });

            if(versionBinderType != null)
            {
                var versionBinderInstance = (IVersionedModelBinder)Activator.CreateInstance(versionBinderType);
                return versionBinderInstance;
            }
            else
            {
                return null;
            }
        }
    }
}
