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
    public class ClientGroupsService : IClientGroupsService
    {
        private readonly ClientContext context;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;

        public ClientGroupsService(
            ClientContext context,
            IStringLocalizer<ClientResources> clientLocalizer)
        {
            this.context = context;
            this.clientLocalizer = clientLocalizer;
        }

        public async Task<Guid> CreateAsync(CreateClientGroupServiceModel model)
        {
            var group = new ClientGroup();

            this.context.ClientGroup.Add(group.FillCommonProperties());
            var groupTranslation = new ClientGroupTranslations
            {
                Name = model.Name,
                Language = model.Language,
                GroupId = group.Id
            };

            this.context.ClientGroupTranslations.Add(groupTranslation.FillCommonProperties());

            await this.context.SaveChangesAsync();

            return group.Id;
        }

        public async Task DeleteAsync(DeleteClientGroupServiceModel model)
        {
            var group = await this.context.ClientGroup.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (group is null)
            {
                throw new CustomException(this.clientLocalizer.GetString("GroupNotFound"), (int)HttpStatusCode.NotFound);
            }

            if (await this.context.ClientGroups.AnyAsync(x => x.GroupId == model.Id && x.IsActive))
            {
                throw new CustomException(this.clientLocalizer.GetString("GroupDeleteClientConflict"), (int)HttpStatusCode.Conflict);
            }

            group.IsActive = false;

            await this.context.SaveChangesAsync();
        }

        public async Task<ClientGroupServiceModel> GetAsync(GetClientGroupServiceModel model)
        {
            var group = await this.context.ClientGroup.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);
            
            if (group is not null)
            {
                var groupItem = new ClientGroupServiceModel
                {
                    Id = group.Id,
                    LastModifiedDate = group.LastModifiedDate,
                    CreatedDate = group.CreatedDate,
                };

                var groupTranslation = await this.context.ClientGroupTranslations.FirstOrDefaultAsync(x => x.GroupId == model.Id && x.Language == model.Language && x.IsActive);

                if (groupTranslation is null)
                {
                    groupTranslation = await this.context.ClientGroupTranslations.FirstOrDefaultAsync(x => x.GroupId == model.Id && x.IsActive);
                }

                groupItem.Name = groupTranslation?.Name;

                return groupItem;
            }

            return default;
        }

        public async Task<PagedResults<IEnumerable<ClientGroupServiceModel>>> GetAsync(GetClientGroupsServiceModel model)
        {
            var groups = this.context.ClientGroup.Where(x => x.IsActive);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                groups = groups.Where(x => x.Translations.Any(x => x.Name.StartsWith(model.SearchTerm)));
            }

            groups = groups.ApplySort(model.OrderBy);

            var pagedResults = groups.PagedIndex(new Pagination(groups.Count(), model.ItemsPerPage), model.PageIndex);

            var pagedGroupServiceModel = new PagedResults<IEnumerable<ClientGroupServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var groupItems = new List<ClientGroupServiceModel>();

            foreach (var group in pagedResults.Data.OrEmptyIfNull().ToList())
            {
                var item = new ClientGroupServiceModel
                {
                    Id = group.Id,
                    LastModifiedDate = group.LastModifiedDate,
                    CreatedDate = group.CreatedDate
                };

                var groupTranslations = await this.context.ClientGroupTranslations.FirstOrDefaultAsync(x => x.Language == model.Language && x.GroupId == group.Id && x.IsActive);
                if (groupTranslations is null)
                {
                    groupTranslations = this.context.ClientGroupTranslations.FirstOrDefault(x => x.GroupId == group.Id && x.IsActive);
                }

                item.Name = groupTranslations?.Name;

                groupItems.Add(item);
            }

            pagedGroupServiceModel.Data = groupItems;

            return pagedGroupServiceModel;
        }

        public async Task<PagedResults<IEnumerable<ClientGroupServiceModel>>> GetByIdsAsync(GetClientGroupsByIdsServiceModel model)
        {
            var groups = this.context.ClientGroup.Where(x => model.Ids.Contains(x.Id) && x.IsActive);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                groups = groups.Where(x => x.Translations.Any(x => x.Name.StartsWith(model.SearchTerm)));
            }

            groups = groups.ApplySort(model.OrderBy);

            var pagedResults = groups.PagedIndex(new Pagination(groups.Count(), model.ItemsPerPage), model.PageIndex);

            var pagedGroupServiceModel = new PagedResults<IEnumerable<ClientGroupServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var groupItems = new List<ClientGroupServiceModel>();

            foreach (var group in pagedResults.Data.OrEmptyIfNull().ToList())
            {
                var item = new ClientGroupServiceModel
                {
                    Id = group.Id,
                    LastModifiedDate = group.LastModifiedDate,
                    CreatedDate = group.CreatedDate
                };

                var groupTranslations = await this.context.ClientGroupTranslations.FirstOrDefaultAsync(x => x.Language == model.Language && x.GroupId == group.Id && x.IsActive);
                if (groupTranslations is null)
                {
                    groupTranslations = this.context.ClientGroupTranslations.FirstOrDefault(x => x.GroupId == group.Id && x.IsActive);
                }

                item.Name = groupTranslations?.Name;

                groupItems.Add(item);
            }

            pagedGroupServiceModel.Data = groupItems;

            return pagedGroupServiceModel;
        }

        public async Task<Guid> UpdateAsync(UpdateClientGroupServiceModel model)
        {
            var group = await this.context.ClientGroup.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);
            
            if (group == null)
            {
                throw new CustomException(this.clientLocalizer.GetString("GroupNotFound"), (int)HttpStatusCode.NotFound);
            }

            var groupTranslation = await this.context.ClientGroupTranslations.FirstOrDefaultAsync(x => x.GroupId == model.Id && x.Language == model.Language && x.IsActive);

            if (groupTranslation is not null)
            {
                groupTranslation.Name = model.Name;
                groupTranslation.LastModifiedDate = DateTime.UtcNow;
            } 
            else
            {
                var newGroupTranslation = new ClientGroupTranslations
                {
                    Name = model.Name,
                    Language = model.Language,
                    GroupId = group.Id
                };

                await this.context.ClientGroupTranslations.AddAsync(newGroupTranslation.FillCommonProperties());
            }

            group.LastModifiedDate = DateTime.UtcNow;

            await this.context.SaveChangesAsync();

            return group.Id;
        }
    }
}
