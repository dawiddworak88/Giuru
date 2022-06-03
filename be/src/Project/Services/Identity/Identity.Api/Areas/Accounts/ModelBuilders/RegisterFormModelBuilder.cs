using Feature.Account;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Identity.Api.Areas.Accounts.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.ModelBuilders
{
    public class RegisterFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, RegisterFormViewModel>
    {
        private readonly IStringLocalizer<AccountResources> accountLocalizer;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly LinkGenerator linkGenerator;

        public RegisterFormModelBuilder(
            IStringLocalizer<AccountResources> accountLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator)
        {
            this.accountLocalizer = accountLocalizer;
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<RegisterFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new RegisterFormViewModel
            {
                Title = this.globalLocalizer.GetString("EltapPartner"),
                Subtitle = this.globalLocalizer.GetString("EltapPartnerDescription"),
                ContactInformationTitle = this.accountLocalizer.GetString("ContactInformation"),
                BusinessInformationTitle = this.accountLocalizer.GetString("BusinessInformation"),
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
                SaveUrl = this.linkGenerator.GetPathByAction("Register", "IdentityApi", new { Area = "Accounts", culture = CultureInfo.CurrentUICulture.Name }),
                OnlineRetailersLabel = this.accountLocalizer.GetString("OnlineRetailers"),
                YesLabel = this.globalLocalizer.GetString("Yes"),
                NoLabel = this.globalLocalizer.GetString("No"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                SaveText = this.accountLocalizer.GetString("ApplySubmit"),
                SelectJobTitle = this.accountLocalizer.GetString("SelectJobTitle"),
                SignInUrl = this.linkGenerator.GetPathByAction("Index", "SignIn", new { Area = "Accounts", culture = CultureInfo.CurrentUICulture.Name })
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
                    Title = this.accountLocalizer.GetString("ContactInformation"),
                    Subtitle = this.accountLocalizer.GetString("ContactInformationDescription")
                },
                new StepViewModel
                {
                    Title = this.accountLocalizer.GetString("BusinessInformation"),
                    Subtitle = this.accountLocalizer.GetString("BusinessInformationDescription")
                }
            };

            return viewModel;
        }
    }
}
