using DownloadCenter.Api.Infrastructure;
using DownloadCenter.Api.ServicesModels.DownloadCenter;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;

namespace DownloadCenter.Api.Services.DownloadCenter
{
    public class DownloadCenterService : IDownloadCenterService
    {
        public readonly DownloadContext context;

        public DownloadCenterService(
            DownloadContext context)
        {
            this.context = context;
        }

        public async Task<PagedResults<IEnumerable<DownloadCenterServiceModel>>> GetAsync(GetDownloadCenterServiceModel model)
        {
            var downloads = this.context.Downloads.Where(x => x.IsActive);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                var category = this.context.CategoryTranslations.Where(x => x.Name.StartsWith(model.Username)).FirstOrDefault();

                downloads = downloads.Where(x => x.CategoryId == category.Id || x.Id.ToString() == model.SearchTerm);
            }

            downloads = downloads.ApplySort(model.OrderBy);

            var pagedResults = downloads.PagedIndex(new Pagination(downloads.Count(), model.ItemsPerPage), model.PageIndex);

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

        public async Task<DownloadCategoriesServiceModel> GetAsync(GetDownloadCategoryServiceModel model)
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
    }
}
