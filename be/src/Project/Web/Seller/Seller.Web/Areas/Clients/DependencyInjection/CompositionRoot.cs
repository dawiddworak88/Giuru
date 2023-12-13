using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.ModelBuilders;
using Seller.Web.Areas.Clients.Repositories.Groups;
using Seller.Web.Areas.Clients.Repositories.Roles;
using Seller.Web.Areas.Clients.Repositories.Managers;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Shared.ViewModels;
using Seller.Web.Areas.Clients.Repositories.Applications;
using Seller.Web.Areas.Clients.Repositories.DeliveryAddresses;
using Seller.Web.Areas.Clients.Repositories.Fields;
using Seller.Web.Areas.Clients.Repositories.FieldOptions;

namespace Seller.Web.Areas.Clients.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterClientsAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IClientGroupsRepository, ClientGroupsRepository>();
            services.AddScoped<IClientRolesRepository, ClientRolesRepository>();
            services.AddScoped<IClientAccountManagersRepository, ClientAccountManagersRepository>();
            services.AddScoped<IClientApplicationsRepository, ClientApplicationsRepository>();
            services.AddScoped<IClientAddressesRepository, ClientAddressesRepository>();
            services.AddScoped<IClientFieldsRepository, ClientFieldsRepository>();
            services.AddScoped<IClientFieldOptionsRepository, ClientFieldOptionsRepository>();

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
            
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ClientAccountManager>>, ClientAccountManagersPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientAccountManagersPageViewModel>, ClientAccountManagersPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientAccountManagerPageViewModel>, ClientAccountManagerPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientAccountManagerFormViewModel>, ClientAccountManagerFormModelBuilder>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ClientApplication>>, ClientApplicationsPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientApplicationsPageViewModel>, ClientApplicationsPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientApplicationPageViewModel>, ClientApplicationPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientApplicationFormViewModel>, ClientApplicationFormModelBuilder>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ClientAddress>>, ClientAddressesPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientAddressesPageViewModel>, ClientAddressesPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientAddressPageViewModel>, ClientAddressPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientAddressFormViewModel>, ClientAddressFormModelBuilder>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ClientField>>, ClientFieldsPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientFieldsPageViewModel>, ClientFieldsPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientFieldPageViewModel>, ClientFieldPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ClientFieldFormViewModel>, ClientFieldFormModelBuilder>();
        }
    }
}