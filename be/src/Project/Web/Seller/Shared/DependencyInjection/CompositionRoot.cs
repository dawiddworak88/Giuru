using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Shared.Headers.ModelBuilders;
using Foundation.Extensions.ModelBuilders;
using Seller.Web.Shared.Footers.ModelBuilders;
using Microsoft.Extensions.Configuration;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.MenuTiles.ModelBuilders;
using Feature.PageContent.MenuTiles.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.Components.Footers.ViewModels;
using Seller.Web.Areas.Clients.Configurations;
using Seller.Web.Areas.Clients.Controllers.Configurations;

namespace Seller.Web.Shared.DependencyInjection
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
