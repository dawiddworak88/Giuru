using Foundation.PageContent.Components.Languages.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.Localization.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using Seller.Web.Areas.Clients.ViewModels;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using Seller.Web.Shared.Repositories.Clients;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientFormViewModel>
    {
        private readonly IClientsRepository clientsRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly IOptionsMonitor<LocalizationSettings> localizationOptions;
        private readonly LinkGenerator linkGenerator;

        public ClientFormModelBuilder(
            IClientsRepository clientsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IOptionsMonitor<LocalizationSettings> localizationOptions,
            LinkGenerator linkGenerator)
        {
            this.clientsRepository = clientsRepository;
            this.globalLocalizer = globalLocalizer;
            this.clientLocalizer = clientLocalizer;
            this.localizationOptions = localizationOptions;
            this.linkGenerator = linkGenerator;
        }

        public async Task<ClientFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var languages = new List<LanguageViewModel>
            {
                new LanguageViewModel { Text = this.globalLocalizer.GetString("SelectLanguage") , Value = string.Empty }
            };

            foreach (var language in this.localizationOptions.CurrentValue.SupportedCultures.Split(','))
            {
                languages.Add(new LanguageViewModel { Text = language.ToUpperInvariant(), Value = language.ToLowerInvariant() });
            }

            var viewModel = new ClientFormViewModel
            {
                Title = this.clientLocalizer.GetString("EditClient"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                ClientDetailText = this.clientLocalizer.GetString("Client"),
                NameLabel = this.globalLocalizer.GetString("NameLabel"),
                EmailLabel = this.globalLocalizer.GetString("EmailLabel"),
                LanguageLabel = this.globalLocalizer.GetString("CommunicationLanguageLabel"),
                NameRequiredErrorMessage = this.globalLocalizer.GetString("NameRequiredErrorMessage"),
                EmailRequiredErrorMessage = this.globalLocalizer.GetString("EmailRequiredErrorMessage"),
                EmailFormatErrorMessage = this.globalLocalizer.GetString("EmailFormatErrorMessage"),
                LanguageRequiredErrorMessage = this.globalLocalizer.GetString("LanguageRequiredErrorMessage"),
                EnterNameText = this.globalLocalizer.GetString("EnterNameText"),
                EnterEmailText = this.globalLocalizer.GetString("EnterEmailText"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                AccountText = this.clientLocalizer.GetString("AccountText"),
                AccountUrl = this.linkGenerator.GetPathByAction("Account", "IdentityApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "ClientsApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                Languages = languages,
                ClientsUrl = this.linkGenerator.GetPathByAction("Index", "Clients", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                NavigateToClientsLabel = this.clientLocalizer.GetString("NavigateToClientsLabel")
                IdLabel = this.globalLocalizer.GetString("Id")
            };

            if (componentModel.Id.HasValue)
            {
                var client = await this.clientsRepository.GetClientAsync(componentModel.Token, componentModel.Language, componentModel.Id);
                if (client != null)
                {
                    viewModel.Id = client.Id;
                    viewModel.Name = client.Name;
                    viewModel.Email = client.Email;
                    viewModel.CommunicationLanguage = client.CommunicationLanguage;
                }
            }

            return viewModel;
        }
    }
}