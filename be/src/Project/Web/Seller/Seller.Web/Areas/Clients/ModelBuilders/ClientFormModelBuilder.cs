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
using Seller.Web.Shared.Repositories.Identity;
using Seller.Web.Areas.Clients.Repositories.Groups;
using System.Linq;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Seller.Web.Areas.Clients.Repositories.Managers;
using Seller.Web.Areas.Global.Repositories;
using Seller.Web.Areas.Global.DomainModels;
using Seller.Web.Areas.Clients.Repositories.DeliveryAddresses;
using Foundation.GenericRepository.Definitions;
using Foundation.Extensions.ExtensionMethods;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientFormViewModel>
    {
        private readonly IClientsRepository _clientsRepository;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IOptionsMonitor<LocalizationSettings> _localizationOptions;
        private readonly IIdentityRepository _identityRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IClientGroupsRepository _clientGroupsRepository;
        private readonly IClientAccountManagersRepository _clientManagersRepository;
        private readonly ICountriesRepository _countriesRepository;
        private readonly IClientDeliveryAddressesRepository _clientDeliveryAddressesRepository;

        public ClientFormModelBuilder(
            IClientsRepository clientsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IOptionsMonitor<LocalizationSettings> localizationOptions,
            IIdentityRepository identityRepository,
            IClientGroupsRepository clientGroupsRepository,
            IClientAccountManagersRepository clientManagersRepository,
            ICountriesRepository countriesRepository,
            IClientDeliveryAddressesRepository clientDeliveryAddressesRepository,
            LinkGenerator linkGenerator)
        {
            _clientsRepository = clientsRepository;
            _globalLocalizer = globalLocalizer;
            _clientLocalizer = clientLocalizer;
            _localizationOptions = localizationOptions;
            _linkGenerator = linkGenerator;
            _identityRepository = identityRepository;
            _clientGroupsRepository = clientGroupsRepository;
            _clientManagersRepository = clientManagersRepository;
            _countriesRepository = countriesRepository;
            _clientDeliveryAddressesRepository = clientDeliveryAddressesRepository;
        }

        public async Task<ClientFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var languages = new List<LanguageViewModel>
            {
                new LanguageViewModel { Text = _globalLocalizer.GetString("SelectLanguage") , Value = string.Empty }
            };

            foreach (var language in _localizationOptions.CurrentValue.SupportedCultures.Split(','))
            {
                languages.Add(new LanguageViewModel { Text = language.ToUpperInvariant(), Value = language.ToLowerInvariant() });
            }

            var viewModel = new ClientFormViewModel
            {
                Title = _clientLocalizer.GetString("EditClient"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                ClientDetailText = _clientLocalizer.GetString("Client"),
                NameLabel = _globalLocalizer.GetString("NameLabel"),
                EmailLabel = _globalLocalizer.GetString("EmailLabel"),
                LanguageLabel = _globalLocalizer.GetString("CommunicationLanguageLabel"),
                NameRequiredErrorMessage = _globalLocalizer.GetString("NameRequiredErrorMessage"),
                EmailRequiredErrorMessage = _globalLocalizer.GetString("EmailRequiredErrorMessage"),
                EmailFormatErrorMessage = _globalLocalizer.GetString("EmailFormatErrorMessage"),
                LanguageRequiredErrorMessage = _globalLocalizer.GetString("LanguageRequiredErrorMessage"),
                EnterNameText = _globalLocalizer.GetString("EnterNameText"),
                EnterEmailText = _globalLocalizer.GetString("EnterEmailText"),
                SaveText = _globalLocalizer.GetString("SaveText"),
                AccountText = _clientLocalizer.GetString("AccountText"),
                AccountUrl = _linkGenerator.GetPathByAction("Account", "IdentityApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                SaveUrl = _linkGenerator.GetPathByAction("Index", "ClientsApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                Languages = languages,
                IdLabel = _globalLocalizer.GetString("Id"),
                PhoneNumberLabel = _globalLocalizer.GetString("PhoneNumberLabel"),
                ClientsUrl = _linkGenerator.GetPathByAction("Index", "Clients", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                NavigateToClientsLabel = _clientLocalizer.GetString("NavigateToClientsLabel"),
                ResetPasswordText = _clientLocalizer.GetString("ResetPasswordText"),
                NoGroupsText = _clientLocalizer.GetString("NoGroupsText"),
                GroupsLabel = _globalLocalizer.GetString("Groups"),
                NoManagersText = _clientLocalizer.GetString("NoManagers"),
                ClientManagerLabel = _globalLocalizer.GetString("Manager"),
                CountryLabel = _globalLocalizer.GetString("Country"),
                DeliveryAddressLabel = _clientLocalizer.GetString("DeliveryAddress"),
                EmailMarketingApprovalLabel = _clientLocalizer.GetString("IsEmailMarketingApproval"),
                SmsMarketingApprovalLabel = _clientLocalizer.GetString("IsSmsMarketingApproval"),
                ExpressedOnLabel = _clientLocalizer.GetString("ExpressedOnLabel")
            };

            if (componentModel.Id.HasValue)
            {
                var client = await _clientsRepository.GetClientAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (client is not null)
                {
                    viewModel.Id = client.Id;
                    viewModel.Name = client.Name;
                    viewModel.Email = client.Email;
                    viewModel.CommunicationLanguage = client.CommunicationLanguage;
                    viewModel.PhoneNumber = client.PhoneNumber;
                    viewModel.ClientGroupsIds = client.ClientGroupIds;
                    viewModel.ClientManagersIds = client.ClientManagerIds;
                    viewModel.CountryId = client.CountryId;
                    viewModel.DefaultDeliveryAddressId = client.DefaultDeliveryAddressId;
                    viewModel.MarketingApprovals = client.MarketingApprovals.Select(x => new ClientMarketingApproval
                    {
                        Name = x.Name,
                        CreatedDate = x.CreatedDate
                    });

                    var user = await _identityRepository.GetAsync(componentModel.Token, componentModel.Language, client.Email);

                    if (user is not null)
                    {
                        viewModel.HasAccount = true;
                    }
                }
            }

            var groups = await _clientGroupsRepository.GetAsync(componentModel.Token, componentModel.Language);

            if (groups is not null)
            {
                viewModel.ClientGroups = groups.Select(x => new ListItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                });
            }

            var managers = await _clientManagersRepository.GetAsync(componentModel.Token, componentModel.Language);

            if (managers is not null)
            {
                viewModel.ClientManagers = managers.Select(x => new ClientAccountManagerViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName
                });
            }

            var countries = await _countriesRepository.GetAsync(componentModel.Token, componentModel.Language, $"{nameof(Country.Name)} asc");

            if (countries is not null)
            {
                viewModel.Countries = countries.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            var deliveryAddresses = await _clientDeliveryAddressesRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, null);

            if (deliveryAddresses.Data is not null)
            {
                viewModel.DeliveryAddresses = deliveryAddresses.Data.Select(x => new ListItemViewModel
                {
                    Id = x.Id,
                    Name = $"{x.Company}, {x.FirstName} {x.LastName}, {x.PostCode} {x.City}"
                });
            }

            return viewModel;
        }
    }
}