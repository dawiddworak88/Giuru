using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.Media.Repositories;

namespace Seller.Web.Areas.Media.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterMediaAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IFilesRepository, FilesRepository>();
        }
    }
}
