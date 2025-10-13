using Client.Api.Infrastructure;
using Client.Api.Infrastructure.Clients.Entities;
using Client.Api.ServicesModels.Addresses;
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

namespace Client.Api.Services.Addresses
{
    public class ClientAddressesService : IClientAddressesService
    {
        private readonly ClientContext _context;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;

        public ClientAddressesService(
            ClientContext context,
            IStringLocalizer<ClientResources> clientLocalizer)
        {
            _context = context;
            _clientLocalizer = clientLocalizer;
        }

        public async Task DeleteAsync(DeleteClientAddressServiceModel model)
        {
            var clientAddress = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (clientAddress is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("ClientAddressNotFound"));
            }

            if (await _context.Clients.AnyAsync(x => x.DefaultDeliveryAddressId == model.Id && x.IsActive))
            {
                throw new ConflictException(_clientLocalizer.GetString("DeliveryAddressDeleteDefaultConflict"));
            }

            if (await _context.Clients.AnyAsync(x => x.DefaultBillingAddressId == model.Id && x.IsActive))
            {
                throw new ConflictException(_clientLocalizer.GetString("BillingAddressDeleteDefaultConflict"));
            }

            clientAddress.IsActive = false;
            clientAddress.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public PagedResults<IEnumerable<ClientAddressServiceModel>> Get(GetClientAddressesServiceModel model)
        {
            var clientsAddresses = _context.Addresses
                    .Where(x => x.IsActive)
                    .Include(x => x.Client)
                    .AsSingleQuery();

            if (model.ClientId.HasValue)
            {
                clientsAddresses = clientsAddresses.Where(x => x.ClientId == model.ClientId);
            }

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                clientsAddresses = clientsAddresses.Where(x => 
                    x.Street.StartsWith(model.SearchTerm) || 
                    x.City.StartsWith(model.SearchTerm) || 
                    x.Region.StartsWith(model.SearchTerm) ||
                    x.PostCode.StartsWith(model.SearchTerm) ||
                    x.PhoneNumber.StartsWith(model.SearchTerm) ||
                    x.Company.StartsWith(model.SearchTerm) ||
                    x.FirstName.StartsWith(model.SearchTerm) ||
                    x.LastName.StartsWith(model.SearchTerm));
            }

            clientsAddresses = clientsAddresses.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<Address>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                clientsAddresses = clientsAddresses.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = clientsAddresses.PagedIndex(new Pagination(clientsAddresses.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = clientsAddresses.PagedIndex(new Pagination(clientsAddresses.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return new PagedResults<IEnumerable<ClientAddressServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.OrEmptyIfNull().Select(x => new ClientAddressServiceModel
                {
                    Id = x.Id,
                    ClientId = x.ClientId,
                    ClientName = x.Client.Name,
                    Company = x.Company,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    CountryId = x.CountryId,
                    Street = x.Street,
                    City = x.City,
                    PhoneNumber = x.PhoneNumber,
                    PostCode = x.PostCode,
                    Region = x.Region,
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }

        public async Task<ClientAddressServiceModel> GetAsync(GetClientAddressServiceModel model)
        {
            var clientAddress = await _context.Addresses
                    .Include(x => x.Client)
                    .AsSingleQuery()
                    .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (clientAddress is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("ClientAddressNotFound"));
            }

            return new ClientAddressServiceModel
            {
                Id = clientAddress.Id,
                ClientId = clientAddress.ClientId,
                ClientName = clientAddress.Client.Name,
                Company = clientAddress.Company,
                FirstName = clientAddress.FirstName,
                LastName = clientAddress.LastName,
                CountryId = clientAddress.CountryId,
                Street = clientAddress.Street,
                City = clientAddress.City,
                PhoneNumber = clientAddress.PhoneNumber,
                PostCode = clientAddress.PostCode,
                Region = clientAddress.Region,
                LastModifiedDate = clientAddress.LastModifiedDate,
                CreatedDate = clientAddress.CreatedDate
            };
        }

        public async Task<Guid> CreateAsync(CreateClientAddressServiceModel model)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == model.ClientId && x.IsActive);

            if (client is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("ClientNotFound"));
            }

            var clientAddress = new Address
            {
                Company = model.Company,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Street = model.Street, 
                City = model.City, 
                PhoneNumber = model.PhoneNumber, 
                PostCode = model.PostCode, 
                CountryId = model.CountryId.Value,
                ClientId = model.ClientId.Value,
                Region = model.Region
            };

            await _context.Addresses.AddAsync(clientAddress.FillCommonProperties());

            if (client.DefaultDeliveryAddressId.HasValue is false)
            {
                client.DefaultDeliveryAddressId = clientAddress.Id;
            }

            if (client.DefaultBillingAddressId.HasValue is false)
            {
                client.DefaultBillingAddressId = clientAddress.Id;
            }

            await _context.SaveChangesAsync();

            return clientAddress.Id;
        }

        public async Task<Guid> UpdateAsync(UpdateClientAddressServiceModel model)
        {
            var clientAddress = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (clientAddress is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("ClientAddressNotFound"));
            }

            clientAddress.ClientId = model.ClientId.Value;
            clientAddress.Company = model.Company;
            clientAddress.FirstName = model.FirstName;
            clientAddress.LastName = model.LastName;
            clientAddress.Street = model.Street;
            clientAddress.City = model.City;
            clientAddress.PhoneNumber = model.PhoneNumber;
            clientAddress.PostCode = model.PostCode; 
            clientAddress.CountryId = model.CountryId.Value;
            clientAddress.Region = model.Region;
            clientAddress.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return clientAddress.Id;
        }

        public PagedResults<IEnumerable<ClientAddressServiceModel>> GetByIds(GetClientAddressesByIdsServiceModel model)
        {
            var clientsAddresses = _context.Addresses
                    .Where(x => model.Ids.Contains(x.Id) && x.IsActive)
                    .Include(x => x.Client)
                    .AsSingleQuery();

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                clientsAddresses = clientsAddresses.Where(x =>
                    x.Street.StartsWith(model.SearchTerm) ||
                    x.City.StartsWith(model.SearchTerm) ||
                    x.Region.StartsWith(model.SearchTerm) ||
                    x.PostCode.StartsWith(model.SearchTerm) ||
                    x.PhoneNumber.StartsWith(model.SearchTerm) ||
                    x.Company.StartsWith(model.SearchTerm) ||
                    x.FirstName.StartsWith(model.SearchTerm) ||
                    x.LastName.StartsWith(model.SearchTerm));
            }

            clientsAddresses = clientsAddresses.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<Address>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                clientsAddresses = clientsAddresses.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = clientsAddresses.PagedIndex(new Pagination(clientsAddresses.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = clientsAddresses.PagedIndex(new Pagination(clientsAddresses.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return new PagedResults<IEnumerable<ClientAddressServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.OrEmptyIfNull().Select(x => new ClientAddressServiceModel
                {
                    Id = x.Id,
                    ClientId = x.ClientId,
                    ClientName = x.Client.Name,
                    Company = x.Company,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    CountryId = x.CountryId,
                    Street = x.Street,
                    City = x.City,
                    PhoneNumber = x.PhoneNumber,
                    PostCode = x.PostCode,
                    Region = x.Region,
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }
    }
}
