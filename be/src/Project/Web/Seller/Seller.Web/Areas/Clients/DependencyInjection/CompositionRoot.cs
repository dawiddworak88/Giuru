using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
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
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientsPageViewModel>, ClientsPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientPageViewModel>, ClientPageModelBuilder>();
            services.AddScoped<IModelBuilder<ClientFormViewModel>, ClientFormModelBuilder>();
        }
    }
}
