using Foundation.Localization.Definitions;
using Foundation.Localization.Extensions;
using Seller.Web.Shared.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Foundation.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Foundation.PageContent.DependencyInjection;
using Foundation.Security.DependencyInjection;
using Seller.Web.Areas.Orders.DependencyInjection;
using Seller.Web.Areas.Clients.DependencyInjection;
using Seller.Web.Areas.Products.DependencyInjection;
using Foundation.ApiExtensions.DependencyInjection;
using Foundation.Account.DependencyInjection;
using Foundation.Extensions.Filters;
using Seller.Web.Areas.Media.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.Extensions.Options;

namespace Seller.Portal
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

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            }).AddNewtonsoftJson();

            services.RegisterClientAccountDependencies(this.Configuration);

            services.RegisterApiExtensionsDependencies();

            services.RegisterLocalizationDependencies();

            services.RegisterGeneralDependencies();

            services.RegisterDependencies();

            services.RegisterOrdersAreaDependencies();

            services.RegisterClientsAreaDependencies();

            services.RegisterProductsAreaDependencies();

            services.RegisterMediaAreaDependencies();

            services.ConfigureSettings(this.Configuration);
        }

        public void Configure(IApplicationBuilder app, IOptionsMonitor<LocalizationSettings> localizationSettings)
        {
            IdentityModelEventSource.ShowPII = true;

            app.UseForwardedHeaders();

            app.UseGeneralException();

            app.UseResponseCompression();

            app.UseGeneralStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCustomRouteRequestLocalizationProvider(localizationSettings);

            app.UseSecurityHeaders(this.Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                            name: "localizedAreaRoute",
                            pattern: "{culture:" + LocalizationConstants.CultureRouteConstraint + "}/{area:exists=Orders}/{controller=Orders}/{action=Index}/{id?}").RequireAuthorization();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area:exists=Orders}/{controller=Orders}/{action=Index}/{id?}").RequireAuthorization();
            });
        }
    }
}
