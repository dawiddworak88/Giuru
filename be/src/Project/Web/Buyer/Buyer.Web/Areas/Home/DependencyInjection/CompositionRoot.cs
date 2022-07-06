using Buyer.Web.Areas.Home.ModelBuilders.Application;
using Buyer.Web.Areas.Home.ModelBuilders.Home;
using Buyer.Web.Areas.Home.ViewModel.Application;
using Buyer.Web.Areas.Home.ViewModel.Home;
using Buyer.Web.Shared.ModelBuilders.Headers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.HeroSliders.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Buyer.Web.Areas.Home.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterHomeDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, HomePageViewModel>, HomePageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, HeroSliderViewModel>, HeroSliderModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, HomePageCarouselGridViewModel>, HomePageCarouselGridModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, HomePageContentGridViewModel>, HomePageContentGridModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, HomePageNewsCarouselGridViewModel>, HomePageNewsModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ApplicationPageViewModel>, ApplicationPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ApplicationFormViewModel>, ApplicationFormModelBuilder>();
        }
    }
}
