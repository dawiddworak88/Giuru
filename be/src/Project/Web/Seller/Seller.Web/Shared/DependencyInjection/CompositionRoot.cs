using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Shared.Headers.ModelBuilders;
using Foundation.Extensions.ModelBuilders;
using Seller.Web.Shared.Footers.ModelBuilders;
using Seller.Web.Shared.MenuTiles.ModelBuilders;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Microsoft.Extensions.Configuration;
using Seller.Web.Shared.Configurations;

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
            services.Configure<AppSettings>(configuration);
        }
    }
}
