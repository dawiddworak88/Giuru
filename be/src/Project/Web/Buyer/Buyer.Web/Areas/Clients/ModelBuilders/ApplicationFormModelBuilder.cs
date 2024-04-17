using Buyer.Web.Areas.Clients.ViewModels;
using Buyer.Web.Shared.Configurations;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.Localization.Definitions;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Languages.ViewModels;
using Foundation.Security.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Clients.ModelBuilders
{
    public class ApplicationFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ApplicationFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<CookieConsentResources> _cookieConsentLocalizer;
        private readonly IOptions<AppSettings> _options;
        private readonly LinkGenerator _linkGenerator;
        private readonly IOptionsMonitor<LocalizationSettings> _localizationOptions;

        public ApplicationFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<CookieConsentResources> cookieConsentLocalizer,
            IOptions<AppSettings> options,
            LinkGenerator linkGenerator,
            IOptionsMonitor<LocalizationSettings> localizationOptions)
        {
            _globalLocalizer = globalLocalizer;
            _linkGenerator = linkGenerator;
            _cookieConsentLocalizer = cookieConsentLocalizer;
            _options = options;
            _localizationOptions = localizationOptions;
        }

        public async Task<ApplicationFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ApplicationFormViewModel
            {
                Title = _globalLocalizer.GetString("ApplicationPartner"),
                Subtitle = _globalLocalizer.GetString("ApplicationPartnerDescription"),
                ContactInformationTitle = _globalLocalizer.GetString("ContactInformation"),
                FirstNameLabel = _globalLocalizer.GetString("FirstName"),
                LastNameLabel = _globalLocalizer.GetString("LastName"),
                EmailLabel = _globalLocalizer.GetString("Email"),
                PhoneNumberLabel = _globalLocalizer.GetString("PhoneNumberLabel"),
                LanguageLabel = _globalLocalizer.GetString("CommunicationLanguageLabel"),
                EmailFormatErrorMessage = _globalLocalizer.GetString("EmailFormatErrorMessage"),
                ContactJobTitleLabel = _globalLocalizer.GetString("ContactJobTitle"),
                FieldRequiredErrorMessage = _globalLocalizer.GetString("FieldRequiredErrorMessage"),
                CompanyNameLabel = _globalLocalizer.GetString("CompanyName"),
                BillingAddressTitle = _globalLocalizer.GetString("BillingAddressTitle"),
                DeliveryAddressTitle = _globalLocalizer.GetString("DeliveryAddressTitle"),
                AddressFullNameLabel = $"{_globalLocalizer.GetString("FirstName")} {_globalLocalizer.GetString("LastName")}",
                AddressPhoneNumberLabel = _globalLocalizer.GetString("PhoneNumberLabel"),
                AddressStreetLabel = _globalLocalizer.GetString("Street"),
                AddressRegionLabel = _globalLocalizer.GetString("Region"),
                AddressPostalCodeLabel = _globalLocalizer.GetString("PostalCode"),
                AddressCityLabel = _globalLocalizer.GetString("City"),
                AddressCountryLabel = _globalLocalizer.GetString("Country"),
                SaveUrl = _linkGenerator.GetPathByAction("Application", "ApplicationsApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                OnlineRetailersLabel = _globalLocalizer.GetString("OnlineRetailers"),
                YesLabel = _globalLocalizer.GetString("Yes"),
                NoLabel = _globalLocalizer.GetString("No"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                SaveText = _globalLocalizer.GetString("ApplyApplicationSubmit"),
                SelectJobTitle = _globalLocalizer.GetString("SelectJobTitle"),
                SignInUrl = _linkGenerator.GetPathByAction("Index", "Home", new { Area = "Home", culture = CultureInfo.CurrentUICulture.Name }),
                AcceptTermsText = _cookieConsentLocalizer.GetString("Accept"),
                DeliveryAddressEqualBillingAddressText = _globalLocalizer.GetString("DeliveryAddressEqualBillingAddressText"),
                PrivacyPolicyUrl = $"{_options.Value.IdentityUrl}{SecurityConstants.PrivacyPolicyEndpoint}",
                RegulationsUrl = $"{_options.Value.IdentityUrl}{SecurityConstants.RegulationsEndpoint}",
                PrivacyPolicy = _globalLocalizer.GetString("LowerPrivacyPolicy"),
                Regulations = _globalLocalizer.GetString("LowerRegulations")
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

            viewModel.Steps = new List<StepViewModel>
            {
                new StepViewModel
                {
                    Title = _globalLocalizer.GetString("ContactInformation"),
                    Subtitle = _globalLocalizer.GetString("ContactInformationDescription")
                },
                new StepViewModel
                {
                    Title = _globalLocalizer.GetString("AdressesInformation"),
                    Subtitle = _globalLocalizer.GetString("AdressesInformationDescription"),
                }
            };

            return viewModel;
        }
    }
}
