using Media.Api.Infrastructure;
using Media.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Media.Api.Services.Media;
using Media.Api.Services.ImageResizers;
using Media.Api.Services.Checksums;

namespace Media.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterMediaApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<IChecksumService, ChecksumService>();
            services.AddScoped<IImageResizeService, ImageResizeService>();
        }

        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<MediaContext>();
            services.AddDbContext<MediaContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite()), ServiceLifetime.Scoped);
        }
    }
}
