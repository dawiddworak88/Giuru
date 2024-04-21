using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.Repositories.NotificationTypes;
using Seller.Web.Areas.Clients.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientNotificationTypeFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientNotificationTypeFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly IClientNotificationTypesRepository _clientNotificationTypesRepository;

        public ClientNotificationTypeFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            LinkGenerator linkGenerator,
            IClientNotificationTypesRepository clientNotificationTypesRepository)
        {
            _globalLocalizer = globalLocalizer;
            _clientLocalizer = clientLocalizer;
            _linkGenerator = linkGenerator;
            _clientNotificationTypesRepository = clientNotificationTypesRepository;
        }

        public async Task<ClientNotificationTypeFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientNotificationTypeFormViewModel
            {
                Title = _clientLocalizer.GetString("EditClientNotificationType"),
                NameRequiredErrorMessage = _globalLocalizer.GetString("NameRequiredErrorMessage"),
                NameLabel = _globalLocalizer.GetString("Name"),
                SaveText = _globalLocalizer.GetString("SaveText"),
                SaveUrl = _linkGenerator.GetPathByAction("Index", "ClientNotificationTypesApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                IdLabel = _globalLocalizer.GetString("Id"),
                ClientNotificationTypesUrl = _linkGenerator.GetPathByAction("Index", "ClientNotificationTypes", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                NavigateToClientNotificationTypes = _clientLocalizer.GetString("BackToNotificationType")
            };

            if (componentModel.Id.HasValue)
            {
                var notificationType = await _clientNotificationTypesRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (notificationType is not null)
                {
                    viewModel.Id = notificationType.Id;
                    viewModel.Name = notificationType.Name;
                }
            }

            return viewModel;
        }
    }
}
