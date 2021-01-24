using EventLogging.Api.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventLogging.Api.DependencyInjection
{
    public static class ConfigurationRoot
    {
        public static void ConfigureDatabaseMigrations(this IApplicationBuilder app, IConfiguration configuration)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<EventLoggingContext>();

                if (!dbContext.AllMigrationsApplied())
                {
                    dbContext.Database.Migrate();
                }
            }
        }
    }
}
