﻿using DownloadCenter.Api.Infrastructure;
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

        public DownloadCenterItemFileServiceModel Get(GetDownloadCenterFileServiceModel model)
        {
            var downloadCenterFile = _context.DownloadCenterCategoryFiles.FirstOrDefault(x => x.MediaId == model.Id && x.IsActive);

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
            var downloadCenterCategories = _context.DownloadCenterCategories.Where(x => x.IsActive && x.IsVisible)
                    .Include(x => x.Translations)
                    .AsSingleQuery();

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

            var subcategories = downloadCenterCategories.Where(x => x.ParentCategoryId.HasValue).ToList();

            return new PagedResults<IEnumerable<DownloadCenterCategoryItemServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.Where(x => x.ParentCategoryId == null).OrEmptyIfNull().Select(x => new DownloadCenterCategoryItemServiceModel
                {
                    Id = x.Id,
                    Name = x.Translations.FirstOrDefault(t => t.CategoryId == x.Id && t.Language == model.Language)?.Name ?? x.Translations.FirstOrDefault(t => t.CategoryId == x.Id)?.Name,
                    Subcategories = subcategories.Where(c => c.ParentCategoryId == x.Id).Select(s => new DownloadCenterSubcategoryServiceModel
                    {
                        Id = s.Id,
                        Name = x.Translations.FirstOrDefault(t => t.CategoryId == s.Id && t.Language == model.Language)?.Name ?? x.Translations.FirstOrDefault(t => t.CategoryId == s.Id)?.Name,
                    }),
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }

        public DownloadCenterCategoryServiceModel GetDownloadCenterCategory(GetDownloadCenterCategoryServiceModel model)
        {
            var downloadCenterCategory = _context.DownloadCenterCategories.FirstOrDefault(x => x.Id == model.Id && x.IsActive && x.IsVisible);

            if (downloadCenterCategory is null)
            {
                throw new CustomException(_downloadCenterLocalizer.GetString("DownloadCenterFileNotFound"), (int)HttpStatusCode.NoContent);
            }

            var subcategories = _context.DownloadCenterCategories.Where(x => x.ParentCategoryId == downloadCenterCategory.Id).ToList();

            return new DownloadCenterCategoryServiceModel
            {
                Id = downloadCenterCategory.Id,
                CategoryName = downloadCenterCategory.Translations.FirstOrDefault(t => t.CategoryId == model.Id && t.IsActive && t.Language == model.Language)?.Name ?? downloadCenterCategory.Translations.FirstOrDefault(t => t.CategoryId == model.Id && t.IsActive)?.Name,
                ParentCategoryId = downloadCenterCategory.ParentCategoryId,
                ParentCategoryName = downloadCenterCategory.Translations.FirstOrDefault(t => t.CategoryId == downloadCenterCategory.ParentCategoryId && t.IsActive && t.Language == model.Language)?.Name ?? downloadCenterCategory.Translations.FirstOrDefault(t => t.CategoryId == downloadCenterCategory.ParentCategoryId && t.IsActive)?.Name,
                Subcategories = subcategories.Select(x => new DownloadCenterSubcategoryServiceModel
                {
                    Id = x.Id,
                    Name = x.Translations.FirstOrDefault(t => t.CategoryId == x.Id && t.IsActive)?.Name ?? x.Translations.FirstOrDefault(t => t.CategoryId == x.Id && t.IsActive)?.Name,
                }),
                Files= downloadCenterCategory.Files.Where(x => x.CategoryId == downloadCenterCategory.Id).OrEmptyIfNull().Select(x => x.MediaId),
                LastModifiedDate = downloadCenterCategory.LastModifiedDate,
                CreatedDate = downloadCenterCategory.CreatedDate
            };
        }

        public PagedResults<IEnumerable<DownloadCenterCategoryFileServiceModel>> GetDownloadCenterCategoryFiles(GetDownloadCenterCategoryFilesServiceModel model)
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
