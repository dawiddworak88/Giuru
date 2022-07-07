using Buyer.Web.Areas.Home.ViewModel.Application;
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

namespace Buyer.Web.Areas.Home.ModelBuilders.Application
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
                Title = this.globalLocalizer.GetString("EltapPartner"),
                Subtitle = this.globalLocalizer.GetString("EltapPartnerDescription"),
                ContactInformationTitle = this.globalLocalizer.GetString("ContactInformation"),
                BusinessInformationTitle = this.globalLocalizer.GetString("BusinessInformation"),
                FirstNameLabel = this.globalLocalizer.GetString("FirstName"),
                LastNameLabel = this.globalLocalizer.GetString("LastName"),
                EmailLabel = this.globalLocalizer.GetString("Email"),
                PhoneNumberLabel = this.globalLocalizer.GetString("PhoneNumberLabel"),
                EmailFormatErrorMessage = this.globalLocalizer.GetString("EmailFormatErrorMessage"),
                ContactJobTitleLabel = this.globalLocalizer.GetString("ContactJobTitle"),
                CompanyPostalCodeLabel = this.globalLocalizer.GetString("PostalCode"),
                FieldRequiredErrorMessage = this.globalLocalizer.GetString("FieldRequiredErrorMessage"),
                CompanyNameLabel = this.globalLocalizer.GetString("CompanyName"),
                CompanyAddressLabel = this.globalLocalizer.GetString("Address"),
                CompanyCityLabel = this.globalLocalizer.GetString("City"),
                CompanyRegionLabel = this.globalLocalizer.GetString("Region"),
                CompanyCountryLabel = this.globalLocalizer.GetString("Country"),
                SaveUrl = this.linkGenerator.GetPathByAction("Application", "ClientsApi", new { Area = "Home", culture = CultureInfo.CurrentUICulture.Name }),
                OnlineRetailersLabel = this.globalLocalizer.GetString("OnlineRetailers"),
                YesLabel = this.globalLocalizer.GetString("Yes"),
                NoLabel = this.globalLocalizer.GetString("No"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                SaveText = this.globalLocalizer.GetString("ApplyApplicationSubmit"),
                SelectJobTitle = this.globalLocalizer.GetString("SelectJobTitle"),
                SignInUrl = this.linkGenerator.GetPathByAction("Index", "Home", new { Area = "Home", culture = CultureInfo.CurrentUICulture.Name }),
                AcceptTermsText = this.cookieConsentLocalizer.GetString("Accept"),
                PrivacyPolicyUrl = $"{this.options.Value.IdentityUrl}{SecurityConstants.PrivacyPolicyEndpoint}",
                RegulationsUrl = $"{this.options.Value.IdentityUrl}{SecurityConstants.RegulationsEndpoint}",
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

            viewModel.Steps = new List<StepViewModel>
            {
                new StepViewModel
                {
                    Title = this.globalLocalizer.GetString("ContactInformation"),
                    Subtitle = this.globalLocalizer.GetString("ContactInformationDescription")
                },
                new StepViewModel
                {
                    Title = this.globalLocalizer.GetString("BusinessInformation"),
                    Subtitle = this.globalLocalizer.GetString("BusinessInformationDescription")
                }
            };

            return viewModel;
        }
    }
}
