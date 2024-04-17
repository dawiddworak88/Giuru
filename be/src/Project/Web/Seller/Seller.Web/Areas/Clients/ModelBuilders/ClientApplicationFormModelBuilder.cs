using DocumentFormat.OpenXml.Drawing.Diagrams;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.Localization.Definitions;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Languages.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Clients.Repositories.Applications;
using Seller.Web.Areas.Clients.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientApplicationFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientApplicationFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IClientApplicationsRepository _clientApplicationsRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IOptionsMonitor<LocalizationSettings> _localizationOptions;

        public ClientApplicationFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientApplicationsRepository clientApplicationsRepository,
            LinkGenerator linkGenerator,
            IOptionsMonitor<LocalizationSettings> localizationOptions)
        {
            _globalLocalizer = globalLocalizer;
            _clientLocalizer = clientLocalizer;
            _clientApplicationsRepository = clientApplicationsRepository;
            _linkGenerator = linkGenerator;
            _localizationOptions = localizationOptions;
        }

        public async Task<ClientApplicationFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientApplicationFormViewModel
            {
                Title = _clientLocalizer.GetString("EditClientApplication"),
                IdLabel = _globalLocalizer.GetString("Id"),
                FirstNameLabel = _globalLocalizer.GetString("FirstName"),
                LastNameLabel = _globalLocalizer.GetString("LastName"),
                EmailLabel = _globalLocalizer.GetString("Email"),
                ContactJobTitleLabel = _globalLocalizer.GetString("ContactJobTitle"),
                PhoneNumberLabel = _globalLocalizer.GetString("PhoneNumberLabel"),
                CompanyNameLabel = _globalLocalizer.GetString("CompanyName"),
                BackToClientsApplicationsText = _clientLocalizer.GetString("BackToClientsApplications"),
                ClientsApplicationsUrl = _linkGenerator.GetPathByAction("Index", "ClientApplications", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                FieldRequiredErrorMessage = _globalLocalizer.GetString("FieldRequiredErrorMessage"),
                EmailFormatErrorMessage = _globalLocalizer.GetString("EmailFormatErrorMessage"),
                SaveUrl = _linkGenerator.GetPathByAction("Index", "ClientsApplicationApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                SaveText = _globalLocalizer.GetString("SaveText"),
                SelectJobTitle = _globalLocalizer.GetString("SelectJobTitle"),
                LanguageLabel = _globalLocalizer.GetString("CommunicationLanguageLabel"),
                BillingAddressTitle = _globalLocalizer.GetString("BillingAddressTitle"),
                DeliveryAddressTitle = _globalLocalizer.GetString("DeliveryAddressTitle"),
                AddressFullNameLabel = $"{_globalLocalizer.GetString("FirstName")} {_globalLocalizer.GetString("LastName")}",
                AddressPhoneNumberLabel = _globalLocalizer.GetString("PhoneNumberLabel"),
                AddressStreetLabel = _globalLocalizer.GetString("Street"),
                AddressRegionLabel = _globalLocalizer.GetString("Region"),
                AddressPostalCodeLabel = _globalLocalizer.GetString("PostalCode"),
                AddressCityLabel = _globalLocalizer.GetString("City"),
                AddressCountryLabel = _globalLocalizer.GetString("Country"),
                DeliveryAddressEqualBillingAddressText = _globalLocalizer.GetString("DeliveryAddressEqualBillingAddressText"),
            };

            viewModel.ContactJobTitles = new List<ContactJobTitle>
            {
                new ContactJobTitle {
                    Name = _globalLocalizer.GetString("SalesRep").Name,
                    Value = _globalLocalizer.GetString("SalesRep").Value
                },
                new ContactJobTitle {
                    Name = _globalLocalizer.GetString("SalesManager").Name,
                    Value = _globalLocalizer.GetString("SalesManager").Value
                },
                new ContactJobTitle {
                    Name = _globalLocalizer.GetString("President").Name,
                    Value = _globalLocalizer.GetString("President").Value
                },
                new ContactJobTitle {
                    Name = _globalLocalizer.GetString("CEO").Name,
                    Value = _globalLocalizer.GetString("CEO").Value
                },
                new ContactJobTitle {
                    Name = _globalLocalizer.GetString("AccountManager").Name,
                    Value = _globalLocalizer.GetString("AccountManager").Value
                },
                new ContactJobTitle {
                    Name = _globalLocalizer.GetString("Owner").Name,
                    Value = _globalLocalizer.GetString("Owner").Value
                },
                new ContactJobTitle {
                    Name = _globalLocalizer.GetString("VicePresident").Name,
                    Value = _globalLocalizer.GetString("VicePresident").Value
                },
                new ContactJobTitle {
                    Name = _globalLocalizer.GetString("GeneralManager").Name,
                    Value = _globalLocalizer.GetString("GeneralManager").Value
                },
                new ContactJobTitle {
                    Name = _globalLocalizer.GetString("OperationsManager").Name,
                    Value = _globalLocalizer.GetString("OperationsManager").Value
                },
                new ContactJobTitle {
                    Name = _globalLocalizer.GetString("Other").Name,
                    Value = _globalLocalizer.GetString("Other").Value
                }
            };

            var languages = new List<LanguageViewModel>
            {
                new LanguageViewModel { Text = _globalLocalizer.GetString("SelectLanguage") , Value = string.Empty }
            };

            foreach (var language in _localizationOptions.CurrentValue.SupportedCultures.Split(','))
            {
                languages.Add(new LanguageViewModel { Text = language.ToUpperInvariant(), Value = language.ToLowerInvariant() });
            }

            viewModel.Languages = languages;

            if (componentModel.Id.HasValue)
            {
                var clientApplication = await _clientApplicationsRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (clientApplication is not null)
                {
                    viewModel.Id = clientApplication.Id;
                    viewModel.CompanyName = clientApplication.CompanyName;
                    viewModel.FirstName = clientApplication.FirstName;
                    viewModel.LastName = clientApplication.LastName;
                    viewModel.Email = clientApplication.Email;
                    viewModel.ContactJobTitle = clientApplication.ContactJobTitle;
                    viewModel.PhoneNumber = clientApplication.PhoneNumber;
                    viewModel.CommunicationLanguage = clientApplication.CommunicationLanguage;
                    viewModel.IsDeliveryAddressEqualBillingAddress = clientApplication.IsDeliveryAddressEqualBillingAddress;

                    viewModel.BillingAddress = new ClientApplicationAddressViewModel
                    {
                        Id = clientApplication.BillingAddress.Id,
                        FullName = clientApplication.BillingAddress.FullName,
                        PhoneNumber = clientApplication.BillingAddress.PhoneNumber,
                        Street = clientApplication.BillingAddress.Street,
                        Region = clientApplication.BillingAddress.Region,
                        PostalCode = clientApplication.BillingAddress.PostalCode,
                        City = clientApplication.BillingAddress.City,
                        Country = clientApplication.BillingAddress.Country
                    };

                    if (!clientApplication.IsDeliveryAddressEqualBillingAddress)
                    {
                        viewModel.DeliveryAddress = new ClientApplicationAddressViewModel
                        {
                            Id = clientApplication.DeliveryAddress.Id,
                            FullName = clientApplication.DeliveryAddress.FullName,
                            PhoneNumber = clientApplication.DeliveryAddress.PhoneNumber,
                            Street = clientApplication.DeliveryAddress.Street,
                            Region = clientApplication.DeliveryAddress.Region,
                            PostalCode = clientApplication.DeliveryAddress.PostalCode,
                            City = clientApplication.DeliveryAddress.City,
                            Country = clientApplication.DeliveryAddress.Country
                        };
                    }
                }
            }

            return viewModel;
        }
    }
}