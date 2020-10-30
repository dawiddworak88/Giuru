using System;
using System.IO;
using System.Reflection;
using Foundation.Account.DependencyInjection;
using Foundation.Extensions.DependencyInjection;
using Foundation.GenericRepository.DependencyInjection;
using Foundation.Localization.DependencyInjection;
using Media.Api.DependencyInjection;
using Media.Api.Shared.Checksums;
using Media.Api.v1.Area.Media.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            services.RegisterBaseLocalizationDependencies();

            services.RegisterApiAccountDependencies(this.Configuration);

            services.RegisterBaseAccountDependencies();

            services.RegisterDatabaseDependencies(this.Configuration);

            services.RegisterMediaDependencies(this.Configuration);

            services.ConfigureGenericRepositoryOptions(this.Configuration);

            services.ConfigureOptions(this.Configuration);

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IChecksumService checksumService)
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

            app.UseAuthenticationAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
