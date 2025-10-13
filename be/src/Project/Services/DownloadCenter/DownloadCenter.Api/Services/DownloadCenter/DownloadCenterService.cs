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

            if (downloadCenterFile is null || downloadCenterFile.Any() is false)
            {
                throw new NotFoundException(_downloadCenterLocalizer.GetString("DownloadCenterFileNotFound"));
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
            var downloadCenterFiles = _context.DownloadCenterCategoryFiles.Where(x => x.IsActive)
                .Include(x => x.Category)
                .Include(x => x.Category.Translations)
                .AsSingleQuery(); ;

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                downloadCenterFiles = downloadCenterFiles.Where(x => x.Id.ToString() == model.SearchTerm || x.Filename.Contains(model.SearchTerm));
            }

            downloadCenterFiles = downloadCenterFiles.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<DownloadCenterCategoryFile>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                downloadCenterFiles = downloadCenterFiles.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = downloadCenterFiles.PagedIndex(new Pagination(downloadCenterFiles.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = downloadCenterFiles.PagedIndex(new Pagination(downloadCenterFiles.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return new PagedResults<IEnumerable<DownloadCenterItemServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults?.Data?.Select(x => new DownloadCenterItemServiceModel
                { 
                    Id = x.MediaId,
                    Filename = x.Filename,
                    Categories = new[] { x.Category.Translations.FirstOrDefault(x => x.Language == model.Language && x.IsActive)?.Name ?? x.Category.Translations.FirstOrDefault(x => x.IsActive)?.Name },
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }

        public async Task<DownloadCenterItemFileServiceModel> GetAsync(GetDownloadCenterFileServiceModel model)
        {
            var downloadCenterFile = await _context.DownloadCenterCategoryFiles.FirstOrDefaultAsync(x => x.MediaId == model.Id && x.IsActive);

            if (downloadCenterFile is null)
            {
                throw new NotFoundException(_downloadCenterLocalizer.GetString("DownloadCenterFileNotFound"));
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

            var subcategories = downloadCenterCategories.Where(x => pagedResults.Data.Select(y => y.Id).Contains(x.ParentCategoryId.Value)).ToList();

            return new PagedResults<IEnumerable<DownloadCenterCategoryItemServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.Where(x => x.ParentCategoryId == null).OrEmptyIfNull().Select(x => new DownloadCenterCategoryItemServiceModel
                {
                    Id = x.Id,
                    Name = x.Translations.FirstOrDefault(t => t.CategoryId == x.Id && t.Language == model.Language)?.Name ?? x.Translations.FirstOrDefault(t => t.CategoryId == x.Id)?.Name,
                    Subcategories = subcategories.Where(c => c.ParentCategoryId == x.Id).Select(s => new DownloadCenterSubcategoryServiceModel
                    {
                        Id = s.Id,
                        Name = s.Translations.FirstOrDefault(t => t.CategoryId == s.Id && t.Language == model.Language)?.Name ?? s.Translations.FirstOrDefault(t => t.CategoryId == s.Id)?.Name
                    }),
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }

        public async Task<DownloadCenterCategoryServiceModel> GetDownloadCenterCategoryAsync(GetDownloadCenterCategoryServiceModel model)
        {
            var downloadCenterCategory = await _context.DownloadCenterCategories
                    .Include(x => x.Translations)
                    .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive && x.IsVisible);

            if (downloadCenterCategory is null)
            {
                throw new NotFoundException(_downloadCenterLocalizer.GetString("DownloadCenterFileNotFound"));
            }

            var subcategories = _context.DownloadCenterCategories.Where(x => x.ParentCategoryId == downloadCenterCategory.Id && x.IsActive && x.IsVisible).ToList();

            return new DownloadCenterCategoryServiceModel
            {
                Id = downloadCenterCategory.Id,
                CategoryName = downloadCenterCategory.Translations.FirstOrDefault(t => t.CategoryId == model.Id && t.IsActive && t.Language == model.Language)?.Name ?? downloadCenterCategory.Translations.FirstOrDefault(t => t.CategoryId == model.Id && t.IsActive)?.Name,
                ParentCategoryId = downloadCenterCategory.ParentCategoryId,
                ParentCategoryName = downloadCenterCategory.Translations.FirstOrDefault(t => t.CategoryId == downloadCenterCategory.ParentCategoryId && t.IsActive && t.Language == model.Language)?.Name ?? downloadCenterCategory.Translations.FirstOrDefault(t => t.CategoryId == downloadCenterCategory.ParentCategoryId && t.IsActive)?.Name,
                Subcategories = subcategories.Select(x => new DownloadCenterSubcategoryServiceModel
                {
                    Id = x.Id,
                    Name = x.Translations.FirstOrDefault(t => t.CategoryId == x.Id && t.Language == model.Language && t.IsActive)?.Name ?? x.Translations.FirstOrDefault(t => t.CategoryId == x.Id && t.IsActive)?.Name,
                }),
                Files = downloadCenterCategory.Files.Select(x => x.MediaId),
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

            if (downloadCenterCategoryFiles is null || downloadCenterCategoryFiles.Any() is false)
            {
                throw new NotFoundException(_downloadCenterLocalizer.GetString("DownloadCenterFileNotFound"));
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
