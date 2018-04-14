using AspNet.Security.OAuth.Yammer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TodoApp.Data;

namespace TodoApp.Web.Auth
{
    public class YammerOptionsFactory : IOptionsFactory<YammerAuthenticationOptions>
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IDataProtectionProvider dataProtection;

        public YammerOptionsFactory(IHttpContextAccessor contextAccessor, IDataProtectionProvider dataProtection)
        {
            this.contextAccessor = contextAccessor;
            this.dataProtection = dataProtection;
        }

        public YammerAuthenticationOptions Create(string name)
        {
            var tenant = contextAccessor.HttpContext.RequestServices.GetRequiredService<Tenant>();

            if (tenant != null)
            {
                var options = new YammerAuthenticationOptions
                {
                    SignInScheme = "Yammer",
                    ClientId = tenant.ClientId,
                    ClientSecret = tenant.ClientSecret
                };

                options.DataProtectionProvider = options.DataProtectionProvider ?? this.dataProtection;

                if (options.Backchannel == null)
                {
                    options.Backchannel = new HttpClient(options.BackchannelHttpHandler ?? new HttpClientHandler());
                    options.Backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("Microsoft ASP.NET Core OAuth handler");
                    options.Backchannel.Timeout = options.BackchannelTimeout;
                    options.Backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
                }

                if (options.StateDataFormat == null)
                {
                    var dataProtector = options.DataProtectionProvider.CreateProtector(
                        typeof(OAuthHandler<YammerAuthenticationOptions>).FullName, name, "v1");

                    options.StateDataFormat = new PropertiesDataFormat(dataProtector);
                }

                return options;
            }

            return null;
        }
    }
}
