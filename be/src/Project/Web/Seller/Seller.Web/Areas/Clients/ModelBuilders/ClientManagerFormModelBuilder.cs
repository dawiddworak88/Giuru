using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.Repositories;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Shared.Repositories.Clients;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientManagerFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientManagerFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IClientsRepository clientsRepository;

        public ClientManagerFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientsRepository clientsRepository,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.clientLocalizer = clientLocalizer;
            this.linkGenerator = linkGenerator;
            this.clientLocalizer = clientLocalizer;
            this.clientsRepository = clientsRepository;
        }

        public async Task<ClientManagerFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientManagerFormViewModel
            {
                IdLabel = this.globalLocalizer.GetString("Id"),
                Title = this.clientLocalizer.GetString("EditClientManager"),
                FieldRequiredErrorMessage = this.globalLocalizer.GetString("FieldRequiredErrorMessage"),
                ClientsLabel = this.clientLocalizer.GetString("Clients"),
                NoClientsText = this.clientLocalizer.GetString("NoClients"),
                SelectClients = this.clientLocalizer.GetString("SelectClients"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                NavigateToManagersText = this.clientLocalizer.GetString("NavigateToManagersText"),
                ManagersUrl = this.linkGenerator.GetPathByAction("Index", "ClientManagers", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred")
            };

            var clients = await this.clientsRepository.GetAllClientsAsync(componentModel.Token, componentModel.Language);

            if (clients is not null)
            {
                viewModel.Clients = clients.Select(x => new ListItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                });
            }

            return viewModel;
        }
    }
}