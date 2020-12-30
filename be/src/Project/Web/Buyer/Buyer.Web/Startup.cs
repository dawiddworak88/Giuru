using Foundation.Localization.Definitions;
using Foundation.Localization.Extensions;
using Buyer.Web.Shared.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Foundation.Extensions.DependencyInjection;
using Foundation.PageContent.DependencyInjection;
using Foundation.Security.DependencyInjection;
using Foundation.ApiExtensions.DependencyInjection;
using Foundation.Account.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.Extensions.Options;

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
            services.AddLocalization();

            services.AddCultureRouteConstraint();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                options.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
            });

            services.AddControllersWithViews();

            services.RegisterClientAccountDependencies(this.Configuration);

            services.RegisterLocalizationDependencies();

            services.RegisterGeneralDependencies();

            services.RegisterDependencies();

            services.RegisterApiExtensionsDependencies();

            services.ConfigureSettings(this.Configuration);
        }

        public void Configure(IApplicationBuilder app, IOptionsMonitor<LocalizationSettings> localizationSettings)
        {
            IdentityModelEventSource.ShowPII = true;

            app.UseForwardedHeaders();

            app.UseGeneralException();

            app.UseResponseCompression();

            app.UseGeneralStaticFiles();

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCustomRequestLocalizationProvider(localizationSettings);

            app.UseSecurityHeaders(this.Configuration);

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
