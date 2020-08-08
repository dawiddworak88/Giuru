using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.Clients.ModelBuilders;
using Seller.Web.Areas.Clients.Repositories;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Areas.Products.ModelBuilders;
using Seller.Web.Shared.Catalogs.ModelBuilders;

namespace Seller.Web.Areas.Clients.DependencyInjection
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
