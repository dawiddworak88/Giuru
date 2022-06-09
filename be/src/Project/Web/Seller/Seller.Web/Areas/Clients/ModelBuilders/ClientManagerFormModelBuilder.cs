using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.Repositories;
using Seller.Web.Areas.Clients.Repositories.Managers;
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
        private readonly IClientManagersRepository clientManagersRepository;
        private readonly LinkGenerator linkGenerator;

        public ClientManagerFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientManagersRepository clientManagersRepository,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.clientLocalizer = clientLocalizer;
            this.linkGenerator = linkGenerator;
            this.clientLocalizer = clientLocalizer;
            this.clientManagersRepository = clientManagersRepository;
        }

        public async Task<ClientManagerFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientManagerFormViewModel
            {
                IdLabel = this.globalLocalizer.GetString("Id"),
                Title = this.clientLocalizer.GetString("EditClientManager"),
                FieldRequiredErrorMessage = this.globalLocalizer.GetString("FieldRequiredErrorMessage"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                NavigateToManagersText = this.clientLocalizer.GetString("NavigateToManagersText"),
                ManagersUrl = this.linkGenerator.GetPathByAction("Index", "ClientManagers", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                EmailFormatErrorMessage = this.globalLocalizer.GetString("EmailFormatErrorMessage"),
                FirstNameLabel = this.globalLocalizer.GetString("FirstName"),
                LastNameLabel = this.globalLocalizer.GetString("LastName"),
                EmailLabel = this.globalLocalizer.GetString("Email"),
                PhoneNumberLabel = this.globalLocalizer.GetString("PhoneNumberLabel"),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "ClientManagersApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
            };

            if (componentModel.Id.HasValue)
            {
                var manager = await this.clientManagersRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (manager is not null)
                {
                    viewModel.Id = manager.Id;
                    viewModel.FirstName = manager.FirstName;
                    viewModel.LastName = manager.LastName;
                    viewModel.Email = manager.Email;
                    viewModel.PhoneNumber = manager.PhoneNumber;
                }
            }

            return viewModel;
        }
    }
}