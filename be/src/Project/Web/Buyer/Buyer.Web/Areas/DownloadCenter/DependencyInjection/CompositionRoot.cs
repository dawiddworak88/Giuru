using Buyer.Web.Areas.DownloadCenter.ModelBuilders;
using Buyer.Web.Areas.DownloadCenter.Repositories;
using Buyer.Web.Areas.DownloadCenter.ViewModel;
using Buyer.Web.Shared.Repositories.Media;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;

namespace Buyer.Web.Areas.DownloadCenter.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDownloadCenterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IDownloadCenterRepository, DownloadCenterRepository>();
            services.AddScoped<IMediaItemsRepository, MediaItemsRepository>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterPageViewModel>, DownloadCenterPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCatalogViewModel>, DownloadCenterCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCategoryDetailsViewModel>, DownloadCenterCategoryDetailsModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCategoryPageViewModel>, DownloadCenterCategoryPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCategoryBreadcrumbsViewModel>, DownloadCenterCategoryBreadcrumbsModelBuilder>();
        }
    }
}
