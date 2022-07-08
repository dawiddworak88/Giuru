using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.DownloadCenter.DomainModels;
using Seller.Web.Areas.DownloadCenter.ModelBuilders;
using Seller.Web.Areas.DownloadCenter.Repositories.Categories;
using Seller.Web.Areas.DownloadCenter.ViewModel;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.DownloadCenter.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDownloadCenterAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Category>>, CategoriesPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoriesPageViewModel>, CategoriesPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoryPageViewModel>, CategoryPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoryFormViewModel>, CategoryFormModelBuilder>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<DomainModels.DownloadCenter>>, DownloadCenterPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterPageViewModel>, DownloadCenterPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterItemPageViewModel>, DownloadCenterItemPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterItemFormViewModel>, DownloadCenterItemFormModelBuilder>();

        }
    }
}
