using DownloadCenter.Api.Infrastructure;
using DownloadCenter.Api.Infrastructure.Entities.Categories;
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
        private readonly DownloadCenterContext context;
        private readonly IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer;

        public DownloadCenterService(
            IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer,
            DownloadCenterContext context)
        {
            this.context = context;
            this.downloadCenterLocalizer = downloadCenterLocalizer;
        }

        public async Task<Guid> CreateAsync(CreateDownloadCenterFileServiceModel model)
        {
            foreach(var categoryId in model.CategoriesIds.OrEmptyIfNull())
            {
                foreach(var fileId in model.Files.OrEmptyIfNull())
                {
                    var categoryFile = new CategoryFile
                    {
                        CategoryId = categoryId,
                        MediaId = fileId
                    };

                    await this.context.CategoryFiles.AddAsync(categoryFile.FillCommonProperties());
                }
            }

            await this.context.SaveChangesAsync();

            return model.Files.FirstOrDefault();
        }

        public async Task DeleteAsync(DeleteDownloadCenterFileServiceModel model)
        {
            var downloadCenterFile = this.context.CategoryFiles.Where(x => x.MediaId == model.Id && x.IsActive);

            if (downloadCenterFile is null)
            {
                throw new CustomException(this.downloadCenterLocalizer.GetString("DownloadCenterFileNotFound"), (int)HttpStatusCode.NotFound);
            }

            foreach(var downloadCenterCategoryFile in downloadCenterFile.OrEmptyIfNull())
            {
                downloadCenterCategoryFile.IsActive = false;
                downloadCenterCategoryFile.LastModifiedDate = DateTime.UtcNow;
            }

            await this.context.SaveChangesAsync();
        }

        public async Task<PagedResults<IEnumerable<DownloadCenterFileServiceModel>>> GetAsync(GetDownloadCenterFilesServiceModel model)
        {
            var downloadCenterFiles = this.context.CategoryFiles.Where(x => x.IsActive);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                var category = this.context.CategoryTranslations.Where(x => x.Name.StartsWith(model.SearchTerm)).FirstOrDefault();

                downloadCenterFiles = downloadCenterFiles.Where(x => x.CategoryId == category.Id || x.Id.ToString() == model.SearchTerm);
            }

            downloadCenterFiles = downloadCenterFiles.ApplySort(model.OrderBy);

            var downloadCenterFileGroups = downloadCenterFiles.ToList().GroupBy(x => x.MediaId);

            var pagedResults = downloadCenterFiles.PagedIndex(new Pagination(downloadCenterFileGroups.Count(), model.ItemsPerPage), model.PageIndex);

            var pagedDownloadCenterFilesServiceModel = new PagedResults<IEnumerable<DownloadCenterFileServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var downloadCenterFilesList = new List<DownloadCenterFileServiceModel>();

            foreach (var downloadCenterFileGroup in downloadCenterFileGroups.OrEmptyIfNull())
            {
                var fileGroup = new DownloadCenterFileServiceModel
                {
                    Id = downloadCenterFileGroup.FirstOrDefault().MediaId,
                    LastModifiedDate = downloadCenterFileGroup.FirstOrDefault().LastModifiedDate,
                    CreatedDate = downloadCenterFileGroup.FirstOrDefault().CreatedDate,
                };

                var namesOfCategories = new List<string>();

                foreach(var categoryId in downloadCenterFileGroup.OrEmptyIfNull().Select(x => x.CategoryId))
                {
                    var downloadCenterFileCategory = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == categoryId && x.Language == model.Language && x.IsActive);

                    if (downloadCenterFileCategory is null)
                    {
                        downloadCenterFileCategory = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == categoryId && x.IsActive);
                    }

                    namesOfCategories.Add(downloadCenterFileCategory?.Name);
                }

                fileGroup.Categories = namesOfCategories;

                downloadCenterFilesList.Add(fileGroup);
            }

            pagedDownloadCenterFilesServiceModel.Data = downloadCenterFilesList;

            return pagedDownloadCenterFilesServiceModel;
        }

        public async Task<DownloadCenterCategoriesServiceModel> GetAsync(GetDownloadCenterFileServiceModel model)
        {
            var downloadCenterFile = await this.context.CategoryFiles.FirstOrDefaultAsync(x => x.MediaId == model.Id && x.IsActive);

            if (downloadCenterFile is null)
            {
                throw new CustomException(this.downloadCenterLocalizer.GetString("DownloadCenterFileNotFound"), (int)HttpStatusCode.NotFound);
            }

            var downloadCenterFileItem = new DownloadCenterCategoriesServiceModel
            {
                Id = downloadCenterFile.MediaId,
                LastModifiedDate = downloadCenterFile.LastModifiedDate,
                CreatedDate = downloadCenterFile.CreatedDate
            };

            var downloadCenterFileCategories = this.context.CategoryFiles.Where(x => x.MediaId == model.Id && x.IsActive);

            if (downloadCenterFileCategories is not null)
            {
                downloadCenterFileItem.CategoriesIds = downloadCenterFileCategories.Select(x => x.CategoryId);
            }

            return downloadCenterFileItem;
        }

        public async Task<PagedResults<IEnumerable<DownloadCenterItemServiceModel>>> GetAsync(GetDownloadCenterItemsServiceModel model)
        {
            var downloadCenterCategories = this.context.Categories.Where(x => x.IsActive && x.IsVisible && x.ParentCategoryId == null);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                downloadCenterCategories = downloadCenterCategories.Where(x => x.Translations.Any(y => y.Name.StartsWith(model.SearchTerm)) || x.Id.ToString() == model.SearchTerm);
            }

            downloadCenterCategories = downloadCenterCategories.ApplySort(model.OrderBy);

            var pagedResults = downloadCenterCategories.PagedIndex(new Pagination(downloadCenterCategories.Count(), model.ItemsPerPage), model.PageIndex);

            var pagedDownloadCenterServiceModel = new PagedResults<IEnumerable<DownloadCenterItemServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var downloadCenterItems = new List<DownloadCenterItemServiceModel>();

            foreach (var downloadCenterItem in pagedResults.Data.OrEmptyIfNull().ToList())
            {
                var item = new DownloadCenterItemServiceModel
                {
                    Id = downloadCenterItem.Id,
                    LastModifiedDate = downloadCenterItem.LastModifiedDate,
                    CreatedDate = downloadCenterItem.CreatedDate
                };

                var categoryTranslation = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCenterItem.Id && x.IsActive && x.Language == model.Language);

                if (categoryTranslation is null)
                {
                    categoryTranslation = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCenterItem.Id && x.IsActive);
                }

                item.Name = categoryTranslation?.Name;

                var categories = this.context.Categories.Where(x => x.ParentCategoryId == downloadCenterItem.Id);

                var downloadCategories = new List<DownloadCenterSubcategoryServiceModel>();

                foreach (var category in categories.OrEmptyIfNull().ToList())
                {
                    var categoryItem = new DownloadCenterSubcategoryServiceModel
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

                downloadCenterItems.Add(item);
            }

            pagedDownloadCenterServiceModel.Data = downloadCenterItems;

            return pagedDownloadCenterServiceModel;
        }

        public async Task<DownloadCenterCategoryServiceModel> GetDownloadCenterCategoryAsync(GetDownloadCenterFilesCategoryServiceModel model)
        {
            var downloadCenterCategory = await this.context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive && x.IsVisible);

            if (downloadCenterCategory is null)
            {
                throw new CustomException(this.downloadCenterLocalizer.GetString("DownloadCenterFileNotFound"), (int)HttpStatusCode.NotFound);
            }

            var category = new DownloadCenterCategoryServiceModel
            {
                Id = downloadCenterCategory.Id,
                ParentCategoryId = downloadCenterCategory.ParentCategoryId,
                LastModifiedDate = downloadCenterCategory.LastModifiedDate,
                CreatedDate = downloadCenterCategory.CreatedDate
            };

            var categoryTranslation = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCenterCategory.Id && x.IsActive && x.Language == model.Language);

            if (categoryTranslation is null)
            {
                categoryTranslation = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCenterCategory.Id && x.IsActive);
            }

            category.CategoryName = categoryTranslation?.Name;

            if (downloadCenterCategory.ParentCategoryId.HasValue)
            {
                var parentCategoryTranslation = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCenterCategory.ParentCategoryId && x.IsActive && x.Language == model.Language);

                if (parentCategoryTranslation is null)
                {
                    parentCategoryTranslation = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCenterCategory.ParentCategoryId && x.IsActive);
                }

                category.ParentCategoryName = parentCategoryTranslation?.Name;
            }

            var subcategories = this.context.Categories.Where(x => x.ParentCategoryId == downloadCenterCategory.Id && x.IsVisible && x.IsActive);

            var downloadCenterSubcategories = new List<DownloadCenterSubcategoryServiceModel>();

            foreach (var subcategory in subcategories.OrEmptyIfNull().ToList())
            {
                var subcategoryItem = new DownloadCenterSubcategoryServiceModel
                {
                    Id = subcategory.Id
                };

                var subcategoryTranslation = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == subcategory.Id && x.IsActive && x.Language == model.Language);

                if (subcategoryTranslation is null)
                {
                    subcategoryTranslation = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == subcategory.Id && x.IsActive);
                }

                subcategoryItem.Name = subcategoryTranslation?.Name;

                downloadCenterSubcategories.Add(subcategoryItem);
            }

            var files = this.context.CategoryFiles.Where(x => x.CategoryId == downloadCenterCategory.Id && x.IsActive);

            if (files.Any())
            {
                category.Files = files.Select(x => x.MediaId);
            }

            category.Subcategories = downloadCenterSubcategories;

            return category;
        }

        public async Task<Guid> UpdateAsync(UpdateDownloadCenterFileServiceModel model)
        {
            var downloadCenterFiles = this.context.CategoryFiles.Where(x => x.MediaId == model.Id && x.IsActive);

            if (downloadCenterFiles is null)
            {
                throw new CustomException(this.downloadCenterLocalizer.GetString("DownloadCenterFilesNotFound"), (int)HttpStatusCode.NotFound);
            }

            foreach(var downloadCenterFile in downloadCenterFiles.OrEmptyIfNull())
            {
                this.context.CategoryFiles.Remove(downloadCenterFile);
            }

            foreach (var categoryId in model.CategoriesIds.OrEmptyIfNull())
            {
                foreach(var fileId in model.Files.OrEmptyIfNull())
                {
                    var file = new CategoryFile
                    {
                        MediaId = fileId,
                        CategoryId = categoryId
                    };

                    await this.context.CategoryFiles.AddAsync(file.FillCommonProperties());
                }
            }

            await this.context.SaveChangesAsync();

            return model.Files.FirstOrDefault();
        }
    }
}
