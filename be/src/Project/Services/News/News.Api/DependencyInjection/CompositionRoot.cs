using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using News.Api.Infrastructure;
using News.Api.Services.Categories;
using News.Api.Services.News;

namespace News.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterNewsApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<INewsService, NewsService>();
        }

        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<NewsContext>();
            services.AddDbContext<NewsContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite()));
        }
    }
}
