using Client.Api.Infrastructure;
using Client.Api.Infrastructure.Groups.Entities;
using Client.Api.ServicesModels.Groups;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
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

namespace Client.Api.Services.Groups
{
    public class GroupsService : IGroupsService
    {
        private readonly ClientContext context;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;

        public GroupsService(
            ClientContext context)
        {
            this.context = context;
        }

        public async Task<Guid> CreateAsync(CreateGroupServiceModel model)
        {
            var group = new Group();

            this.context.Groups.Add(group.FillCommonProperties());
            var groupTranslation = new GroupTranslation
            {
                Name = model.Name,
                Language = model.Language,
                GroupId = group.Id
            };

            this.context.GroupTranslations.Add(groupTranslation.FillCommonProperties());
            await this.context.SaveChangesAsync();

            return group.Id;
        }

        public async Task DeleteAsync(DeleteGroupServiceModel model)
        {
            var group = await this.context.Groups.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (group is null)
            {
                throw new CustomException(this.clientLocalizer.GetString("GroupNotFound"), (int)HttpStatusCode.NotFound);
            }

            if (await this.context.ClientsGroups.AnyAsync(x => x.GroupId == model.Id && x.IsActive))
            {
                throw new CustomException(this.clientLocalizer.GetString("GroupDeleteClientConflict"), (int)HttpStatusCode.Conflict);
            }

            group.IsActive = false;

            await this.context.SaveChangesAsync();
        }

        public async Task<GroupServiceModel> GetAsync(GetGroupServiceModel model)
        {
            var group = await this.context.Groups.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);
            
            if (group is not null)
            {
                var groupItem = new GroupServiceModel
                {
                    Id = group.Id,
                    LastModifiedDate = group.LastModifiedDate,
                    CreatedDate = group.CreatedDate,
                };

                var groupTranslation = await this.context.GroupTranslations.FirstOrDefaultAsync(x => x.GroupId == model.Id && x.Language == model.Language && x.IsActive);

                if (groupTranslation is null)
                {
                    groupTranslation = await this.context.GroupTranslations.FirstOrDefaultAsync(x => x.GroupId == model.Id && x.IsActive);
                }

                groupItem.Name = groupTranslation?.Name;

                return groupItem;
            }

            return default;
        }

        public async Task<PagedResults<IEnumerable<GroupServiceModel>>> GetAsync(GetGroupsServiceModel model)
        {
            var groups = this.context.Groups.Where(x => x.IsActive);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                groups = groups.Where(x => x.Translations.Any(x => x.Name.StartsWith(model.SearchTerm)));
            }

            groups = groups.ApplySort(model.OrderBy);

            var pagedResults = groups.PagedIndex(new Pagination(groups.Count(), model.ItemsPerPage), model.PageIndex);

            var pagedGroupServiceModel = new PagedResults<IEnumerable<GroupServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var groupItems = new List<GroupServiceModel>();

            foreach (var group in pagedResults.Data.OrEmptyIfNull().ToList())
            {
                var item = new GroupServiceModel
                {
                    Id = group.Id,
                    LastModifiedDate = group.LastModifiedDate,
                    CreatedDate = group.CreatedDate
                };

                var groupTranslations = await this.context.GroupTranslations.FirstOrDefaultAsync(x => x.Language == model.Language && x.GroupId == group.Id && x.IsActive);
                if (groupTranslations is null)
                {
                    groupTranslations = this.context.GroupTranslations.FirstOrDefault(x => x.GroupId == group.Id && x.IsActive);
                }

                item.Name = groupTranslations?.Name;

                groupItems.Add(item);
            }

            pagedGroupServiceModel.Data = groupItems;

            return pagedGroupServiceModel;
        }

        public async Task<PagedResults<IEnumerable<GroupServiceModel>>> GetByIdsAsync(GetGroupsByIdsServiceModel model)
        {
            var groups = this.context.Groups.Where(x => model.Ids.Contains(x.Id) && x.IsActive);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                groups = groups.Where(x => x.Translations.Any(x => x.Name.StartsWith(model.SearchTerm)));
            }

            groups = groups.ApplySort(model.OrderBy);

            var pagedResults = groups.PagedIndex(new Pagination(groups.Count(), model.ItemsPerPage), model.PageIndex);

            var pagedGroupServiceModel = new PagedResults<IEnumerable<GroupServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var groupItems = new List<GroupServiceModel>();

            foreach (var group in pagedResults.Data.OrEmptyIfNull().ToList())
            {
                var item = new GroupServiceModel
                {
                    Id = group.Id,
                    LastModifiedDate = group.LastModifiedDate,
                    CreatedDate = group.CreatedDate
                };

                var groupTranslations = await this.context.GroupTranslations.FirstOrDefaultAsync(x => x.Language == model.Language && x.GroupId == group.Id && x.IsActive);
                if (groupTranslations is null)
                {
                    groupTranslations = this.context.GroupTranslations.FirstOrDefault(x => x.GroupId == group.Id && x.IsActive);
                }

                item.Name = groupTranslations?.Name;

                groupItems.Add(item);
            }

            pagedGroupServiceModel.Data = groupItems;

            return pagedGroupServiceModel;
        }

        public async Task<Guid> UpdateAsync(UpdateGroupServiceModel model)
        {
            var group = await this.context.Groups.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);
            
            if (group == null)
            {
                throw new CustomException(this.clientLocalizer.GetString("GroupNotFound"), (int)HttpStatusCode.NotFound);
            }

            var groupTranslation = await this.context.GroupTranslations.FirstOrDefaultAsync(x => x.GroupId == model.Id && x.Language == model.Language && x.IsActive);

            if (groupTranslation is not null)
            {
                groupTranslation.Name = model.Name;
                groupTranslation.LastModifiedDate = DateTime.UtcNow;
            } else
            {
                var newGroupTranslation = new GroupTranslation
                {
                    Name = model.Name,
                    Language = model.Language,
                    GroupId = group.Id
                };

                await this.context.GroupTranslations.AddAsync(newGroupTranslation.FillCommonProperties());
            }

            group.LastModifiedDate = DateTime.UtcNow;

            await this.context.SaveChangesAsync();

            return group.Id;
        }
    }
}
