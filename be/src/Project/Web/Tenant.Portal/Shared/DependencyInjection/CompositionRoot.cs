using Microsoft.Extensions.DependencyInjection;
using Tenant.Portal.Shared.Headers.ModelBuilders;
using Tenant.Portal.Areas.Orders.ViewModel;
using Tenant.Portal.Areas.Orders.ModelBuilders;
using Foundation.Extensions.ModelBuilders;
using Tenant.Portal.Shared.Footers.ModelBuilders;
using Microsoft.Extensions.Configuration;
using Tenant.Portal.Shared.Configurations;
using Tenant.Portal.Shared.MenuTiles.ModelBuilders;
using Feature.PageContent.MenuTiles.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.Components.Footers.ViewModels;

namespace Tenant.Portal.Shared.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IModelBuilder<OrderPageViewModel>, OrderPageModelBuilder>();
            services.AddScoped<IModelBuilder<HeaderViewModel>, HeaderModelBuilder>();
            services.AddScoped<IModelBuilder<MenuTilesViewModel>, MenuTilesModelBuilder>();
            services.AddScoped<IModelBuilder<FooterViewModel>, FooterModelBuilder>();
            services.AddScoped<IModelBuilder<LogoViewModel>, LogoModelBuilder>();
        }

        public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServicesEndpointsConfiguration>(configuration.GetSection("ServicesEndpoints"));
        }
    }
}
