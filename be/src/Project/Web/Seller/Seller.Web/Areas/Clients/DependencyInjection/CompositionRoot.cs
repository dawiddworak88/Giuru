using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.Clients.ModelBuilders;
using Seller.Web.Areas.Clients.Repositories;
using Seller.Web.Areas.Clients.ViewModels;

namespace Seller.Web.Areas.Clients.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterClientsAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IClientsRepository, ClientsRepository>();
            services.AddScoped<IModelBuilder<ClientsPageViewModel>, ClientsPageModelBuilder>();
            services.AddScoped<IModelBuilder<ClientPageViewModel>, ClientPageModelBuilder>();
            services.AddScoped<IModelBuilder<ClientFormViewModel>, ClientFormModelBuilder>();
        }
    }
}
