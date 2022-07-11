using DownloadCenter.Api.Infrastructure;
using DownloadCenter.Api.Infrastructure.Entities.DownloadCenter;
using DownloadCenter.Api.ServicesModels.DownloadCenter;
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
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;

namespace DownloadCenter.Api.Services.DownloadCenter
{
    public class DownloadCenterService : IDownloadCenterService
    {
        private readonly DownloadContext context;
        private readonly IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer;

        public DownloadCenterService(
            IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer,
            DownloadContext context)
        {
            this.context = context;
            this.downloadCenterLocalizer = downloadCenterLocalizer;
        }

        public async Task<Guid> CreateAsync(CreateDownloadCenterItemServiceModel model)
        {
            var downloadCenterItem = new DownloadCenterItem
            {
                CategoryId = model.CategoryId.Value,
                Order = model.Order.Value
            };

            await this.context.DownloadCenterItems.AddAsync(downloadCenterItem.FillCommonProperties());
            await this.context.SaveChangesAsync();

            return downloadCenterItem.Id;
        }

        public async Task DeleteAsync(DeleteDownloadCenterItemServiceModel model)
        {
            var downloadCenterItem = await this.context.DownloadCenterItems.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (downloadCenterItem is null)
            {
                throw new CustomException(this.downloadCenterLocalizer.GetString("DownloadCenterNotFound"), (int)HttpStatusCode.NotFound);
            }

            downloadCenterItem.IsActive = false;

            await this.context.SaveChangesAsync();
        }

        public async Task<PagedResults<IEnumerable<DownloadCenterServiceModel>>> GetAsync(GetDownloadCenterServiceModel model)
        {
            var downloadCenter = this.context.DownloadCenterItems.Where(x => x.IsActive);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                var category = this.context.CategoryTranslations.Where(x => x.Name.StartsWith(model.Username)).FirstOrDefault();

                downloadCenter = downloadCenter.Where(x => x.CategoryId == category.Id || x.Id.ToString() == model.SearchTerm);
            }

            downloadCenter = downloadCenter.ApplySort(model.OrderBy);

            var pagedResults = downloadCenter.PagedIndex(new Pagination(downloadCenter.Count(), model.ItemsPerPage), model.PageIndex);

            var pagedDownloadsServiceModel = new PagedResults<IEnumerable<DownloadCenterServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var downloadItems = new List<DownloadCenterServiceModel>();

            foreach (var downloadItem in pagedResults.Data.OrEmptyIfNull().ToList())
            {
                var item = new DownloadCenterServiceModel
                {
                    Id = downloadItem.Id,
                    CategoryId = downloadItem.CategoryId,
                    Order = downloadItem.Order,
                    LastModifiedDate = downloadItem.LastModifiedDate,
                    CreatedDate = downloadItem.CreatedDate
                };

                var categoryTranslation = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadItem.CategoryId && x.IsActive && x.Language == model.Language);

                if (categoryTranslation is null)
                {
                    categoryTranslation = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadItem.CategoryId && x.IsActive);
                }

                item.CategoryName = categoryTranslation?.Name;

                var categories = this.context.Categories.Where(x => x.ParentCategoryId == downloadItem.CategoryId);

                var downloadCategories = new List<DownloadCategoryServiceModel>();

                foreach (var category in categories.OrEmptyIfNull().ToList())
                {
                    var categoryItem = new DownloadCategoryServiceModel
                    {
                        Id = category.Id
                    };

                    var categoryItemTranslation = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == category.Id && x.IsActive && x.Language == model.Language);

                    if (categoryItemTranslation is null)
                    {
                        categoryItemTranslation = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == category.Id && x.IsActive);
                    }

                    categoryItem.Name = categoryItemTranslation?.Name;

                    downloadCategories.Add(categoryItem);
                }

                item.Categories = downloadCategories;

                downloadItems.Add(item);
            }

            pagedDownloadsServiceModel.Data = downloadItems;

            return pagedDownloadsServiceModel;
        }

        public async Task<DownloadCenterItemServiceModel> GetAsync(GetDownloadCenterItemServiceModel model)
        {
            var downloadCenterItem = await this.context.DownloadCenterItems.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (downloadCenterItem is null)
            {
                throw new CustomException("", (int)HttpStatusCode.NotFound);
            }

            var item = new DownloadCenterItemServiceModel
            {
                Id = downloadCenterItem.Id,
                CategoryId = downloadCenterItem.CategoryId,
                Order = downloadCenterItem.Order,
                LastModifiedDate = downloadCenterItem.LastModifiedDate,
                CreatedDate = downloadCenterItem.CreatedDate
            };

            var categoryTranslation = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCenterItem.Id && x.IsActive && x.Language == model.Language);

            if (categoryTranslation is null)
            {
                categoryTranslation = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCenterItem.Id && x.IsActive);
            }

            item.CategoryName = categoryTranslation?.Name;

            return item;
        }

        public async Task<DownloadCategoriesServiceModel> GetDownloadCenterCategoryAsync(GetDownloadCategoryServiceModel model)
        {
            var downloadCategory = await this.context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (downloadCategory is null)
            {
                throw new CustomException("", (int)HttpStatusCode.NotFound);
            }

            var item = new DownloadCategoriesServiceModel
            {
                Id = downloadCategory.Id,
                LastModifiedDate = downloadCategory.LastModifiedDate,
                CreatedDate = downloadCategory.CreatedDate
            };

            var categoryTranslation = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCategory.Id && x.IsActive && x.Language == model.Language);

            if (categoryTranslation is null)
            {
                categoryTranslation = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCategory.Id && x.IsActive);
            }

            item.CategoryName = categoryTranslation?.Name;

            var categories = this.context.Categories.Where(x => x.ParentCategoryId == downloadCategory.Id);

            var downloadCategories = new List<DownloadCategoryServiceModel>();

            foreach (var category in categories.OrEmptyIfNull().ToList())
            {
                var categoryItem = new DownloadCategoryServiceModel
                {
                    Id = category.Id
                };

                var categoryItemTranslation = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == category.Id && x.IsActive && x.Language == model.Language);

                if (categoryItemTranslation is null)
                {
                    categoryItemTranslation = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == category.Id && x.IsActive);
                }

                categoryItem.Name = categoryItemTranslation?.Name;

                downloadCategories.Add(categoryItem);
            }

            item.Categories = downloadCategories;

            return item;
        }

        public async Task<Guid> UpdateAsync(UpdateDownloadCenterItemServiceModel model)
        {
            var downloadCenterItem = await this.context.DownloadCenterItems.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (downloadCenterItem is null)
            {
                throw new CustomException("", (int)HttpStatusCode.NotFound);
            }

            downloadCenterItem.CategoryId = model.CategoryId.Value;
            downloadCenterItem.Order = model.Order;
            downloadCenterItem.LastModifiedDate = DateTime.UtcNow;

            await this.context.SaveChangesAsync();

            return downloadCenterItem.Id;
        }
    }
}
