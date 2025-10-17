using Client.Api.Infrastructure;
using Client.Api.Infrastructure.Groups.Entities;
using Client.Api.Infrastructure.Managers.Entities;
using Client.Api.ServicesModels.Clients;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
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

namespace Client.Api.Services.Clients
{
    public class ClientsService : IClientsService
    {
        private readonly ClientContext _context;
        private readonly IStringLocalizer _clientLocalizer;

        public ClientsService(
            ClientContext context,
            IStringLocalizer<ClientResources> clientLocalizer)
        {
            _context = context;
            _clientLocalizer = clientLocalizer;
        }

        public PagedResults<IEnumerable<ClientServiceModel>> Get(GetClientsServiceModel model)
        {
            var clients = _context.Clients.Where(x => x.IsActive);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                clients = clients.Where(x => x.Name.StartsWith(model.SearchTerm) || x.Email.StartsWith(model.SearchTerm));
            }

            clients = clients.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<Infrastructure.Clients.Entities.Client>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                clients = clients.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = clients.PagedIndex(new Pagination(clients.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = clients.PagedIndex(new Pagination(clients.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            var pagedClientServiceModel = new PagedResults<IEnumerable<ClientServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var clientsList = new List<ClientServiceModel>();

            foreach (var client in pagedResults.Data.OrEmptyIfNull().ToList())
            {
                var item = new ClientServiceModel
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email,
                    CountryId = client.CountryId,
                    PreferedCurrencyId = client.CurrencyId,
                    CommunicationLanguage = client.Language,
                    PhoneNumber = client.PhoneNumber,
                    IsDisabled = client.IsDisabled,
                    DefaultDeliveryAddressId = client.DefaultDeliveryAddressId,
                    DefaultBillingAddressId = client.DefaultBillingAddressId,
                    LastModifiedDate = client.LastModifiedDate,
                    CreatedDate = client.CreatedDate
                };

                var clientGroups = _context.ClientsGroups.Where(x => x.ClientId == client.Id && x.IsActive).Select(x => x.GroupId);

                if (clientGroups is not null)
                {
                    item.ClientGroupIds = clientGroups;
                }

                var clientManagers = _context.ClientsAccountManagers.Where(x => x.ClientId == client.Id && x.IsActive).Select(x => x.ClientManagerId);

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
            var existingClient = await _context.Clients.FirstOrDefaultAsync(x => x.SellerId == model.OrganisationId.Value && x.Id == model.Id && x.IsActive);
            
            if (existingClient is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("ClientNotFound"));
            }

            var client = new ClientServiceModel
            {
                Id = existingClient.Id,
                Name = existingClient.Name,
                Email = existingClient.Email,
                CountryId = existingClient.CountryId,
                PreferedCurrencyId = existingClient.CurrencyId,
                OrganisationId = existingClient.OrganisationId,
                CommunicationLanguage = existingClient.Language,
                PhoneNumber = existingClient.PhoneNumber,
                IsDisabled = existingClient.IsDisabled,
                DefaultDeliveryAddressId = existingClient.DefaultDeliveryAddressId,
                DefaultBillingAddressId = existingClient.DefaultBillingAddressId,
                LastModifiedDate = existingClient.LastModifiedDate,
                CreatedDate = existingClient.CreatedDate
            };

            var clientGroups = _context.ClientsGroups.Where(x => x.ClientId == existingClient.Id && x.IsActive).Select(x => x.GroupId);

            if (clientGroups is not null)
            {
                client.ClientGroupIds = clientGroups;
            }

            var clientManagers = _context.ClientsAccountManagers.Where(x => x.ClientId == existingClient.Id && x.IsActive).Select(x => x.ClientManagerId);

            if (clientManagers is not null)
            {
                client.ClientManagerIds = clientManagers;
            }

            return client;
        }

        public async Task DeleteAsync(DeleteClientServiceModel model)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (client is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("ClientNotFound"));
            }

            if (await _context.Addresses.AnyAsync(x => x.ClientId == model.Id && x.IsActive))
            {
                throw new ConflictException(_clientLocalizer.GetString("ClientDeleteAddressConflict"));
            }

            client.IsActive = false;
            client.IsDisabled = true;
            client.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<ClientServiceModel> UpdateAsync(UpdateClientServiceModel serviceModel)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == serviceModel.Id && x.SellerId == serviceModel.OrganisationId.Value && x.IsActive);

            if (client is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("ClientNotFound"));
            }

            client.Name = serviceModel.Name;
            client.Email = serviceModel.Email;
            client.CountryId = serviceModel.CountryId;
            client.CurrencyId = serviceModel.PreferedCurrencyId;
            client.Language = serviceModel.CommunicationLanguage;
            client.PhoneNumber = serviceModel.PhoneNumber;
            client.OrganisationId = serviceModel.ClientOrganisationId.Value;
            client.DefaultDeliveryAddressId = serviceModel.DefaultDeliveryAddressId;
            client.DefaultBillingAddressId = serviceModel.DefaultBillingAddressId;
            client.LastModifiedDate = DateTime.UtcNow;
            client.IsDisabled = serviceModel.IsDisabled;

            var clientGroups = _context.ClientsGroups.Where(x => x.ClientId == serviceModel.Id && x.IsActive);

            foreach (var clientGroup in clientGroups.OrEmptyIfNull())
            {
                _context.ClientsGroups.Remove(clientGroup);
            }

            foreach (var group in serviceModel.ClientGroupIds.OrEmptyIfNull())
            {
                var groupItem = new ClientsGroup
                {
                    ClientId = client.Id,
                    GroupId = group
                };

                await _context.ClientsGroups.AddAsync(groupItem.FillCommonProperties());
            }

