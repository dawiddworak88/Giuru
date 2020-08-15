using Media.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Media.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<MediaContext>();

            services.AddDbContext<MediaContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite()));
        }
    }
}
