﻿using Buyer.Web.Areas.Clients.ModelBuilders;
using Buyer.Web.Areas.Clients.ViewModels;
using Buyer.Web.Areas.Home.ModelBuilders;
using Buyer.Web.Areas.Home.Repositories;
using Buyer.Web.Areas.Home.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.HeroSliders.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Buyer.Web.Areas.Home.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterHomeDependencies(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IHeroSliderRepository, HeroSliderRepository>();

            // Model Builders
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
