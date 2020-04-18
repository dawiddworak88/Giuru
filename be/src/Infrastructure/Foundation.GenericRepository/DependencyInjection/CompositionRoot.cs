using Foundation.GenericRepository.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.GenericRepository.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void ConfigureGenericRepositoryOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConnectionStringsConfiguration>(configuration.GetSection("ConnectionStrings"));
        }
    }
}
