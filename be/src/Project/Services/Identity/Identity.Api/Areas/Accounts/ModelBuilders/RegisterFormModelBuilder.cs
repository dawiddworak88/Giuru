using Feature.Account;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Identity.Api.Areas.Accounts.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
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
                LogisticalInformationTitle = this.accountLocalizer.GetString("LogisticalInformation"),
                FirstNameLabel = this.globalLocalizer.GetString("FirstName"),
                LastNameLabel = this.globalLocalizer.GetString("LastName"),
                EmailLabel = this.globalLocalizer.GetString("Email"),
                PhoneNumberLabel = this.globalLocalizer.GetString("PhoneNumberLabel"),
                FirstNameRequiredErrorMessage = this.accountLocalizer.GetString("FirstNameRequiredErrorMessage"),
                LastNameRequiredErrorMessage = this.accountLocalizer.GetString("LastNameRequiredErrorMessage"),
                EmailRequiredErrorMessage = this.globalLocalizer.GetString("EmailRequiredErrorMessage"),
                EmailFormatErrorMessage = this.globalLocalizer.GetString("EmailFormatErrorMessage"),
                PhoneRequiredErrorMessage = this.globalLocalizer.GetString("PhoneRequiredErrorMessage"),
                ContactJobTitleLabel = this.globalLocalizer.GetString("ContactJobTitleLabel"),
                ContactJobTitleRequiredErrorMessage = this.globalLocalizer.GetString("ContactJobTitleRequiredErrorMessage"),
                PostalCodeLabel = this.globalLocalizer.GetString("PostalCode"),
                PostalCodeRequiredErrorMessage = this.globalLocalizer.GetString("PostalCodeRequiredErrorMessage")
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
                },
                new StepViewModel
                {
                    Title = this.accountLocalizer.GetString("LogisticalInformation"),
                    Subtitle = this.accountLocalizer.GetString("LogisticalInformationDescription")
                }
            };

            return viewModel;
        }
    }
}
