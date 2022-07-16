using Buyer.Web.Areas.Clients.ViewModels;
using Buyer.Web.Shared.Configurations;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
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

        public ApplicationFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<CookieConsentResources> cookieConsentLocalizer,
            IOptions<AppSettings> options,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
            this.cookieConsentLocalizer = cookieConsentLocalizer;
            this.options = options;
        }

        public async Task<ApplicationFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ApplicationFormViewModel
            {
                Title = globalLocalizer.GetString("EltapPartner"),
                Subtitle = globalLocalizer.GetString("EltapPartnerDescription"),
                ContactInformationTitle = globalLocalizer.GetString("ContactInformation"),
                BusinessInformationTitle = globalLocalizer.GetString("BusinessInformation"),
                FirstNameLabel = globalLocalizer.GetString("FirstName"),
                LastNameLabel = globalLocalizer.GetString("LastName"),
                EmailLabel = globalLocalizer.GetString("Email"),
                PhoneNumberLabel = globalLocalizer.GetString("PhoneNumberLabel"),
                EmailFormatErrorMessage = globalLocalizer.GetString("EmailFormatErrorMessage"),
                ContactJobTitleLabel = globalLocalizer.GetString("ContactJobTitle"),
                CompanyPostalCodeLabel = globalLocalizer.GetString("PostalCode"),
                FieldRequiredErrorMessage = globalLocalizer.GetString("FieldRequiredErrorMessage"),
                CompanyNameLabel = globalLocalizer.GetString("CompanyName"),
                CompanyAddressLabel = globalLocalizer.GetString("Address"),
                CompanyCityLabel = globalLocalizer.GetString("City"),
                CompanyRegionLabel = globalLocalizer.GetString("Region"),
                CompanyCountryLabel = globalLocalizer.GetString("Country"),
                SaveUrl = linkGenerator.GetPathByAction("Application", "ApplicationsApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                OnlineRetailersLabel = globalLocalizer.GetString("OnlineRetailers"),
                YesLabel = globalLocalizer.GetString("Yes"),
                NoLabel = globalLocalizer.GetString("No"),
                GeneralErrorMessage = globalLocalizer.GetString("AnErrorOccurred"),
                SaveText = globalLocalizer.GetString("ApplyApplicationSubmit"),
                SelectJobTitle = globalLocalizer.GetString("SelectJobTitle"),
                SignInUrl = linkGenerator.GetPathByAction("Index", "Home", new { Area = "Home", culture = CultureInfo.CurrentUICulture.Name }),
                AcceptTermsText = cookieConsentLocalizer.GetString("Accept"),
                PrivacyPolicyUrl = $"{options.Value.IdentityUrl}{SecurityConstants.PrivacyPolicyEndpoint}",
                RegulationsUrl = $"{options.Value.IdentityUrl}{SecurityConstants.RegulationsEndpoint}",
                PrivacyPolicy = globalLocalizer.GetString("LowerPrivacyPolicy"),
                Regulations = globalLocalizer.GetString("LowerRegulations")
            };

            viewModel.ContactJobTitles = new List<ContactJobTitle>
            {
                new ContactJobTitle {
                    Name = globalLocalizer.GetString("SalesRep").Name,
                    Value = globalLocalizer.GetString("SalesRep").Value
                },
                new ContactJobTitle {
                    Name = globalLocalizer.GetString("SalesManager").Name,
                    Value = globalLocalizer.GetString("SalesManager").Value
                },
                new ContactJobTitle {
                    Name = globalLocalizer.GetString("President").Name,
                    Value = globalLocalizer.GetString("President").Value
                },
                new ContactJobTitle {
                    Name = globalLocalizer.GetString("CEO").Name,
                    Value = globalLocalizer.GetString("CEO").Value
                },
                new ContactJobTitle {
                    Name = globalLocalizer.GetString("AccountManager").Name,
                    Value = globalLocalizer.GetString("AccountManager").Value
                },
                new ContactJobTitle {
                    Name = globalLocalizer.GetString("Owner").Name,
                    Value = globalLocalizer.GetString("Owner").Value
                },
                new ContactJobTitle {
                    Name = globalLocalizer.GetString("VicePresident").Name,
                    Value = globalLocalizer.GetString("VicePresident").Value
                },
                new ContactJobTitle {
                    Name = globalLocalizer.GetString("GeneralManager").Name,
                    Value = globalLocalizer.GetString("GeneralManager").Value
                },
                new ContactJobTitle {
                    Name = globalLocalizer.GetString("OperationsManager").Name,
                    Value = globalLocalizer.GetString("OperationsManager").Value
                },
                new ContactJobTitle {
                    Name = globalLocalizer.GetString("Other").Name,
                    Value = globalLocalizer.GetString("Other").Value
                }
            };

            viewModel.Steps = new List<StepViewModel>
            {
                new StepViewModel
                {
                    Title = globalLocalizer.GetString("ContactInformation"),
                    Subtitle = globalLocalizer.GetString("ContactInformationDescription")
                },
                new StepViewModel
                {
                    Title = globalLocalizer.GetString("BusinessInformation"),
                    Subtitle = globalLocalizer.GetString("BusinessInformationDescription")
                }
            };

            return viewModel;
        }
    }
}
