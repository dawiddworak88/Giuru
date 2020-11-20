using Catalog.Api.Configurations;
using Catalog.Api.Infrastructure;
using Catalog.Api.v1.Areas.Products.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Catalog.Api.DependencyInjection
{
    public static class ConfigurationRoot
    {
        public static void ConfigureDatabaseMigrations(this IApplicationBuilder app, IConfiguration configuration)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<CatalogContext>();

                if (!dbContext.AllMigrationsApplied())
                {
                    dbContext.Database.Migrate();
                    dbContext.EnsureSeeded(configuration);
                }
            }
        }

        public static void ConfigureSearchIndexing(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var productService = scope.ServiceProvider.GetService<IProductService>();

                if (productService.IsEmptyAsync().Result)
                {
                    productService.IndexAllAsync().RunSynchronously();
                }
            }
        }

        public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration);
        }
    }
}
