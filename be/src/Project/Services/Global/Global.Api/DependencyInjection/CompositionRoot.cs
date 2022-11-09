using Global.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Global.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<GlobalContext>();
            services.AddDbContext<GlobalContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite()));
        }
    }
}
