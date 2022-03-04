using Buyer.Web.Areas.News.ModelBuilders;
using Buyer.Web.Areas.News.Repositories;
using Buyer.Web.Areas.News.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;

namespace Buyer.Web.Areas.News.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterNewsDependencies(this IServiceCollection services)
        {
            services.AddScoped<INewsRepository, NewsRepository>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, NewsPageViewModel>, NewsPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, NewsCatalogViewModel>, NewsCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, NewsItemPageViewModel>, NewsItemPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, NewsItemDetailsViewModel>, NewsItemDetailsModelBuilder>();
        }
    }
}
