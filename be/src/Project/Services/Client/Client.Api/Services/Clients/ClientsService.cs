using Client.Api.Infrastructure;
using Client.Api.Infrastructure.DiscountCodes.Entities;
using Client.Api.Infrastructure.Groups.Entities;
using Client.Api.Infrastructure.Managers.Entities;
using Client.Api.IntegrationEvents;
using Client.Api.ServicesModels.Clients;
using Foundation.EventBus.Abstractions;
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
        private readonly IEventBus _eventBus;

        public ClientsService(
            ClientContext context,
            IStringLocalizer<ClientResources> clientLocalizer,
            IEventBus eventBus)
        {
            _context = context;
            _clientLocalizer = clientLocalizer;
            _eventBus = eventBus;
        }

        public PagedResults<IEnumerable<ClientServiceModel>> Get(GetClientsServiceModel model)
        {
            var clients = _context.Clients.Where(x => x.SellerId == model.OrganisationId.Value && x.IsActive);

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
            var pageClients = pagedResults.Data.OrEmptyIfNull().ToList();
            var discountCodeIds = GetDiscountCodeIds(pageClients.Select(x => x.Id));

            foreach (var client in pageClients)
            {
                var item = new ClientServiceModel
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email,
                    CountryId = client.CountryId,
                    PreferedCurrencyId = client.CurrencyId,
                    OrganisationId = client.OrganisationId,
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

                item.DiscountCodeIds = discountCodeIds.TryGetValue(client.Id, out var clientDiscountCodeIds) ? clientDiscountCodeIds : new List<Guid>();

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

            client.DiscountCodeIds = GetDiscountCodeIds(new[] { existingClient.Id }).GetValueOrDefault(existingClient.Id) ?? new List<Guid>();

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

            // Validated before any tracked field changes, so a rejected assignment leaves the client untouched.
            var discountCodeIds = await GetAssignableDiscountCodeIdsAsync(serviceModel.DiscountCodeIds, serviceModel.IsSeller, serviceModel.OrganisationId.Value);

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

            // Null leaves the assignments unchanged, an empty list clears them.
            if (discountCodeIds is not null)
            {
                var clientDiscountCodes = await _context.ClientsDiscountCodes.Where(x => x.ClientId == client.Id && x.IsActive).ToListAsync();

                foreach (var clientDiscountCode in clientDiscountCodes.Where(x => discountCodeIds.Contains(x.DiscountCodeId) is false))
                {
                    _context.ClientsDiscountCodes.Remove(clientDiscountCode);
                }

                foreach (var discountCodeId in discountCodeIds.Except(clientDiscountCodes.Select(x => x.DiscountCodeId)))
                {
                    var discountCodeItem = new ClientsDiscountCode
                    {
                        ClientId = client.Id,
                        DiscountCodeId = discountCodeId
                    };

                    await _context.ClientsDiscountCodes.AddAsync(discountCodeItem.FillCommonProperties());
                }
            }

            await _context.SaveChangesAsync();

            var upsertedClientMessage = new UpsertedClientIntegrationEvent
            {
                ClientId = client.Id
            };

            _eventBus.Publish(upsertedClientMessage);

            return await GetAsync(new GetClientServiceModel { Id = client.Id, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }

        public async Task<ClientServiceModel> CreateAsync(CreateClientServiceModel serviceModel)
        {
            var exsitingClient = _context.Clients.FirstOrDefault(x => x.Email == serviceModel.Email && x.IsActive);

            if (exsitingClient is not null)
            {
                throw new ConflictException(_clientLocalizer.GetString("ClientExists"));
            }

            var discountCodeIds = await GetAssignableDiscountCodeIdsAsync(serviceModel.DiscountCodeIds, serviceModel.IsSeller, serviceModel.OrganisationId.Value);

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

            foreach (var discountCodeId in discountCodeIds.OrEmptyIfNull())
            {
                var clientDiscountCode = new ClientsDiscountCode
                {
                    ClientId = client.Id,
                    DiscountCodeId = discountCodeId
                };

                await _context.ClientsDiscountCodes.AddAsync(clientDiscountCode.FillCommonProperties());
            }

            await _context.SaveChangesAsync();

            var upsertedClientMessage = new UpsertedClientIntegrationEvent
            {
                ClientId = client.Id
            };

            _eventBus.Publish(upsertedClientMessage);

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
                              OrganisationId = c.OrganisationId,
                              CommunicationLanguage = c.Language,
                              PhoneNumber = c.PhoneNumber,
                              IsDisabled = c.IsDisabled,
                              DefaultDeliveryAddressId = c.DefaultDeliveryAddressId,
                              DefaultBillingAddressId = c.DefaultBillingAddressId,
                              LastModifiedDate = c.LastModifiedDate,
                              CreatedDate = c.CreatedDate
                          };

            PagedResults<IEnumerable<ClientServiceModel>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                clients = clients.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = clients.PagedIndex(new Pagination(clients.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = clients.PagedIndex(new Pagination(clients.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            var pageClients = pagedResults.Data.OrEmptyIfNull().ToList();
            var discountCodeIds = GetDiscountCodeIds(pageClients.Select(x => x.Id.Value));

            foreach (var pageClient in pageClients)
            {
                pageClient.DiscountCodeIds = discountCodeIds.TryGetValue(pageClient.Id.Value, out var clientDiscountCodeIds) ? clientDiscountCodeIds : new List<Guid>();
            }

            return new PagedResults<IEnumerable<ClientServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pageClients
            };
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

        /// <summary>
        /// The ids to assign, validated, or null when the request does not touch assignments.
        /// Only active codes of the caller's own seller can be assigned, so a crafted request cannot attach another seller's code.
        /// </summary>
        private async Task<List<Guid>> GetAssignableDiscountCodeIdsAsync(IEnumerable<Guid> discountCodeIds, bool isSeller, Guid sellerId)
        {
            if (discountCodeIds is null)
            {
                return null;
            }

            if (isSeller is false)
            {
                throw new CustomException(_clientLocalizer.GetString("DiscountCodeAssignmentForbidden"), (int)HttpStatusCode.Forbidden);
            }

            var distinctIds = discountCodeIds.Distinct().ToList();

            if (distinctIds.Count == 0)
            {
                return distinctIds;
            }

            var validCount = await _context.DiscountCodes.CountAsync(x => distinctIds.Contains(x.Id) && x.SellerId == sellerId && x.IsActive);

            if (validCount != distinctIds.Count)
            {
                throw new UnprocessableEntityException(_clientLocalizer.GetString("DiscountCodeAssignmentInvalid"));
            }

            return distinctIds;
        }

        /// <summary>
        /// The ids of the active discount codes assigned to each of the clients. Deleted codes never appear.
        /// </summary>
        private Dictionary<Guid, List<Guid>> GetDiscountCodeIds(IEnumerable<Guid> clientIds)
        {
            var ids = clientIds.ToList();

            var assignments = (from cdc in _context.ClientsDiscountCodes
                               join dc in _context.DiscountCodes on cdc.DiscountCodeId equals dc.Id
                               where ids.Contains(cdc.ClientId) && cdc.IsActive && dc.IsActive
                               select new { cdc.ClientId, cdc.DiscountCodeId }).ToList();

            return assignments
                .GroupBy(x => x.ClientId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.DiscountCodeId).ToList());
        }
    }
}
