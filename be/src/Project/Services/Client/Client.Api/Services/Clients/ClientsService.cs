using Client.Api.Infrastructure;
using Client.Api.ServicesModels;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Client.Api.Services.Clients
{
    public class ClientsService : IClientsService
    {
        private readonly ClientContext context;
        private readonly IStringLocalizer clientLocalizer;

        public ClientsService(
            ClientContext context,
            IStringLocalizer<ClientResources> clientLocalizer)
        {
            this.context = context;
            this.clientLocalizer = clientLocalizer;
        }

        public async Task<PagedResults<IEnumerable<ClientServiceModel>>> GetAsync(GetClientsServiceModel model)
        {
            var clients = from c in this.context.Clients
                          where c.SellerId == model.OrganisationId.Value && c.IsActive
                          select new ClientServiceModel
                          {
                              Id = c.Id,
                              Name = c.Name,
                              Email = c.Email,
                              CommunicationLanguage = c.Language,
                              PhoneNumber = c.PhoneNumber,
                              LastModifiedDate = c.LastModifiedDate,
                              CreatedDate = c.CreatedDate
                          };

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                clients = clients.Where(x => x.Name.StartsWith(model.SearchTerm));
            }

            clients = clients.ApplySort(model.OrderBy);

            return clients.PagedIndex(new Pagination(clients.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<ClientServiceModel> GetAsync(GetClientServiceModel model)
        {
            var clients = from c in this.context.Clients
                            where c.SellerId == model.OrganisationId.Value && c.Id == model.Id && c.IsActive
                            select new ClientServiceModel
                            {
                                Id = c.Id,
                                Name = c.Name,
                                Email = c.Email,
                                CommunicationLanguage = c.Language,
                                PhoneNumber = c.PhoneNumber,
                                LastModifiedDate = c.LastModifiedDate,
                                CreatedDate = c.CreatedDate
                            };

            return await clients.FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(DeleteClientServiceModel model)
        {
            var client = await this.context.Clients.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (client == null)
            {
                throw new CustomException(this.clientLocalizer.GetString("ClientNotFound"), (int)HttpStatusCode.NotFound);
            }

            client.IsActive = false;

            await this.context.SaveChangesAsync();
        }

        public async Task<ClientServiceModel> UpdateAsync(UpdateClientServiceModel serviceModel)
        {
            var client = await this.context.Clients.FirstOrDefaultAsync(x => x.Id == serviceModel.Id && x.SellerId == serviceModel.OrganisationId.Value && x.IsActive);

            if (client == null)
            {
                throw new CustomException(this.clientLocalizer.GetString("ClientNotFound"), (int)HttpStatusCode.NotFound);
            }

            client.Name = serviceModel.Name;
            client.Email = serviceModel.Email;
            client.Language = serviceModel.CommunicationLanguage;
            client.PhoneNumber = serviceModel.PhoneNumber;
            client.OrganisationId = serviceModel.ClientOrganisationId.Value;

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetClientServiceModel { Id = client.Id, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }

        public async Task<ClientServiceModel> CreateAsync(CreateClientServiceModel serviceModel)
        {
            var exsitingClient = this.context.Clients.FirstOrDefault(x => x.Email == serviceModel.Email && x.IsActive);
            if (exsitingClient is not null)
            {
                throw new CustomException(this.clientLocalizer.GetString("ClientExists"), (int)HttpStatusCode.NotFound);
            }

            var client = new Infrastructure.Clients.Entities.Client
            {
                Name = serviceModel.Name,
                Email = serviceModel.Email,
                Language = serviceModel.CommunicationLanguage,
                OrganisationId = serviceModel.ClientOrganisationId.Value,
                PhoneNumber = serviceModel.PhoneNumber,
                SellerId = serviceModel.OrganisationId.Value
            };

            this.context.Clients.Add(client.FillCommonProperties());

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetClientServiceModel { Id = client.Id, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }

        public async Task<PagedResults<IEnumerable<ClientServiceModel>>> GetByIdsAsync(GetClientsByIdsServiceModel model)
        {
            var clients = from c in this.context.Clients
                          where model.Ids.Contains(c.Id) && c.SellerId == model.OrganisationId.Value && c.IsActive
                          select new ClientServiceModel
                          {
                              Id = c.Id,
                              Name = c.Name,
                              Email = c.Email,
                              CommunicationLanguage = c.Language,
                              PhoneNumber = c.PhoneNumber,
                              LastModifiedDate = c.LastModifiedDate,
                              CreatedDate = c.CreatedDate
                          };

            return clients.PagedIndex(new Pagination(clients.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<ClientServiceModel> GetByOrganisationAsync(GetClientByOrganisationServiceModel model)
        {
            var clients = from c in this.context.Clients
                          where c.OrganisationId == model.Id.Value && c.IsActive
                          select new ClientServiceModel
                          {
                              Id = c.Id,
                              Name = c.Name,
                              Email = c.Email,
                              CommunicationLanguage = c.Language,
                              PhoneNumber = c.PhoneNumber,
                              LastModifiedDate = c.LastModifiedDate,
                              CreatedDate = c.CreatedDate
                          };

            return await clients.FirstOrDefaultAsync();
        }
    }
}
