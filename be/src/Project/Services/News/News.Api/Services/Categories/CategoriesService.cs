using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
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
        private readonly NewsContext newsContext;
        private readonly IStringLocalizer<NewsResources> newsLocalizer;

        public CategoriesService(
            NewsContext newsContext,
            IStringLocalizer<NewsResources> newsLocalizer)
        {
            this.newsContext = newsContext;
            this.newsLocalizer = newsLocalizer;
        }

        public async Task<CategoryServiceModel> CreateAsync(CreateCategoryServiceModel model)
        {
            var category = new Category
            {
                ParentCategoryId = model.ParentCategoryId
            };

            this.newsContext.Categories.Add(category.FillCommonProperties());
            var categoryTranslation = new CategoryTranslation
            {
                Name = model.Name,
                Language = model.Language,
                CategoryId = category.Id
            };

            this.newsContext.CategoryTranslations.Add(categoryTranslation.FillCommonProperties());
            await this.newsContext.SaveChangesAsync();

            return await this.GetAsync(new GetCategoryServiceModel { Id = category.Id, Language = model.Language, Username = model.Username, OrganisationId = model.OrganisationId });
        }

        public async Task DeleteAsync(DeleteCategoryServiceModel model)
        {
            var category = await this.newsContext.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category is null)
            {
                throw new CustomException(this.newsLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NoContent);
            }

            if (await this.newsContext.Categories.AnyAsync(x => x.ParentCategoryId == category.Id && x.IsActive))
            {
                throw new CustomException(this.newsLocalizer.GetString("SubcategoriesDeleteCategoryConflict"), (int)HttpStatusCode.Conflict);
            }

            if (await this.newsContext.NewsItems.AnyAsync(x => x.CategoryId == category.Id && x.IsActive))
            {
                throw new CustomException(this.newsLocalizer.GetString("CategoryDeleteNewsConflict"), (int)HttpStatusCode.Conflict);
            }

            category.IsActive = false;

            await this.newsContext.SaveChangesAsync();
        }

        public async Task<CategoryServiceModel> GetAsync(GetCategoryServiceModel model)
        {
            var category = this.newsContext.Categories.FirstOrDefault(x => x.Id == model.Id && x.IsActive);

            if (category is not null)
            {
                var item = new CategoryServiceModel
                {
                    Id = category.Id,
                    ParentCategoryId = category.ParentCategoryId,
                    LastModifiedDate = category.LastModifiedDate,
                    CreatedDate = category.CreatedDate
                };

                var categoryTranslations = this.newsContext.CategoryTranslations.FirstOrDefault(x => x.Language == model.Language && x.CategoryId == category.Id && x.IsActive);
                
                if (categoryTranslations is null)
                {
                    categoryTranslations = this.newsContext.CategoryTranslations.FirstOrDefault(x => x.IsActive);
                }

                item.Name = categoryTranslations?.Name;

                if (category.ParentCategoryId.HasValue)
                {
                    var categoryParentTranslations = this.newsContext.CategoryTranslations.FirstOrDefault(x => x.Language == model.Language && x.CategoryId == category.ParentCategoryId && x.IsActive);
                    
                    if (categoryParentTranslations is null)
                    {
                        categoryParentTranslations = this.newsContext.CategoryTranslations.FirstOrDefault(x => x.CategoryId == category.ParentCategoryId && x.IsActive);
                    }

                    item.ParentCategoryName = categoryParentTranslations?.Name;
                }

                return item;
            }

            return default;
        }

        public async Task<PagedResults<IEnumerable<CategoryServiceModel>>> GetAsync(GetCategoriesServiceModel model)
        {
            var categories = this.newsContext.Categories.Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                categories = categories.Where(x => x.Translations.Any(x => x.Name.StartsWith(model.SearchTerm)));
            }

            categories = categories.ApplySort(model.OrderBy);

            var pagedResults = categories.PagedIndex(new Pagination(categories.Count(), model.ItemsPerPage), model.PageIndex);

            var pagedCategoriesServiceModel = new PagedResults<IEnumerable<CategoryServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var categoriesItems = new List<CategoryServiceModel>();
            foreach (var categoryItem in pagedResults.Data.ToList())
            {
                var category = new CategoryServiceModel
                {
                    Id = categoryItem.Id,
                    ParentCategoryId = categoryItem.ParentCategoryId,
                    LastModifiedDate = categoryItem.LastModifiedDate,
                    CreatedDate = categoryItem.CreatedDate
                };

                var categoryTranslations = this.newsContext.CategoryTranslations.FirstOrDefault(x => x.Language == model.Language && x.CategoryId == category.Id && x.IsActive);
                
                if (categoryTranslations is null)
                {
                    categoryTranslations = this.newsContext.CategoryTranslations.FirstOrDefault(x => x.IsActive);
                }

                category.Name = categoryTranslations?.Name;

                if (categoryItem.ParentCategoryId.HasValue)
                {
                    var categoryParentTranslations = this.newsContext.CategoryTranslations.FirstOrDefault(x => x.Language == model.Language && x.CategoryId == category.ParentCategoryId && x.IsActive);
                    
                    if (categoryParentTranslations is null)
                    {
                        categoryParentTranslations = this.newsContext.CategoryTranslations.FirstOrDefault(x => x.IsActive);
                    }

                    category.ParentCategoryName = categoryParentTranslations?.Name;
                }

                categoriesItems.Add(category);
            };

            pagedCategoriesServiceModel.Data = categoriesItems;

            return pagedCategoriesServiceModel;
        }

        public async Task<CategoryServiceModel> UpdateAsync(UpdateCategoryServiceModel model)
        {
            var category = await this.newsContext.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);
            
            if (category is null)
            {
                throw new CustomException(this.newsLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NoContent);
            }

            if (model.ParentCategoryId.HasValue)
            {
                var parentCategory = await this.newsContext.Categories.FirstOrDefaultAsync(x => x.Id == model.ParentCategoryId && x.IsActive);

                if (parentCategory is null)
                {
                    throw new CustomException(this.newsLocalizer.GetString("ParentCategoryNotFound"), (int)HttpStatusCode.NoContent);
                }

                category.ParentCategoryId = model.ParentCategoryId;
            }

            var categoryTranslation = this.newsContext.CategoryTranslations.FirstOrDefault(x => x.CategoryId == model.Id && x.Language == model.Language && x.IsActive);
            
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

                this.newsContext.CategoryTranslations.Add(newCategoryTranslation.FillCommonProperties());
            }

            category.LastModifiedDate = DateTime.UtcNow;

            await this.newsContext.SaveChangesAsync();

            return await this.GetAsync(new GetCategoryServiceModel { Id = category.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }
    }
}