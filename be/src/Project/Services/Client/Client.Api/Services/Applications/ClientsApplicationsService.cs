using Client.Api.Infrastructure;
using Client.Api.Infrastructure.Clients.Entities;
using Client.Api.ServicesModels.Applications;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
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
        private readonly IStringLocalizer<ClientResources> clientLocalizer;

        public ClientsApplicationsService(
            ClientContext context,
            IStringLocalizer<ClientResources> clientLocalizer)
        {
            this.context = context;
            this.clientLocalizer = clientLocalizer;
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

            await this.context.AddAsync(clientApplication.FillCommonProperties());
            await this.context.SaveChangesAsync();

            return clientApplication.Id;
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
                                          CompanyRegion = c.CompanyRegion
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
                throw new CustomException(this.clientLocalizer.GetString("ClientNotFound"), (int)HttpStatusCode.NotFound);
            }

            var clientApplication = new ClientApplicationServiceModel
            {
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
                CompanyRegion = existingApplication.CompanyRegion
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
                                          CompanyRegion = c.CompanyRegion
                                      };

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                clientsApplications = clientsApplications.Where(x => x.CompanyName.StartsWith(model.SearchTerm));
            }

            clientsApplications = clientsApplications.ApplySort(model.OrderBy);

            return clientsApplications.PagedIndex(new Pagination(clientsApplications.Count(), model.ItemsPerPage), model.PageIndex);
        }
    }
}
