using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using News.Api.Infrastructure;
using News.Api.Infrastructure.Entities.Categories;
using News.Api.ServicesModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace News.Api.Services.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly NewsContext _context;
        private readonly IStringLocalizer<NewsResources> _newsLocalizer;

        public CategoriesService(
            NewsContext context,
            IStringLocalizer<NewsResources> newsLocalizer)
        {
            _context = context;
            _newsLocalizer = newsLocalizer;
        }

        public async Task<CategoryServiceModel> CreateAsync(CreateCategoryServiceModel model)
        {
            var category = new Category
            {
                ParentCategoryId = model.ParentCategoryId
            };

            _context.Categories.Add(category.FillCommonProperties());

            var categoryTranslation = new CategoryTranslation
            {
                Name = model.Name,
                Language = model.Language,
                CategoryId = category.Id
            };

            _context.CategoryTranslations.Add(categoryTranslation.FillCommonProperties());

            await _context.SaveChangesAsync();

            return await this.GetAsync(new GetCategoryServiceModel { Id = category.Id, Language = model.Language, Username = model.Username, OrganisationId = model.OrganisationId });
        }

        public async Task DeleteAsync(DeleteCategoryServiceModel model)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category is null)
            {
                throw new NotFoundException(_newsLocalizer.GetString("CategoryNotFound"));
            }

            if (await _context.Categories.AnyAsync(x => x.ParentCategoryId == category.Id && x.IsActive))
            {
                throw new ConflictException(_newsLocalizer.GetString("SubcategoriesDeleteCategoryConflict"));
            }

            if (await _context.NewsItems.AnyAsync(x => x.CategoryId == category.Id && x.IsActive))
            {
                throw new ConflictException(_newsLocalizer.GetString("CategoryDeleteNewsConflict"));
            }

            category.IsActive = false;

            await _context.SaveChangesAsync();
        }

        public async Task<CategoryServiceModel> GetAsync(GetCategoryServiceModel model)
        {
            var category = await _context.Categories
                    .Include(x => x.Translations)
                    .Include(x => x.ParentCategory)
                    .Include(x => x.ParentCategory.Translations)
                    .AsSingleQuery()
                    .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category is null)
            {
                throw new NotFoundException(_newsLocalizer.GetString("CategoryNotFound"));
            }

            return new CategoryServiceModel
            {
                Id = category.Id,
                Name = category.Translations.FirstOrDefault(t => t.CategoryId == category.Id && t.Language == model.Language && t.IsActive)?.Name ?? category.Translations.FirstOrDefault(t => t.CategoryId == category.Id && t.IsActive)?.Name,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = category.ParentCategory?.Translations?.FirstOrDefault(t => t.CategoryId == category.ParentCategoryId && t.Language == model.Language && t.IsActive)?.Name ?? category.ParentCategory?.Translations?.FirstOrDefault(t => t.CategoryId == category.ParentCategoryId && t.IsActive)?.Name,
                LastModifiedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow
            };
        }

        public PagedResults<IEnumerable<CategoryServiceModel>> Get(GetCategoriesServiceModel model)
        {
            var categories = _context.Categories.Where(x => x.IsActive)
                    .Include(x => x.ParentCategory)
                    .Include(x => x.ParentCategory.Translations)
                    .Include(x => x.Translations)
                    .AsSingleQuery();

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                categories = categories.Where(x => x.Translations.Any(x => x.Name.StartsWith(model.SearchTerm)));
            }
            categories = categories.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<Category>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                categories = categories.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = categories.PagedIndex(new Pagination(categories.Count(), Constants.MaxItemsPerPage), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = categories.PagedIndex(new Pagination(categories.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return new PagedResults<IEnumerable<CategoryServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = categories.OrEmptyIfNull().Select(x => new CategoryServiceModel
                {
                    Id = x.Id,
                    Name = x.Translations.FirstOrDefault(t => t.CategoryId == x.Id && t.Language == model.Language && t.IsActive)?.Name ?? x.Translations.FirstOrDefault(t => t.CategoryId == x.Id && t.IsActive)?.Name,
                    ParentCategoryId = x.ParentCategoryId,
                    ParentCategoryName = x.ParentCategory?.Translations?.FirstOrDefault(t => t.CategoryId == x.ParentCategoryId && t.Language == model.Language && t.IsActive)?.Name ?? x.ParentCategory?.Translations?.FirstOrDefault(t => t.CategoryId == x.ParentCategoryId && t.IsActive)?.Name,
                    LastModifiedDate= x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }

        public async Task<CategoryServiceModel> UpdateAsync(UpdateCategoryServiceModel model)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);
            
            if (category is null)
            {
                throw new NotFoundException(_newsLocalizer.GetString("CategoryNotFound"));
            }

            if (model.ParentCategoryId.HasValue)
            {
                var parentCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == model.ParentCategoryId && x.IsActive);

                if (parentCategory is null)
                {
                    throw new NotFoundException(_newsLocalizer.GetString("ParentCategoryNotFound"));
                }

                category.ParentCategoryId = model.ParentCategoryId;
            }

            var categoryTranslation = _context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == model.Id && x.Language == model.Language && x.IsActive);
            
            if (categoryTranslation is not null)
            {
                categoryTranslation.Name = model.Name;
                categoryTranslation.LastModifiedDate = DateTime.UtcNow;
            }
            else
            {
                var newCategoryTranslation = new CategoryTranslation
                {
                    CategoryId = category.Id,
                    Name = model.Name
                };

                _context.CategoryTranslations.Add(newCategoryTranslation.FillCommonProperties());
            }

            category.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await this.GetAsync(new GetCategoryServiceModel { Id = category.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }
    }
}