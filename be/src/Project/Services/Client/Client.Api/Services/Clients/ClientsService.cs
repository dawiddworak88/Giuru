using Client.Api.Infrastructure;
using Client.Api.Infrastructure.Groups.Entities;
using Client.Api.Infrastructure.Managers.Entities;
using Client.Api.ServicesModels.Clients;
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
            var clients = this.context.Clients.Where(x => x.IsActive);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                clients = clients.Where(x => x.Name.StartsWith(model.SearchTerm));
            }

            clients = clients.ApplySort(model.OrderBy);

            var pagedResults = clients.PagedIndex(new Pagination(clients.Count(), model.ItemsPerPage), model.PageIndex);

            var pagedClientServiceModel = new PagedResults<IEnumerable<ClientServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var clientsList = new List<ClientServiceModel>();

            foreach (var client in pagedResults.Data.OrEmptyIfNull().ToList())
            {
                var item = new ClientServiceModel
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email,
                    CommunicationLanguage = client.Language,
                    PhoneNumber = client.PhoneNumber,
                    LastModifiedDate = client.LastModifiedDate,
                    CreatedDate = client.CreatedDate
                };

                var clientGroups = this.context.ClientsGroups.Where(x => x.ClientId == client.Id && x.IsActive).Select(x => x.GroupId);

                if (clientGroups is not null)
                {
                    item.ClientGroupIds = clientGroups;
                }

                var clientManagers = this.context.ClientsAccountManagers.Where(x => x.ClientId == client.Id && x.IsActive).Select(x => x.ClientManagerId);

                if (clientManagers is not null)
                {
                    item.ClientManagerIds = clientManagers;
                }

                clientsList.Add(item);
            }

            pagedClientServiceModel.Data = clientsList;

            return pagedClientServiceModel;
        }

        public async Task<ClientServiceModel> GetAsync(GetClientServiceModel model)
        {
            var existingClient = await this.context.Clients.FirstOrDefaultAsync(x => x.SellerId == model.OrganisationId.Value && x.Id == model.Id && x.IsActive);
            
            if (existingClient is null)
            {
                throw new CustomException(this.clientLocalizer.GetString("ClientNotFound"), (int)HttpStatusCode.NotFound);
            }

            var client = new ClientServiceModel
            {
                Id = existingClient.Id,
                Name = existingClient.Name,
                Email = existingClient.Email,
                CommunicationLanguage = existingClient.Language,
                PhoneNumber = existingClient.PhoneNumber,
                LastModifiedDate = existingClient.LastModifiedDate,
                CreatedDate = existingClient.CreatedDate
            };

            var clientGroups = this.context.ClientsGroups.Where(x => x.ClientId == existingClient.Id && x.IsActive).Select(x => x.GroupId);

            if (clientGroups is not null)
            {
                client.ClientGroupIds = clientGroups;
            }

            var clientManagers = this.context.ClientsAccountManagers.Where(x => x.ClientId == existingClient.Id && x.IsActive).Select(x => x.ClientManagerId);

            if (clientManagers is not null)
            {
                client.ClientManagerIds = clientManagers;
            }
            
            return client;
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

            var clientGroups = this.context.ClientsGroups.Where(x => x.ClientId == serviceModel.Id && x.IsActive);

            foreach (var clientGroup in clientGroups.OrEmptyIfNull())
            {
                this.context.ClientsGroups.Remove(clientGroup);
            }

            foreach (var group in serviceModel.ClientGroupIds.OrEmptyIfNull())
            {
                var groupItem = new ClientsGroup
                {
                    ClientId = client.Id,
                    GroupId = group
                };

                await this.context.ClientsGroups.AddAsync(groupItem.FillCommonProperties());
            }

            var clientManagers = this.context.ClientsAccountManagers.Where(x => x.ClientId == serviceModel.Id && x.IsActive);

            foreach (var clientManager in clientManagers.OrEmptyIfNull())
            {
                this.context.ClientsAccountManagers.Remove(clientManager);
            }

            foreach (var managerId in serviceModel.ClientManagerIds.OrEmptyIfNull())
            {
                var managerItem = new ClientsAccountManagers
                {
                    ClientId = client.Id,
                    ClientManagerId = managerId
                };

                await this.context.ClientsAccountManagers.AddAsync(managerItem.FillCommonProperties());
            }

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

            foreach (var group in serviceModel.ClientGroupIds.OrEmptyIfNull())
            {
                var clientGroup = new ClientsGroup
                {
                    ClientId = client.Id,
                    GroupId = group
                };

                await this.context.ClientsGroups.AddAsync(clientGroup.FillCommonProperties());
            }

            foreach (var managerId in serviceModel.ClientManagerIds.OrEmptyIfNull())
            {
                var clientManager = new ClientsAccountManagers
                {
                    ClientId = client.Id,
                    ClientManagerId = managerId
                };

                await this.context.ClientsAccountManagers.AddAsync(clientManager.FillCommonProperties());
            }

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
