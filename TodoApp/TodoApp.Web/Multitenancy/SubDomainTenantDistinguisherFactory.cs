using Dotnettency;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Data;

namespace TodoApp.Web.Multitenancy
{
    public class SubDomainTenantDistinguisherFactory : ITenantDistinguisherFactory<Tenant>
    {
        private readonly IHttpContextAccessor context;

        public SubDomainTenantDistinguisherFactory(IHttpContextAccessor httpContextAccessor)
        {
            this.context = httpContextAccessor;
        }

        public Task<TenantDistinguisher> IdentifyContext()
        {
            var hostName = this.context?.HttpContext?.Request?.Host.ToString();
            if (!String.IsNullOrEmpty(hostName))
            {
                var domainSegments = hostName.Split(".");
                if (domainSegments.Count() > 0)
                {
                    var distinguisher = new TenantDistinguisher(new Uri($"tenant://{domainSegments[0]}"));
                    return Task.FromResult(distinguisher);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new Exception("Unable to find the subdomain in the hostname to identify the tenant");
            }
        }
    }
}
