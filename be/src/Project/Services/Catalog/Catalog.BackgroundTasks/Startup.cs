using Catalog.BackgroundTasks.DependencyInjection;
using Catalog.BackgroundTasks.IntegrationEvents;
using Foundation.Catalog.DependencyInjection;
using Foundation.EventBus.Abstractions;
using Foundation.EventLog.DependencyInjection;
using Foundation.Extensions.Filters;
using Foundation.Localization.Definitions;
using Foundation.Localization.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;

namespace Catalog.BackgroundTasks
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            }).AddNewtonsoftJson();

            services.AddLocalization();

            services.AddApiVersioning();

            services.RegisterDatabaseDependencies(this.Configuration);

            services.RegisterSearchDependencies(this.Configuration);

            services.RegisterEventBus(this.Configuration);

            services.RegisterEventLogDependencies();

            services.RegisterCatalogBaseDependencies();

            services.RegisterCatalogBackgroundTasksDependencies();
        }

        public void Configure(IApplicationBuilder app, IOptionsMonitor<LocalizationSettings> localizationSettings)
        {
            IdentityModelEventSource.ShowPII = true;

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCustomHeaderRequestLocalizationProvider(localizationSettings);

            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<RebuildCatalogSearchIndexIntegrationEvent, IIntegrationEventHandler<RebuildCatalogSearchIndexIntegrationEvent>>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
