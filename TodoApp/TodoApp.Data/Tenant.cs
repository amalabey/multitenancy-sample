using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp.Data
{
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subdomain { get; set; }
        public string TenantDbHost { get; set; }
        public string TenantDbName { get; set; }
        public string TenantDbUser { get; set; }
        public string TenantDbPassword { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
