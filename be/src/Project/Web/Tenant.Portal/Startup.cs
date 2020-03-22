using Foundation.Localization.Definitions;
using Foundation.Localization.Extensions;
using Tenant.Portal.Shared.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Feature.Localization.DependencyInjection;
using Foundation.Database.Shared.DependencyInjection;
using Feature.Account.DependencyInjection;
using Foundation.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

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

            services.ConfigureOptions(this.Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptionsMonitor<LocalizationConfiguration> localizationOptions)
        {
            app.UseForwardedHeaders();

            env.UseGeneralException(app);

            app.ConfigureDatabaseMigrations();

            app.UseResponseCompression();

            app.UseGeneralStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseRequestLocalizationWithRouteCultureProvider(localizationOptions.CurrentValue);

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
