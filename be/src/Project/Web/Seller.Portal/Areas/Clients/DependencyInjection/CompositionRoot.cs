using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;
using Seller.Portal.Areas.Clients.ModelBuilders;
using Seller.Portal.Areas.Clients.Repositories;
using Seller.Portal.Areas.Clients.ViewModels;
using Seller.Portal.Areas.Products.ModelBuilders;
using Seller.Portal.Shared.Catalogs.ModelBuilders;

namespace Seller.Portal.Areas.Clients.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterClientsAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IClientsRepository, ClientsRepository>();
            services.AddScoped<ICatalogModelBuilder, CatalogModelBuilder>();
            services.AddScoped<IModelBuilder<ClientPageViewModel>, ClientPageModelBuilder>();
            services.AddScoped<IModelBuilder<ClientDetailPageViewModel>, ClientDetailPageModelBuilder>();
            services.AddScoped<IModelBuilder<ClientDetailFormViewModel>, ClientDetailFormModelBuilder>();
        }
    }
}
