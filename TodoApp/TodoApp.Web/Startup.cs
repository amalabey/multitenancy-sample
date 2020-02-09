using System;
using System.Data.SqlClient;
using Dotnettency;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Data;
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
                        .AddPerRequestContainerMiddlewareServices();
                    });
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMultitenancy<Tenant>((options) =>
            {
                options.UsePerTenantContainers();
            });

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

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
