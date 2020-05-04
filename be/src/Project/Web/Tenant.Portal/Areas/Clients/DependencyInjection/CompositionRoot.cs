using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tenant.Portal.Areas.Clients.Configurations;
using Tenant.Portal.Areas.Clients.Controllers.Configurations;
using Tenant.Portal.Areas.Clients.ModelBuilders;
using Tenant.Portal.Areas.Clients.ViewModels;

namespace Tenant.Portal.Areas.Clients.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterClientsAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IModelBuilder<ClientPageViewModel>, ClientPageModelBuilder>();
            services.AddScoped<IModelBuilder<ClientCatalogViewModel>, ClientCatalogModelBuilder>();
            services.AddScoped<IModelBuilder<ClientDetailPageViewModel>, ClientDetailPageModelBuilder>();
            services.AddScoped<IModelBuilder<ClientDetailFormViewModel>, ClientDetailFormModelBuilder>();
        }
    }
}
