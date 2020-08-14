using Microsoft.Extensions.DependencyInjection;
using Buyer.Web.Shared.Headers.ModelBuilders;
using Foundation.Extensions.ModelBuilders;
using Buyer.Web.Shared.Footers.ModelBuilders;
using Microsoft.Extensions.Configuration;
using Buyer.Web.Shared.Configurations;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Buyer.Web.Shared.Headers.ViewModels;
using Buyer.Web.Areas.Home.DependencyInjection;

namespace Buyer.Web.Shared.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.RegisterHomeDependencies();

            services.AddScoped<IModelBuilder<BuyerHeaderViewModel>, HeaderModelBuilder>();
            services.AddScoped<IModelBuilder<FooterViewModel>, FooterModelBuilder>();
            services.AddScoped<IModelBuilder<LogoViewModel>, LogoModelBuilder>();
        }

        public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServicesEndpointsConfiguration>(configuration.GetSection("ServicesEndpoints"));
        }
    }
}
