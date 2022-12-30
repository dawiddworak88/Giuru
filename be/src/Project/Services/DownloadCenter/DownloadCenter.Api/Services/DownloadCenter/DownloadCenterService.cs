using DownloadCenter.Api.Infrastructure;
using DownloadCenter.Api.Infrastructure.Entities.DownloadCenterCategories;
using DownloadCenter.Api.ServicesModels.DownloadCenter;
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
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;

namespace DownloadCenter.Api.Services.DownloadCenter
{
    public class DownloadCenterService : IDownloadCenterService
    {
        private readonly DownloadCenterContext _context;
        private readonly IStringLocalizer<DownloadCenterResources> _downloadCenterLocalizer;

        public DownloadCenterService(
            IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer,
            DownloadCenterContext context)
        {
            _context = context;
            _downloadCenterLocalizer = downloadCenterLocalizer;
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

                    await _context.DownloadCenterCategoryFiles.AddAsync(categoryFile.FillCommonProperties());
                }
            }

            await _context.SaveChangesAsync();

            return model.Files.FirstOrDefault().Id;
        }

        public async Task DeleteAsync(DeleteDownloadCenterItemServiceModel model)
        {
            var downloadCenterFile = _context.DownloadCenterCategoryFiles.Where(x => x.MediaId == model.Id && x.IsActive);

            if (downloadCenterFile is null)
            {
                throw new CustomException(_downloadCenterLocalizer.GetString("DownloadCenterFileNotFound"), (int)HttpStatusCode.NoContent);
            }

            foreach(var downloadCenterCategoryFile in downloadCenterFile.OrEmptyIfNull())
            {
                downloadCenterCategoryFile.IsActive = false;
                downloadCenterCategoryFile.LastModifiedDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<PagedResults<IEnumerable<DownloadCenterItemServiceModel>>> GetAsync(GetDownloadCenterFilesServiceModel model)
        {
            var downloadCenterFiles = _context.DownloadCenterCategoryFiles.Where(x => x.IsActive);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                var searchFiles = downloadCenterFiles.Where(x => x.Id.ToString() == model.SearchTerm || x.Filename.Contains(model.SearchTerm));

                var categoryTranslation = await _context.DownloadCenterCategoryTranslations.FirstOrDefaultAsync(x => x.Name.StartsWith(model.SearchTerm));

                if (categoryTranslation is not null)
                {
                    searchFiles = downloadCenterFiles.Where(x => x.CategoryId == categoryTranslation.CategoryId);
                }

                downloadCenterFiles = searchFiles;
            }

            downloadCenterFiles = downloadCenterFiles.ApplySort(model.OrderBy);

            var downloadCenterFileGroups = downloadCenterFiles.ToList().GroupBy(x => x.MediaId);

            PagedResults<IEnumerable<DownloadCenterCategoryFile>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                downloadCenterFiles = downloadCenterFiles.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = downloadCenterFiles.PagedIndex(new Pagination(downloadCenterFileGroups.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = downloadCenterFiles.PagedIndex(new Pagination(downloadCenterFileGroups.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

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

                var translations = _context.DownloadCenterCategoryTranslations.Where(x => downloadCenterFileGroup.Select(y => y.CategoryId).Contains(x.CategoryId)).ToList();

                foreach (var categoryId in downloadCenterFileGroup.OrEmptyIfNull().Select(x => x.CategoryId))
                {
                    var categoryName = translations.FirstOrDefault(x => x.CategoryId == categoryId && x.Language == model.Language && x.IsActive)?.Name ?? translations.FirstOrDefault(x => x.CategoryId == categoryId && x.IsActive)?.Name;

                    namesOfCategories.Add(categoryName);
                }

                fileGroup.Categories = namesOfCategories;

                downloadCenterFilesList.Add(fileGroup);
            }

            pagedDownloadCenterFilesServiceModel.Data = downloadCenterFilesList;

            return pagedDownloadCenterFilesServiceModel;
        }

        public async Task<DownloadCenterItemFileServiceModel> GetAsync(GetDownloadCenterFileServiceModel model)
        {
            var downloadCenterFile = await _context.DownloadCenterCategoryFiles.FirstOrDefaultAsync(x => x.MediaId == model.Id && x.IsActive);

            if (downloadCenterFile is null)
            {
                throw new CustomException(_downloadCenterLocalizer.GetString("DownloadCenterFileNotFound"), (int)HttpStatusCode.NoContent);
            }

            var downloadCenterFileItem = new DownloadCenterItemFileServiceModel
            {
                Id = downloadCenterFile.MediaId,
                LastModifiedDate = downloadCenterFile.LastModifiedDate,
                CreatedDate = downloadCenterFile.CreatedDate
            };

            var downloadCenterFileCategories = _context.DownloadCenterCategoryFiles.Where(x => x.MediaId == model.Id && x.IsActive);

            if (downloadCenterFileCategories is not null)
            {
                downloadCenterFileItem.CategoriesIds = downloadCenterFileCategories.Select(x => x.CategoryId);
            }

            return downloadCenterFileItem;
        }

        public PagedResults<IEnumerable<DownloadCenterCategoryItemServiceModel>> Get(GetDownloadCenterItemsServiceModel model)
        {
            var downloadCenterCategories = _context.DownloadCenterCategories.Where(x => x.IsActive && x.IsVisible && x.ParentCategoryId == null);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                downloadCenterCategories = downloadCenterCategories.Where(x => x.Translations.Any(y => y.Name.StartsWith(model.SearchTerm)) || x.Id.ToString() == model.SearchTerm);
            }

            downloadCenterCategories = downloadCenterCategories.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<DownloadCenterCategory>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                downloadCenterCategories = downloadCenterCategories.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = downloadCenterCategories.PagedIndex(new Pagination(downloadCenterCategories.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = downloadCenterCategories.PagedIndex(new Pagination(downloadCenterCategories.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            var translations = _context.DownloadCenterCategoryTranslations.Where(x => pagedResults.Data.Select(y => y.Id).Contains(x.CategoryId)).ToList();

            var subcategories = _context.DownloadCenterCategories.Where(x => pagedResults.Data.Select(y => y.ParentCategoryId).Contains(x.Id)).ToList();

            return new PagedResults<IEnumerable<DownloadCenterCategoryItemServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.OrEmptyIfNull().Select(x => new DownloadCenterCategoryItemServiceModel
                {
                    Id = x.Id,
                    Name = translations.FirstOrDefault(t => t.CategoryId == x.Id && t.Language == model.Language)?.Name ?? translations.FirstOrDefault(t => t.CategoryId == x.Id)?.Name,
                    Subcategories = subcategories.Select(s => new DownloadCenterSubcategoryServiceModel
                    {
                        Id = s.Id,
                        Name = translations.FirstOrDefault(t => t.CategoryId == s.Id && t.Language == model.Language)?.Name ?? translations.FirstOrDefault(t => t.CategoryId == s.Id)?.Name,
                    }),
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }

        public async Task<DownloadCenterCategoryServiceModel> GetDownloadCenterCategoryAsync(GetDownloadCenterCategoryServiceModel model)
        {
            var downloadCenterCategory = await _context.DownloadCenterCategories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive && x.IsVisible);

            if (downloadCenterCategory is null)
            {
                throw new CustomException(_downloadCenterLocalizer.GetString("DownloadCenterFileNotFound"), (int)HttpStatusCode.NoContent);
            }

            var category = new DownloadCenterCategoryServiceModel
            {
                Id = downloadCenterCategory.Id,
                ParentCategoryId = downloadCenterCategory.ParentCategoryId,
                LastModifiedDate = downloadCenterCategory.LastModifiedDate,
                CreatedDate = downloadCenterCategory.CreatedDate
            };

            var categoryTranslation = _context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCenterCategory.Id && x.IsActive && x.Language == model.Language);

            if (categoryTranslation is null)
            {
                categoryTranslation = _context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCenterCategory.Id && x.IsActive);
            }

            category.CategoryName = categoryTranslation?.Name;

            if (downloadCenterCategory.ParentCategoryId.HasValue)
            {
                var parentCategoryTranslation = _context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCenterCategory.ParentCategoryId && x.IsActive && x.Language == model.Language);

                if (parentCategoryTranslation is null)
                {
                    parentCategoryTranslation = _context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.CategoryId == downloadCenterCategory.ParentCategoryId && x.IsActive);
                }

                category.ParentCategoryName = parentCategoryTranslation?.Name;
            }

            var subcategories = _context.DownloadCenterCategories.Where(x => x.ParentCategoryId == downloadCenterCategory.Id && x.IsVisible && x.IsActive);

            var downloadCenterSubcategories = new List<DownloadCenterSubcategoryServiceModel>();

            foreach (var subcategory in subcategories.OrEmptyIfNull().ToList())
            {
                var subcategoryItem = new DownloadCenterSubcategoryServiceModel
                {
                    Id = subcategory.Id
                };

                var subcategoryTranslation = _context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.CategoryId == subcategory.Id && x.IsActive && x.Language == model.Language);

                if (subcategoryTranslation is null)
                {
                    subcategoryTranslation = _context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.CategoryId == subcategory.Id && x.IsActive);
                }

                subcategoryItem.Name = subcategoryTranslation?.Name;

                downloadCenterSubcategories.Add(subcategoryItem);
            }

            var files = _context.DownloadCenterCategoryFiles.Where(x => x.CategoryId == downloadCenterCategory.Id && x.IsActive);

            if (files.Any())
            {
                category.Files = files.Select(x => x.MediaId);
            }

            category.Subcategories = downloadCenterSubcategories;

            return category;
        }

        public async Task<PagedResults<IEnumerable<DownloadCenterCategoryFileServiceModel>>> GetDownloadCenterCategoryFilesAsync(GetDownloadCenterCategoryFilesServiceModel model)
        {
            var downloadCenterCategoryFiles = from f in _context.DownloadCenterCategoryFiles
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
                downloadCenterCategoryFiles = downloadCenterCategoryFiles.Where(x => x.Filename.Contains(model.SearchTerm) || x.Id.ToString() == model.SearchTerm);
            }

            downloadCenterCategoryFiles = downloadCenterCategoryFiles.ApplySort(model.OrderBy);

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                downloadCenterCategoryFiles = downloadCenterCategoryFiles.Take(Constants.MaxItemsPerPageLimit);

                return downloadCenterCategoryFiles.PagedIndex(new Pagination(downloadCenterCategoryFiles.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }

            return downloadCenterCategoryFiles.PagedIndex(new Pagination(downloadCenterCategoryFiles.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
        }

        public async Task<Guid> UpdateAsync(UpdateDownloadCenterItemServiceModel model)
        {
            var downloadCenterCategoryFiles = _context.DownloadCenterCategoryFiles.Where(x => x.MediaId == model.Id && x.IsActive);

            if (downloadCenterCategoryFiles is null)
            {
                throw new CustomException(_downloadCenterLocalizer.GetString("DownloadCenterFilesNotFound"), (int)HttpStatusCode.NoContent);
            }

            foreach(var downloadCenterCategoryFile in downloadCenterCategoryFiles.OrEmptyIfNull())
            {
                _context.DownloadCenterCategoryFiles.Remove(downloadCenterCategoryFile);
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

                    await _context.DownloadCenterCategoryFiles.AddAsync(file.FillCommonProperties());
                }
            }

            await _context.SaveChangesAsync();

            return model.Files.FirstOrDefault().Id;
        }

        public async Task UpdateFileNameAsync(Guid? id, string name)
        {
            var files = _context.DownloadCenterCategoryFiles.Where(x => x.MediaId == id && x.IsActive);

            foreach(var file in files.OrEmptyIfNull())
            {
                file.Filename = name;
            }

            await _context.SaveChangesAsync();
        }
    }
}
