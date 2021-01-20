using Media.Api.v1.Areas.Media.Repositories;
using Media.Api.v1.Areas.Media.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Media.Api.v1.Areas.Media.DependencyInjection
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
