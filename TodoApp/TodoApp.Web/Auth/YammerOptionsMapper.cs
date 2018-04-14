using AspNet.Security.OAuth.Yammer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TodoApp.Data;

namespace TodoApp.Web.Auth
{
    public class YammerOptionsMapper : IOptionsMapper<YammerAuthenticationOptions>
    {
        private readonly IDataProtectionProvider dataProtection;

        public YammerOptionsMapper(IDataProtectionProvider dataProtection)
        {
            this.dataProtection = dataProtection;
        }

        public YammerAuthenticationOptions CreateOptions(string scheme, Tenant tenant)
        {
            if (tenant == null)
            {
                return null;
            }

            var options = new YammerAuthenticationOptions
            {
                SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme,
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
                    typeof(OAuthHandler<YammerAuthenticationOptions>).FullName, scheme, "v1");

                options.StateDataFormat = new PropertiesDataFormat(dataProtector);
            }

            return options;
        }
    }
}
