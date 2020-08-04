using Catalog.Api.v1.Areas.Clients.DependencyInjection;
using Catalog.Api.v1.Areas.Schemas.DependencyInjection;
using Catalog.Api.v1.Areas.Products.DependencyInjection;
using Foundation.Database.Shared.DependencyInjection;
using Foundation.GenericRepository.DependencyInjection;
using Foundation.Localization.DependencyInjection;
using Foundation.Mailing.DependencyInjection;
using Foundation.Schema.DependencyInjection;
using Foundation.Taxonomy.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using Foundation.Account.DependencyInjection;

namespace Api
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

            services.RegisteSchemaDependencies();

            services.RegisterTaxonomyDependencies();

            services.RegisterClientDependencies();

            services.RegisterProductDependencies();

            services.RegisterOrderDependencies();

            services.RegisterDatabaseDependencies(this.Configuration);

            services.RegisterMailingDependencies(this.Configuration);

            services.ConfigureGenericRepositoryOptions(this.Configuration);

            services.AddApiVersioning();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            IdentityModelEventSource.ShowPII = true;

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthenticationAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
