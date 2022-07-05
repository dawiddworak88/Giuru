using Buyer.Web.Areas.Download.ModelBuilders;
using Buyer.Web.Areas.Download.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;

namespace Buyer.Web.Areas.Download.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDownloadDependencies(this IServiceCollection services)
        {
            //services.AddScoped<ICategoriesRepository, CategoriesRepository>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DownloadPageViewModel>, DownloadPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DownloadCatalogViewModel>, DownloadCatalogModelBuilder>();
        }
    }
}
