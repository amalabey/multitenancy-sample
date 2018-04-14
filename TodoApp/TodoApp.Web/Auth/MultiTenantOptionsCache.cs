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
    public class MultiTenantOptionsCache<TOptions> : IOptionsMonitorCache<TOptions> where TOptions : class, new()
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IPostConfigureOptions<TOptions> postConfig;
        private readonly IOptionsMapper<TOptions> mapper;
        private IDictionary<string, TOptions> cache;

        public MultiTenantOptionsCache(IHttpContextAccessor contextAccessor,
            IPostConfigureOptions<TOptions> postConfig,
            IOptionsMapper<TOptions> mapper)
        {
            this.contextAccessor = contextAccessor;
            this.postConfig = postConfig;
            this.mapper = mapper;
            this.cache = new Dictionary<string, TOptions>();
        }

        public void Clear()
        {
            this.cache.Clear();
        }

        public TOptions GetOrAdd(string name, Func<TOptions> createOptions)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            var key = this.GetKey(name);

            if (this.cache.ContainsKey(key))
            {
                return this.cache[key];
            }
            else
            {
                TOptions options = null;
                var tenant = this.contextAccessor.HttpContext.RequestServices.GetRequiredService<Tenant>();
                if (tenant != null)
                {
                    options = this.mapper.CreateOptions(name, tenant);
                    this.postConfig.PostConfigure(name, options);
                }
                else
                {
                    options = createOptions();
                }

                if (options == null)
                {
                    throw new Exception("Unable to create options for the current tenant");
                }

                this.cache.Add(key, options);
                return options;
            }
        }

        public bool TryAdd(string name, TOptions options)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            var key = this.GetKey(name);

            if (this.cache.ContainsKey(key))
            {
                return false;
            }
            else
            {
                this.cache.Add(key, options);
                return true;
            }
        }

        public bool TryRemove(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            var key = this.GetKey(name);

            if (this.cache.ContainsKey(key))
            {
                this.cache.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GetKey(string scheme)
        {
            // Use <scheme>@<tenant> if tenant context available
            var key = scheme.ToLowerInvariant();
            var tenant = this.contextAccessor.HttpContext.RequestServices.GetRequiredService<Tenant>();
            if (tenant != null)
            {
                key = $"{scheme}@{tenant.Name}".ToLowerInvariant();
            }

            return key;
        }
    }
}
