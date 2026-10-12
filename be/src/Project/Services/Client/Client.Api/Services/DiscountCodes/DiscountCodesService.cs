using Client.Api.Infrastructure;
using Client.Api.Infrastructure.DiscountCodes.Entities;
using Client.Api.ServicesModels.DiscountCodes;
using Client.Api.Validators.DiscountCodes;
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
        private const string CodeIndexName = "IX_DiscountCodes_SellerId_Code";
        private const string AssignmentIndexName = "IX_ClientsDiscountCodes_DiscountCodeId_ClientId";

        private readonly ClientContext _context;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;

        public DiscountCodesService(
            ClientContext context,
            IStringLocalizer<ClientResources> clientLocalizer)
        {
            _context = context;
            _clientLocalizer = clientLocalizer;
        }

        public async Task<DiscountCodeServiceModel> CreateAsync(CreateDiscountCodeServiceModel model)
        {
            var sellerId = model.OrganisationId.Value;
            var code = model.Code.Trim();
            var clientIds = model.ClientIds.OrEmptyIfNull().Distinct().ToList();

            if (await _context.DiscountCodes.AnyAsync(x => x.SellerId == sellerId && x.Code == code && x.IsActive))
            {
                throw new ConflictException(_clientLocalizer.GetString("DiscountCodeExists"));
            }

            await EnsureClientsCanBeAssignedAsync(sellerId, clientIds);

            var discountCode = new DiscountCode
            {
                Code = code,
                Description = NormalizeDescription(model.Description),
                IsDisabled = model.IsDisabled,
                SellerId = sellerId
            };

            _context.DiscountCodes.Add(discountCode.FillCommonProperties());

            foreach (var clientId in clientIds)
            {
                _context.ClientsDiscountCodes.Add(new ClientsDiscountCode
                {
                    ClientId = clientId,
                    DiscountCodeId = discountCode.Id
                }.FillCommonProperties());
            }

            await SaveChangesAsync();

            return await GetAsync(new GetDiscountCodeServiceModel { Id = discountCode.Id, OrganisationId = sellerId });
        }

        public async Task<DiscountCodeServiceModel> UpdateAsync(UpdateDiscountCodeServiceModel model)
        {
            var sellerId = model.OrganisationId.Value;
            var code = model.Code.Trim();
            var clientIds = model.ClientIds.OrEmptyIfNull().Distinct().ToList();

            var discountCode = await FindDiscountCodeAsync(model.Id.Value, sellerId);

            if (await _context.DiscountCodes.AnyAsync(x => x.SellerId == sellerId && x.Code == code && x.IsActive && x.Id != discountCode.Id))
            {
                throw new ConflictException(_clientLocalizer.GetString("DiscountCodeExists"));
            }

            // Everything that can reject the request runs before the first change is made.
            await EnsureClientsCanBeAssignedAsync(sellerId, clientIds);

            SetOriginalVersion(discountCode, model.Version);

            discountCode.Code = code;
            discountCode.Description = NormalizeDescription(model.Description);
            discountCode.IsDisabled = model.IsDisabled;

            // The code row is always touched, even for an assignment-only edit, so that its row version
            // moves on and a second open form cannot silently replace this assignment list.
            discountCode.LastModifiedDate = DateTime.UtcNow;

            var existingLinks = await _context.ClientsDiscountCodes.Where(x => x.DiscountCodeId == discountCode.Id).ToListAsync();
            var requestedClientIds = clientIds.ToHashSet();

            foreach (var link in existingLinks.Where(x => requestedClientIds.Contains(x.ClientId) is false))
            {
                _context.ClientsDiscountCodes.Remove(link);
            }

            var linkedClientIds = existingLinks.Select(x => x.ClientId).ToHashSet();

            foreach (var clientId in requestedClientIds.Where(x => linkedClientIds.Contains(x) is false))
            {
                _context.ClientsDiscountCodes.Add(new ClientsDiscountCode
                {
                    ClientId = clientId,
                    DiscountCodeId = discountCode.Id
                }.FillCommonProperties());
            }

            await SaveChangesAsync();

            return await GetAsync(new GetDiscountCodeServiceModel { Id = discountCode.Id, OrganisationId = sellerId });
        }

        public async Task DeleteAsync(DeleteDiscountCodeServiceModel model)
        {
            var discountCode = await FindDiscountCodeAsync(model.Id.Value, model.OrganisationId.Value);

            SetOriginalVersion(discountCode, model.Version);

            discountCode.IsActive = false;
            discountCode.LastModifiedDate = DateTime.UtcNow;

            var links = await _context.ClientsDiscountCodes.Where(x => x.DiscountCodeId == discountCode.Id).ToListAsync();

            _context.ClientsDiscountCodes.RemoveRange(links);

            await SaveChangesAsync();
        }

        public async Task<DiscountCodeServiceModel> GetAsync(GetDiscountCodeServiceModel model)
        {
            var sellerId = model.OrganisationId.Value;
            var discountCode = await FindDiscountCodeAsync(model.Id.Value, sellerId);

            var clientIds = await (from l in _context.ClientsDiscountCodes
                                   join c in _context.Clients on l.ClientId equals c.Id
                                   where l.DiscountCodeId == discountCode.Id && c.IsActive
                                   orderby c.Name, c.Id
                                   select l.ClientId).ToListAsync();

            return new DiscountCodeServiceModel
            {
                Id = discountCode.Id,
                Code = discountCode.Code,
                Description = discountCode.Description,
                IsDisabled = discountCode.IsDisabled,
                ClientIds = clientIds,
                ClientsCount = clientIds.Count,
                Version = EncodeVersion(discountCode.RowVersion),
                LastModifiedDate = discountCode.LastModifiedDate,
                CreatedDate = discountCode.CreatedDate
            };
        }

        public async Task<PagedResults<IEnumerable<DiscountCodeServiceModel>>> GetAsync(GetDiscountCodesServiceModel model)
        {
            var sellerId = model.OrganisationId.Value;

            var discountCodes = from d in _context.DiscountCodes
                                where d.SellerId == sellerId && d.IsActive
                                select new DiscountCodeRow
                                {
                                    Id = d.Id,
                                    Code = d.Code,
                                    Description = d.Description,
                                    IsDisabled = d.IsDisabled,
                                    ClientsCount = (from l in _context.ClientsDiscountCodes
                                                    join c in _context.Clients on l.ClientId equals c.Id
                                                    where l.DiscountCodeId == d.Id && c.IsActive
                                                    select l.Id).Count(),
                                    RowVersion = d.RowVersion,
                                    LastModifiedDate = d.LastModifiedDate,
                                    CreatedDate = d.CreatedDate
                                };

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                discountCodes = discountCodes.Where(x => x.Code.StartsWith(model.SearchTerm));
            }

            discountCodes = discountCodes.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<DiscountCodeRow>> pagedRows;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                discountCodes = discountCodes.Take(Constants.MaxItemsPerPageLimit);

                pagedRows = discountCodes.PagedIndex(new Pagination(discountCodes.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedRows = discountCodes.PagedIndex(new Pagination(discountCodes.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            var rows = await MaterializeAsync(pagedRows.Data);

            return new PagedResults<IEnumerable<DiscountCodeServiceModel>>(pagedRows.Total, pagedRows.PageSize)
            {
                Data = rows.Select(x => new DiscountCodeServiceModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Description = x.Description,
                    IsDisabled = x.IsDisabled,
                    ClientsCount = x.ClientsCount,
                    Version = EncodeVersion(x.RowVersion),
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                }).ToList()
            };
        }

        public async Task<PagedResults<IEnumerable<DiscountCodeClientOptionServiceModel>>> GetClientOptionsAsync(GetDiscountCodeClientOptionsServiceModel model)
        {
            var sellerId = model.OrganisationId.Value;

            // Only this seller's own active clients may be offered for assignment. Disabled clients stay
            // selectable: a disabled account is not a deleted one.
            var clients = from c in _context.Clients
                          where c.SellerId == sellerId && c.IsActive
                          select new DiscountCodeClientOptionServiceModel
                          {
                              Id = c.Id,
                              Name = c.Name
                          };

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                clients = clients.Where(x => x.Name.Contains(model.SearchTerm));
            }

            clients = clients.ApplySort(model.OrderBy, "Name asc");

            PagedResults<IEnumerable<DiscountCodeClientOptionServiceModel>> pagedClients;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                clients = clients.Take(Constants.MaxItemsPerPageLimit);

                pagedClients = clients.PagedIndex(new Pagination(clients.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedClients = clients.PagedIndex(new Pagination(clients.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return new PagedResults<IEnumerable<DiscountCodeClientOptionServiceModel>>(pagedClients.Total, pagedClients.PageSize)
            {
                Data = await MaterializeAsync(pagedClients.Data)
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

            var isAssigned = await _context.ClientsDiscountCodes.AnyAsync(x => x.ClientId == client.Id && x.DiscountCodeId == discountCode.Id);

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

        private async Task<DiscountCode> FindDiscountCodeAsync(Guid id, Guid sellerId)
        {
            var discountCode = await _context.DiscountCodes.FirstOrDefaultAsync(x => x.Id == id && x.SellerId == sellerId && x.IsActive);

            if (discountCode is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("DiscountCodeNotFound"));
            }

            return discountCode;
        }

        private async Task EnsureClientsCanBeAssignedAsync(Guid sellerId, IReadOnlyCollection<Guid> clientIds)
        {
            if (clientIds.Count == 0)
            {
                return;
            }

            var assignableCount = await _context.Clients.CountAsync(x => clientIds.Contains(x.Id) && x.SellerId == sellerId && x.IsActive);

            if (assignableCount != clientIds.Count)
            {
                throw new UnprocessableEntityException(_clientLocalizer.GetString("DiscountCodeClientsInvalid"));
            }
        }

        private void SetOriginalVersion(DiscountCode discountCode, string version)
        {
            if (DiscountCodeRules.TryDecodeVersion(version, out var rowVersion) is false)
            {
                throw new ConflictException(_clientLocalizer.GetString("DiscountCodeChanged"));
            }

            _context.Entry(discountCode).Property(x => x.RowVersion).OriginalValue = rowVersion;
        }

        private async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConflictException(_clientLocalizer.GetString("DiscountCodeChanged"));
            }
            catch (DbUpdateException ex) when (IsUniqueIndexViolation(ex, CodeIndexName))
            {
                throw new ConflictException(_clientLocalizer.GetString("DiscountCodeExists"));
            }
            catch (DbUpdateException ex) when (IsUniqueIndexViolation(ex, AssignmentIndexName))
            {
                // The same client was assigned by somebody else since this form was opened.
                throw new ConflictException(_clientLocalizer.GetString("DiscountCodeChanged"));
            }
        }

        // An empty page comes back as an in-memory list, which the EF async operators refuse to enumerate.
        private static async Task<List<T>> MaterializeAsync<T>(IEnumerable<T> page)
        {
            return page is IAsyncEnumerable<T> && page is IQueryable<T> query
                ? await query.ToListAsync()
                : page.ToList();
        }

        private static string NormalizeDescription(string description)
        {
            return string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        }

        private static string EncodeVersion(byte[] rowVersion)
        {
            return rowVersion is null ? null : Convert.ToBase64String(rowVersion);
        }

        private static DiscountCodeValidationServiceModel Invalid(DiscountCodeValidationStatus status)
        {
            return new DiscountCodeValidationServiceModel { Status = status };
        }

        private static bool IsUniqueIndexViolation(DbUpdateException exception, string indexName)
        {
            return exception.InnerException is SqlException sqlException
                && (sqlException.Number == SqlDuplicateKeyErrorNumber || sqlException.Number == SqlUniqueConstraintErrorNumber)
                && sqlException.Message.Contains(indexName, StringComparison.OrdinalIgnoreCase);
        }

        internal sealed class DiscountCodeRow
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
            public bool IsDisabled { get; set; }
            public int ClientsCount { get; set; }
            public byte[] RowVersion { get; set; }
            public DateTime? LastModifiedDate { get; set; }
            public DateTime CreatedDate { get; set; }
        }
    }
}
