using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.Repositories.Roles;
using Seller.Web.Areas.Clients.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientRoleFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientRoleFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IClientRolesRepository clientRolesRepository;

        public ClientRoleFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientRolesRepository clientRolesRepository,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.clientLocalizer = clientLocalizer;
            this.linkGenerator = linkGenerator;
            this.clientRolesRepository = clientRolesRepository;
        }

        public async Task<ClientRoleFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientRoleFormViewModel
            {
                Title = this.clientLocalizer.GetString("EditClientRole"),
                NameRequiredErrorMessage = this.globalLocalizer.GetString("NameRequiredErrorMessage"),
                NameLabel = this.globalLocalizer.GetString("Name"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "ClientRolesApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                IdLabel = this.globalLocalizer.GetString("Id"),
                RolesUrl = this.linkGenerator.GetPathByAction("Index", "ClientRoles", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                NavigateToCliensRoles = this.clientLocalizer.GetString("BackToRoles")
            };

            var role = await this.clientRolesRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

            if (role is not null)
            {
                viewModel.Id = role.Id;
                viewModel.Name = role.Name;
            }

            return viewModel;
        }
    }
}