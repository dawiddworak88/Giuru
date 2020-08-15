using Media.Api.v1.Area.Media.Repositories;
using Media.Api.v1.Area.Media.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Media.Api.v1.Area.Media.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterMediaDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IMediaRepository, MediaRepository>();
        }
    }
}
