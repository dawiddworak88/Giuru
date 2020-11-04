using Media.Api.Infrastructure;
using Media.Api.Shared.Checksums;
using Media.Api.Shared.ImageResizers;
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
            services.AddScoped<IChecksumService, ChecksumService>();
            services.AddScoped<IImageResizeService, ImageResizeService>();

            services.AddDbContext<MediaContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite()));
        }
    }
}
