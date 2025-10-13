using DownloadCenter.Api.Infrastructure;
using DownloadCenter.Api.Infrastructure.Entities.DownloadCenterCategories;
using DownloadCenter.Api.ServicesModels.Categories;
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
using System.Net;
using System.Threading.Tasks;

namespace DownloadCenter.Api.Services.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IStringLocalizer<DownloadCenterResources> _downloadCenterLocalizer;
        private readonly DownloadCenterContext _context;

        public CategoriesService(
            IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer,
            DownloadCenterContext context)
        {
            _downloadCenterLocalizer = downloadCenterLocalizer;
            _context = context;
        }

        public async Task<Guid> CreateAsync(CreateCategoryServiceModel model)
        {
            var category = new DownloadCenterCategory
            {
                ParentCategoryId = model.ParentCategoryId,
                IsVisible = model.IsVisible,
                SellerId = model.OrganisationId.Value,
                Order = 0
            };

            await _context.DownloadCenterCategories.AddAsync(category.FillCommonProperties());

            var categoryTranslation = new DownloadCenterCategoryTranslation
            {
                Name = model.Name,
                Language = model.Language,
                CategoryId = category.Id
            };

            await _context.DownloadCenterCategoryTranslations.AddAsync(categoryTranslation.FillCommonProperties());
            await _context.SaveChangesAsync();

            return category.Id;
        }

        public async Task DeleteAsync(DeleteCategoryServiceModel model)
        {
            var category = await _context.DownloadCenterCategories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category is null)
            {
                throw new NotFoundException(_downloadCenterLocalizer.GetString("CategoryNotFound"));
            }

            if (await _context.DownloadCenterCategories.AnyAsync(x => x.ParentCategoryId == category.Id && x.IsActive))
            {
                throw new ConflictException(_downloadCenterLocalizer.GetString("SubcategoriesDeleteCategoryConflict"));
            }

            if (await _context.DownloadCenterCategoryFiles.AnyAsync(x => x.CategoryId == model.Id && x.IsActive))
            {
                throw new ConflictException(_downloadCenterLocalizer.GetString("CategoryFileConflict"));
            }

            category.IsActive = false;

            await _context.SaveChangesAsync();
        }

        public async Task<CategoryServiceModel> GetAsync(GetCategoryServiceModel model)
        {
            var category = await _context.DownloadCenterCategories
                    .Include(x => x.Translations)
                    .Include(x => x.ParentCategory)
                    .Include(x => x.ParentCategory.Translations)
                    .AsSingleQuery()
                    .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category is null)
            {
                throw new NotFoundException(_downloadCenterLocalizer.GetString("CategoryNotFound"));
            }

            return new CategoryServiceModel
            {
                Id = category.Id,
                Name = category.Translations.FirstOrDefault(t => t.CategoryId == category.Id && t.Language == model.Language)?.Name ?? category.Translations.FirstOrDefault(t => t.CategoryId == category.Id)?.Name,
                IsVisible = category.IsVisible,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = category.ParentCategory?.Translations?.FirstOrDefault(t => t.CategoryId == category.ParentCategoryId && t.Language == model.Language)?.Name ?? category.ParentCategory?.Translations?.FirstOrDefault(t => t.CategoryId == category.ParentCategoryId)?.Name,
                LastModifiedDate = category.LastModifiedDate,
                CreatedDate = category.CreatedDate
            };
        }

        public PagedResults<IEnumerable<CategoryServiceModel>> Get(GetCategoriesServiceModel model)
        {
            var categories = _context.DownloadCenterCategories.Where(x => x.IsActive)
                    .Include(x => x.ParentCategory)
                    .Include(x => x.ParentCategory.Translations)
                    .Include(x => x.Translations)
                    .AsSingleQuery();

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                categories = categories.Where(x => x.Translations.Any(x => x.Name.StartsWith(model.SearchTerm)));
            }

            categories = categories.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<DownloadCenterCategory>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                categories = categories.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = categories.PagedIndex(new Pagination(categories.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = categories.PagedIndex(new Pagination(categories.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return new PagedResults<IEnumerable<CategoryServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.OrEmptyIfNull().Select(x => new CategoryServiceModel
                {
                    Id = x.Id,
                    Name = x.Translations.FirstOrDefault(t => t.CategoryId == x.Id && t.Language == model.Language)?.Name ?? x.Translations.FirstOrDefault(t => t.CategoryId == x.Id)?.Name,
                    ParentCategoryId = x.ParentCategoryId,
                    ParentCategoryName = x.ParentCategory?.Translations?.FirstOrDefault(t => t.CategoryId == x.ParentCategoryId && t.Language == model.Language)?.Name ?? x.ParentCategory?.Translations?.FirstOrDefault(t => t.CategoryId == x.ParentCategoryId)?.Name,
                    IsVisible = x.IsVisible,
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }

        public async Task<Guid> UpdateAsync(UpdateCategoryServiceModel model)
        {
            var category = await _context.DownloadCenterCategories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category is null)
            {
                throw new NotFoundException(_downloadCenterLocalizer.GetString("CategoryNotFound"));
            }

            var categoryTranslation = await _context.DownloadCenterCategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == model.Id && x.Language == model.Language && x.IsActive);

            if (categoryTranslation is not null)
            {
                categoryTranslation.Name = model.Name;
                categoryTranslation.LastModifiedDate = DateTime.UtcNow;
            }
            else
            {
                var newCategoryTranslation = new DownloadCenterCategoryTranslation
                {
                    CategoryId = category.Id,
                    Name = model.Name,
                    Language = model.Language
                };

                _context.DownloadCenterCategoryTranslations.Add(newCategoryTranslation.FillCommonProperties());
            }

            category.ParentCategoryId = model.ParentCategoryId;
            category.IsVisible = model.IsVisible;
            category.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return category.Id;
        }
    }
}
