using Microsoft.Extensions.DependencyInjection;
using Seller.Portal.Shared.Headers.ModelBuilders;
using Foundation.Extensions.ModelBuilders;
using Seller.Portal.Shared.Footers.ModelBuilders;
using Microsoft.Extensions.Configuration;
using Seller.Portal.Shared.Configurations;
using Seller.Portal.Shared.MenuTiles.ModelBuilders;
using Feature.PageContent.MenuTiles.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.Components.Footers.ViewModels;
using Seller.Portal.Areas.Clients.Configurations;
using Seller.Portal.Areas.Clients.Controllers.Configurations;

namespace Seller.Portal.Shared.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IModelBuilder<HeaderViewModel>, HeaderModelBuilder>();
            services.AddScoped<IModelBuilder<MenuTilesViewModel>, MenuTilesModelBuilder>();
            services.AddScoped<IModelBuilder<FooterViewModel>, FooterModelBuilder>();
            services.AddScoped<IModelBuilder<LogoViewModel>, LogoModelBuilder>();
        }

        public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServicesEndpointsConfiguration>(configuration.GetSection("ServicesEndpoints"));

            services.Configure<ApiConfiguration>(configuration.GetSection("ServicesEndpoints").GetSection("Api"));
            services.Configure<EndpointsConfiguration>(configuration.GetSection("ServicesEndpoints").GetSection("Api").GetSection("Endpoints"));
        }
    }
}
