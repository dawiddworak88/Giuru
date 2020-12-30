using System;
using System.IO;
using System.Reflection;
using Foundation.Account.DependencyInjection;
using Foundation.Extensions.DependencyInjection;
using Foundation.Localization.Definitions;
using Foundation.Localization.Extensions;
using Media.Api.DependencyInjection;
using Media.Api.Shared.Checksums;
using Media.Api.v1.Area.Media.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;

namespace Media.Api
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
            services.AddControllers();

            services.AddLocalization();

            services.RegisterApiAccountDependencies(this.Configuration);

            services.RegisterDatabaseDependencies(this.Configuration);

            services.RegisterMediaDependencies(this.Configuration);

            services.ConfigureSettings(this.Configuration);

            services.AddApiVersioning();

            services.RegisterGeneralDependencies();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Media API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IOptionsMonitor<LocalizationSettings> localizationSettings, IChecksumService checksumService)
        {
            IdentityModelEventSource.ShowPII = true;

            app.UseSwagger();

            app.ConfigureDatabaseMigrations(this.Configuration, checksumService);

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Media API");
                c.RoutePrefix = string.Empty;
            });

            app.UseResponseCompression();

            app.UseGeneralStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCustomRequestLocalizationProvider(localizationSettings);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
