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
using Seller.Web.Areas.Clients.Repositories.NotificationTypes;
using Seller.Web.Areas.Clients.DomainModels;
using Foundation.Extensions.ExtensionMethods;
using Seller.Web.Areas.Clients.Repositories.NotificationTypesApprovals;
using Seller.Web.Areas.Clients.Repositories.Fields;
using Seller.Web.Areas.Clients.Repositories.FieldValues;

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
        private readonly IClientAddressesRepository _clientAddressesRepository;
        private readonly IClientNotificationTypesRepository _clientNotificationTypesRepository;
        private readonly IClientNotificationTypesApprovalsRepository _clientNotificationTypeApprovalRepository;
        private readonly IClientFieldsRepository _clientFieldsRepository;
        private readonly IClientFieldValuesRepository _clientFieldValuesRepository;
        private readonly ICurrenciesRepository _currenciesRepository;

        public ClientFormModelBuilder(
            IClientsRepository clientsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IOptionsMonitor<LocalizationSettings> localizationOptions,
            IIdentityRepository identityRepository,
            IClientGroupsRepository clientGroupsRepository,
            IClientAccountManagersRepository clientManagersRepository,
            ICountriesRepository countriesRepository,
            IClientAddressesRepository clientAddressesRepository,
            IClientFieldsRepository clientFieldsRepository,
            IClientFieldValuesRepository clientFieldValuesRepository,
            LinkGenerator linkGenerator,
            IClientNotificationTypesRepository clientNotificationTypesRepository,
            IClientNotificationTypesApprovalsRepository clientNotificationTypeApprovalRepository,
            ICurrenciesRepository currenciesRepository)
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
            _clientAddressesRepository = clientAddressesRepository;
            _clientNotificationTypesRepository = clientNotificationTypesRepository;
            _clientNotificationTypeApprovalRepository = clientNotificationTypeApprovalRepository;
            _clientFieldsRepository = clientFieldsRepository;
            _clientFieldValuesRepository = clientFieldValuesRepository;
            _currenciesRepository = currenciesRepository;
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
                PreferedCurrencyLabel = _clientLocalizer.GetString("PreferedCurrencyLabel"),
                DeliveryAddressLabel = _clientLocalizer.GetString("DeliveryAddress"),
                BillingAddressLabel = _clientLocalizer.GetString("BillingAddress"),
                ExpressedOnLabel = _clientLocalizer.GetString("ExpressedOnLabel"),
                ActiveLabel = _globalLocalizer.GetString("Active"),
                InActiveLabel = _globalLocalizer.GetString("InActive")
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
                    viewModel.IsDisabled = client.IsDisabled;
                    viewModel.PreferedCurrencyId = client.PreferedCurrencyId;
                    viewModel.DefaultDeliveryAddressId = client.DefaultDeliveryAddressId;
                    viewModel.DefaultBillingAddressId = client.DefaultBillingAddressId;

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

            var currencies = await _currenciesRepository.GetAsync(componentModel.Token, componentModel.Language, $"{nameof(Country.Name)} asc");

            if (currencies is not null)
            {
                viewModel.Currencies = currencies.Select(x => new ListItemViewModel { Id = x.Id, Name = x.CurrencyCode });
            }

            var clientAddresses = await _clientAddressesRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, null);

            if (clientAddresses.Data is not null)
            {
                viewModel.ClientAddresses = clientAddresses.Data.Select(x => new ListItemViewModel
                {
                    Id = x.Id,
                    Name = $"{x.Company}, {x.FirstName} {x.LastName}, {x.PostCode} {x.City}"
                });
            }

            var clientTypesApprovals = (await _clientNotificationTypesRepository.GetAsync(componentModel.Token, componentModel.Language, $"{nameof(ClientNotificationType.CreatedDate)} asc"))
                    .OrEmptyIfNull().Select(x => new ClientNotificationTypeViewModel
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();

            if (componentModel.Id.HasValue)
            {
                var clientApprovals = await _clientNotificationTypeApprovalRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                foreach (var clientTypeApproval in clientTypesApprovals.OrEmptyIfNull())
                {
                    if (clientApprovals.Any(x => x.NotificationTypeId == clientTypeApproval.Id))
                    {
                        var clientApproval = clientApprovals.FirstOrDefault(x => x.NotificationTypeId == clientTypeApproval.Id);

                        clientTypeApproval.ApprovalDate = clientApproval.ApprovalDate;
                        clientTypeApproval.IsApproved = clientApproval.IsApproved;
                    }
                }
            }

            viewModel.ClientApprovals = clientTypesApprovals;

            var clientFields = await _clientFieldsRepository.GetAsync(componentModel.Token, componentModel.Language);

            if (clientFields is not null)
            {
                var clientFieldsValues = await _clientFieldValuesRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                viewModel.ClientFields = clientFields.Select(x =>
                {
                    var fieldValue = clientFieldsValues.FirstOrDefault(y => y.FieldDefinitionId == x.Id)?.FieldValue;

                    return new ClientFieldViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Value = fieldValue,
                        Type = x.Type,
                        IsRequired = x.IsRequired,
                        Options = x.Options.Select(y => new ClientFieldOptionViewModel
                        {
                            Name = y.Name,
                            Value = y.Value
                        })
                    };
                }); 
            }

            return viewModel;
        }
    }
}