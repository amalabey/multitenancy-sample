using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Yammer;
using Dotnettency;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TodoApp.Data;
using TodoApp.Web.Auth;
using TodoApp.Web.Multitenancy;
using TodoApp.Web.Services;

namespace TodoApp.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Register config database
            var configDbConnection = Configuration["ConnectionStrings:ConfigDatabase"];
            services.AddDbContext<ConfigDbContext>(options => options.UseSqlServer(configDbConnection));

            // To override yammer settings per tenant
            //services.AddSingleton<IAuthenticationSchemeProvider, MultiTenantAuthenticationSchemeProvider>();
            //services.AddSingleton<IOptionsFactory<YammerAuthenticationOptions>, YammerOptionsFactory>();
            services.AddSingleton<IOptionsMapper<YammerAuthenticationOptions>, YammerOptionsMapper>();
            services.AddSingleton<IOptionsMonitorCache<YammerAuthenticationOptions>, MultiTenantOptionsCache<YammerAuthenticationOptions>>();

            // Add Yammer authentication
            services.AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie()
                .AddYammer("Yammer", options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.ClientId = "xxxxxx";
                    options.ClientSecret = "xxxxxxx";
                });

            return services.AddAspNetCoreMultiTenancy<Tenant>((multiTenantOptions) =>
            {
                multiTenantOptions
                    .DistinguishTenantsWith<SubDomainTenantDistinguisherFactory>()
                    .InitialiseTenant<TenantShellFactory>()
                    .ConfigureTenantContainers((containerBuilder) =>
                    {
                        containerBuilder
                        .WithStructureMap((tenant, tenantServices) =>
                        {
                            // Configure tenant specific services
                            var connectionBuilder = new SqlConnectionStringBuilder
                            {
                                DataSource = tenant.TenantDbHost,
                                InitialCatalog = tenant.TenantDbName,
                                UserID = tenant.TenantDbUser,
                                Password = tenant.TenantDbPassword
                            };
                            tenantServices.AddDbContext<TodoDataContext>(options => options.UseSqlServer(connectionBuilder.ToString()));

                            if(tenant.Subdomain == "contoso")
                            {
                                tenantServices.AddSingleton<IEditionProvider, EnterpriseEditionProvider>();
                            }
                            else
                            {
                                tenantServices.AddSingleton<IEditionProvider, StandardEditionProvider>();
                            }
                        })
                        .AddPerRequestContainerMiddlewareServices()
                        .AddPerTenantMiddlewarePipelineServices();
                    })
                    .ConfigureTenantMiddleware((configureOptions) =>
                    {
                        configureOptions.OnInitialiseTenantPipeline((tenantContext, appBuilder) =>
                        {
                            appBuilder.UseAuthentication();

                            var tenant = tenantContext.Tenant;
                            if(tenant.Subdomain == "northwind")
                            {
                                appBuilder.UseWelcomePage();
                            }
                        });
                    });
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseMultitenancy<Tenant>((options) =>
            {
                options.UsePerTenantContainers();
                options.UsePerTenantMiddlewarePipeline();
            });

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=TodoItems}/{action=Index}/{id?}");
            });
        }
    }
}
