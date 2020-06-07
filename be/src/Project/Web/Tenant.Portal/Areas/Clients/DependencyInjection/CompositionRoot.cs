using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;
using Tenant.Portal.Areas.Clients.ModelBuilders;
using Tenant.Portal.Areas.Clients.ViewModels;
using Tenant.Portal.Areas.Products.ModelBuilders;
using Tenant.Portal.Shared.Catalogs.ModelBuilders;

namespace Tenant.Portal.Areas.Clients.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterClientsAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICatalogModelBuilder, CatalogModelBuilder>();
            services.AddScoped<IModelBuilder<ClientPageViewModel>, ClientPageModelBuilder>();
            services.AddScoped<IModelBuilder<ClientDetailPageViewModel>, ClientDetailPageModelBuilder>();
            services.AddScoped<IModelBuilder<ClientDetailFormViewModel>, ClientDetailFormModelBuilder>();
        }
    }
}
