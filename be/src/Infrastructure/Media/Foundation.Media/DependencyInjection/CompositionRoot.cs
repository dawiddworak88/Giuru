using Foundation.Media.Services.CdnServices;
using Foundation.Media.Services.FileTypeServices;
using Foundation.Media.Services.MediaServices;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Media.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterFoundationMediaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<ICdnService, CloudinaryCdnService>();
            services.AddScoped<IFileTypeService, FileTypeService>();
        }
    }
}
