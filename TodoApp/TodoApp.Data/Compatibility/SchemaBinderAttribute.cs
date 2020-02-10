using System;

namespace TodoApp.Data.Compatibility
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class SchemaBinderAttribute : System.Attribute
    {
        public SchemaBinderAttribute(int schemaVersion)
        {
            SchemaVersion = schemaVersion;
        }

        public int SchemaVersion { get; set; }
    }
}
