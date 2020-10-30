using Foundation.GenericRepository.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.GenericRepository.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void ConfigureGenericRepositoryOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEntityService, EntityService>();
        }
    }
}
