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
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<CookieConsentResources> cookieConsentLocalizer;
        private readonly IOptions<AppSettings> options;
        private readonly LinkGenerator linkGenerator;
        private readonly IOptionsMonitor<LocalizationSettings> _localizationOptions;

        public ApplicationFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<CookieConsentResources> cookieConsentLocalizer,
            IOptions<AppSettings> options,
            LinkGenerator linkGenerator,
            IOptionsMonitor<LocalizationSettings> localizationOptions)
        {
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
            this.cookieConsentLocalizer = cookieConsentLocalizer;
            this.options = options;
            _localizationOptions = localizationOptions;
        }

        public async Task<ApplicationFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ApplicationFormViewModel
            {
                Title = this.globalLocalizer.GetString("ApplicationPartner"),
                Subtitle = this.globalLocalizer.GetString("ApplicationPartnerDescription"),
                ContactInformationTitle = this.globalLocalizer.GetString("ContactInformation"),
                FirstNameLabel = this.globalLocalizer.GetString("FirstName"),
                LastNameLabel = this.globalLocalizer.GetString("LastName"),
                EmailLabel = this.globalLocalizer.GetString("Email"),
                PhoneNumberLabel = this.globalLocalizer.GetString("PhoneNumberLabel"),
                LanguageLabel = this.globalLocalizer.GetString("CommunicationLanguageLabel"),
                EmailFormatErrorMessage = this.globalLocalizer.GetString("EmailFormatErrorMessage"),
                ContactJobTitleLabel = this.globalLocalizer.GetString("ContactJobTitle"),
                FieldRequiredErrorMessage = this.globalLocalizer.GetString("FieldRequiredErrorMessage"),
                CompanyNameLabel = this.globalLocalizer.GetString("CompanyName"),
                BillingAddressTitle = this.globalLocalizer.GetString("BillingAddressTitle"),
                DeliveryAddressTitle = this.globalLocalizer.GetString("DeliveryAddressTitle"),
                AddressFullNameLabel = $"{this.globalLocalizer.GetString("FirstName")} {this.globalLocalizer.GetString("LastName")}",
                AddressPhoneNumberLabel = this.globalLocalizer.GetString("PhoneNumberLabel"),
                AddressStreetLabel = this.globalLocalizer.GetString("Street"),
                AddressRegionLabel = this.globalLocalizer.GetString("Region"),
                AddressPostalCodeLabel = this.globalLocalizer.GetString("PostalCode"),
                AddressCityLabel = this.globalLocalizer.GetString("City"),
                AddressCountryLabel = this.globalLocalizer.GetString("Country"),
                SaveUrl = this.linkGenerator.GetPathByAction("Application", "ApplicationsApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                OnlineRetailersLabel = this.globalLocalizer.GetString("OnlineRetailers"),
                YesLabel = this.globalLocalizer.GetString("Yes"),
                NoLabel = this.globalLocalizer.GetString("No"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                SaveText = this.globalLocalizer.GetString("ApplyApplicationSubmit"),
                SelectJobTitle = this.globalLocalizer.GetString("SelectJobTitle"),
                SignInUrl = this.linkGenerator.GetPathByAction("Index", "Home", new { Area = "Home", culture = CultureInfo.CurrentUICulture.Name }),
                AcceptTermsText = this.cookieConsentLocalizer.GetString("Accept"),
                DeliveryAddressEqualBillingAddressText = this.globalLocalizer.GetString("DeliveryAddressEqualBillingAddressText"),
                PrivacyPolicyUrl = $"{options.Value.IdentityUrl}{SecurityConstants.PrivacyPolicyEndpoint}",
                RegulationsUrl = $"{options.Value.IdentityUrl}{SecurityConstants.RegulationsEndpoint}",
                PrivacyPolicy = this.globalLocalizer.GetString("LowerPrivacyPolicy"),
                Regulations = this.globalLocalizer.GetString("LowerRegulations")
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

            viewModel.Steps = new List<StepViewModel>
            {
                new StepViewModel
                {
                    Title = this.globalLocalizer.GetString("ContactInformation"),
                    Subtitle = this.globalLocalizer.GetString("ContactInformationDescription")
                },
                new StepViewModel
                {
                    Title = this.globalLocalizer.GetString("AdressesInformation"),
                    Subtitle = this.globalLocalizer.GetString("AdressesInformationDescription"),
                }
            };

            return viewModel;
        }
    }
}
