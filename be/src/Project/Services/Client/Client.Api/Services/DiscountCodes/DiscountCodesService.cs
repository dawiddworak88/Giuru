using Client.Api.Infrastructure;
using Client.Api.Infrastructure.DiscountCodes.Entities;
using Client.Api.ServicesModels.DiscountCodes;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.Data.SqlClient;
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
        private const int SqlDuplicateKeyErrorNumber = 2601;
        private const int SqlUniqueConstraintErrorNumber = 2627;

        private readonly ClientContext _context;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;

        public DiscountCodesService(
            ClientContext context,
            IStringLocalizer<ClientResources> clientLocalizer)
        {
            _context = context;
            _clientLocalizer = clientLocalizer;
        }

        public async Task<Guid> CreateAsync(CreateDiscountCodeServiceModel model)
        {
            var sellerId = model.OrganisationId.Value;
            var code = model.Code.Trim();

            if (await _context.DiscountCodes.AnyAsync(x => x.SellerId == sellerId && x.Code == code && x.IsActive))
            {
                throw new ConflictException(_clientLocalizer.GetString("DiscountCodeExists"));
            }

            var discountCode = new DiscountCode
            {
                Code = code,
                Description = model.Description?.Trim(),
                IsDisabled = model.IsDisabled,
                SellerId = sellerId
            };

            await _context.DiscountCodes.AddAsync(discountCode.FillCommonProperties());

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsUniqueIndexViolation(ex))
            {
                // A concurrent create of the same code won the race on the filtered unique index.
                throw new ConflictException(_clientLocalizer.GetString("DiscountCodeExists"));
            }

            return discountCode.Id;
        }

        public async Task<Guid> UpdateAsync(UpdateDiscountCodeServiceModel model)
        {
            var discountCode = await _context.DiscountCodes.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (discountCode is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("DiscountCodeNotFound"));
            }

            // The code text is immutable: a renamed code would silently change what every assignment means.
            discountCode.Description = model.Description?.Trim();
            discountCode.IsDisabled = model.IsDisabled;
            discountCode.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return discountCode.Id;
        }

        public async Task DeleteAsync(DeleteDiscountCodeServiceModel model)
        {
            var discountCode = await _context.DiscountCodes.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (discountCode is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("DiscountCodeNotFound"));
            }

            var now = DateTime.UtcNow;

            discountCode.IsActive = false;
            discountCode.LastModifiedDate = now;

            var assignments = await _context.ClientsDiscountCodes.Where(x => x.DiscountCodeId == discountCode.Id && x.IsActive).ToListAsync();

            foreach (var assignment in assignments)
            {
                assignment.IsActive = false;
                assignment.LastModifiedDate = now;
            }

            await _context.SaveChangesAsync();
        }

        public PagedResults<IEnumerable<DiscountCodeServiceModel>> Get(GetDiscountCodesServiceModel model)
        {
            var sellerId = model.OrganisationId.Value;

            var discountCodes = from d in _context.DiscountCodes
                                where d.SellerId == sellerId && d.IsActive
                                select new DiscountCodeServiceModel
                                {
                                    Id = d.Id,
                                    Code = d.Code,
                                    Description = d.Description,
                                    IsDisabled = d.IsDisabled,
                                    LastModifiedDate = d.LastModifiedDate,
                                    CreatedDate = d.CreatedDate
                                };

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                discountCodes = discountCodes.Where(x => x.Code.StartsWith(model.SearchTerm));
            }

            discountCodes = discountCodes.ApplySort(model.OrderBy);

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                discountCodes = discountCodes.Take(Constants.MaxItemsPerPageLimit);

                return discountCodes.PagedIndex(new Pagination(discountCodes.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }

            return discountCodes.PagedIndex(new Pagination(discountCodes.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
        }

        public async Task<DiscountCodeServiceModel> GetAsync(GetDiscountCodeServiceModel model)
        {
            var discountCode = await _context.DiscountCodes.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (discountCode is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("DiscountCodeNotFound"));
            }

            return new DiscountCodeServiceModel
            {
                Id = discountCode.Id,
                Code = discountCode.Code,
                Description = discountCode.Description,
                IsDisabled = discountCode.IsDisabled,
                LastModifiedDate = discountCode.LastModifiedDate,
                CreatedDate = discountCode.CreatedDate
            };
        }

        public async Task<DiscountCodeValidationServiceModel> ValidateAsync(ValidateDiscountCodeServiceModel model)
        {
            var code = model.Code.Trim();
            var clientId = model.ClientId.Value;

            if (model.OrganisationId.HasValue is false || model.OrganisationId.Value == Guid.Empty)
            {
                return Invalid(DiscountCodeValidationStatus.ClientUnknown);
            }

            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == clientId && x.IsActive);

            if (client is null)
            {
                return Invalid(DiscountCodeValidationStatus.ClientUnknown);
            }

            // A seller may ask about any client of their own. Anyone else may ask only about the client of their own
            // organisation, which is what the client's own account and its team members have in common. The email is
            // not compared: a team member signs in with an email of their own.
            var isAuthorisedForClient = model.IsSeller
                ? client.SellerId == model.OrganisationId.Value
                : client.OrganisationId == model.OrganisationId.Value;

            if (isAuthorisedForClient is false)
            {
                return Invalid(DiscountCodeValidationStatus.ClientUnknown);
            }

            var discountCode = await _context.DiscountCodes.FirstOrDefaultAsync(x => x.SellerId == client.SellerId && x.Code == code && x.IsActive);

            if (discountCode is null)
            {
                return Invalid(DiscountCodeValidationStatus.NotFound);
            }

            if (discountCode.IsDisabled)
            {
                return Invalid(DiscountCodeValidationStatus.Disabled);
            }

            var isAssigned = await _context.ClientsDiscountCodes.AnyAsync(x => x.ClientId == client.Id && x.DiscountCodeId == discountCode.Id && x.IsActive);

            if (isAssigned is false)
            {
                return Invalid(DiscountCodeValidationStatus.NotAssigned);
            }

            return new DiscountCodeValidationServiceModel
            {
                Status = DiscountCodeValidationStatus.Valid,
                DiscountCode = discountCode.Code
            };
        }

        private static DiscountCodeValidationServiceModel Invalid(DiscountCodeValidationStatus status)
        {
            return new DiscountCodeValidationServiceModel { Status = status };
        }

        private static bool IsUniqueIndexViolation(DbUpdateException exception)
        {
            return exception.InnerException is SqlException sqlException
                && (sqlException.Number == SqlDuplicateKeyErrorNumber || sqlException.Number == SqlUniqueConstraintErrorNumber);
        }
    }
}
