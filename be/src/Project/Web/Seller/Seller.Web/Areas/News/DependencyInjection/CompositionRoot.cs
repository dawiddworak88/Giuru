using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.News.DomainModels;
using Seller.Web.Areas.News.ModelBuilders;
using Seller.Web.Areas.News.ViewModel;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.News.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterNewsAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Category>>, CategoriesPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoriesPageViewModel>, CategoriesPageModelBuilder>();
        }
    }
}
