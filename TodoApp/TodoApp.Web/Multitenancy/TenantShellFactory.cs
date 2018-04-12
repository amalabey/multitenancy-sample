using Dotnettency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Data;

namespace TodoApp.Web.Multitenancy
{
    public class TenantShellFactory : ITenantShellFactory<Tenant>
    {
        private readonly ConfigDbContext configDb;

        public TenantShellFactory(ConfigDbContext configDb)
        {
            this.configDb = configDb;
        }

        public Task<TenantShell<Tenant>> Get(TenantDistinguisher identifier)
        {
            var uri = identifier.Uri;
            if (uri.Scheme == "tenant")
            {
                var tenantSubdomain = uri.Host.ToLowerInvariant();
                var tenant = this.configDb.Tenants
                    .Where(t => t.Subdomain.ToLowerInvariant() == tenantSubdomain)
                    .FirstOrDefault<Tenant>();
                var tenantShell = new TenantShell<Tenant>(tenant);
                return Task.FromResult(tenantShell);
            }
            else
            {
                throw new Exception("Uri scheme is not 'tenant'. PathBasedTenantDistinguisher factory must be used with this tenant shell factory");
            }
        }
    }
}
