using Foundation.Database.Shared.Contexts;
using Foundation.Database.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Database.Shared.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DatabaseContext"),
                    opt => opt.MigrationsAssembly("Foundation.Database")));
        }
    }
}
