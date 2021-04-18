using EventLogging.Api.Infrastructure;
using EventLogging.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventLogging.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<EventLoggingContext>();

            services.AddDbContext<EventLoggingContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite()));
        }

        public static void RegisterEventLoggingDependencies (this IServiceCollection services)
        {
            services.AddScoped<IEventLoggingService, EventLoggingService>();
        }
    }
}
