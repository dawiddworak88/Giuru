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
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly IClientApplicationsRepository clientApplicationsRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IOptionsMonitor<LocalizationSettings> _localizationOptions;

        public ClientApplicationFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientApplicationsRepository clientApplicationsRepository,
            LinkGenerator linkGenerator,
            IOptionsMonitor<LocalizationSettings> localizationOptions)
        {
            this.globalLocalizer = globalLocalizer;
            this.clientLocalizer = clientLocalizer;
            this.clientApplicationsRepository = clientApplicationsRepository;
            this.linkGenerator = linkGenerator;
            _localizationOptions = localizationOptions;
        }

        public async Task<ClientApplicationFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientApplicationFormViewModel
            {
                Title = this.clientLocalizer.GetString("EditClientApplication"),
                IdLabel = this.globalLocalizer.GetString("Id"),
                FirstNameLabel = this.globalLocalizer.GetString("FirstName"),
                LastNameLabel = this.globalLocalizer.GetString("LastName"),
                EmailLabel = this.globalLocalizer.GetString("Email"),
                ContactJobTitleLabel = this.globalLocalizer.GetString("ContactJobTitle"),
                PhoneNumberLabel = this.globalLocalizer.GetString("PhoneNumberLabel"),
                CompanyNameLabel = this.globalLocalizer.GetString("CompanyName"),
                BackToClientsApplicationsText = this.clientLocalizer.GetString("BackToClientsApplications"),
                ClientsApplicationsUrl = this.linkGenerator.GetPathByAction("Index", "ClientApplications", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                FieldRequiredErrorMessage = this.globalLocalizer.GetString("FieldRequiredErrorMessage"),
                EmailFormatErrorMessage = this.globalLocalizer.GetString("EmailFormatErrorMessage"),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "ClientsApplicationApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                SelectJobTitle = this.globalLocalizer.GetString("SelectJobTitle"),
                LanguageLabel = this.globalLocalizer.GetString("CommunicationLanguageLabel"),
                BillingAddressTitle = this.globalLocalizer.GetString("BillingAddressTitle"),
                DeliveryAddressTitle = this.globalLocalizer.GetString("DeliveryAddressTitle"),
                AddressFullNameLabel = $"{this.globalLocalizer.GetString("FirstName")} {this.globalLocalizer.GetString("LastName")}",
                AddressPhoneNumberLabel = this.globalLocalizer.GetString("PhoneNumberLabel"),
                AddressStreetLabel = this.globalLocalizer.GetString("Street"),
                AddressRegionLabel = this.globalLocalizer.GetString("Region"),
                AddressPostalCodeLabel = this.globalLocalizer.GetString("PostalCode"),
                AddressCityLabel = this.globalLocalizer.GetString("City"),
                AddressCountryLabel = this.globalLocalizer.GetString("Country"),
                DeliveryAddressEqualBillingAddressText = this.globalLocalizer.GetString("DeliveryAddressEqualBillingAddressText"),
            };

            viewModel.ContactJobTitles = new List<ContactJobTitle>
            {
                new ContactJobTitle {
                    Name = this.globalLocalizer.GetString("SalesRep").Name,
                    Value = this.globalLocalizer.GetString("SalesRep").Value
                },
                new ContactJobTitle {
                    Name = this.globalLocalizer.GetString("SalesManager").Name,
                    Value = this.globalLocalizer.GetString("SalesManager").Value
                },
                new ContactJobTitle {
                    Name = this.globalLocalizer.GetString("President").Name,
                    Value = this.globalLocalizer.GetString("President").Value
                },
                new ContactJobTitle {
                    Name = this.globalLocalizer.GetString("CEO").Name,
                    Value = this.globalLocalizer.GetString("CEO").Value
                },
                new ContactJobTitle {
                    Name = this.globalLocalizer.GetString("AccountManager").Name,
                    Value = this.globalLocalizer.GetString("AccountManager").Value
                },
                new ContactJobTitle {
                    Name = this.globalLocalizer.GetString("Owner").Name,
                    Value = this.globalLocalizer.GetString("Owner").Value
                },
                new ContactJobTitle {
                    Name = this.globalLocalizer.GetString("VicePresident").Name,
                    Value = this.globalLocalizer.GetString("VicePresident").Value
                },
                new ContactJobTitle {
                    Name = this.globalLocalizer.GetString("GeneralManager").Name,
                    Value = this.globalLocalizer.GetString("GeneralManager").Value
                },
                new ContactJobTitle {
                    Name = this.globalLocalizer.GetString("OperationsManager").Name,
                    Value = this.globalLocalizer.GetString("OperationsManager").Value
                },
                new ContactJobTitle {
                    Name = this.globalLocalizer.GetString("Other").Name,
                    Value = this.globalLocalizer.GetString("Other").Value
                }
            };

            var languages = new List<LanguageViewModel>
            {
                new LanguageViewModel { Text = this.globalLocalizer.GetString("SelectLanguage") , Value = string.Empty }
            };

            foreach (var language in _localizationOptions.CurrentValue.SupportedCultures.Split(','))
            {
                languages.Add(new LanguageViewModel { Text = language.ToUpperInvariant(), Value = language.ToLowerInvariant() });
            }

            viewModel.Languages = languages;

            if (componentModel.Id.HasValue)
            {
                var clientApplication = await this.clientApplicationsRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

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

            return viewModel;
        }
    }
}