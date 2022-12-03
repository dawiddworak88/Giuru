using Client.Api.Configurations;
using Client.Api.Infrastructure;
using Client.Api.Infrastructure.Clients.Entities;
using Client.Api.ServicesModels.Applications;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.Mailing.Configurations;
using Foundation.Mailing.Models;
using Foundation.Mailing.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Client.Api.Services.Applications
{
    public class ClientsApplicationsService : IClientsApplicationsService
    {
        private readonly ClientContext context;
        private readonly IMailingService mailingService;
        private readonly IOptionsMonitor<AppSettings> options;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly IOptionsMonitor<MailingConfiguration> mailingOptions;
        public ClientsApplicationsService(
            ClientContext context,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IOptionsMonitor<MailingConfiguration> mailingOptions,
            IMailingService mailingService,
            IOptionsMonitor<AppSettings> options)
        {
            this.context = context;
            this.clientLocalizer = clientLocalizer;
            this.globalLocalizer = globalLocalizer;
            this.mailingOptions = mailingOptions;
            this.options = options;
            this.mailingService = mailingService;
        }

        public async Task<Guid> CreateAsync(CreateClientApplicationServiceModel model)
        {
            var clientApplication = new ClientsApplication
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                ContactJobTitle = model.ContactJobTitle,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CompanyName = model.CompanyName,
                CompanyAddress = model.CompanyAddress,
                CompanyCountry = model.CompanyCountry,
                CompanyCity = model.CompanyCity,
                CompanyRegion = model.CompanyRegion,
                CompanyPostalCode = model.CompanyPostalCode
            };

            await this.context.ClientsApplications.AddAsync(clientApplication.FillCommonProperties());
            await this.context.SaveChangesAsync();

            await this.mailingService.SendTemplateAsync(new TemplateEmail
            {
                RecipientEmailAddress = model.Email,
                RecipientName = model.FirstName + " " + model.LastName,
                SenderEmailAddress = this.mailingOptions.CurrentValue.SenderEmail,
                SenderName = this.mailingOptions.CurrentValue.SenderName,
                TemplateId = this.options.CurrentValue.ActionSendGridClientApplyConfirmationTemplateId,
                DynamicTemplateData = new
                {
                    welcomeLabel = this.globalLocalizer.GetString("Welcome").Value,
                    firstName = model.FirstName,
                    lastName = model.LastName,
                    subject = this.clientLocalizer.GetString("ClientApplyConfirmationSubject").Value,
                    lineOne = this.clientLocalizer.GetString("ClientApplyConfirmation").Value
                }
            });

            await this.mailingService.SendTemplateAsync(new TemplateEmail
            {
                RecipientEmailAddress = this.options.CurrentValue.ApplyRecipientEmail,
                RecipientName = this.mailingOptions.CurrentValue.SenderName,
                SenderEmailAddress = this.mailingOptions.CurrentValue.SenderEmail,
                SenderName = this.mailingOptions.CurrentValue.SenderName,
                TemplateId = this.options.CurrentValue.ActionSendGridClientApplyTemplateId,
                DynamicTemplateData = new
                {
                    firstName = model.FirstName,
                    lastName = model.LastName,
                    email = model.Email,
                    phoneNumberLabel = this.globalLocalizer.GetString("PhoneNumberLabel").Value,
                    phoneNumber = model.PhoneNumber,
                    subject = $"{model.CompanyName} - {model.FirstName} {model.LastName} - {this.clientLocalizer.GetString("ClientApplySubject").Value}",
                    contactInformation = this.globalLocalizer.GetString("ContactInformation").Value,
                    businessInformation = this.globalLocalizer.GetString("BusinessInformation").Value,
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

            return clientApplication.Id;
        }

        public async Task DeleteAsync(DeleteClientApplicationServiceModel model)
        {
            var clientApplication = await this.context.ClientsApplications.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (clientApplication is null)
            {
                throw new CustomException(this.clientLocalizer.GetString("ClientApplicationNotFound"), (int)HttpStatusCode.NoContent);
            }

            clientApplication.IsActive = false;

            await this.context.SaveChangesAsync();
        }

        public async Task<PagedResults<IEnumerable<ClientApplicationServiceModel>>> GetAsync(GetClientsApplicationsServiceModel model)
        {
            var clientsApplications = from c in this.context.ClientsApplications
                                      where c.IsActive
                                      select new ClientApplicationServiceModel
                                      {
                                          Id = c.Id,
                                          FirstName = c.FirstName,
                                          LastName = c.LastName,
                                          ContactJobTitle = c.ContactJobTitle,
                                          PhoneNumber = c.PhoneNumber,
                                          Email = c.Email,
                                          CompanyAddress = c.CompanyAddress,
                                          CompanyCity = c.CompanyCity,
                                          CompanyCountry = c.CompanyCountry,
                                          CompanyName = c.CompanyName,
                                          CompanyPostalCode = c.CompanyPostalCode,
                                          CompanyRegion = c.CompanyRegion,
                                          LastModifiedDate = c.LastModifiedDate,
                                          CreatedDate = c.CreatedDate
                                      };

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                clientsApplications = clientsApplications.Where(x => x.CompanyName.StartsWith(model.SearchTerm));
            }

            clientsApplications = clientsApplications.ApplySort(model.OrderBy);

            return clientsApplications.PagedIndex(new Pagination(clientsApplications.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<ClientApplicationServiceModel> GetAsync(GetClientApplicationServiceModel model)
        {
            var existingApplication = await this.context.ClientsApplications.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (existingApplication is null)
            {
                throw new CustomException(this.clientLocalizer.GetString("ClientApplicationNotFound"), (int)HttpStatusCode.NoContent);
            }

            var clientApplication = new ClientApplicationServiceModel
            {
                Id = existingApplication.Id,
                FirstName = existingApplication.FirstName,
                LastName = existingApplication.LastName,
                Email = existingApplication.Email,
                PhoneNumber = existingApplication.PhoneNumber,
                ContactJobTitle = existingApplication.ContactJobTitle,
                CompanyAddress = existingApplication.CompanyAddress,
                CompanyCity = existingApplication.CompanyCity,
                CompanyCountry = existingApplication.CompanyCountry,
                CompanyName = existingApplication.CompanyName,
                CompanyPostalCode = existingApplication.CompanyPostalCode,
                CompanyRegion = existingApplication.CompanyRegion,
                LastModifiedDate = existingApplication.LastModifiedDate,
                CreatedDate = existingApplication.CreatedDate
            };

            return clientApplication;
        }

        public async Task<PagedResults<IEnumerable<ClientApplicationServiceModel>>> GetByIds(GetClientsApplicationsByIdsServiceModel model)
        {
            var clientsApplications = from c in this.context.ClientsApplications
                                      where model.Ids.Contains(c.Id) && c.IsActive
                                      select new ClientApplicationServiceModel
                                      {
                                          Id = c.Id,
                                          FirstName = c.FirstName,
                                          LastName = c.LastName,
                                          ContactJobTitle = c.ContactJobTitle,
                                          PhoneNumber = c.PhoneNumber,
                                          Email = c.Email,
                                          CompanyAddress = c.CompanyAddress,
                                          CompanyCity = c.CompanyCity,
                                          CompanyCountry = c.CompanyCountry,
                                          CompanyName = c.CompanyName,
                                          CompanyPostalCode = c.CompanyPostalCode,
                                          CompanyRegion = c.CompanyRegion,
                                          LastModifiedDate = c.LastModifiedDate,
                                          CreatedDate = c.CreatedDate
                                      };

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                clientsApplications = clientsApplications.Where(x => x.CompanyName.StartsWith(model.SearchTerm));
            }

            clientsApplications = clientsApplications.ApplySort(model.OrderBy);

            return clientsApplications.PagedIndex(new Pagination(clientsApplications.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<Guid> UpdateAsync(UpdateClientApplicationServiceModel model)
        {
            var clientApplication = await this.context.ClientsApplications.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (clientApplication == null)
            {
                throw new CustomException(this.clientLocalizer.GetString("ClientApplicationNotFound"), (int)HttpStatusCode.NoContent);
            }

            clientApplication.FirstName = model.FirstName;
            clientApplication.LastName = model.LastName;
            clientApplication.ContactJobTitle = model.ContactJobTitle;
            clientApplication.Email = model.Email;
            clientApplication.PhoneNumber = model.PhoneNumber;
            clientApplication.CompanyCity = model.CompanyCity;
            clientApplication.CompanyPostalCode = model.CompanyPostalCode;
            clientApplication.CompanyRegion = model.CompanyRegion;
            clientApplication.CompanyCountry = model.CompanyCountry;
            clientApplication.CompanyAddress = model.CompanyAddress;
            clientApplication.CompanyName = model.CompanyName;

            await this.context.SaveChangesAsync();

            return clientApplication.Id;
        }
    }
}
