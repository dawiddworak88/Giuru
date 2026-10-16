using Client.Api.Infrastructure;
using Client.Api.Infrastructure.DiscountCodes.Entities;
using Client.Api.ServicesModels.DiscountCodes;
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
using System.Threading.Tasks;

namespace Client.Api.Services.DiscountCodes
{
    public class DiscountCodesService : IDiscountCodesService
    {
        private readonly ClientContext _context;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;

        public DiscountCodesService(
            ClientContext context,
            IStringLocalizer<ClientResources> clientLocalizer)
        {
            _context = context;
            _clientLocalizer = clientLocalizer;
        }

        public async Task<PagedResults<IEnumerable<DiscountCodeServiceModel>>> GetAsync(GetDiscountCodesServiceModel model)
        {
            var discountCodes = _context.DiscountCodes.Where(x => x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                var searchTerm = model.SearchTerm.Trim();

                discountCodes = discountCodes.Where(x => x.Code.Contains(searchTerm));
            }

            discountCodes = discountCodes.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<DiscountCode>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                discountCodes = discountCodes.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = discountCodes.PagedIndex(new Pagination(discountCodes.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = discountCodes.PagedIndex(new Pagination(discountCodes.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            var pageCodes = pagedResults.Data.OrEmptyIfNull().ToList();
            var pageCodeIds = pageCodes.Select(x => x.Id).ToList();

            var clientCounts = await _context.DiscountCodeClients
                .Where(x => pageCodeIds.Contains(x.DiscountCodeId) && x.IsActive)
                .Join(_context.Clients.Where(c => c.IsActive), x => x.ClientId, c => c.Id, (x, c) => x.DiscountCodeId)
                .GroupBy(x => x)
                .Select(x => new { DiscountCodeId = x.Key, Count = x.Count() })
                .ToDictionaryAsync(x => x.DiscountCodeId, x => x.Count);

            return new PagedResults<IEnumerable<DiscountCodeServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pageCodes.Select(x => new DiscountCodeServiceModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Description = x.Description,
                    IsDisabled = x.IsDisabled,
                    ClientsCount = clientCounts.GetValueOrDefault(x.Id),
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                }).ToList()
            };
        }

        public async Task<DiscountCodeServiceModel> GetAsync(GetDiscountCodeServiceModel model)
        {
            var discountCode = await _context.DiscountCodes.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (discountCode is null)
            {
                return default;
            }

            var clientIds = await GetAssignedClientIdsAsync(discountCode.Id);

            return new DiscountCodeServiceModel
            {
                Id = discountCode.Id,
                Code = discountCode.Code,
                Description = discountCode.Description,
                IsDisabled = discountCode.IsDisabled,
                ClientsCount = clientIds.Count,
                ClientIds = clientIds,
                LastModifiedDate = discountCode.LastModifiedDate,
                CreatedDate = discountCode.CreatedDate
            };
        }

        public async Task<Guid> SaveAsync(SaveDiscountCodeServiceModel model)
        {
            var sellerId = model.OrganisationId.Value;
            var code = model.Code.Trim();
            var clientIds = model.ClientIds.OrEmptyIfNull().Distinct().ToList();

            DiscountCode discountCode = null;

            if (model.Id.HasValue)
            {
                discountCode = await _context.DiscountCodes.FirstOrDefaultAsync(x => x.Id == model.Id.Value && x.SellerId == sellerId && x.IsActive);

                if (discountCode is null)
                {
                    throw new NotFoundException(_clientLocalizer.GetString("DiscountCodeNotFound"));
                }
            }

            var knownClientsCount = await _context.Clients.CountAsync(x => x.SellerId == sellerId && x.IsActive && clientIds.Contains(x.Id));

            if (knownClientsCount != clientIds.Count)
            {
                throw new UnprocessableEntityException(_clientLocalizer.GetString("DiscountCodeClientNotFound"));
            }

            if (discountCode is not null)
            {
                if (string.Equals(discountCode.Code, code, StringComparison.Ordinal) is false)
                {
                    throw new UnprocessableEntityException(_clientLocalizer.GetString("DiscountCodeCodeCannotBeChanged"));
                }

                discountCode.Description = NormalizeDescription(model.Description);
                discountCode.IsDisabled = model.IsDisabled;
                discountCode.LastModifiedDate = DateTime.UtcNow;
            }
            else
            {
                if (await _context.DiscountCodes.AnyAsync(x => x.SellerId == sellerId && x.IsActive && x.Code == code))
                {
                    throw new ConflictException(_clientLocalizer.GetString("DiscountCodeExists"));
                }

                discountCode = new DiscountCode
                {
                    Code = code,
                    Description = NormalizeDescription(model.Description),
                    IsDisabled = model.IsDisabled,
                    SellerId = sellerId
                };

                _context.DiscountCodes.Add(discountCode.FillCommonProperties());
            }

            var existingAssignments = await _context.DiscountCodeClients.Where(x => x.DiscountCodeId == discountCode.Id && x.IsActive).ToListAsync();

            foreach (var assignment in existingAssignments.Where(x => clientIds.Contains(x.ClientId) is false))
            {
                assignment.IsActive = false;
                assignment.LastModifiedDate = DateTime.UtcNow;
            }

            foreach (var clientId in clientIds.Where(id => existingAssignments.Any(x => x.ClientId == id) is false))
            {
                _context.DiscountCodeClients.Add(new DiscountCodeClient
                {
                    DiscountCodeId = discountCode.Id,
                    ClientId = clientId
                }.FillCommonProperties());
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // A concurrent create with the same code text loses to the filtered unique index.
                if (model.Id.HasValue is false && await _context.DiscountCodes.AsNoTracking().AnyAsync(x => x.SellerId == sellerId && x.IsActive && x.Code == code && x.Id != discountCode.Id))
                {
                    throw new ConflictException(_clientLocalizer.GetString("DiscountCodeExists"));
                }

                throw;
            }

            return discountCode.Id;
        }

        public async Task DeleteAsync(DeleteDiscountCodeServiceModel model)
        {
            var discountCode = await _context.DiscountCodes.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (discountCode is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("DiscountCodeNotFound"));
            }

            discountCode.IsActive = false;
            discountCode.LastModifiedDate = DateTime.UtcNow;

            var assignments = await _context.DiscountCodeClients.Where(x => x.DiscountCodeId == discountCode.Id && x.IsActive).ToListAsync();

            foreach (var assignment in assignments)
            {
                assignment.IsActive = false;
                assignment.LastModifiedDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        private async Task<List<Guid>> GetAssignedClientIdsAsync(Guid discountCodeId)
        {
            return await _context.DiscountCodeClients
                .Where(x => x.DiscountCodeId == discountCodeId && x.IsActive)
                .Join(_context.Clients.Where(c => c.IsActive), x => x.ClientId, c => c.Id, (x, c) => c.Id)
                .ToListAsync();
        }

        private static string NormalizeDescription(string description)
        {
            return string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        }
    }
}
