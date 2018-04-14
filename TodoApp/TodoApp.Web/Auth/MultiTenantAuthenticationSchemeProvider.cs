using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Data;

namespace TodoApp.Web.Auth
{
    public class MultiTenantAuthenticationSchemeProvider : AuthenticationSchemeProvider
    {
        private readonly IHttpContextAccessor contextAccessor;

        public MultiTenantAuthenticationSchemeProvider(IOptions<AuthenticationOptions> options,
            IHttpContextAccessor contextAccessor) : base(options)
        {
            this.contextAccessor = contextAccessor;
        }

        public override async Task<IEnumerable<AuthenticationScheme>> GetRequestHandlerSchemesAsync()
        {
            var tenant = await this.contextAccessor.HttpContext.RequestServices.GetRequiredService<Task<Tenant>>();
            var schemes = await base.GetRequestHandlerSchemesAsync();
            return schemes;
        }

    }
}
