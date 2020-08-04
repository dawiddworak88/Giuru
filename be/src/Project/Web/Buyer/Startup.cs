using Foundation.Localization.Definitions;
using Foundation.Localization.Extensions;
using Buyer.Web.Shared.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Foundation.Database.Shared.DependencyInjection;
using Foundation.Extensions.DependencyInjection;
using Foundation.PageContent.DependencyInjection;
using Foundation.Security.DependencyInjection;

namespace AspNetCore
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

            services.RegisterGeneralDependencies();

            services.RegisterDependencies();

            services.ConfigureOptions(this.Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptionsMonitor<LocalizationConfiguration> localizationOptions)
        {
            app.UseForwardedHeaders();

            app.UseGeneralException();

            app.UseResponseCompression();

            app.UseGeneralStaticFiles();

            app.UseRouting();

            app.UseRequestLocalizationWithRouteCultureProvider(localizationOptions.CurrentValue);

            app.UseSecurityHeaders();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                            name: "localizedAreaRoute",
                            pattern: "{culture:" + LocalizationConstants.CultureRouteConstraint + "}/{area:exists=Home}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area:exists=Home}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
