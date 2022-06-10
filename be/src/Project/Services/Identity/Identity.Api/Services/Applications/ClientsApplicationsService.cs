using Feature.Account;
using Foundation.Localization;
using Foundation.Mailing.Configurations;
using Foundation.Mailing.Models;
using Foundation.Mailing.Services;
using Identity.Api.Configurations;
using Identity.Api.ServicesModels.Applications;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Identity.Api.Services.Applications
{
    public class ClientsApplicationsService : IClientsApplicationsService
    {
        private readonly IMailingService mailingService;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IOptionsMonitor<MailingConfiguration> mailingOptions;
        private readonly IOptionsMonitor<AppSettings> identityOptions;
        private readonly IStringLocalizer<AccountResources> accountLocalizer;

        public ClientsApplicationsService(
            IMailingService mailingService,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IOptionsMonitor<MailingConfiguration> mailingOptions,
            IOptionsMonitor<AppSettings> identityOptions,
            IStringLocalizer<AccountResources> accountLocalizer) 
        { 
            this.mailingService = mailingService;
            this.globalLocalizer = globalLocalizer;
            this.mailingOptions = mailingOptions;
            this.identityOptions = identityOptions;
            this.accountLocalizer = accountLocalizer;
        }

        public async Task CreateAsync(CreateClientApplicationServiceModel model)
        {
            await this.mailingService.SendTemplateAsync(new TemplateEmail
            {
                RecipientEmailAddress = model.Email,
                RecipientName = model.FirstName + " " + model.LastName,
                SenderEmailAddress = this.mailingOptions.CurrentValue.SenderEmail,
                SenderName = this.mailingOptions.CurrentValue.SenderName,
                TemplateId = this.identityOptions.CurrentValue.ActionSendGridClientApplyConfirmationTemplateId,
                DynamicTemplateData = new
                {
                    welcomeLabel = this.globalLocalizer.GetString("Welcome").Value,
                    firstName = model.FirstName,
                    lastName = model.LastName,
                    subject = this.accountLocalizer.GetString("ClientApplyConfirmationSubject").Value,
                    lineOne = this.accountLocalizer.GetString("ClientApplyConfirmation").Value
                }
            });

            await this.mailingService.SendTemplateAsync(new TemplateEmail
            {
                RecipientEmailAddress = this.identityOptions.CurrentValue.ApplyRecipientEmail,
                RecipientName = this.mailingOptions.CurrentValue.SenderName,
                SenderEmailAddress = this.mailingOptions.CurrentValue.SenderEmail,
                SenderName = this.mailingOptions.CurrentValue.SenderName,
                TemplateId = this.identityOptions.CurrentValue.ActionSendGridClientApplyTemplateId,
                DynamicTemplateData = new
                {
                    firstName = model.FirstName,
                    lastName = model.LastName,
                    email = model.Email,
                    phoneNumberLabel = this.globalLocalizer.GetString("PhoneNumberLabel").Value,
                    phoneNumber = model.PhoneNumber,
                    subject = $"{model.CompanyName} - {model.FirstName} {model.LastName} - {this.accountLocalizer.GetString("ClientApplySubject").Value}",
                    contactInformation = this.accountLocalizer.GetString("ContactInformation").Value,
                    businessInformation = this.accountLocalizer.GetString("BusinessInformation").Value,
                    firstNameLabel = this.globalLocalizer.GetString("FirstName").Value,
                    lastNameLabel = this.globalLocalizer.GetString("LastName").Value,
                    companyNameLabel = this.globalLocalizer.GetString("CompanyName").Value,
                    companyName = model.CompanyName,
                    addressLabel = this.globalLocalizer.GetString("Address").Value,
                    address = model.CompanyAddress,
                    cityLabel = this.globalLocalizer.GetString("City").Value,
                    city = model.CompanyCity,
                    regionLabel = this.globalLocalizer.GetString("Region").Value,
                    region = model.CompanyRegion,
                    postalCodeLabel = this.globalLocalizer.GetString("PostalCode").Value,
                    postalCode = model.CompanyPostalCode,
                    contactJobLabel = this.globalLocalizer.GetString("ContactJobTitle").Value,
                    contactJobTitle = model.ContactJobTitle,
                    countryLabel = this.globalLocalizer.GetString("Country").Value,
                    country = model.CompanyCountry
                }
            });
        }
    }
}
