using DownloadCenter.Api.Infrastructure;
using DownloadCenter.Api.Services.Categories;
using DownloadCenter.Api.Services.DownloadCenter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DownloadCenter.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDownloadCenterApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<IDownloadCenterService, DownloadCenterService>();
        }

        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<DownloadContext>();
            services.AddDbContext<DownloadContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite()));
        }
    }
}
