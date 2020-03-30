using Foundation.Localization.Definitions;
using Foundation.Localization.Extensions;
using Tenant.Portal.Shared.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Foundation.Database.Shared.DependencyInjection;
using Feature.Account.DependencyInjection;
using Foundation.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Feature.PageContent.DependencyInjection;
using Feature.Security.DependencyInjection;
using Tenant.Portal.Areas.Orders.DependencyInjection;
using Tenant.Portal.Areas.Clients.DependencyInjection;
using Tenant.Portal.Areas.Products.DependencyInjection;

namespace Tenant.Portal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalizationConfiguration(this.Configuration);

            services.AddLocalization();

            services.AddCultureRouteConstraint();

            services.AddHttpContextAccessor();

            services.AddControllersWithViews();

            services.RegisterClientAccountDependencies(this.Configuration);

            services.RegisterLocalizationDependencies();

            services.RegisterDatabaseDependencies(this.Configuration);

            services.RegisterGeneralDependencies();

            services.RegisterDependencies();

            services.RegisterOrdersAreaDependencies();

            services.RegisterClientsAreaDependencies();

            services.RegisterProductsAreaDependencies();

            services.ConfigureOptions(this.Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptionsMonitor<LocalizationConfiguration> localizationOptions)
        {
            app.UseForwardedHeaders();

            app.UseGeneralException();

            app.ConfigureDatabaseMigrations(this.Configuration);

            app.UseResponseCompression();

            app.UseGeneralStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseRequestLocalizationWithRouteCultureProvider(localizationOptions.CurrentValue);

            app.UseSecurityHeaders();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                            name: "localizedAreaRoute",
                            pattern: "{culture:" + LocalizationConstants.CultureRouteConstraint + "}/{area:exists=Orders}/{controller=Order}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area:exists=Orders}/{controller=Order}/{action=Index}/{id?}");
            });
        }
    }
}
