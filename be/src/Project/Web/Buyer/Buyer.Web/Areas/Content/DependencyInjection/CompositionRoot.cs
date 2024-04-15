using Buyer.Web.Areas.Content.ComponentModels;
using Buyer.Web.Areas.Content.ModelBuilders;
using Buyer.Web.Areas.Content.Repositories;
using Buyer.Web.Areas.Content.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;

namespace Buyer.Web.Areas.Content.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterContentDependencies(this IServiceCollection services)
        {
            services.AddScoped<IContentRepository, ContentRepository>();

            services.AddScoped<IAsyncComponentModelBuilder<ContentComponentModel, ContentPageViewModel>, ContentPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ContentComponentModel, StrapiContentWidgetsViewModel>, StrapiContentWidgetsModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<CarouselGridComponentModel, CarouselGridViewModel>, CarouselGridModelBuilder>();
        }
    }
}
