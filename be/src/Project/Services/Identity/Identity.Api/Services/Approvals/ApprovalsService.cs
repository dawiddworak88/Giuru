using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Identity.Api.Infrastructure;
using Identity.Api.Infrastructure.Approvals.Entities;
using Identity.Api.ServicesModels.Approvals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Identity.Api.Services.Approvals
{
    public class ApprovalsService : IApprovalsService
    {
        private readonly IdentityContext _context;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;

        public ApprovalsService(
            IdentityContext context,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            _context = context;
            _globalLocalizer = globalLocalizer;
        }

        public async Task<Guid> CreateAsync(CreateApprovalServiceModel model)
        {
            var approval = new Approval();

            await _context.Approvals.AddAsync(approval.FillCommonProperties());

            var approvalTranslation = new ApprovalTranslation
            {
                ApprovalId = approval.Id,
                Name = model.Name,
                Description = model.Description,
                Language = model.Language
            };

            await _context.ApprovalTranslations.AddAsync(approvalTranslation.FillCommonProperties());
            await _context.SaveChangesAsync();

            return approval.Id;
        }

        public async Task DeleteAsync(DeleteApprovalServiceModel model)
        {
            var approval = await _context.Approvals.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (approval is null)
            {
                throw new NotFoundException(_globalLocalizer.GetString("ApprovalNotFound"));
            }

            approval.IsActive = false;

            await _context.SaveChangesAsync();
        }

        public PagedResults<IEnumerable<ApprovalServiceModel>> Get(GetApprovalsServiceModel model)
        {
            var approvals = _context.Approvals.Where(x => x.IsActive)
                .Include(x => x.Translations)
                .AsSingleQuery()
                .Select(x => new ApprovalServiceModel
                {
                    Id = x.Id,
                    Name = x.Translations.FirstOrDefault(t => t.ApprovalId == x.Id && t.Language == model.Language) != null ? x.Translations.FirstOrDefault(t => t.ApprovalId == x.Id && t.Language == model.Language).Name : x.Translations.FirstOrDefault(t => t.ApprovalId == x.Id).Name,
                    Description = x.Translations.FirstOrDefault(t => t.ApprovalId == x.Id && t.Language == model.Language) != null ? x.Translations.FirstOrDefault(t => t.ApprovalId == x.Id && t.Language == model.Language).Description : x.Translations.FirstOrDefault(t => t.ApprovalId == x.Id).Description,
                    CreatedDate = x.CreatedDate,
                    LastModifiedDate = x.LastModifiedDate
                });

            if (string.IsNullOrEmpty(model.SearchTerm) is false)
            {
                approvals = approvals.Where(x => x.Name.StartsWith(model.SearchTerm));
            }

            approvals = approvals.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<ApprovalServiceModel>> pagedResult;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                approvals = approvals.Take(Constants.MaxItemsPerPageLimit);

                pagedResult = approvals.PagedIndex(new Pagination(approvals.Count(), Constants.MaxItemsPerPage), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResult = approvals.PagedIndex(new Pagination(approvals.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return pagedResult;
        }

        public async Task<ApprovalServiceModel> GetAsync(GetApprovalServiceModel model)
        {
            var approval = await _context.Approvals.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (approval is null)
            {
                throw new NotFoundException(_globalLocalizer.GetString("ApprovalNotFound"));
            }

            return new ApprovalServiceModel
            {
                Id = approval.Id,
                Name = approval.Translations.FirstOrDefault(t => t.ApprovalId == approval.Id && t.Language == model.Language) != null ? approval.Translations.FirstOrDefault(t => t.ApprovalId == approval.Id && t.Language == model.Language).Name : approval.Translations.FirstOrDefault(t => t.ApprovalId == approval.Id).Name,
                Description = approval.Translations.FirstOrDefault(t => t.ApprovalId == approval.Id && t.Language == model.Language) != null ? approval.Translations.FirstOrDefault(t => t.ApprovalId == approval.Id && t.Language == model.Language).Description : approval.Translations.FirstOrDefault(t => t.ApprovalId == approval.Id).Description,
                CreatedDate = approval.CreatedDate
            };
        }

        public async Task<Guid> UpdateAsync(UpdateApprovalServiceModel model)
        {
            var approval = await _context.Approvals.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (approval is null)
            {
                throw new NotFoundException(_globalLocalizer.GetString("ApprovalNotFound"));
            }

            var approvalTranslation = await _context.ApprovalTranslations.FirstOrDefaultAsync(x => x.ApprovalId == approval.Id && x.IsActive && x.Language == model.Language);

            if (approvalTranslation is not null)
            {
                approvalTranslation.Name = model.Name;
                approvalTranslation.Description = model.Description;
                approvalTranslation.LastModifiedDate = DateTime.UtcNow;
            }
            else
            {
                var newApprovalTranslation = new ApprovalTranslation
                {
                    Name = model.Name,
                    Description = model.Description,
                    Language = model.Language,
                    ApprovalId = approval.Id
                };

                await _context.ApprovalTranslations.AddAsync(newApprovalTranslation.FillCommonProperties());
            }

            approval.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return approval.Id;
        }
    }
}
