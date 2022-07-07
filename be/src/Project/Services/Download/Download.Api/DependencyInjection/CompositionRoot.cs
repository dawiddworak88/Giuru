using Download.Api.Infrastructure;
using Download.Api.Services.Categories;
using Download.Api.Services.Downloads;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Download.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDownloadApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<IDownloadsService, DownloadsService>();
        }

        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<DownloadContext>();
            services.AddDbContext<DownloadContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite()));
        }
    }
}
