using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.ModelBuilders;
using Seller.Web.Areas.Clients.Repositories.Groups;
using Seller.Web.Areas.Clients.Repositories.Roles;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Clients.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterClientsAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IClientGroupsRepository, ClientGroupsRepository>();
            services.AddScoped<IClientRolesRepository, ClientRolesRepository>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Client>>, ClientsPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientsPageViewModel>, ClientsPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientPageViewModel>, ClientPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientFormViewModel>, ClientFormModelBuilder>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ClientGroup>>, ClientGroupsPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientGroupsPageViewModel>, ClientGroupsPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientGroupPageViewModel>, ClientGroupPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientGroupFormViewModel>, ClientGroupFormModelBuilder>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ClientRole>>, ClientRolesPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientRolesPageViewModel>, ClientRolesPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientRolePageViewModel>, ClientRolePageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientRoleFormViewModel>, ClientRoleFormModelBuilder>();
        }
    }
}