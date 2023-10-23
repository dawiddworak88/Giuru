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
                throw new CustomException(_clientLocalizer.GetString("ClientAddressNotFound"), (int)HttpStatusCode.NoContent);
            }

            if (await _context.Clients.AnyAsync(x => x.DefaultDeliveryAddressId == model.Id && x.IsActive))
            {
                throw new CustomException("AddressDeleteDefaultConflict", (int)HttpStatusCode.Conflict);
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

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                clientsAddresses = clientsAddresses.Where(x => 
                    x.Street.StartsWith(model.SearchTerm) || 
                    x.City.StartsWith(model.SearchTerm) || 
                    x.Region.StartsWith(model.SearchTerm) ||
                    x.PostCode.StartsWith(model.SearchTerm) ||
                    x.PhoneNumber.StartsWith(model.SearchTerm) ||
                    x.Recipient.StartsWith(model.SearchTerm));
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
                    Recipient = x.Recipient,
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
                throw new CustomException(_clientLocalizer.GetString("ClientAddressNotFound"), (int)HttpStatusCode.NoContent);
            }

            return new ClientAddressServiceModel
            {
                Id = clientAddress.Id,
                ClientId = clientAddress.ClientId,
                ClientName = clientAddress.Client.Name,
                Recipient = clientAddress.Recipient,
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
                throw new CustomException(_clientLocalizer.GetString("ClientNotFound"), (int)HttpStatusCode.NoContent);
            }

            var clientAddress = new Address
            {
                Recipient = model.Recipient,
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

            await _context.SaveChangesAsync();

            return clientAddress.Id;
        }

        public async Task<Guid> UpdateAsync(UpdateClientAddressServiceModel model)
        {
            var clientAddress = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (clientAddress is null)
            {
                throw new CustomException(_clientLocalizer.GetString("ClientAddressNotFound"), (int)HttpStatusCode.NoContent);
            }

            clientAddress.ClientId = model.ClientId.Value;
            clientAddress.Recipient = model.Recipient;
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
    }
}
