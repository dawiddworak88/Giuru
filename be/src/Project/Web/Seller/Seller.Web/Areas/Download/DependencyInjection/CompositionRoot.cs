using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.Download.DomainModels;
using Seller.Web.Areas.Download.ModelBuilders;
using Seller.Web.Areas.Download.Repositories.Categories;
using Seller.Web.Areas.Download.ViewModel;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Download.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDownloadAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Category>>, CategoriesPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoriesPageViewModel>, CategoriesPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoryPageViewModel>, CategoryPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoryFormViewModel>, CategoryFormModelBuilder>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<DomainModels.Download>>, DownloadsPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DownloadsPageViewModel>, DownloadsPageModelBuilder>();
        }
    }
}
