using Client.Api.Configurations;
using Client.Api.Infrastructure;
using Client.Api.Infrastructure.Clients.Entities;
using Client.Api.ServicesModels.Applications;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
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
        private readonly ClientContext _context;
        private readonly IMailingService _mailingService;
        private readonly IOptionsMonitor<AppSettings> _options;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IOptionsMonitor<MailingConfiguration> _mailingOptions;
        public ClientsApplicationsService(
            ClientContext context,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IOptionsMonitor<MailingConfiguration> mailingOptions,
            IMailingService mailingService,
            IOptionsMonitor<AppSettings> options)
        {
            _context = context;
            _clientLocalizer = clientLocalizer;
            _globalLocalizer = globalLocalizer;
            _mailingOptions = mailingOptions;
            _options = options;
            _mailingService = mailingService;
        }

        public async Task<Guid> CreateAsync(CreateClientApplicationServiceModel model)
        {
            var clientApplication = new ClientsApplication
            {
                CompanyName = model.CompanyName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ContactJobTitle = model.ContactJobTitle,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CommunicationLanguage = model.CommunicationLanguage,
                IsDeliveryAddressEqualBillingAddress = model.IsDeliveryAddressEqualBillingAddress
            };

            if (model.IsDeliveryAddressEqualBillingAddress)
            {
                var addresses = new ClientsApplicationAddress
                {
                    FullName = model.BillingAddress.FullName,
                    PhoneNumber = model.BillingAddress.PhoneNumber,
                    Street = model.BillingAddress.Street,
                    Region = model.BillingAddress.Region,
                    PostalCode = model.BillingAddress.PostalCode,
                    City = model.BillingAddress.City,
                    Country = model.BillingAddress.Country
                };

                await _context.ClientsApplicationAddresses.AddAsync(addresses.FillCommonProperties());

                clientApplication.BillingAddressId = addresses.Id;
            }
            else
            {
                var bilingAddress = new ClientsApplicationAddress
                {
                    FullName = model.BillingAddress.FullName,
                    PhoneNumber = model.BillingAddress.PhoneNumber,
                    Street = model.BillingAddress.Street,
                    Region = model.BillingAddress.Region,
                    PostalCode = model.BillingAddress.PostalCode,
                    City = model.BillingAddress.City,
                    Country = model.BillingAddress.Country
                };

                var deliveryAddress = new ClientsApplicationAddress
                {
                    FullName = model.DeliveryAddress.FullName,
                    PhoneNumber = model.DeliveryAddress.PhoneNumber,
                    Street = model.DeliveryAddress.Street,
                    PostalCode = model.DeliveryAddress.PostalCode,
                    Region = model.DeliveryAddress.Region,
                    City = model.DeliveryAddress.City,
                    Country = model.DeliveryAddress.Country
                };

                await _context.ClientsApplicationAddresses.AddAsync(bilingAddress.FillCommonProperties());
                await _context.ClientsApplicationAddresses.AddAsync(deliveryAddress.FillCommonProperties());

                clientApplication.BillingAddressId = bilingAddress.Id;
                clientApplication.DeliveryAddressId = deliveryAddress.Id;
            }

            await _context.ClientsApplications.AddAsync(clientApplication.FillCommonProperties());
            await _context.SaveChangesAsync();

            await _mailingService.SendTemplateAsync(new TemplateEmail
            {
                RecipientEmailAddress = model.Email,
                RecipientName = model.FirstName + " " + model.LastName,
                SenderEmailAddress = _mailingOptions.CurrentValue.SenderEmail,
                SenderName = _mailingOptions.CurrentValue.SenderName,
                TemplateId = _options.CurrentValue.ActionSendGridClientApplyConfirmationTemplateId,
                DynamicTemplateData = new
                {
                    welcomeLabel = _globalLocalizer.GetString("Welcome").Value,
                    firstName = model.FirstName,
                    lastName = model.LastName,
                    subject = _clientLocalizer.GetString("ClientApplyConfirmationSubject").Value,
                    lineOne = _clientLocalizer.GetString("ClientApplyConfirmation").Value
                }
            });

            await _mailingService.SendTemplateAsync(new TemplateEmail
            {
                RecipientEmailAddress = _options.CurrentValue.ApplyRecipientEmail,
                RecipientName = _mailingOptions.CurrentValue.SenderName,
                SenderEmailAddress = _mailingOptions.CurrentValue.SenderEmail,
                SenderName = _mailingOptions.CurrentValue.SenderName,
                TemplateId = _options.CurrentValue.ActionSendGridClientApplyTemplateId,
                DynamicTemplateData = new
                {
                    firstName = model.FirstName,
                    lastName = model.LastName,
                    email = model.Email,
                    phoneNumberLabel = _globalLocalizer.GetString("PhoneNumberLabel").Value,
                    phoneNumber = model.PhoneNumber,
                    subject = $"{model.CompanyName} - {model.FirstName} {model.LastName} - {_clientLocalizer.GetString("ClientApplySubject").Value}",
                    contactInformation = _globalLocalizer.GetString("ContactInformation").Value,
                    businessInformation = _globalLocalizer.GetString("BusinessInformation").Value,
                    firstNameLabel = _globalLocalizer.GetString("FirstName").Value,
                    lastNameLabel = _globalLocalizer.GetString("LastName").Value,
                    companyNameLabel = _globalLocalizer.GetString("CompanyName").Value,
                    companyName = model.CompanyName,
                    addressLabel = _globalLocalizer.GetString("Address").Value,
                    address = model.BillingAddress.Street,
                    cityLabel = _globalLocalizer.GetString("City").Value,
                    city = model.BillingAddress.City,
                    regionLabel = _globalLocalizer.GetString("Region").Value,
                    region = model.BillingAddress.Region,
                    postalCodeLabel = _globalLocalizer.GetString("PostalCode").Value,
                    postalCode = model.BillingAddress.PostalCode,
                    contactJobLabel = _globalLocalizer.GetString("ContactJobTitle").Value,
                    contactJobTitle = model.ContactJobTitle,
                    countryLabel = _globalLocalizer.GetString("Country").Value,
                    country = model.BillingAddress.Country
                }
            });

            return clientApplication.Id;
        }

        public async Task DeleteAsync(DeleteClientApplicationServiceModel model)
        {
            var clientApplication = await _context.ClientsApplications.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (clientApplication is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("ClientApplicationNotFound"));
            }

            if (clientApplication.IsDeliveryAddressEqualBillingAddress)
            {
                var addresses = await _context.ClientsApplicationAddresses.FirstOrDefaultAsync(x => x.Id == clientApplication.BillingAddressId && x.IsActive);

                addresses.IsActive = false;
            }
            else
            {
                var billingAddress = await _context.ClientsApplicationAddresses.FirstOrDefaultAsync(x => x.Id == clientApplication.BillingAddressId && x.IsActive);

                billingAddress.IsActive = false;

                var deliveryAddress = await _context.ClientsApplicationAddresses.FirstOrDefaultAsync(x => x.Id == clientApplication.DeliveryAddressId && x.IsActive);

                deliveryAddress.IsActive = false;
            }

            clientApplication.IsActive = false;

            await _context.SaveChangesAsync();
        }

        public async Task<PagedResults<IEnumerable<ClientApplicationServiceModel>>> GetAsync(GetClientsApplicationsServiceModel model)
        {
            var clientsApplications = from c in _context.ClientsApplications
                                      where c.IsActive
                                      select new ClientApplicationServiceModel
                                      {
                                          Id = c.Id,
                                          CompanyName = c.CompanyName,
                                          FirstName = c.FirstName,
                                          LastName = c.LastName,
                                          ContactJobTitle = c.ContactJobTitle,
                                          PhoneNumber = c.PhoneNumber,
                                          Email = c.Email,
                                          LastModifiedDate = c.LastModifiedDate,
                                          CreatedDate = c.CreatedDate
                                      };

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                clientsApplications = clientsApplications.Where(x => x.CompanyName.StartsWith(model.SearchTerm));
            }

            clientsApplications = clientsApplications.ApplySort(model.OrderBy);

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                clientsApplications = clientsApplications.Take(Constants.MaxItemsPerPageLimit);

                return clientsApplications.PagedIndex(new Pagination(clientsApplications.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }

            return clientsApplications.PagedIndex(new Pagination(clientsApplications.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
        }

        public async Task<ClientApplicationServiceModel> GetAsync(GetClientApplicationServiceModel model)
        {
            var existingApplication = await _context.ClientsApplications.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (existingApplication is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("ClientApplicationNotFound"));
            }

            var clientApplication = new ClientApplicationServiceModel
            {
                Id = existingApplication.Id,
                CompanyName = existingApplication.CompanyName,
                FirstName = existingApplication.FirstName,
                LastName = existingApplication.LastName,
                Email = existingApplication.Email,
                PhoneNumber = existingApplication.PhoneNumber,
                CommunicationLanguage = existingApplication.CommunicationLanguage,
                ContactJobTitle = existingApplication.ContactJobTitle,
                LastModifiedDate = existingApplication.LastModifiedDate,
                CreatedDate = existingApplication.CreatedDate,
                IsDeliveryAddressEqualBillingAddress = existingApplication.IsDeliveryAddressEqualBillingAddress,
            };

            if (existingApplication.IsDeliveryAddressEqualBillingAddress)
            {
                var address = await _context.ClientsApplicationAddresses.FirstOrDefaultAsync(x => x.Id == existingApplication.BillingAddressId && x.IsActive);

                var addresses = new ClientApplicationAddressServiceModel
                {
                    Id = address.Id,
                    FullName = address.FullName,
                    PhoneNumber = address.PhoneNumber,
                    Region = address.Region,
                    Street = address.Street,
                    PostalCode = address.PostalCode,
                    City = address.City,
                    Country = address.Country
                };

                clientApplication.BillingAddress = addresses;
            }
            else
            {
                var billingAddress = await _context.ClientsApplicationAddresses.FirstOrDefaultAsync(x => x.Id == existingApplication.BillingAddressId && x.IsActive);

                if (billingAddress is not null)
                {
                    clientApplication.BillingAddress = new ClientApplicationAddressServiceModel
                    {
                        Id = billingAddress.Id,
                        FullName = billingAddress.FullName,
                        PhoneNumber = billingAddress.PhoneNumber,
                        Region = billingAddress.Region,
                        Street = billingAddress.Street,
                        PostalCode = billingAddress.PostalCode,
                        City = billingAddress.City,
                        Country = billingAddress.Country
                    };
                }

                var deliveryAddress = await _context.ClientsApplicationAddresses.FirstOrDefaultAsync(x => x.Id == existingApplication.DeliveryAddressId && x.IsActive);

                if (deliveryAddress is not null) 
                {
                    clientApplication.DeliveryAddress = new ClientApplicationAddressServiceModel
                    {
                        Id = deliveryAddress.Id,
                        FullName = deliveryAddress.FullName,
                        PhoneNumber = deliveryAddress.PhoneNumber,
                        Region = deliveryAddress.Region,
                        Street = deliveryAddress.Street,
                        PostalCode = deliveryAddress.PostalCode,
                        City = deliveryAddress.City,
                        Country = deliveryAddress.Country
                    };
                }
                else
                {
                    clientApplication.DeliveryAddress = new ClientApplicationAddressServiceModel();
                }
            }

            return clientApplication;
        }

        public async Task<PagedResults<IEnumerable<ClientApplicationServiceModel>>> GetByIds(GetClientsApplicationsByIdsServiceModel model)
        {
            var clientsApplications = from c in _context.ClientsApplications
                                      where model.Ids.Contains(c.Id) && c.IsActive
                                      select new ClientApplicationServiceModel
                                      {
                                          Id = c.Id,
                                          CompanyName = c.CompanyName,
                                          FirstName = c.FirstName,
                                          LastName = c.LastName,
                                          ContactJobTitle = c.ContactJobTitle,
                                          PhoneNumber = c.PhoneNumber,
                                          Email = c.Email,
                                          LastModifiedDate = c.LastModifiedDate,
                                          CreatedDate = c.CreatedDate
                                      };

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                clientsApplications = clientsApplications.Where(x => x.CompanyName.StartsWith(model.SearchTerm));
            }

            clientsApplications = clientsApplications.ApplySort(model.OrderBy);

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                clientsApplications = clientsApplications.Take(Constants.MaxItemsPerPageLimit);

                return clientsApplications.PagedIndex(new Pagination(clientsApplications.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }

            return clientsApplications.PagedIndex(new Pagination(clientsApplications.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
        }

        public async Task<Guid> UpdateAsync(UpdateClientApplicationServiceModel model)
        {
            var clientApplication = await _context.ClientsApplications.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (clientApplication == null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("ClientApplicationNotFound"));
            }

            clientApplication.CompanyName = model.CompanyName;
            clientApplication.FirstName = model.FirstName;
            clientApplication.LastName = model.LastName;
            clientApplication.ContactJobTitle = model.ContactJobTitle;
            clientApplication.Email = model.Email;
            clientApplication.PhoneNumber = model.PhoneNumber;
            clientApplication.CommunicationLanguage = model.CommunicationLanguage;
            clientApplication.IsDeliveryAddressEqualBillingAddress = model.IsDeliveryAddressEqualBillingAddress;

            if (model.IsDeliveryAddressEqualBillingAddress)
            {
                var address = await _context.ClientsApplicationAddresses.FirstOrDefaultAsync(x => x.Id == model.BillingAddress.Id && x.IsActive);

                if (address is not null)
                {
                    address.FullName = model.BillingAddress.FullName;
                    address.PhoneNumber = model.BillingAddress.PhoneNumber;
                    address.Region = model.BillingAddress.Region;
                    address.Street = model.BillingAddress.Street;
                    address.PostalCode = model.BillingAddress.PostalCode;
                    address.City = model.BillingAddress.City;
                    address.Country = model.BillingAddress.Country;
                }
                else
                {
                    var billingAndDeliveryAddresses = new ClientsApplicationAddress
                    {
                        FullName = model.BillingAddress.FullName,
                        PhoneNumber = model.BillingAddress.PhoneNumber,
                        Region = model.BillingAddress.Region,
                        Street = model.BillingAddress.Street,
                        PostalCode = model.BillingAddress.PostalCode,
                        City = model.BillingAddress.City,
                        Country = model.BillingAddress.Country
                    };

                    _context.ClientsApplicationAddresses.Add(billingAndDeliveryAddresses.FillCommonProperties());

                    clientApplication.BillingAddressId = billingAndDeliveryAddresses.Id;
                }
            }
            else
            {
                var billingAddress = await _context.ClientsApplicationAddresses.FirstOrDefaultAsync(x => x.Id == model.BillingAddress.Id && x.IsActive);

                if (billingAddress is not null)
                {
                    billingAddress.FullName = model.BillingAddress.FullName;
                    billingAddress.PhoneNumber = model.BillingAddress.PhoneNumber;
                    billingAddress.Region = model.BillingAddress.Region;
                    billingAddress.Street = model.BillingAddress.Street;
                    billingAddress.PostalCode = model.BillingAddress.PostalCode;
                    billingAddress.City = model.BillingAddress.City;
                    billingAddress.Country = model.BillingAddress.Country;
                }
                else
                {
                    var newBillingAddress = new ClientsApplicationAddress
                    {
                        FullName = model.BillingAddress.FullName,
                        PhoneNumber = model.BillingAddress.PhoneNumber,
                        Region = model.BillingAddress.Region,
                        Street = model.BillingAddress.Street,
                        PostalCode = model.BillingAddress.PostalCode,
                        City = model.BillingAddress.City,
                        Country = model.BillingAddress.Country
                    };

                    _context.ClientsApplicationAddresses.Add(newBillingAddress.FillCommonProperties());

                    clientApplication.BillingAddressId = newBillingAddress.Id;
                }

                var deliveryAddress = await _context.ClientsApplicationAddresses.FirstOrDefaultAsync(x => x.Id == model.DeliveryAddress.Id && x.IsActive);

                if (deliveryAddress is not null)
                {
                    deliveryAddress.FullName = model.DeliveryAddress.FullName;
                    deliveryAddress.PhoneNumber = model.DeliveryAddress.PhoneNumber;
                    deliveryAddress.Region = model.DeliveryAddress.Region;
                    deliveryAddress.Street = model.DeliveryAddress.Street;
                    deliveryAddress.PostalCode = model.DeliveryAddress.PostalCode;
                    deliveryAddress.City = model.DeliveryAddress.City;
                    deliveryAddress.Country = model.DeliveryAddress.Country;
                }
                else
                {
                    var newDeliveryAddress = new ClientsApplicationAddress
                    {
                        FullName = model.DeliveryAddress.FullName,
                        PhoneNumber = model.DeliveryAddress.PhoneNumber,
                        Region = model.DeliveryAddress.Region,
                        Street = model.DeliveryAddress.Street,
                        PostalCode = model.DeliveryAddress.PostalCode,
                        City = model.DeliveryAddress.City,
                        Country = model.DeliveryAddress.Country
                    };

                    _context.ClientsApplicationAddresses.Add(newDeliveryAddress.FillCommonProperties());

                    clientApplication.DeliveryAddressId = newDeliveryAddress.Id;
                }
            }

            await _context.SaveChangesAsync();

            return clientApplication.Id;
        }
    }
}
