using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.News.DomainModels;
using Seller.Web.Areas.News.ModelBuilders;
using Seller.Web.Areas.News.Repositories.Categories;
using Seller.Web.Areas.News.ViewModel;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.News.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterNewsAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Category>>, CategoriesPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoriesPageViewModel>, CategoriesPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoryPageViewModel>, CategoryPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoryFormViewModel>, CategoryFormModelBuilder>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<NewsItem>>, NewsPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, NewsPageViewModel>, NewsPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, NewsItemPageViewModel>, NewsItemPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, NewsItemFormViewModel>, NewsItemFormModelBuilder>();
        }
    }
}
