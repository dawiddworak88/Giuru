using Foundation.Mailing.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using Foundation.Account.DependencyInjection;
using Catalog.Api.DependencyInjection;
using Foundation.Extensions.Filters;
using Microsoft.Extensions.Options;
using Foundation.Localization.Definitions;
using Foundation.Localization.Extensions;
using Foundation.Catalog.DependencyInjection;
using Foundation.EventLog.DependencyInjection;

namespace Catalog.Api
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

            services.RegisterApiAccountDependencies(this.Configuration);

            services.RegisterDatabaseDependencies(this.Configuration);

            services.RegisterCatalogBaseDependencies();

            services.RegisterCatalogApiDependencies();

            services.RegisterMailingDependencies(this.Configuration);

            services.AddApiVersioning();

            services.RegisterSearchDependencies(this.Configuration);

            services.RegisterEventBus(this.Configuration);

            services.RegisterEventLogDependencies();

            services.ConfigureSettings(this.Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IOptionsMonitor<LocalizationSettings> localizationSettings)
        {
            IdentityModelEventSource.ShowPII = true;

            app.UseSwagger();

            app.ConfigureDatabaseMigrations(this.Configuration);

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCustomHeaderRequestLocalizationProvider(this.Configuration, localizationSettings);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
