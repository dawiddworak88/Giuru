using Foundation.Localization.Definitions;
using Foundation.Localization.Extensions;
using Seller.Web.Shared.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Foundation.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Foundation.PageContent.DependencyInjection;
using Foundation.Security.DependencyInjection;
using Seller.Web.Areas.Orders.DependencyInjection;
using Seller.Web.Areas.Clients.DependencyInjection;
using Seller.Web.Areas.Products.DependencyInjection;
using Foundation.ApiExtensions.DependencyInjection;
using Foundation.Localization.DependencyInjection;
using Foundation.Account.DependencyInjection;

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
            services.AddLocalizationConfiguration(this.Configuration);

            services.RegisterBaseLocalizationDependencies();

            services.AddLocalization();

            services.AddCultureRouteConstraint();

            services.AddControllersWithViews();

            services.RegisterClientAccountDependencies(this.Configuration);

            services.RegisterApiExtensionsDependencies();

            services.RegisterLocalizationDependencies();

            // services.RegisterDatabaseDependencies(this.Configuration);

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

            app.UseResponseCompression();

            app.UseGeneralStaticFiles();

            app.UseRouting();

            app.UseAuthenticationAuthorization();

            app.UseRequestLocalizationWithRouteCultureProvider(localizationOptions.CurrentValue);

            app.UseSecurityHeaders(this.Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                            name: "localizedAreaRoute",
                            pattern: "{culture:" + LocalizationConstants.CultureRouteConstraint + "}/{area:exists=Orders}/{controller=Order}/{action=Index}/{id?}").RequireAuthorization();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area:exists=Orders}/{controller=Order}/{action=Index}/{id?}").RequireAuthorization();
            });
        }
    }
}