            var clientManagers = _context.ClientsAccountManagers.Where(x => x.ClientId == serviceModel.Id && x.IsActive);

            foreach (var clientManager in clientManagers.OrEmptyIfNull())
            {
                _context.ClientsAccountManagers.Remove(clientManager);
            }

            foreach (var managerId in serviceModel.ClientManagerIds.OrEmptyIfNull())
            {
                var managerItem = new ClientsAccountManagers
                {
                    ClientId = client.Id,
                    ClientManagerId = managerId
                };

                await _context.ClientsAccountManagers.AddAsync(managerItem.FillCommonProperties());
            }

            await _context.SaveChangesAsync();

            return await GetAsync(new GetClientServiceModel { Id = client.Id, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }

        public async Task<ClientServiceModel> CreateAsync(CreateClientServiceModel serviceModel)
        {
            var exsitingClient = _context.Clients.FirstOrDefault(x => x.Email == serviceModel.Email && x.IsActive);

            if (exsitingClient is not null)
            {
                throw new ConflictException(_clientLocalizer.GetString("ClientExists"));
            }

            var client = new Infrastructure.Clients.Entities.Client
            {
                Name = serviceModel.Name,
                Email = serviceModel.Email,
                CountryId = serviceModel.CountryId,
                CurrencyId = serviceModel.PreferedCurrencyId,
                Language = serviceModel.CommunicationLanguage,
                OrganisationId = serviceModel.ClientOrganisationId.Value,
                PhoneNumber = serviceModel.PhoneNumber,
                IsDisabled = false,
                SellerId = serviceModel.OrganisationId.Value,
                DefaultDeliveryAddressId = serviceModel.DefaultDeliveryAddressId,
                DefaultBillingAddressId = serviceModel.DefaultBillingAddressId
            };

            _context.Clients.Add(client.FillCommonProperties());

            foreach (var group in serviceModel.ClientGroupIds.OrEmptyIfNull())
            {
                var clientGroup = new ClientsGroup
                {
                    ClientId = client.Id,
                    GroupId = group
                };

                await _context.ClientsGroups.AddAsync(clientGroup.FillCommonProperties());
            }

            foreach (var managerId in serviceModel.ClientManagerIds.OrEmptyIfNull())
            {
                var clientManager = new ClientsAccountManagers
                {
                    ClientId = client.Id,
                    ClientManagerId = managerId
                };

                await _context.ClientsAccountManagers.AddAsync(clientManager.FillCommonProperties());
            }

            await _context.SaveChangesAsync();

            return await GetAsync(new GetClientServiceModel { Id = client.Id, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }

        public PagedResults<IEnumerable<ClientServiceModel>> GetByIds(GetClientsByIdsServiceModel model)
        {
            var clients = from c in _context.Clients
                          where model.Ids.Contains(c.Id) && c.SellerId == model.OrganisationId.Value && c.IsActive
                          select new ClientServiceModel
                          {
                              Id = c.Id,
                              Name = c.Name,
                              Email = c.Email,
                              CountryId = c.CountryId,
                              PreferedCurrencyId = c.CurrencyId,
                              CommunicationLanguage = c.Language,
                              PhoneNumber = c.PhoneNumber,
                              IsDisabled = c.IsDisabled,
                              DefaultDeliveryAddressId = c.DefaultDeliveryAddressId,
                              DefaultBillingAddressId = c.DefaultBillingAddressId,
                              LastModifiedDate = c.LastModifiedDate,
                              CreatedDate = c.CreatedDate
                          };

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                clients = clients.Take(Constants.MaxItemsPerPageLimit);

                return clients.PagedIndex(new Pagination(clients.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }

            return clients.PagedIndex(new Pagination(clients.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
        }

        public async Task<ClientServiceModel> GetByOrganisationAsync(GetClientByOrganisationServiceModel model)
        {
            var clients = from c in _context.Clients
                          where c.OrganisationId == model.Id.Value && c.IsActive
                          select new ClientServiceModel
                          {
                              Id = c.Id,
                              Name = c.Name,
                              Email = c.Email,
                              CountryId = c.CountryId,
                              OrganisationId = c.OrganisationId,
                              PreferedCurrencyId = c.CurrencyId,
                              CommunicationLanguage = c.Language,
                              PhoneNumber = c.PhoneNumber,
                              IsDisabled = c.IsDisabled,
                              DefaultDeliveryAddressId = c.DefaultDeliveryAddressId,
                              DefaultBillingAddressId = c.DefaultBillingAddressId,
                              LastModifiedDate = c.LastModifiedDate,
                              CreatedDate = c.CreatedDate
                          };

            return await clients.FirstOrDefaultAsync();
        }

        public Task<ClientServiceModel> GetByEmailAsync(GetClientByEmailServiceModel model)
        {
            var client = from c in _context.Clients
                         where c.Email == model.Email && c.IsActive
                         select new ClientServiceModel
                         {
                             Id = c.Id,
                             Name = c.Name,
                             Email = c.Email,
                             CountryId = c.CountryId,
                             OrganisationId = c.OrganisationId,
                             PreferedCurrencyId = c.CurrencyId,
                             CommunicationLanguage = c.Language,
                             PhoneNumber = c.PhoneNumber,
                             IsDisabled = c.IsDisabled,
                             DefaultDeliveryAddressId = c.DefaultDeliveryAddressId,
                             DefaultBillingAddressId = c.DefaultBillingAddressId,
                             LastModifiedDate = c.LastModifiedDate,
                             CreatedDate = c.CreatedDate
                         };

            if (client is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("ClientNotFound"));
            }

            return client.FirstOrDefaultAsync();
        }
    }
}
