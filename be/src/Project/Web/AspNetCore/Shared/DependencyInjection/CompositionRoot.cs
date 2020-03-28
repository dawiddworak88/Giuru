using Microsoft.Extensions.DependencyInjection;
using AspNetCore.Shared.Headers.ModelBuilders;
using AspNetCore.Areas.Home.ViewModel;
using AspNetCore.Areas.Home.ModelBuilders;
using Foundation.Extensions.ModelBuilders;
using AspNetCore.Shared.Footers.ModelBuilders;
using Microsoft.Extensions.Configuration;
using AspNetCore.Shared.Configurations;
using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.Components.Footers.ViewModels;

namespace AspNetCore.Shared.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IModelBuilder<HomePageViewModel>, HomePageModelBuilder>();
            services.AddScoped<IModelBuilder<HeaderViewModel>, HeaderModelBuilder>();
            services.AddScoped<IModelBuilder<FooterViewModel>, FooterModelBuilder>();
            services.AddScoped<IModelBuilder<LogoViewModel>, LogoModelBuilder>();
        }

        public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServicesEndpointsConfiguration>(configuration.GetSection("ServicesEndpoints"));
        }
    }
}
