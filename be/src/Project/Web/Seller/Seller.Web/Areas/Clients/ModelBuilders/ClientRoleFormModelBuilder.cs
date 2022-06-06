using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
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

        public ClientRoleFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.clientLocalizer = clientLocalizer;
            this.linkGenerator = linkGenerator;
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

            return viewModel;
        }
    }
}