using DownloadCenter.Api.Infrastructure;
using DownloadCenter.Api.Infrastructure.Entities.DownloadCenterCategories;
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

        public async Task<Guid> CreateAsync(CreateDownloadCenterItemServiceModel model)
        {
            foreach(var categoryId in model.CategoriesIds.OrEmptyIfNull())
            {
                foreach(var file in model.Files.OrEmptyIfNull())
                {
                    var categoryFile = new DownloadCenterCategoryFile
                    {
                        CategoryId = categoryId,
                        MediaId = file.Id,
                        Filename = file.Filename
                    };

                    await this.context.DownloadCenterCategoryFiles.AddAsync(categoryFile.FillCommonProperties());
                }
            }

            await this.context.SaveChangesAsync();

            return model.Files.FirstOrDefault().Id;
        }

        public async Task DeleteAsync(DeleteDownloadCenterItemServiceModel model)
        {
            var downloadCenterFile = this.context.DownloadCenterCategoryFiles.Where(x => x.MediaId == model.Id && x.IsActive);

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

        public async Task<PagedResults<IEnumerable<DownloadCenterItemServiceModel>>> GetAsync(GetDownloadCenterFilesServiceModel model)
        {
            var downloadCenterFiles = this.context.DownloadCenterCategoryFiles.Where(x => x.IsActive);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                var searchFiles = downloadCenterFiles.Where(x => x.Id.ToString() == model.SearchTerm || x.Filename.StartsWith(model.SearchTerm));

                var categoryTranslation = await this.context.DownloadCenterCategoryTranslations.FirstOrDefaultAsync(x => x.Name.StartsWith(model.SearchTerm));

                if (categoryTranslation is not null)
                {
                    searchFiles = downloadCenterFiles.Where(x => x.CategoryId == categoryTranslation.CategoryId);
                }

                downloadCenterFiles = searchFiles;
            }

            downloadCenterFiles = downloadCenterFiles.ApplySort(model.OrderBy);

            var downloadCenterFileGroups = downloadCenterFiles.ToList().GroupBy(x => x.MediaId);

            var pagedResults = downloadCenterFiles.PagedIndex(new Pagination(downloadCenterFileGroups.Count(), model.ItemsPerPage), model.PageIndex);

            var pagedDownloadCenterFilesServiceModel = new PagedResults<IEnumerable<DownloadCenterItemServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var downloadCenterFilesList = new List<DownloadCenterItemServiceModel>();

            foreach (var downloadCenterFileGroup in downloadCenterFileGroups.OrEmptyIfNull())
            {
                var fileGroup = new DownloadCenterItemServiceModel
                {
                    Id = downloadCenterFileGroup.FirstOrDefault().MediaId,
                    Filename = downloadCenterFileGroup.FirstOrDefault().Filename,
                    LastModifiedDate = downloadCenterFileGroup.FirstOrDefault().LastModifiedDate,
                    CreatedDate = downloadCenterFileGroup.FirstOrDefault().CreatedDate,
                };

                var namesOfCategories = new List<string>();

                foreach(var categoryId in downloadCenterFileGroup.OrEmptyIfNull().Select(x => x.CategoryId))
                {
                    var downloadCenterFileCategory = this.context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.CategoryId == categoryId && x.Language == model.Language && x.IsActive);

                    if (downloadCenterFileCategory is null)
                    {
                        downloadCenterFileCategory = this.context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.CategoryId == categoryId && x.IsActive);
                    }

                    namesOfCategories.Add(downloadCenterFileCategory?.Name);
                }

                fileGroup.Categories = namesOfCategories;

                downloadCenterFilesList.Add(fileGroup);
            }

            pagedDownloadCenterFilesServiceModel.Data = downloadCenterFilesList;

            return pagedDownloadCenterFilesServiceModel;
        }

        public async Task<DownloadCenterItemFileServiceModel> GetAsync(GetDownloadCenterFileServiceModel model)
        {
            var downloadCenterFile = await this.context.DownloadCenterCategoryFiles.FirstOrDefaultAsync(x => x.MediaId == model.Id && x.IsActive);

            if (downloadCenterFile is null)
            {
                throw new CustomException(this.downloadCenterLocalizer.GetString("DownloadCenterFileNotFound"), (int)HttpStatusCode.NotFound);
            }

            var downloadCenterFileItem = new DownloadCenterItemFileServiceModel
            {
                Id = downloadCenterFile.MediaId,
                LastModifiedDate = downloadCenterFile.LastModifiedDate,
                CreatedDate = downloadCenterFile.CreatedDate
            };

            var downloadCenterFileCategories = this.context.DownloadCenterCategoryFiles.Where(x => x.MediaId == model.Id && x.IsActive);

            if (downloadCenterFileCategories is not null)
            {
                downloadCenterFileItem.CategoriesIds = downloadCenterFileCategories.Select(x => x.CategoryId);
            }

            return downloadCenterFileItem;
        }

        public async Task<PagedResults<IEnumerable<DownloadCenterCategoryItemServiceModel>>> GetAsync(GetDownloadCenterItemsServiceModel model)
        {
            var downloadCenterCategories = this.context.DownloadCenterCategories.Where(x => x.IsActive && x.IsVisible && x.ParentCategoryId == null);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                downloadCenterCategories = downloadCenterCategories.Where(x => x.Translations.Any(y => y.Name.StartsWith(model.SearchTerm)) || x.Id.ToString() == model.SearchTerm);
            }

            downloadCenterCategories = downloadCenterCategories.ApplySort(model.OrderBy);

            var pagedResults = downloadCenterCategories.PagedIndex(new Pagination(downloadCenterCategories.Count(), model.ItemsPerPage), model.PageIndex);

            var pagedDownloadCenterServiceModel = new PagedResults<IEnumerable<DownloadCenterCategoryItemServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var downloadCenterItems = new List<DownloadCenterCategoryItemServiceModel>();

            foreach (var downloadCenterItem in pagedResults.Data.OrEmptyIfNull().ToList())
            {
                var item = new DownloadCenterCategoryItemServiceModel
                {
                    Id = downloadCenterItem.Id,
                    LastModifiedDate = downloadCenterItem.LastModifiedDate,
                    CreatedDate = downloadCenterItem.CreatedDate
                };

                var categoryTranslation = this.context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCenterItem.Id && x.IsActive && x.Language == model.Language);

                if (categoryTranslation is null)
                {
                    categoryTranslation = this.context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCenterItem.Id && x.IsActive);
                }

                item.Name = categoryTranslation?.Name;

                var categories = this.context.DownloadCenterCategories.Where(x => x.ParentCategoryId == downloadCenterItem.Id && x.IsActive && x.IsVisible);

                var downloadCategories = new List<DownloadCenterSubcategoryServiceModel>();

                foreach (var category in categories.OrEmptyIfNull().ToList())
                {
                    var categoryItem = new DownloadCenterSubcategoryServiceModel
                    {
                        Id = category.Id
                    };

                    var categoryItemTranslation = this.context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.CategoryId == category.Id && x.IsActive && x.Language == model.Language);

                    if (categoryItemTranslation is null)
                    {
                        categoryItemTranslation = this.context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.CategoryId == category.Id && x.IsActive);
                    }

                    categoryItem.Name = categoryItemTranslation?.Name;

                    downloadCategories.Add(categoryItem);
                }

                item.Subcategories = downloadCategories;

                downloadCenterItems.Add(item);
            }

            pagedDownloadCenterServiceModel.Data = downloadCenterItems;

            return pagedDownloadCenterServiceModel;
        }

        public async Task<DownloadCenterCategoryServiceModel> GetDownloadCenterCategoryAsync(GetDownloadCenterCategoryServiceModel model)
        {
            var downloadCenterCategory = await this.context.DownloadCenterCategories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive && x.IsVisible);

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

            var categoryTranslation = this.context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCenterCategory.Id && x.IsActive && x.Language == model.Language);

            if (categoryTranslation is null)
            {
                categoryTranslation = this.context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCenterCategory.Id && x.IsActive);
            }

            category.CategoryName = categoryTranslation?.Name;

            if (downloadCenterCategory.ParentCategoryId.HasValue)
            {
                var parentCategoryTranslation = this.context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCenterCategory.ParentCategoryId && x.IsActive && x.Language == model.Language);

                if (parentCategoryTranslation is null)
                {
                    parentCategoryTranslation = this.context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCenterCategory.ParentCategoryId && x.IsActive);
                }

                category.ParentCategoryName = parentCategoryTranslation?.Name;
            }

            var subcategories = this.context.DownloadCenterCategories.Where(x => x.ParentCategoryId == downloadCenterCategory.Id && x.IsVisible && x.IsActive);

            var downloadCenterSubcategories = new List<DownloadCenterSubcategoryServiceModel>();

            foreach (var subcategory in subcategories.OrEmptyIfNull().ToList())
            {
                var subcategoryItem = new DownloadCenterSubcategoryServiceModel
                {
                    Id = subcategory.Id
                };

                var subcategoryTranslation = this.context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.CategoryId == subcategory.Id && x.IsActive && x.Language == model.Language);

                if (subcategoryTranslation is null)
                {
                    subcategoryTranslation = this.context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.CategoryId == subcategory.Id && x.IsActive);
                }

                subcategoryItem.Name = subcategoryTranslation?.Name;

                downloadCenterSubcategories.Add(subcategoryItem);
            }

            var files = this.context.DownloadCenterCategoryFiles.Where(x => x.CategoryId == downloadCenterCategory.Id && x.IsActive);

            if (files.Any())
            {
                category.Files = files.Select(x => x.MediaId);
            }

            category.Subcategories = downloadCenterSubcategories;

            return category;
        }

        public async Task<PagedResults<IEnumerable<DownloadCenterCategoryFileServiceModel>>> GetDownloadCenterCategoryFilesAsync(GetDownloadCenterCategoryFilesServiceModel model)
        {
            var downloadCenterCategoryFiles = from f in this.context.DownloadCenterCategoryFiles
                                              where f.CategoryId == model.Id && f.IsActive
                                              select new DownloadCenterCategoryFileServiceModel
                                              {
                                                  Id = f.MediaId,
                                                  Filename = f.Filename,
                                                  LastModifiedDate = f.LastModifiedDate,
                                                  CreatedDate = f.CreatedDate
                                              };

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                downloadCenterCategoryFiles = downloadCenterCategoryFiles.Where(x => x.Filename.StartsWith(model.SearchTerm) || x.Id.ToString() == model.SearchTerm);
            }

            downloadCenterCategoryFiles = downloadCenterCategoryFiles.ApplySort(model.OrderBy);


            return downloadCenterCategoryFiles.PagedIndex(new Pagination(downloadCenterCategoryFiles.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<Guid> UpdateAsync(UpdateDownloadCenterItemServiceModel model)
        {
            var downloadCenterCategoryFiles = this.context.DownloadCenterCategoryFiles.Where(x => x.MediaId == model.Id && x.IsActive);

            if (downloadCenterCategoryFiles is null)
            {
                throw new CustomException(this.downloadCenterLocalizer.GetString("DownloadCenterFilesNotFound"), (int)HttpStatusCode.NotFound);
            }

            foreach(var downloadCenterCategoryFile in downloadCenterCategoryFiles.OrEmptyIfNull())
            {
                this.context.DownloadCenterCategoryFiles.Remove(downloadCenterCategoryFile);
            }

            foreach (var categoryId in model.CategoriesIds.OrEmptyIfNull())
            {
                foreach(var downloadCenterCategoryFile in model.Files.OrEmptyIfNull())
                {
                    var file = new DownloadCenterCategoryFile
                    {
                        MediaId = downloadCenterCategoryFile.Id,
                        Filename = downloadCenterCategoryFile.Filename,
                        CategoryId = categoryId
                    };

                    await this.context.DownloadCenterCategoryFiles.AddAsync(file.FillCommonProperties());
                }
            }

            await this.context.SaveChangesAsync();

            return model.Files.FirstOrDefault().Id;
        }

        public async Task UpdateFileNameAsync(Guid? id, string name)
        {
            var files = this.context.DownloadCenterCategoryFiles.Where(x => x.MediaId == id && x.IsActive);

            foreach(var file in files.OrEmptyIfNull())
            {
                file.Filename = name;
            }

            await this.context.SaveChangesAsync();
        }
    }
}
