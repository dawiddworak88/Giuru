using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.DownloadCenter.DomainModels;
using Seller.Web.Areas.DownloadCenter.ModelBuilders;
using Seller.Web.Areas.DownloadCenter.Repositories.DownloadCenter;
using Seller.Web.Areas.DownloadCenter.Repositories.DownloadCenterCategories;
using Seller.Web.Areas.DownloadCenter.ViewModel;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.DownloadCenter.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDownloadCenterAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IDownloadCenterCategoriesRepository, DownloadCenterCategoriesRepository>();
            services.AddScoped<IDownloadCenterRepository, DownloadCenterRepository>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<DownloadCenterCategory>>, DownloadCenterCategoriesPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCategoriesPageViewModel>, DownloadCenterCategoriesPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCategoryPageViewModel>, DownloadCenterCategoryPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCategoryFormViewModel>, DownloadCenterCategoryFormModelBuilder>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<DownloadCenterItem>>, DownloadCenterPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterPageViewModel>, DownloadCenterPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterItemPageViewModel>, DownloadCenterItemPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterItemFormViewModel>, DownloadCenterItemFormModelBuilder>();

        }
    }
}
