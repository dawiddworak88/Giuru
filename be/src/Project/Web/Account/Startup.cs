using Foundation.Localization.Definitions;
using Foundation.Localization.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Feature.Localization.DependencyInjection;
using Foundation.Database.Shared.DependencyInjection;
using Feature.Account.DependencyInjection;
using Account.Shared.DependencyInjection;
using Foundation.Extensions.DependencyInjection;

namespace Account
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

            services.RegisterLocalizationDependencies();

            services.RegisterDatabaseDependencies(this.Configuration);

            services.RegisterAccountDependencies(this.Configuration);

            services.RegisterGeneralDependencies();

            services.RegisterDependencies();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptionsMonitor<LocalizationConfiguration> localizationOptions)
        {
            app.UseForwardedHeaders();

            env.UseGeneralException(app);

            app.ConfigureDatabaseMigrations();

            app.UseResponseCompression();

            app.UseGeneralStaticFiles();

            app.UseRouting();

            app.UseAccountIdentityServer();

            app.UseAuthorization();

            app.UseRequestLocalizationWithRouteCultureProvider(localizationOptions.CurrentValue);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                            name: "localizedAreaRoute",
                            pattern: "{culture:" + LocalizationConstants.CultureRouteConstraint + "}/{area:exists=Accounts}/{controller=SignIn}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area:exists=Accounts}/{controller=SignIn}/{action=Index}/{id?}");
            });
        }
    }
}